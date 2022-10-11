using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chevron.Ipls.Web_V3_5.UI.Controls;

namespace Chevron.HRPD.UI.WebForms.MasterPages
{
    public partial class Main : System.Web.UI.MasterPage
    {

        #region Properties

        public string LastUpdateDate
        {
            get { return LitLastUpdate.Text; }

            set { LitLastUpdate.Text = value; }
        }
        public string Copyright
        {
            get { return LitCopyright.Text; }

            set { LitCopyright.Text = value; }
        }
        public string Confidential
        {
            get { return LitConfidential.Text; }

            set { LitConfidential.Text = value; }
        }
        public string Layout
        {
            get { return Page.Form.Attributes["class"]; }

            set {
                if (value.Contains("layout-394"))
                    {
                        if (value.Contains("welcome"))
                        {
                            value = value.Replace("welcome","");
                        }


                        welcomeMessage.Visible = false;
                    }
                    else
                    {
                        sidenav.Visible = false;
                    }

                Page.Form.Attributes["class"] = value;
                }

        }
        public Boolean ShowSideNav
        {
            get { return sidenav.Visible; }

            set { sidenav.Visible = value; }
        }

        #endregion

        #region Methods

        void SetOpco(string text, string url)
        {
            HLOpco.Text = text;

            HLOpco.NavigateUrl = url;
        }

        void SetContentContact(string text, string url)
        {
            HLContentContact.Text = text;

            HLContentContact.NavigateUrl = url;
        }

        void SetTechnicalContact(string text, string url)
        {
            HLTechnicalContact.Text = text;

            HLTechnicalContact.NavigateUrl = url;
        }

        void SetHallmark(string AlternateText, string url, int Height, int Width)
        {
            hallmark.AlternateText = AlternateText;

            hallmark.ImageUrl = url;
            
            hallmark.Height = Height;
            
            hallmark.Width = Width;

        }

        void SetSiteName(string AlternateText, string url, int Height, int Width)
        { 
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            TopNav.DataBind();   
        }
        protected void TopNav_MenuItemDataBound(Object sender,System.Web.UI.WebControls.MenuEventArgs e)
        {
            ShouldItemBeEnabled(sender, e);
        }

        protected void Sidenav_MenuItemDataBound(Object sender,System.Web.UI.WebControls.MenuEventArgs e)
        {
            ShouldItemBeEnabled(sender, e);
        }

        private void ShouldItemBeEnabled(Object sender,System.Web.UI.WebControls.MenuEventArgs e)
        {
            Boolean IsVisible = true;

            IBSMenu myMenu = (IBSMenu)sender;
            try
            {
                SiteMapNode siteNode = (SiteMapNode)e.Item.DataItem;

                IsVisible = Boolean.Parse(siteNode["Visible"].ToString());

            }
            catch (Exception ex)
            {
            //We only care if the attribute is present. The default behaivor is visible.
            }

            if (!IsVisible)
            {
                myMenu.HiddenItems.Add(e.Item.NavigateUrl);
            }
        }
        // TopNav.MenuItemDataBound sidenav.MenuItemDataBound
        #endregion
    }
}