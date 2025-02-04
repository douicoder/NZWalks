using System.Net;

namespace NZWalks.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //Waithing for the error <---------------------|
                await next(httpContext);                                 //|
            }                                                                   //|
            catch (Exception ex)                                           //|
            {                                                                   //|
                var errorId = Guid.NewGuid();                          //|
                                                                                //|
                // Log This Exception <-----------------------|
                // logging this error in txt file of console
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return A Custom Exrror Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                //this msg will apper if any error occurs
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                };
                //conterts the model into json to make it look preety
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
