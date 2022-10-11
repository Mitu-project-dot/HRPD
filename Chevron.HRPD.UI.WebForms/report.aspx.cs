//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using Chevron.HRPD.Common.Helpers;
//using Microsoft.Reporting.WebForms;
//namespace Chevron.HRPD.UI.WebForms
//{
//    public partial class report : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
                
//                if (HttpContext.Current.Request.QueryString["RBGuid"] != null)
//                {
//                    System.Guid guid;
//                    if (System.Guid.TryParse(HttpContext.Current.Request.QueryString["RBGuid"], out guid))
//                    {
//                        if (HttpContext.Current.Session[HttpContext.Current.Request.QueryString["RBGuid"]] != null)
//                        {
//                            loadReport(HttpContext.Current.Request.QueryString["RBGuid"]);
//                        }
//                        else if (HttpContext.Current.Cache[HttpContext.Current.Request.QueryString["RBGuid"]] != null)
//                        {
//                            getReport(HttpContext.Current.Request.QueryString["RBGuid"]);
//                        }
//                    }

//                }
//            }
//        }

//        protected void loadReport(string guid)
//        { 
//            try
//            {
//                ReportHelper rh = (ReportHelper)HttpContext.Current.Session[guid];

//                HttpContext.Current.Session.Remove(guid);
            
//                rv.ServerReport.ReportPath = rh.ReportPath;

//                rv.ServerReport.ReportServerUrl = new Uri(rh.ReportServer);

//                rv.ServerReport.SetParameters(rh.ReportParameters);

//                //Initialize the Report

//                rv.ServerReport.Refresh();
//            }
            
//            catch (Exception ex)
//            {
//                throw ex;
//            }

            

//        }
//        protected void getReport(string guid)
//        {
//             try
//            {
//            //Pull the Report Builder out of the Session and remove it from session
//            ReportHelper rh = (ReportHelper)HttpContext.Current.Cache[guid];

//            HttpContext.Current.Cache.Remove(guid);

//            rv.ServerReport.ReportPath = rh.ReportPath;

//            rv.ServerReport.ReportServerUrl = new Uri(rh.ReportServer);

//            rv.ServerReport.SetParameters(rh.ReportParameters);

//            //Initialize the Report
//            Warning[] warnings = new Warning[] {};

//            string[] streamids = new String[] {};

//            string mimeType = "";

//            string encoding = "";

//            string extension = "";

//            Byte[] bytes;

//            //generate PDF and add to documents list
//            bytes = rv.ServerReport.Render(rh.Type, null, out mimeType, out encoding, out extension, out streamids, out warnings);

//            streamids = null;

//            HttpContext.Current.Cache.Add(guid + rh.Type, bytes, null, DateTime.Now.AddMinutes(1), new TimeSpan(0, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
//        }
            
//            catch (Exception ex)
//            {
//                throw ex;
//            }

           
//        }


//    }
//}