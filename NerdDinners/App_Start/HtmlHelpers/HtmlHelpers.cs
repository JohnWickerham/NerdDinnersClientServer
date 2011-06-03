using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace NerdDinners.HtmlHelpers
{
    public static class NerdDinnerHelpers
    {
        public static HtmlHelper GetPageHelper(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((System.Web.Mvc.WebViewPage) WebPageContext.Current.Page).Html;
        }

        public static MvcHtmlString SubmitButton(this HtmlHelper html, string caption)
        {
            var st = string.Format("<input type=\"submit\" value=\"{0}\"/>", caption);
            return new MvcHtmlString(st);
        }

        public static MvcHtmlString SubmitButton(this HtmlHelper helper, string id, string type, string caption, object htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.GenerateId(id);
            builder.MergeAttribute("value", caption);
            builder.MergeAttribute("type", type);
            if (htmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return new MvcHtmlString(builder.ToString());
        }

        //public static string Image(this HtmlHelper helper, string id, string url, string alternateText)
        //{
        //    return Image(helper, id, url, alternateText, null);
        //}

        //public static string Image(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        //{
        //    // Create tag builder
        //    var builder = new TagBuilder("img");

        //    // Create valid id
        //    builder.GenerateId(id);

        //    // Add attributes
        //    builder.MergeAttribute("src", url);
        //    builder.MergeAttribute("alt", alternateText);
        //    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

        //    // Render tag
        //    return builder.ToString(TagRenderMode.SelfClosing);
        //}
    }

    

}