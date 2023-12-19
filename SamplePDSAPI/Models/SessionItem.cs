using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePDSAPI.Models
{
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
