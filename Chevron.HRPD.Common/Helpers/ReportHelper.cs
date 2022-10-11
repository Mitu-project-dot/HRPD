//using Microsoft.Reporting.WebForms;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Web;
//using System;
//namespace Chevron.HRPD.Common.Helpers
//{
//    public class ReportHelper
//    {


//        #region Properties

//        public string ReportPath { get; set; }

//        public string ReportServer { get; set; }

//        public List<ReportParameter> ReportParameters{ get; set; }

//        public string Type { get; set; }

//        #endregion

//        #region Methods

//        public ReportHelper(string reportPath)
//        {
//            //Report path must be specified in web.config file
//            this.ReportPath = GetReportFolder() + reportPath;

//            //Report Server must be specified in web.config file
//            this.ReportServer = GetReportServer();

//            this.ReportParameters = new List<ReportParameter>();

//            this.Type = "";
//        }

//        private string GetReportServer()
//        {
//            //Check web config for correct connection info

//            AppSettingsReader configReader = new AppSettingsReader();

//            //Report Server must be specified in web.config file
//            return configReader.GetValue("reportingServer", typeof(string)).ToString();
//        }


//        private string GetReportFolder()
//        {

//            //Check web config for correct connection info

//            AppSettingsReader configReader = new AppSettingsReader();

//            //Report path must be specified in web.config file
//            return configReader.GetValue("reportFolder", typeof(string)).ToString();
//        }

//        public void AddReportParameter(string parameterName, string parameter)
//        {
//            ReportParameters.Add(new ReportParameter(parameterName, parameter));
//        }

//        public void AddMultiValueReportParameter(string parameterName, string[] parameter)
//        {
//            ReportParameters.Add(new ReportParameter(parameterName, parameter));
//        }

//        public string GoToReport()
//        {
//            System.Guid g = System.Guid.NewGuid();

//            //Store current report in session
//            HttpContext.Current.Session.Add(g.ToString(), this);

//            return VirtualPathUtility.ToAbsolute("~/report.aspx") + "?RBGuid=" + g.ToString();
//        }

//        #endregion
        
//    }

//}
