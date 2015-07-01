using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// Bootstrap扩展 http://www.cnblogs.com/OceanEyes/p/creating-bootstrap-helpers.html
    /// </summary>
    public static class BootstrapExtensions
    {

        public static MvcHtmlString BootstrapButton(this HtmlHelper helper, string caption, ButtonStyle style, ButtonSizeStyle size)
        {
            if (size != ButtonSizeStyle.Normal)
            {
                return new MvcHtmlString(string.Format("<button type=\"button\" class=\"btn btn-{0} btn-{1}\">{2}</button>", style.ToString().ToLower(), ToBootstrapSize(size), caption));
            }
            return new MvcHtmlString(string.Format("<button type=\"button\" class=\"btn btn-{0}\">{1}</button>", style.ToString().ToLower(), caption));
        }

        private static string ToBootstrapSize(ButtonSizeStyle size)
        {
            string bootstrapSize = string.Empty;
            switch (size)
            {
                case ButtonSizeStyle.Large:
                    bootstrapSize = "lg";
                    break;

                case ButtonSizeStyle.Small:
                    bootstrapSize = "sm";
                    break;

                case ButtonSizeStyle.ExtraSmall:
                    bootstrapSize = "xs";
                    break;
            }
            return bootstrapSize;
        }
    }


    #region 链式写法实现
    public class CtripFluentDiv : IHtmlString
    {
        private string _userName;
        private string _message;

        public CtripFluentDiv(string message)
        {
            _message = message;
        }
        public IHtmlString SetUserName(string userName)
        {
            _userName = userName;
            //需要返回实现IHtmlString对象的实例
            return new Fluent<CtripFluentDiv>(this);
        }

        public string ToHtmlString()
        {
            var alertDiv = new TagBuilder("div");

            alertDiv.InnerHtml = _message + "," + _userName;

            return alertDiv.ToString();
        }
    }

    public static class CtripExtensions
    {
        public static CtripFluentDiv CtripHeader(this HtmlHelper html, string message)
        {
            return new CtripFluentDiv(message);
        }
    } 
    #endregion

    /// <summary>
    /// Fluent基类必须实现IHtmlString
    /// </summary>
    public class Fluent<T> : IHtmlString where T : IHtmlString
    {
        private readonly T _parent;

        public Fluent(T parent)
        {
            _parent = parent;
        }

        public string ToHtmlString()
        {
            return _parent.ToHtmlString();
        }
    }

    #region 链式复杂写法，参见 http://www.cnblogs.com/OceanEyes/p/creating-bootstrap-helpers.html， 可改进为泛型

    public interface IAlertFluent : IHtmlString
    {
        IAlertFluent Dismissible(bool canDismiss = true);
    }

    public interface IAlert : IAlertFluent
    {
        IAlertFluent Danger();
        IAlertFluent Info();
        IAlertFluent Success();
        IAlertFluent Warning();
    }


    public class Alert : IAlert
    {
        private AlertStyle _style;
        private bool _dismissible;
        private string _message;

        public Alert(string message)
        {
            _message = message;
        }

        public IAlertFluent Danger()
        {
            _style = AlertStyle.Danger;
            return new AlertFluent(this);
        }

        public IAlertFluent Info()
        {
            _style = AlertStyle.Info;
            return new AlertFluent(this);
        }

        public IAlertFluent Success()
        {
            _style = AlertStyle.Success;
            return new AlertFluent(this);
        }

        public IAlertFluent Warning()
        {
            _style = AlertStyle.Warning;
            return new AlertFluent(this);
        }

        public IAlertFluent Dismissible(bool canDismiss = true)
        {
            this._dismissible = canDismiss;
            return new AlertFluent(this);
        }

        public string ToHtmlString()
        {
            var alertDiv = new TagBuilder("div");
            alertDiv.AddCssClass("alert");
            alertDiv.AddCssClass("alert-" + _style.ToString().ToLower());
            alertDiv.InnerHtml = _message;

            if (_dismissible)
            {
                alertDiv.AddCssClass("alert-dismissable");
                alertDiv.InnerHtml += AddCloseButton();
            }

            return alertDiv.ToString();
        }

        private static TagBuilder AddCloseButton()
        {
            var closeButton = new TagBuilder("button");
            closeButton.AddCssClass("close");
            closeButton.Attributes.Add("data-dismiss", "alert");
            closeButton.InnerHtml = "&times;";
            return closeButton;
        }
    }


    public class AlertFluent : IAlertFluent
    {
        private readonly Alert _parent;

        public AlertFluent(Alert parent)
        {
            _parent = parent;
        }

        public IAlertFluent Dismissible(bool canDismiss = true)
        {
            return _parent.Dismissible(canDismiss);
        }

        public string ToHtmlString()
        {
            return _parent.ToHtmlString();
        }
    }

    public static class AlertHelper
    {
        public static Alert Alert(this HtmlHelper html, string message)
        {
            return new Alert(message);
        }
    }


    #endregion


    /// <summary>
    /// 创建自动闭合的Helpers
    /// </summary>
    public class Panel : IDisposable
    {
        private readonly TextWriter _writer;

        public Panel(HtmlHelper helper, string title, PanelStyle style = PanelStyle.Default)
        {
            _writer = helper.ViewContext.Writer;

            var panelDiv = new TagBuilder("div");
            panelDiv.AddCssClass("panel-" + style.ToString().ToLower());
            panelDiv.AddCssClass("panel");

            var panelHeadingDiv = new TagBuilder("div");
            panelHeadingDiv.AddCssClass("panel-heading");

            var heading3Div = new TagBuilder("h3");
            heading3Div.AddCssClass("panel-title");
            heading3Div.SetInnerText(title);

            var panelBodyDiv = new TagBuilder("div");
            panelBodyDiv.AddCssClass("panel-body");

            panelHeadingDiv.InnerHtml = heading3Div.ToString();

            string html = string.Format("{0}{1}{2}", panelDiv.ToString(TagRenderMode.StartTag), panelHeadingDiv, panelBodyDiv.ToString(TagRenderMode.StartTag));
            _writer.Write(html);
        }

        public void Dispose()
        {
            _writer.Write("</div></div>");
        }
    }

    public static class PanelHelper
    {
        public static Panel Panel(this HtmlHelper html, string title, PanelStyle style = PanelStyle.Default)
        {
            return new Panel(html, title, style);
        }
    }
}