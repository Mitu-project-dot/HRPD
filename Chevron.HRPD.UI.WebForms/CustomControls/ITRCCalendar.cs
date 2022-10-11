using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Globalization;
namespace Chevron.HRPD.UI.WebForms.CustomControls
{
    
    [ValidationProperty("Text")]
    public class ITRCCalendar : ITRCBaseTextbox
    {
        #region Fields
        
        private CalendarExtender calExt;

        #endregion

        #region Properties
        
        //Set regional date format
        public string Format
        {
            set
            {
                EnsureChildControls();
                calExt.Format = value;
            }
        }

        
        #endregion


        #region Methods

        //Create Textbox and Calendar controls
        protected override void CreateChildControls()
        {
        

                control = new TextBox();

                control.ID = this.ID + "_txtCalendar";

                control.Attributes.Add("readonly", "readonly");

                this.Controls.Add(control);

                calExt = new CalendarExtender();

                calExt.ID = this.ID + "_calExt";

                calExt.CssClass = "calendar_default";
            
                calExt.TargetControlID = control.ID;
            
                calExt.Format = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;

                calExt.PopupPosition = CalendarPosition.Right;

                this.Controls.Add(calExt);
     
        
        }
        protected override void Render(HtmlTextWriter writer)
        {
            control.RenderControl(writer);

            calExt.RenderControl(writer);
        }

        #endregion

    }
}
