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
    public partial class App : Application
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
                    mainView.Show();
                }
            }
            catch(Exception ex) 
            {
                //TODO: output exception to user or logs. 
            }
        }
    }
}
