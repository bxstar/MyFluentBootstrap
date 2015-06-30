using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// 放在System.Web.Mvc命名空间可以减少对命名空间的引用，不要冲突
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 浏览器总在一个新打开、未命名的窗口中载入目标文档。
        /// </summary>
        _blank,
        /// <summary>
        /// 这个目标使得文档载入父窗口或者包含来超链接引用的框架的框架集。如果这个引用是在窗口或者在顶级框架中，那么它与目标 _self 等效。
        /// </summary>
        _parent,
        /// <summary>
        /// 这个目标的值对所有没有指定目标的 <a> 标签是默认目标，它使得目标文档载入并显示在相同的框架或者窗口中作为源文档。这个目标是多余且不必要的，除非和文档标题 <base> 标签中的 target 属性一起使用。
        /// </summary>
        _self,
        /// <summary>
        /// 这个目标使得文档载入包含这个超链接的窗口，用 _top 目标将会清除所有被包含的框架并将文档载入整个浏览器窗口。
        /// </summary>
        _top
    }
}