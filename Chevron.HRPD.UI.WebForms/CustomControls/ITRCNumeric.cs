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
    public class ITRCNumeric : ITRCBaseTextbox
    {
        #region Fields

        private Enums.Types fieldType;

        private MaskedEditExtender mskEditExtender;

        string digits;

        string decimals;

        #endregion

        #region Properties

        // Number, Decimal or Money
        public Enums.Types Type
        {
            get
            {
                EnsureChildControls();

                return fieldType;
            }
            set
            {
                EnsureChildControls();

                fieldType = value;

                if (fieldType == Enums.Types.Money)
                {
                    mskEditExtender.DisplayMoney = MaskedEditShowSymbol.Left;
                }
                else
                {
                    mskEditExtender.DisplayMoney = MaskedEditShowSymbol.None;
                }

                SetMask();
           
            }
        }

        // Set/Get max number of digits 
        public int NumberOfDigits
        {
            set
            {
                EnsureChildControls();

                digits = string.Empty;

                if (value > 0)
                {
                    for (int i = 0; i < value; i++)
                    {
                        digits = string.Concat(digits, "9");
                    }
                }

                SetMask();

            }
        }

        // Set/Get max number of decimals 
        public int NumberOfDecimals
        {
            set
            {
                EnsureChildControls();

                decimals = string.Empty;

                if (value > 0)
                {
                    for (int i = 0; i < value; i++)
                    {
                        decimals = string.Concat(decimals, "9");
                    }
                }

                SetMask();

            }
        }

        

        //Available to allow programmer to set a custom mask
        public string Mask
        {
            get
            {
                return mskEditExtender.Mask;
            }
            set
            {
                mskEditExtender.Mask = value;
            }
        }

        #endregion

        #region Methods


        private void SetMask()
        {
            if (fieldType == Enums.Types.Number)
            {
                mskEditExtender.Mask = digits;
            }
            else
            {
                //Sets currency separator depending on region culture
                mskEditExtender.Mask = digits + System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + decimals;
            }



        }

        //Create Textbox and Mask controls
        protected override void CreateChildControls()
        {


                control = new TextBox();

                control.ID = this.ID + "_txtNumber";

                this.Controls.Add(control);

                mskEditExtender = new MaskedEditExtender();

                mskEditExtender.ID = this.ID + "_mskNumber";

                mskEditExtender.TargetControlID = control.ID;

                mskEditExtender.MaskType = MaskedEditType.Number;

                mskEditExtender.PromptCharacter = " ";

                mskEditExtender.InputDirection = MaskedEditInputDirection.RightToLeft;

                this.Controls.Add(mskEditExtender);
                
                //Set the default number of digits and decimals

                digits = "99999999";

                decimals = "99";

                SetMask();
        }

        //Render and bind the controls correctly
        protected override void Render(HtmlTextWriter writer)
        {
            control.RenderControl(writer);

            mskEditExtender.RenderControl(writer);
        }
        #endregion
    }
}
