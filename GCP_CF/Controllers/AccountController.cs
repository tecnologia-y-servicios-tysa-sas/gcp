﻿using GCP_CF.Helpers;
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
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            ViewBag.SesionIniciada = User.Identity.IsAuthenticated;
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            string correoElectronico = form["correoElectronico"];
            string password = form["password"];

            try
            {
                using (GCPContext db = new GCPContext()) {

                    UserManager um = new UserManager();
                    Usuarios dbUser = um.EsValido(db, correoElectronico, password);

                    if (dbUser != null && dbUser.EsActivo)
                    {
                        UserState userState = new UserState();
                        userState.FromUser(dbUser);

                        IdentitySignIn(userState);

                        return RedirectToAction("Index", "Home");
                    }
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
            return RedirectToAction("Login", "Account");
        }

        private void IdentitySignIn(UserState userState, bool isPersistent = false)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userState.Email),
                    new Claim(ClaimTypes.Name, userState.Name),
                    new Claim("UserState", userState.ToString())
                };

            if (userState.IsSuperUser)
            {
                claims.Add(new Claim(ClaimTypes.Role, RolHelper.SUPERUSUARIO));
            }
            if (userState.IsSuperUser || userState.CanWrite)
            {
                claims.Add(new Claim(ClaimTypes.Role, RolHelper.ESCRITURA));
            }
            if (userState.IsSuperUser || !userState.CanWrite)
            {
                claims.Add(new Claim(ClaimTypes.Role, RolHelper.LECTURA));
            }
            if (userState.IsSuperUser || userState.AllContracts)
            {
                claims.Add(new Claim(ClaimTypes.Role, RolHelper.TODOS_LOS_CONTRATOS));
            }
            if (!userState.IsSuperUser && !string.IsNullOrEmpty(userState.ContractIds))
            {
                claims.Add(new Claim(RolHelper.LISTADO_CONTRATOS, userState.ContractIds));
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
                var userStateString = GetClaim(claims, "UserState");

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