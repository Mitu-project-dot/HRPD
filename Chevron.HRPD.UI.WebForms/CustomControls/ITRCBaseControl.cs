using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Chevron.HRPD.UI.WebForms.CustomControls
{
    public abstract class ITRCBaseControl<T> : CompositeControl, INamingContainer where T : WebControl,  new ()
    {
        #region Properties
        protected T control;

        // Sets T control client id as Custom Control client id for validation
        public override string ClientID
        {
            get
            {
                EnsureChildControls();
                return control.ClientID;
            }
        }

        //Use T control TabIndex as custom control TabIndex
        public override short TabIndex
        {
            set
            {
                EnsureChildControls();
                control.TabIndex = value;
            }
        }
        #endregion
    }
}