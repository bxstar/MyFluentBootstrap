using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// ActionLink扩展
    /// </summary>
    public static class ActionLinkExtensions
    {
        /// <summary>
        /// 接受浏览器打开类型
        /// </summary>
        public static MvcHtmlString ActionLink(this HtmlHelper helper, string linkText, string url, TargetType targetType)
        {
            string strTargetType = targetType.ToString();
            return new MvcHtmlString(string.Format("<a href=\"{0}\" target=\"{1}\" >{2}</a>", url, targetType, linkText));
        }
    }
}