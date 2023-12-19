using SamplePDSAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.DV.PD.Scripting;

namespace SamplePDSAPI.ViewModels
{
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
            Sessions = new List<SessionItem>();
            SetSessions();
        }

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
                foreach(var image in _pdPlan.Sessions.First(s=>s.Id == SelectedSession.SessionId).PortalDoseImages)
                {
                    SelectedSession.SessionAnalyses.Add()
                }
            }
        }
    }
}
