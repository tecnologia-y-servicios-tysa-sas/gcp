using GCP_CF.Helpers;
using GCP_CF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GCP_CF.Controllers
{
    public class LoginController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            string usuario = form["usuario"];
            string password = form["password"];

            try
            {
                UserManager um = new UserManager();
                Usuarios dbUser = um.EsValido(db, usuario, password);

                if (dbUser != null)
                {
                    UserState userState = new UserState()
                    {
                        UserId = dbUser.Usuario,
                        Name = dbUser.NombreCompleto,
                        Email = dbUser.CorreoElectronico,
                        IsSuperUser = RolHelper.UsuarioTieneRol(dbUser.IdRoles, 0),
                        RoleList = RolHelper.ObtenerRolesUsuario(dbUser.IdRoles).Select(s => s.Value).ToArray<string>()
                    };

                    IdentitySignIn(userState);

                    return RedirectToAction("Index", "Home");
                }

                // invalid username or password
                ModelState.AddModelError("", "Usuario o contraseña no válidos.");
                return View();
            }
            catch (Exception ex)
            {
                // invalid username or password
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Logout()
        {
            IdentitySignOut();
            return RedirectToAction("Index", "Login");
        }

        private void IdentitySignIn(UserState userState, bool isPersistent = false)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userState.UserId),
                        new Claim(ClaimTypes.Name, userState.Name),
                        new Claim("userstate", userState.ToString())
                    };

            if (userState.RoleList.Any())
            {
                var roleClaims = userState.RoleList.Select(r => new Claim(ClaimTypes.Role, r));
                claims.AddRange(roleClaims);
            }

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            }, identity);


            Session["UserState"] = userState;
        }

        private void IdentitySignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            Session["UserState"] = null;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            UserState userState = new UserState();
            if (User is ClaimsPrincipal)
            {
                var user = User as ClaimsPrincipal;
                var claims = user.Claims.ToList();
                var userStateString = GetClaim(claims, "userState");

                if (!string.IsNullOrEmpty(userStateString))
                {
                    userState.FromString(userStateString);
                    Session["UserState"] = userState;
                }
            }
        }

        private string GetClaim(List<Claim> claims, string item)
        {
            Claim claim = claims.Where(c => c.Type == item).FirstOrDefault();
            return claim != null ? claim.Value : string.Empty;
        }
    }
}