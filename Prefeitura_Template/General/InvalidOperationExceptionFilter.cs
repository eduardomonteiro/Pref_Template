using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Prefeitura_Template.General
{
    public class InvalidOperationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode statusCode;
            if (context.Exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            context.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(context.Exception.Message)
            };
        }
    }
}