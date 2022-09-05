using System;
using System.Net;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CinemaBookingSystem.Api.Extensions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        #region CustomExceptionMiddleware()

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        #endregion

        #region Invoke()

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
        }

        #endregion

        #region HandleExceptionAsync()

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            string result = null;
            context.Response.ContentType = "application/json";
            if (exception is HttpStatusCodeException)
            {
                result = new ErrorDetails()
                {
                    Message = exception.Message,
                    StatusCode = (int)exception.StatusCode
                }.ToString();
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            else
            {
                result = new ErrorDetails()
                {
                    Message = "Runtime Error",
                    StatusCode = (int)HttpStatusCode.BadRequest
                }.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return context.Response.WriteAsync(result);
        }

        #endregion

        #region HandleExceptionAsync()

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string result = new ErrorDetails()
            {
                Message = exception.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError
            }.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(result);
        }

        #endregion 
    }
}
