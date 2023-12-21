using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePDSAPI.Models
{
    /// <summary>
    /// Model to hold the Analysis description and Gamma Analysis Image.
    /// </summary>
    public class FieldAnalysisModel
    {
        public string ImageDescription { get; set; }
        public string AnalysisResult { get; set; }
        public PlotModel GammaImage { get; set; }
    }
}
