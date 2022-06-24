using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendForFrontend.Controllers
{
    [ApiController]
    [Route("bff/[controller]/[action]")]
    public class AuthController : Controller
    {
        [HttpGet]
        [SwaggerOperation("Авторизовать пользователя")]
        public ActionResult Login(string returnUrl = "/")
        {
            return new ChallengeResult(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return new SignOutResult(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
        }

        [HttpGet]
        [SwaggerOperation("Получить claims пользователя")]
        public ActionResult Claims()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claims = ((ClaimsIdentity)this.User.Identity).Claims.Select(c =>
                    new { type = c.Type, value = c.Value })
                    .ToArray();

                return Json(new { isAuthenticated = true, claims = claims });
            }

            return Json(new { isAuthenticated = false });
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation("Получить Bearer токен пользователя (использовать только в целях тестирования!)")]
        public string Token()
        {
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return HttpContext.Request.Headers["Authorization"].ToString();
            }
            return "NO TOKEN";
        }
    }
}
