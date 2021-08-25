using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace URLShortenerAPI.Filters
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {


            //TODO - Authentication
            //if(actionContext.Request != null)
            //{
            //    var action = actionContext.ActionDescriptor.ActionName;
            //    var controller = actionContext.ActionDescriptor.ControllerDescriptor;
            //    var token = HttpContext.Current.Request.Headers("SecurityToken")
            //
            //    Call some JWTAuthentication or something to verify access to the application using Action, Controller and token.
            //    If something is wrong
            //          return HttpResponse with message "Access Denied" or something
            //
            //       
            //}

            base.OnActionExecuting(actionContext);
        }
    }
}