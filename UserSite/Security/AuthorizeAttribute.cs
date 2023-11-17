using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserSite.Security;

public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
    {
        Arguments = new object[] { claim };
    }
}

public class AuthorizeFilter : IAuthorizationFilter
{
    readonly string[] _claim;

    public AuthorizeFilter(params string[] claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)  
    {  
        var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
        _ = context.HttpContext.User.Identity as ClaimsIdentity;
        var isAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        if (isAuthenticated)
        {  
            bool flagClaim = false;  
            foreach (var item in _claim)  
            {  
                if (context.HttpContext.User.HasClaim("Role", item))  
                    flagClaim = true;  
            }  
            if (!flagClaim)  
            {  
                if (isAjaxRequest)  
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; //Set HTTP 401   
                else  
                    context.Result = new RedirectResult("~/AuthPage/Index");  
            }  
        }  
        else  
        {  
            if (isAjaxRequest)  
            {  
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; //Set HTTP 403 -   
            }  
            else  
            {  
                context.Result = new RedirectResult("~/AuthPage/Index");  
            }  
        }
    }  
}