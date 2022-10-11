using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Chevron.HRPD.UI.WebForms.CustomControls
{
    public class ITRCBaseTextbox : ITRCBaseControl<TextBox>
    {
        // Gets / Sets text of Textbox control
        public string Text
        {
            get
            {
                EnsureChildControls();
                return control.Text;
            }
            set
            {
                EnsureChildControls();
                control.Text = value;
            }
        }
    }
}