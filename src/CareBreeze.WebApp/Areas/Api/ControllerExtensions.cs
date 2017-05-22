using CareBreeze.WebApp.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace CareBreeze.WebApp.Areas.Api
{
    public static class ControllerExtensions
    {
        public static IActionResult CreatedAtArea(this ControllerBase controller, string action, object id, BaseResult result)
        {
            return controller.CreatedAtAction(action, new { area = "Api", id = id }, result);
        }

        public static IEnumerable<Error> Errors(this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    yield return new Error
                    {
                        Message = error.ErrorMessage,
                        Parameter = key
                    };
                }
            }
        }
    }
}
