using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace AlatAssessment.Helpers
{
    public class LgaValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var lga=context.Lgas.FirstOrDefault(x=>x.Id== (int)value);

            if (lga==null)
                return new ValidationResult("lgaId is not valid.");

            var stateId = validationContext.ObjectType.GetProperty(nameof(AddCustomerDTO.StateId));
            if (stateId == null) return new ValidationResult("stateId is invalid.");

            var val = stateId.GetValue(validationContext.ObjectInstance);
            if(val==null) return new ValidationResult("stateId is invalid.");

            var stateIdValue= (int)val;
            return lga.StateId == stateIdValue? ValidationResult.Success : new ValidationResult("stateId does not match lgaId.");
        }
    }

    public class ModelStateValidator : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                await Task.FromResult(false);
                return;
            }
            await next();
        }
    }
}
