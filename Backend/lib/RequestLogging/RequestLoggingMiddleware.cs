using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RequestLogging
{
    public class RequestLoggingMiddleware
    {
        //private readonly RequestDelegate _next;
        //private readonly ILogger _logger;

        //public RequestLoggingMiddleware(RequestDelegate next,
        //                                        ILogger<RequestLoggingMiddleware> logger)
        //{
        //    _next = next;
        //    _logger = logger;
        //}

        //public async Task Invoke(HttpContext context)
        //{
        //    _logger.LogInformation("RequestLoggingMiddleware : Invoke");
        //await _next(context);

        ////_logger.LogInformation(await FormatRequest(context.Request));

        ////var originalBodyStream = context.Response.Body;

        ////using (var responseBody = new MemoryStream())
        ////{
        ////    context.Response.Body = responseBody;

        ////    await _next(context);

        ////    _logger.LogInformation(await FormatResponse(context.Response));
        ////    await responseBody.CopyToAsync(originalBodyStream);
        ////}
        //}

        //private async Task<string> FormatRequest(HttpRequest request)
        //{
        //    var body = request.Body;
        //    request.EnableRewind();

        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
        //    var bodyAsText = Encoding.UTF8.GetString(buffer);
        //    request.Body = body;

        //    return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        //}

        //private async Task<string> FormatResponse(HttpResponse response)
        //{
        //    response.Body.Seek(0, SeekOrigin.Begin);
        //    var text = await new StreamReader(response.Body).ReadToEndAsync();
        //    response.Body.Seek(0, SeekOrigin.Begin);

        //    return $"Response {text}";
        //}

        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation("InvokeAsync");
            //context.Session.SetString("RefId", Guid.NewGuid().ToString());
            await LogResponse(context);

        }

        private async Task LogResponse(HttpContext context)
        {
            var originalRequestBodyStream = context.Request.Body;
            var originalBodyStream = context.Response.Body;
            var request = context.Request;
            var requestBody = "";
            request.EnableBuffering();

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);
                logger.LogInformation(await FormatResponse(context, requestBody));
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequest(HttpContext context)
        {

            //var request = context.Request;
            //var requestBody = "";
            //request.EnableBuffering();

            //using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            //{
            //    requestBody = await reader.ReadToEndAsync();
            //    request.Body.Position = 0;
            //}

            //var builder = new StringBuilder(Environment.NewLine);
            //var refId = context.Session.GetString("RefId");
            //builder.AppendLine($"Ref id : {refId}");
            //builder.AppendLine($"Request URL : {request.Scheme}://{request.Host.Host}{request.Host.Port.GetValueOrDefault(80)}{request.Path}{request.QueryString.ToString()}");
            //builder.AppendLine($"Request Method: {request.Method}");
            //builder.AppendLine("Request header");
            //foreach (var header in request.Headers)
            //    builder.AppendLine($"  {header.Key}:{header.Value}");

            //builder.AppendLine($"Request body:{requestBody}");
            //logger.LogInformation(builder.ToString());

        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            var request = context.Request;
            var requestBody = "";
            request.EnableBuffering();
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            var builder = new StringBuilder(Environment.NewLine);
            //var refId = context.Session.GetString("RefId");
            var refId = context.Session.Id;

            string url = request.Host.Port != null ? $"{request.Scheme}://{request.Host.Host}:{request.Host.Port}{request.Path}{request.QueryString.ToString()}"
               : $"{request.Scheme}://{request.Host.Host}{request.Path}{request.QueryString.ToString()}";


            builder.AppendLine($"Ref id : {refId}");
            builder.AppendLine($"Request URL : {url}");
            builder.AppendLine($"Request Method: {request.Method}");
            builder.AppendLine("Request header");
            foreach (var header in request.Headers)
                builder.AppendLine($"  {header.Key}:{header.Value}");

            builder.AppendLine($"Request body:{requestBody}");
            return builder.ToString();
        }


        private async Task<string> FormatResponse(HttpContext context, string requesTBody)
        {
            var request = context.Request;
            

            var builder = new StringBuilder(Environment.NewLine);
            string url = request.Host.Port != null ? $"{request.Scheme}://{request.Host.Host}:{request.Host.Port}{request.Path}{request.QueryString.ToString()}"
                : $"{request.Scheme}://{request.Host.Host}{request.Path}{request.QueryString.ToString()}";
            

            builder.AppendLine($"Request URL : {url}");
            builder.AppendLine($"Request Method: {request.Method}");
            builder.AppendLine("Request Header");
            foreach (var header in request.Headers)
                builder.AppendLine($"  {header.Key}:{header.Value}");

            builder.AppendLine($"Request body:{requesTBody}");

            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            //builder.AppendLine($"Ref id : {refId}");
            builder.AppendLine($"Response Status: {response.StatusCode} {(HttpStatusCode)response.StatusCode}");
            builder.AppendLine($"Response header");
            foreach (var header in response.Headers)
                builder.AppendLine($"  {header.Key}:{header.Value}");
            builder.AppendLine($"Response body:{body}");
            return builder.ToString();
        }
    }
}
