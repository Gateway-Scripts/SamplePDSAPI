using SamplePDSAPI.Models;
using SamplePDSAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.DV.PD.Scripting;

namespace SamplePDSAPI.ViewModels
{
    /// <summary>
    /// ViewModel for Sessions and Field Analysis Display
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string v)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        private PDPlanSetup _pdPlan;

        public List<SessionItem> Sessions { get; private set; }
        private SessionItem _selectedSession;

        public SessionItem SelectedSession
        {
            get { return _selectedSession; }
            set 
            { 
                _selectedSession = value;
                OnPropertyChanged("SelectedSession");
                SetSessionAnalysisItems();
            }
        }
        private List<SessionItem> analyzedItems { get; set; }
        public MainViewModel(PDPlanSetup plan)
        {
            _pdPlan = plan;
            analyzedItems = new List<SessionItem>();
            Sessions = new List<SessionItem>();
            SetSessions();
            PDCalculationService.SetAnalysisTemplate();
        }
        /// <summary>
        /// Extract measured sessions from the current PDPlan
        /// </summary>
        private void SetSessions()
        {
            foreach(var session in _pdPlan.Sessions) 
            {
                Sessions.Add(new SessionItem
                {
                    SessionDateTime = session.SessionDate,
                    SessionId = session.Id,
                    SessionDescription = $"{session.Id} - {session.SessionDate}"
                });
            }
        }
        /// <summary>
        /// Calculate the SessionAnalyses.
        /// 1. If the session has alredy been calculated, the analysis should be set to the analysis stored in memory.
        /// 2. If this is a new analysis, use the PDCalculationService to calculate the analysis.
        /// </summary>
        private void SetSessionAnalysisItems()
        {
            SelectedSession.SessionAnalyses.Clear();
            if (analyzedItems.Any(ai=>ai.SessionDescription == SelectedSession.SessionDescription))
            {
                foreach(var analysis in analyzedItems.FirstOrDefault(ai=>ai.SessionDescription == SelectedSession.SessionDescription).SessionAnalyses)
                {
                    SelectedSession.SessionAnalyses.Add(analysis);
                }
            }
            else
            {
                var session = _pdPlan.Sessions.First(s => s.Id == SelectedSession.SessionId);
                foreach(var image in session.PortalDoseImages)
                {
                    SelectedSession.SessionAnalyses.Add(PDCalculationService.PerformAnalysis(image, image.PDBeam.PredictedDoseImage));
                }
            }
        }
    }
}
