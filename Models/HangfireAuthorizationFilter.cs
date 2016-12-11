using System.Collections.Generic;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AnalyseVisitorsTool.Models
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private SignInManager<ApplicationUser> _signinmanager;
        private IHttpContextAccessor _httpcontext;

        public HangfireAuthorizationFilter(SignInManager<ApplicationUser> signinmanager, IHttpContextAccessor httpcontext)
        {
            this._signinmanager = signinmanager;
            this._httpcontext = httpcontext;
        }

        public bool Authorize(DashboardContext context)
        {
            return this._signinmanager.IsSignedIn(this._httpcontext.HttpContext.User);
        }
    }
}
