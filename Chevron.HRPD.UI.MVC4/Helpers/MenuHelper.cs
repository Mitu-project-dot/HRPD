using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System;

namespace Chevron.HRPD.UI.MVC4.Helpers
{
    /// <summary>
    /// Menu HTML Helper
    /// </summary>
    /// <remarks>Generates the navigational links for the menu. 
    /// The design of this class follows the example from Microsoft ASP.NET MVC 
    /// site http://www.asp.net/mvc/tutorials/providing-website-navigation-with-sitemaps-cs
    /// </remarks>
    public static class MenuHelper
    {
        private static SiteMapNode mainNode;

        #region Public Methods

        /// <summary>
        /// Menus the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>A string containing a list of links. The links are rendered in an HTML unordered list <ul> tag.</returns>
        /// <remarks>Extension method that extends the <see cref="HtmlHelper"/> class.</remarks>
        public static string Menu(this HtmlHelper helper, string id, string smp)
        {
            var sb = new StringBuilder();

            // Render each top level node
            var topLevelNodes = SiteMap.Providers[smp].RootNode.ChildNodes;

            //Create top main menu
            if (!string.IsNullOrEmpty(id) && id.Equals("topnav"))
            {
                // Create opening unordered list tag
                sb.AppendLine("<ul id=\"" + id + "\">");

                // Add all nodes for main menu
                foreach (SiteMapNode mainNode in topLevelNodes)
                {
                    if (IsAccessibleToUser(HttpContext.Current, mainNode))
                        AddChildNode(sb, mainNode, true);
                }

                // Close unordered list tag
                sb.AppendLine("</ul>");
            }
            else //Side menu sub-nav
            {
                //Loop through the top level nodes in reverse so we'll end at the top level node
                for (int i = topLevelNodes.Count - 1; i >= 0; i--)
                {
                    mainNode = topLevelNodes[i];
                    //See if top level nodes matches and has child nodes in the sitemap
                    if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(mainNode.Url))
                    {
                        if (mainNode.ChildNodes.Count > 0)
                        {
                            sb.AppendLine("<h1 style=\"margin: 5px 0; padding: 0;\"><a href=\"" + mainNode.Url + "\">" + mainNode.Title + "</a></h1>");
                            // Create opening unordered list tag
                            sb.AppendLine("<ul id=\"" + id + "\">");

                            //Build the subnav items
                            foreach (SiteMapNode child in mainNode.ChildNodes)
                                if (IsAccessibleToUser(HttpContext.Current, child))
                                    AddChildNode(sb, child, false);

                            // Close unordered list tag
                            sb.AppendLine("</ul>");

                            // Exit the loop when we've found our match
                            break;
                        }
                        else
                            break;
                    }                    
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds the child node.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="node">The node.</param>
        /// <param name="topMenu">Boolean for main menu so that all visible nodes are added to the menu</param>
        private static void AddChildNode(StringBuilder sb, SiteMapNode node, bool topMenu)
        {
            if (node.ChildNodes.Count > 0
                && (node == node.Provider.CurrentNode || IsChildSelected(node.ChildNodes) || HttpContext.Current.Request.Url.AbsoluteUri.Contains(node.Url) || topMenu))
            {
                
                sb.AppendLine("<li>");
                sb.AppendLine(CreateMenuItem(node));
                if (topMenu)
                    sb.AppendLine("<ul>");
                else
                    sb.AppendLine("<ul id=\"sidenav\">");

                foreach (SiteMapNode child in node.ChildNodes)
                    if (IsAccessibleToUser(HttpContext.Current, child))
                        AddChildNode(sb, child, topMenu);
                sb.AppendLine("</ul></li>");
            }
            else
            {
                sb.AppendLine("<li>");
                sb.AppendLine(CreateMenuItem(node));
                sb.AppendLine("</li>");
            }
        }

        /// <summary>
        /// Determines whether [is child selected] [the specified child nodes].
        /// </summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <returns>
        /// 	<c>true</c> if [is child selected] [the specified child nodes]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsChildSelected(SiteMapNodeCollection childNodes)
        {
            return childNodes.Cast<SiteMapNode>().Any(childNode => childNode == childNode.Provider.CurrentNode);
        }

        private static string CreateMenuItem(SiteMapNode node)
        {
            var selected = string.Empty;

            if (node == node.Provider.CurrentNode || IsChildSelected(node.ChildNodes))
                selected = "class=\"selected\"";

            var target = node["target"];

            if (!string.IsNullOrEmpty(target))
            {
                if (string.IsNullOrEmpty(node.Description))
                    return string.Format("<a href=\"{0}\" {1} target=\"{2}\">{3}</a>", node.Url, selected, target, node.Title);

                return string.Format("<a href=\"{0}\" title=\"{1}\" {2} target=\"{3}\">{4}</a>", node.Url, node.Description, selected, target, node.Title);
            }

            if (string.IsNullOrEmpty(node.Description))
                return string.Format("<a href=\"{0}\" {1}>{2}</a>", node.Url, selected, node.Title);

            return string.Format("<a href=\"{0}\" title=\"{1}\" {2}>{3}</a>", node.Url, node.Description, selected, node.Title);
        }

        //Check security of the Site Map nodes
        private static bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            //Node cannot be null
            if (node == null)
                return false;

            //Context cannot be null
            if (context == null)
                return false;

            try
            {
                //Check user role with sitemap node roles
                if ((node.Roles != null) && (node.Roles.Count > 0))
                {
                    //Check each roles in the node
                    foreach (string role in node.Roles)
                    {
                        //Found a role that isn't * and that doesn't match the user role
                        if (!string.Equals(role, "*", StringComparison.InvariantCultureIgnoreCase) && ((context.User == null) || !context.User.IsInRole(role)))
                        {
                            continue;
                        }
                        //Found a match or *
                        return true;
                    }
                }

                //Find nodes with no set security - these are allowed to all
                if (node.Roles == null || node.Roles.Count == 0)
                    return true;

                //Block everything that doesn't explicitly match something above.
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}