using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading;
using System.Security.Claims;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;

namespace Vitalyst.AdaptiveLearning.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserContextProvider _userContextProvider = new UserContextProvider();
        
        public ActionResult msteamsSilent()
        {           
            LoadViewBags();
            return PartialView();
        }

        [HttpGet]
        public ActionResult SilentPost(string idToken)
        {
            //JwtHelper jwt = new JwtHelper();
            try
            {
                //string accessToken = jwt.ProcessJwt(idToken, Request, Response, Session, HttpContext);

                //return Json(new { accessToken = accessToken }, JsonRequestBehavior.AllowGet);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} Exception: {ex.Message}");
                //_log.AddActionLog("SilentPost", "Exception Posting", ex.Message, 11);

                return Json(new { errorMessage = "Error. " + ex.Message });
            }
        }

        // GET: Auth
        public ActionResult msteamsSilentStart()
        {           
            LoadViewBags();
            return PartialView();
        }
        public ActionResult msteamsSilentEnd()
        {
            LoadViewBags();
            return PartialView();
        }

        [HttpPost]
        public void Log(string sourceAction)
        {            
        }

        [HttpGet]
        public bool IsUserAuthenticated()
        {
            //return _userContextProvider.GetUserContext(Thread.CurrentPrincipal as ClaimsPrincipal).TenantId != null;
            return false;
        }

        private void LoadViewBags()
        {
            ViewBag.ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        }
    }
}