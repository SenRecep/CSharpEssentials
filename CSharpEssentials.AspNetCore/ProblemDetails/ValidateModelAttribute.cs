using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CSharpEssentials.AspNetCore;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var errors= context.ModelState
            .Where(arg => arg.Value != null)
            .SelectMany(state=> state.Value!.Errors.Select(x=>Error.Validation($"validation.{state.Key}", x.ErrorMessage)))
            .ToArray();
        context.Result = errors.ToActionResult(context.HttpContext);
    }
}