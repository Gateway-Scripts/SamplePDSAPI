using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SamplePDSAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.CA.Scripting;
using VMS.DV.PD.Scripting;

namespace SamplePDSAPI.Services
{
    public static class PDCalculationService
    {
        private static PDTemplate _pdTemplate;
        /// <summary>
        /// Set PDTemplate parameters from the App.Config
        /// Other parameters are hard-coded, i.e. alignment, normalization, and ROI.
        /// </summary>
        public static void SetAnalysisTemplate()
        {
            double doseDifference = Convert.ToDouble(ConfigurationManager.AppSettings["DoseDifference"]);
            double distanceToAgreement = Convert.ToDouble(ConfigurationManager.AppSettings["DistanceToAgreement"]);
            _pdTemplate = new PDTemplate(
                false, false, false, false,
                AnalysisMode.CU,
                NormalizationMethod.MaxPredictedDose,
                true, 0.05, ROIType.None,
                10,
                doseDifference,
                distanceToAgreement,
                false,
                new List<EvaluationTestDesc> 
                { 
                    new EvaluationTestDesc(EvaluationTestKind.GammaAreaLessThanOne,
                    0.0,
                    Convert.ToDouble(ConfigurationManager.AppSettings["Tolerance"]),
                    true)
                });
        }
        /// <summary>
        /// Perofrm analysis
        /// </summary>
        /// <param name="portalImage">Measured Portal Dosimetry image</param>
        /// <param name="predictedImage">Predicted Portal Dosimetry Image</param>
        /// <returns>FieldAnalysisModel</returns>
        public static FieldAnalysisModel PerformAnalysis(PortalDoseImage portalImage, DoseImage predictedImage)
        {
            FieldAnalysisModel fieldModel = new FieldAnalysisModel();
            fieldModel.ImageDescription = $"Beam: {portalImage.PDBeam.Id}, Image: {portalImage.Id}\nDate: {portalImage.HistoryDateTime}";
            var analysis = portalImage.CreateTransientAnalysis(_pdTemplate, predictedImage);
            fieldModel.AnalysisResult =$"Gamma Analysis [{_pdTemplate.GammaParamDoseDiff*100.0}%/{_pdTemplate.GammaParamDTA}mm] = {analysis.EvaluationTests.First().TestValue*100.0:F2}%";
            fieldModel.GammaImage = GetGammaImage(analysis.GammaImage);
            return fieldModel;
        }
        /// <summary>
        /// Converts Portal Dosimetry API image into an OxyPlot PlotView
        /// </summary>
        /// <param name="gammaImage">Gamma Image from Portal Dosimetry Analysis</param>
        /// <returns>Plot View (OxyPlot.WPF)</returns>
        private static PlotModel GetGammaImage(ImageRT gammaImage)
        {
            var gImage = gammaImage.FramesRT.First();
            ushort[,] pixels = new ushort[gImage.XSize, gImage.YSize];
            double[,] plotPixels = new double[gImage.XSize, gImage.YSize];
            gammaImage.FramesRT.First().GetVoxels(0, pixels);

            for (int i = 0; i < gImage.XSize; i++)
            {
                for (int j = 0; j < gImage.YSize; j++)
                {
                    double value = gImage.VoxelToDisplayValue(pixels[i, j]);
                    plotPixels[i, gImage.YSize - 1 - j] = value < 1 ? value : value * 100.0;
                }
            }
            PlotModel plt = new PlotModel();
            plt.Axes.Add(new LinearColorAxis
            {
                Palette = OxyPalettes.Plasma(100),
                IsAxisVisible = false
            });
            HeatMapSeries hms = new HeatMapSeries()
            {
                X0 = 0,
                X1 = gImage.XSize,
                Y0 = 0,
                Y1 = gImage.YSize,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = plotPixels,

            };
            plt.Series.Add(hms);
            return plt;
        }
    }
}