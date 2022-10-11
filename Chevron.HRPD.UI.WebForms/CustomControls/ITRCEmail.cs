using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
namespace Chevron.HRPD.UI.WebForms.CustomControls
{
    [ValidationProperty("Text")]
    public class ITRCEmail : ITRCBaseTextbox
    {
        #region Fields

        private RegularExpressionValidator regExpValidator;

        #endregion

        #region Methods

        //Create Textbox and Calendar controls
        protected override void CreateChildControls()
        {


            control = new TextBox();

            control.ID = this.ID + "_txtEmail";

            this.Controls.Add(control);

            regExpValidator = new RegularExpressionValidator();

            regExpValidator.ID = this.ID + "_regExpValidator";

            regExpValidator.ControlToValidate = control.ID;

            regExpValidator.ValidationExpression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            regExpValidator.ErrorMessage = "Email Format is invalid";

            regExpValidator.Display = ValidatorDisplay.Dynamic;


            this.Controls.Add(regExpValidator);


        }
        protected override void Render(HtmlTextWriter writer)
        {
            control.RenderControl(writer);

            regExpValidator.RenderControl(writer);
        }

        #endregion

    }
}