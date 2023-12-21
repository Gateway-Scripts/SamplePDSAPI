using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePDSAPI.Models
{
    /// <summary>
    /// Abstraction of the Portal Dosimetry Session.
    /// On the selection of a SessionItem, the SessionAnalyses collection will display the analysis results. 
    /// </summary>
    public class SessionItem
    {
        public string SessionId { get; set; }
        public DateTime SessionDateTime { get; set; }
        public string SessionDescription { get; set; }
        public ObservableCollection<FieldAnalysisModel> SessionAnalyses { get; set; }
        public SessionItem()
        {
           SessionAnalyses = new ObservableCollection<FieldAnalysisModel>();
        }

    }
}
