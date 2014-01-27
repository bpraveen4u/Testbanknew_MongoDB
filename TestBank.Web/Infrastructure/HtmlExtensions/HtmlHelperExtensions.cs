using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;
using TestBank.Web.ViewModels;

namespace TestBank.Web.Infrastructure.HtmlExtensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<QuestionViewModel.OptionViewModel> listOfValues, string uniqueId)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list 
                foreach (var item in listOfValues)
                {
                    // Generate an id to be given to the radio button field 
                    var id = string.Format("{0}_{1}", uniqueId, item.Id);

                    // Create and populate a radio button using the existing html helpers 
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Id, new { id = id, style="width:20px;" }).ToHtmlString();

                    // Create the html string that will be returned to the client 
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
                    sb.AppendFormat("<div class=\"RadioButton\">{0} {1}</div>", radio, label);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        //public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
        //    this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression,
        //    IEnumerable<QuestionViewModel.OptionViewModel> listOfValues, string uniqueId)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    var sb = new StringBuilder();

        //    if (listOfValues != null)
        //    {
        //        // Create a radio button for each item in the list 
        //        foreach (var item in listOfValues)
        //        {
        //            // Generate an id to be given to the radio button field 
        //            var id = string.Format("{0}_{1}", uniqueId, item.Id);

        //            // Create and populate a radio button using the existing html helpers 
        //            var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
        //            var radio = htmlHelper.RadioButtonFor(expression, item.Id, new { id = id, style = "width:20px;" }).ToHtmlString();

        //            // Create the html string that will be returned to the client 
        //            // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
        //            sb.AppendFormat("<div class=\"RadioButton\">{0} {1}</div>", radio, label);
        //        }
        //    }

        //    return MvcHtmlString.Create(sb.ToString());
        //} 

        public static string ActivePage(this HtmlHelper helper, string controller, string action)
        {
            string classValue = "";

            if (helper.ViewContext.Controller.ValueProvider.GetValue("controller") != null)
            {
                string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
                string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

                if (currentController == controller && currentAction == action)
                {
                    classValue = "current_page_item";
                }
            }

            return classValue;
        }

        //public static string IncludeJavascriptFile(string fileName)
        //{
        //    return Url.Content("<root>/Javascript/Files/" + fileName);
        //}
    }
}