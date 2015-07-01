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
        /// 指定网址，支持浏览器打开类型
        /// </summary>
        public static MvcHtmlString ActionLink(this HtmlHelper helper, string linkText, string actionNameOrUrl, TargetType targetType)
        {
            string url = GetRealUrlPath(helper, actionNameOrUrl);
            string strTargetType = targetType.ToString();
            return new MvcHtmlString(string.Format("<a href=\"{0}\" target=\"{1}\" >{2}</a>", url, targetType, linkText));
        }

        /// <summary>
        /// 带图片的链接，支持http的url
        /// </summary>
        public static MvcHtmlString ActionLinkWithImage(this HtmlHelper helper, string imgSrc, string actionNameOrUrl)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);

            string url = GetRealUrlPath(helper, actionNameOrUrl);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };
            tagBuilder.MergeAttribute("href", url);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 带图片的链接，支持contorl,route,不支持http的url
        /// </summary>
        public static MvcHtmlString ActionLinkWithImage(this HtmlHelper helper, string imgSrc, string actionName, string controllerName, object routeValue = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);

            string url = urlHelper.Action(actionName, controllerName, routeValue);

            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };
            tagBuilder.MergeAttribute("href", url);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 根据上下文请求，获取Url或者ActionName内容的真实路径
        /// </summary>
        private static string GetRealUrlPath(HtmlHelper helper, string actionNameOrUrl)
        {
            string url = string.Empty;
            if (actionNameOrUrl.StartsWith("http") || actionNameOrUrl.StartsWith("/"))
            {
                url = actionNameOrUrl;
            }
            else
            {
                url = new UrlHelper(helper.ViewContext.RequestContext).Action(actionNameOrUrl);
            }
            return url;
        }
    }
}