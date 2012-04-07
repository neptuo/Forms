using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Button(this HtmlHelper html, MvcHtmlString content)
        {
            return Button(html, content.ToHtmlString());
        }

        public static MvcHtmlString Button(this HtmlHelper html, string content)
        {
            return MvcHtmlString.Create(String.Format("<div class=\"special-button\"><div class=\"left\"></div><div class=\"middle\">{0}</div><div class=\"right\"></div><div class=\"clear\"></div></div>", content));
        }

        public static MvcHtmlString GrayButton(this HtmlHelper html, MvcHtmlString content)
        {
            return GrayButton(html, content.ToHtmlString());
        }

        public static MvcHtmlString GrayButton(this HtmlHelper html, string content)
        {
            return MvcHtmlString.Create(String.Format("<div class=\"special-button gray\"><div class=\"left\"></div><div class=\"middle\">{0}</div><div class=\"right\"></div><div class=\"clear\"></div></div>", content));
        }
    }
}