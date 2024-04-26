using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Common.Helpers
{
    public static class CommonExceptions
    {
        public static NotFoundObjectResult ThrowNotFoundObjectResult(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState.AddModelError(key, errorMessage);
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Status = StatusCodes.Status404NotFound
            };

            return new NotFoundObjectResult(problemDetails);
        }

        public static BadRequestObjectResult ThrowBadRequestObjectResult(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState.AddModelError(key, errorMessage);
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };

            return new BadRequestObjectResult(problemDetails);
        }
    }
}
