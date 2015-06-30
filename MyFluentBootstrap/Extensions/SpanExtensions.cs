using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// http://www.cnblogs.com/hiflora/p/3613942.html
    /// </summary>
    public static class SpanExtensions
    {
        public static MvcHtmlString Span(this HtmlHelper helper, string id, string text)
        {
            return new MvcHtmlString(String.Format(@"<span id=""{0}"">{1}</span>", id, text));
        }

        public static MvcHtmlString Span(this HtmlHelper helper, string id, string text, string css, object htmlAttributes)
        {
            //创意某一个Tag的TagBuilder
            var builder = new TagBuilder("span");

            //创建Id,注意要先设置IdAttributeDotReplacement属性后再执行GenerateId方法.
            builder.IdAttributeDotReplacement = "-";
            builder.GenerateId(id);

            //添加属性            
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            //添加样式
            builder.AddCssClass(css);
            //或者用下面这句的形式也可以: builder.MergeAttribute("class", css);

            //添加内容,以下两种方式均可
            //builder.InnerHtml = text;
            builder.SetInnerText(text);

            //输出控件
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));

        }
    }

}