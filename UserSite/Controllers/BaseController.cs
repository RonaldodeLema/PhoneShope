using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Internals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace UserSite.Controllers;

public class BaseController : Controller
{
    public void CreateAuthenticationTicket(User user)
    {
        var key = Encoding.ASCII.GetBytes(SiteKeys.Token);
        var jwToken = new JwtSecurityToken(
            issuer: SiteKeys.WebSiteDomain,
            audience: SiteKeys.WebSiteDomain,
            claims: GetUserClaims(user),
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwToken);
        HttpContext.Session.SetString("JWToken", token);
    }

    private static IEnumerable<Claim> GetUserClaims(User user)
    {
        var claims = new List<Claim>();
        var claim = new Claim(ClaimTypes.Name, user.Username);
        claims.Add(claim);
        claim = new Claim("Role", "user");
        claims.Add(claim);
        return claims.AsEnumerable();
    }
}