/// THIS APPLICATION Source code is provide for educational pruposes only
/// To show some of the features available in the Portal Dosimetry Scripting API.
/// This application in no way holds any warranties or gaurantees and is not intented for clinical
/// or research use of any kind.
/// Please take care in evaluating any scripts or scripting functions prior to implementing them 
/// into clinical or research use.

using SamplePDSAPI.ViewModels;
using SamplePDSAPI.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VMS.DV.PD.Scripting;
using pdsapi = VMS.DV.PD.Scripting;

namespace SamplePDSAPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private string _patientId;
        private string _pdPlanId;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                using (pdsapi.Application pdApp = pdsapi.Application.CreateApplication())
                {
                    VMS.DV.PD.UI.Base.VTransientImageDataMgr.CreateInstance(true);
                    if (e.Args.Any())
                    {
                        _patientId = e.Args.First().Split(';').First();
                        _pdPlanId = e.Args.First().Split(';').Last();
                    }
                    else
                    {
                        MessageBox.Show("Missing input arguments");
                        this.Shutdown();
                    }
                    Patient patient = pdApp.OpenPatientById(_patientId);//check in case null patient.
                    PDPlanSetup plan = patient.PDPlanSetups.FirstOrDefault(ps => ps.Id.Equals(_pdPlanId));//check null case for plan
                    MainView mainView = new MainView();
                    mainView.DataContext = new MainViewModel(plan);
                    mainView.ShowDialog();
                }
            }
            catch(Exception ex) 
            {
                //TODO: output exception to user or logs. 
            }
        }
    }
}
