using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        //private readonly UserManager<AppUser> userManager;

        //public LogUserActivity(UserManager<AppUser> userManager)
        //{
        //    this.userManager = userManager;
        //}

        /* Filter Vs MiddleWare
         * The Filters are a part of MVC, so they are scoped entirely 
        to the MVC middleware. Middleware only has access to the HttpContext */
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            //var aut = this.userManager.Users;

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            var userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}
