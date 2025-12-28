using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Live.MagicAuth.SignOut;

public static class SignOutRoutes
{
    public static void MapSignOutRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/logout", async (HttpContext context) =>
        {
            context.Session.Clear();

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Results.Json(new { status = "ok", message = "Logged out successfully" });
        });
    }
}
