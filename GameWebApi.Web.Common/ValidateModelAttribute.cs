using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GameWebApi.Web.Common
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// - This attribute checks if the model that was sent to Web API has all the required parameters
        /// - Check NewPlayer model that has some required properties defined
        /// - Check PlayersContoller Create -action where this is being used
        /// - http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}