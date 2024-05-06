using Application.DTOs;
using Common.Exceptions;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentResults;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Text;
using ClosedXML.Excel;

namespace CustomSoftWebApi.Extensions
{
    public static class ControllerExtensions
    {
        public static ObjectResult ToErrorResponse<T>(this Result<T> result)
        {
            if(result.HasError<DatabaseError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "Database.error",
                    Title = "Dabase error related",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = result.Errors.First().Message
                };

                return new ObjectResult(problemDetails);
            }

            if (result.HasError<NotFoundError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "NotFound.error",
                    Title = "NotFound.error",
                    Status = StatusCodes.Status404NotFound,
                    Detail = result.Errors.First().Message
                };

                return new NotFoundObjectResult(problemDetails);
            }

            if (result.HasError<BadRequestError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "BadRequest.error",
                    Title = "BadRequest.error",
                    Status = StatusCodes.Status404NotFound,
                    Detail = result.Errors.First().Message
                };

                return new NotFoundObjectResult(problemDetails);
            }

            var generalProblemDetails = new ProblemDetails()
            {
                Type = "Dabase Error type",
                Title = "Dabase error related",
                Status = StatusCodes.Status500InternalServerError,
                Detail = result.Errors.First().Message
            };

            return new ObjectResult(generalProblemDetails);

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

        public static NotFoundObjectResult ThrowNotFoundObjectResult(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState.AddModelError(key, errorMessage);
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Status = StatusCodes.Status404NotFound
            };

            return new NotFoundObjectResult(problemDetails);
        }

        public static PagedProveedorDTO ToPagedResult(this List<ProveedorDto> proveedors, int pageNumber, int pageSize, int totalCount)
        {
            var response = new PagedProveedorDTO()
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = proveedors,
            };

            return response;
        }

        public static HttpResponseMessage ToMessageErrorResponse<T>(this Result<T> result)
        {
            var responseMessage = new HttpResponseMessage();
            if (result.HasError<DatabaseError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "Database.error",
                    Title = "Dabase error related",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = result.Errors.First().Message
                };

                responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responseMessage.Content = new StringContent(JsonSerializer.Serialize(problemDetails), Encoding.UTF8, "application/problem+json");

                return responseMessage;
            }

            if (result.HasError<NotFoundError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "NotFound.error",
                    Title = "NotFound.error",
                    Status = StatusCodes.Status404NotFound,
                    Detail = result.Errors.First().Message
                };

                responseMessage.StatusCode = System.Net.HttpStatusCode.NotFound;
                responseMessage.Content = new StringContent(JsonSerializer.Serialize(problemDetails), Encoding.UTF8, "application/problem+json");

                return responseMessage;
            }

            if (result.HasError<BadRequestError>())
            {
                var problemDetails = new ProblemDetails()
                {
                    Type = "BadRequest.error",
                    Title = "BadRequest.error",
                    Status = StatusCodes.Status404NotFound,
                    Detail = result.Errors.First().Message
                };

                responseMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;
                responseMessage.Content = new StringContent(JsonSerializer.Serialize(problemDetails), Encoding.UTF8, "application/problem+json");

                return responseMessage;
            }

            var generalProblemDetails = new ProblemDetails()
            {
                Type = "Dabase Error type",
                Title = "Dabase error related",
                Status = StatusCodes.Status500InternalServerError,
                Detail = result.Errors.First().Message
            };

            responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            responseMessage.Content = new StringContent(JsonSerializer.Serialize(generalProblemDetails), Encoding.UTF8, "application/problem+json");

            return responseMessage;
        }

        public static Stream ToStream(this Result<XLWorkbook> result)
        {
            var memoryStream = new MemoryStream();
            result.Value.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}
