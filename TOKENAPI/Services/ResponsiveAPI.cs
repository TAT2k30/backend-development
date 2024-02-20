using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TOKENAPI.Models;

namespace TOKENAPI.Services
{
    public class ResponsiveAPI<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public List<string> Errors { get; set; }
        public ResponsiveAPI(T data, string message, int status)
        {
            Data = data;
            Message = message;
            Status = status;
            Errors = new List<string>();
        }
        public static ActionResult BadRequest(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(x => x.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            var response = new ResponsiveAPI<T>(default, "Invalid data", 400)
            {
                Errors = errors,
            };
            return new BadRequestObjectResult(response);
        }
        public static ActionResult Exception(Exception ex)
        {
            var response = new ResponsiveAPI<T>(default, ex.Message, 500)
            {
                Errors = new List<string> { ex.ToString() }
            };
            return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
