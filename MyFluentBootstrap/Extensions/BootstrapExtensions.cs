using System;
using System.Collections.Generic;
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
    public class CtripFluentDiv : IFluent
    {
        private string _userName;
        private string _message;

        public CtripFluentDiv(string message)
        {
            _message = message;
        }
        public IFluent SetUserName(string userName)
        {
            _userName = userName;
            //需要返回实现IHtmlString对象的实例
            return new Fluent<CtripFluentDiv>(this);
        }


        public IFluent Dismissible(bool canDismiss = true)
        {
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


    #region 链式复杂写法
    public interface IAlert : IFluent
    {
        IFluent Danger();
        IFluent Info();
        IFluent Success();
        IFluent Warning();
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

        public IFluent Danger()
        {
            _style = AlertStyle.Danger;
            return new Fluent<Alert>(this);
        }

        public IFluent Info()
        {
            _style = AlertStyle.Info;
            return new Fluent<Alert>(this);
        }

        public IFluent Success()
        {
            _style = AlertStyle.Success;
            return new Fluent<Alert>(this);
        }

        public IFluent Warning()
        {
            _style = AlertStyle.Warning;
            return new Fluent<Alert>(this);
        }

        public IFluent Dismissible(bool canDismiss = true)
        {
            this._dismissible = canDismiss;
            return new Fluent<Alert>(this);
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

    public static class AlertExtensions
    {
        public static Alert Alert(this HtmlHelper html, string message)
        {
            return new Alert(message);
        }
    } 
    #endregion



    /// <summary>
    /// 创建Fluent Helpers
    /// </summary>
    public interface IFluent : IHtmlString
    {
        IFluent Dismissible(bool canDismiss = true);
    }

    public class Fluent<T> : IFluent where T : IFluent
    {
        private readonly T _parent;

        public Fluent(T parent)
        {
            _parent = parent;
        }

        public IFluent Dismissible(bool canDismiss = true)
        {
            return _parent.Dismissible(canDismiss);
        }

        public string ToHtmlString()
        {
            return _parent.ToHtmlString();
        }
    }
}