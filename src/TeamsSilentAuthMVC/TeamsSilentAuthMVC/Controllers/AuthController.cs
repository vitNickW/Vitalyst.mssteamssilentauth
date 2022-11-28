using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;
using Vitalyst.AdaptiveLearning.MVC.Helpers;

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
            JwtHelper jwt = new JwtHelper();
            try
            {
                string accessToken = jwt.ProcessJwt(idToken, Request, Response, Session, HttpContext);

                return Json(new { accessToken = accessToken }, JsonRequestBehavior.AllowGet);
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