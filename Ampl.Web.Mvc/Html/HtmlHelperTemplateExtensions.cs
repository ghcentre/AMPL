using Ampl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ampl.Web.Mvc.Html
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods for <see cref="HtmlHelper{TModel}"/>.
    /// </summary>
    /// <remarks>
    /// These methods are useful for views which are not strongly-typed but require strongly-typed HtmlHelper extensions.
    /// </remarks>
    public static class HtmlHelperTemplateExtensions
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HtmlHelper"/> for the specified model type.
        /// </summary>
        /// <param name="html">The HtmlHelper passed to the view.</param>
        /// <param name="model">The model passed to the view. May be <see langword="null"/>.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="viewContext">The <see cref="ViewContext"/> passed to the view.</param>
        /// <returns>
        /// The method returns a new instance of the
        /// typed HtmlHelper&lt;T&gt; where T is the <paramref name="modelType"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="model"/> and <paramref name="modelType"/> do not match.
        /// </exception>
        public static HtmlHelper ForModelOfType(this HtmlHelper html, object model, Type modelType, ViewContext viewContext)
        {
            Check.NotNull(html, nameof(html));
            Check.NotNull(modelType, nameof(modelType));
            Check.NotNull(viewContext, nameof(viewContext));

            if (model != null && !modelType.IsAssignableFrom(model.GetType()))
            {
                throw new InvalidOperationException("The type specified can not be used with the model specified.");
            }
            
            var existingViewData = viewContext.ViewData;

            var viewDataDictionaryType = typeof(ViewDataDictionary<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(modelType);
            var constructor = viewDataDictionaryType.GetConstructor(new[] { typeof(ViewDataDictionary) });

            var viewDataDictionary = (ViewDataDictionary)constructor.Invoke(new[] { existingViewData });
            viewDataDictionary.Model = model;

            var modelViewContext = new ViewContext(viewContext.Controller.ControllerContext,
                                                   viewContext.View,
                                                   viewDataDictionary,
                                                   viewContext.TempData,
                                                   viewContext.Writer);

            var viewDataContainer = new ViewDataContainer() { ViewData = viewContext.ViewData };

            var htmlHelperType = typeof(HtmlHelper<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(modelType);

            var htmlHelper = (HtmlHelper)Activator.CreateInstance(htmlHelperType,
                                                                  modelViewContext,
                                                                  viewDataContainer,
                                                                  html.RouteCollection);
            return htmlHelper;
        }

        /// <summary>
        /// Returns HTML markup for each property in the object that is represented by the expression.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The HTML markup for each property in the object that is represented by the expression.</returns>
        public static MvcHtmlString DisplayFor(this HtmlHelper html, Type modelType, string expression)
        {
            Check.NotNull(html, nameof(html));
            Check.NotNull(modelType, nameof(modelType));
            Check.NotNullOrEmptyString(expression, nameof(expression));

            var lambda = MakeLambda(modelType, expression);
            var method = FindMethod(typeof(DisplayExtensions), "DisplayFor", 2, x => true, modelType, lambda.ReturnType);

            var content = (MvcHtmlString)method.Invoke(null, new object[] { html, lambda });
            return content;
        }

        /// <summary>
        /// Returns an HTML input element for each property in the object that is represented by the expression.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>An HTML input element for each property in the object that is represented by the expression.</returns>
        public static MvcHtmlString EditorFor(this HtmlHelper html, Type modelType, string expression)
        {
            Check.NotNull(html, nameof(html));
            Check.NotNull(modelType, nameof(modelType));
            Check.NotNullOrEmptyString(expression, nameof(expression));

            var lambda = MakeLambda(modelType, expression);
            var method = FindMethod(typeof(EditorExtensions), "EditorFor", 2, x => true, modelType, lambda.ReturnType);

            var content = (MvcHtmlString)method.Invoke(null, new object[] { html, lambda });
            return content;
        }

        /// <summary>
        /// Returns an HTML label element and the property name of the property that is represented 
        /// by the specified expression.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="modelType"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor(this HtmlHelper html, Type modelType, string expression, object htmlAttributes)
        {
            Check.NotNull(html, nameof(html));
            Check.NotNull(modelType, nameof(modelType));
            Check.NotNullOrEmptyString(expression, nameof(expression));

            var lambda = MakeLambda(modelType, expression);
            var method = FindMethod(typeof(LabelExtensions),
                                    "LabelFor",
                                    3,
                                    x => x[2].ParameterType == typeof(object),
                                    modelType,
                                    lambda.ReturnType);

            var content = (MvcHtmlString)method.Invoke(null, new object[] { html, lambda, htmlAttributes });
            return content;
        }

        /// <summary>
        /// Returns the HTML markup for a validation-error message for each data field that
        /// is represented by the specified expression.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="modelType"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ValidationMessageFor(this HtmlHelper html,
                                                         Type modelType,
                                                         string expression,
                                                         object htmlAttributes)
        {
            Check.NotNull(html, nameof(html));
            Check.NotNull(modelType, nameof(modelType));
            Check.NotNullOrEmptyString(expression, nameof(expression));

            var lambda = MakeLambda(modelType, expression);
            var method = FindMethod(typeof(ValidationExtensions),
                                    "ValidationMessageFor",
                                    4,
                                    x => x[2].ParameterType == typeof(string) && x[3].ParameterType == typeof(object),
                                    modelType,
                                    lambda.ReturnType);

            var content = (MvcHtmlString)method.Invoke(null, new object[] { html, lambda, null, htmlAttributes });
            return content;
        }


        private static LambdaExpression MakeLambda(Type modelType, string expression)
        {
            var parameter = Expression.Parameter(modelType, "model");
            var memberInfo = modelType.GetMembers().First(x => x.Name == expression);
            var memberAccess = Expression.MakeMemberAccess(parameter, memberInfo);

            var lambda = Expression.Lambda(memberAccess, parameter);
            return lambda;
        }

        private static MethodInfo FindMethod(Type classType,
                                             string methodName,
                                             int paramCount,
                                             Func<ParameterInfo[], bool> additionalChecks,
                                             Type modelType,
                                             Type lambdaReturnType)
        {
            var method = classType.GetMethods()
                .First(x => x.Name == methodName &&
                            x.GetGenericArguments().Length == 2 &&
                            x.GetParameters().Length == paramCount &&
                            additionalChecks(x.GetParameters()));

            var genericMethod = method.MakeGenericMethod(modelType, lambdaReturnType);
            return genericMethod;
        }


        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }
    }
}
