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
        private static PDPlanSetup _pdPlan;
        private static PDTemplate _pdTemplate;
        public static void SetPlan(PDPlanSetup pdPlan)
        {
            _pdPlan = pdPlan;

        }
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
                    0.95,
                    true)
                });
        }

    }
}
