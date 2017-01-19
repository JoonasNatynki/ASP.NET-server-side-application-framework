using System;
using System.Net;
using System.Web.Http.ExceptionHandling;

namespace GameWebApi.Web.Common
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            //Checks if the exception is of type NotImplementedException
            if (context.Exception is NotImplementedException)
            {
                //Returns a customized result for the client (we don't want that client sees the whole stacktrace)
                context.Result = new SimpleErrorResult(context.Request, HttpStatusCode.NotImplemented, "Action is not implemented");
            }
        }
    }
}