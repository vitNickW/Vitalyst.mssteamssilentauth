using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using RestSharp;
using System.Configuration;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Vitalyst.AdaptiveLearning.MVC.Helpers
{
    internal class JwtHelper : Controller /* InheritOnly here to handle requests and session references */
    {
        //private readonly ActionLogRepository _log = new ActionLogRepository("JwtHelper", "Claims");

        internal string ProcessJwt(string idToken,
            HttpRequestBase httpRequestBase,
            HttpResponseBase httpResponseBase,
            HttpSessionStateBase httpSessionStateBase,
            HttpContextBase httpContextBase)
        {
            RestResponse response = GetAccessToken(idToken);

            string responseBody = response.Content;

            if (response.IsSuccessful == false)
                throw new Exception(responseBody);

            if (responseBody == null)
                throw new Exception("ResponseBody could not be found.");

            var jcdsBody = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (jcdsBody == null)
                throw new Exception("jcdsBody could not be found.");

            string accessToken = jcdsBody.access_token;

            if (accessToken == null)
                throw new Exception("accessToken could not be found.");

            //Debug.WriteLine(HttpContext);

            CheckBuildCookie(".AspNet.Cookies", accessToken, httpRequestBase, httpResponseBase);
            BuildClaim(accessToken, httpSessionStateBase, httpContextBase);

            return accessToken;
        }

        internal RestResponse GetAccessToken(string idToken)
        {
            //_log.AddActionLog("msteamsSilent", "GetRestResponse", "Started", 91);
            var body = $"assertion={idToken}" +
                $"&requested_token_use=on_behalf_of" +
                $"&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer" +
                $"&client_id={ConfigurationManager.AppSettings["ida:ClientId"]}" + //* Removed tenantId in order to work in multi-tenant environment.
                $"&client_secret={ConfigurationManager.AppSettings["ida:ClientSecret"]}" +
                $"&scope=User.Read";

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddBody(body, "application/x-www-form-urlencoded");

            var client = new RestClient("https://login.microsoftonline.com/common/oauth2/v2.0/token");//* Changed tenantId to "common" for multi-tenant.
            RestResponse response = client.Execute(request);
            return response;
        }

        internal void BuildClaim(string accessToken, HttpSessionStateBase httpSessionStateBase, HttpContextBase httpContextBase)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var token = jsonToken as JwtSecurityToken;
            var x = new JwtSecurityTokenHandler().WriteToken(token);

            // Create a session variable to get a session cookie created.
            httpSessionStateBase["sample"] = "sample";

            var claims = token.Payload.Select(p => new Claim(MapClaimType(p.Key), p.Value.ToString())).ToList();
            claims.Add(new Claim("IsInTeams", "true"));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            var ctx = httpContextBase.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
        }

        internal string MapClaimType(string typeName)
        {
            string result;
            switch (typeName)
            {
                case "unique_name":
                    result = ClaimTypes.NameIdentifier;
                    break;
                case "oid":
                    result = "http://schemas.microsoft.com/identity/claims/objectidentifier";
                    break;
                case "name":
                    result = ClaimTypes.Name;
                    break;
                case "given_name":
                    result = ClaimTypes.GivenName;
                    break;
                case "family_name":
                    result = ClaimTypes.Surname;
                    break;
                case "tid":
                    result = "http://schemas.microsoft.com/identity/claims/tenantid";
                    break;
                case "upn":
                    result = ClaimTypes.Upn;
                    break;
                default:
                    result = typeName;
                    break;
            }
            return result;
        }

        public void CheckBuildCookie(string cookieName, string token, HttpRequestBase httpRequestBase, HttpResponseBase httpResponseBase)
        {
            if (httpRequestBase.Cookies[cookieName] == null)
            {
                // Create the cookie
                System.Web.HttpCookie sameSiteCookie = new System.Web.HttpCookie(cookieName);

                // Set a value for the cookie
                //sameSiteCookie.Value = hdnIdToken;                     
                sameSiteCookie.Value = token;

                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS
                sameSiteCookie.Secure = true;

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                sameSiteCookie.HttpOnly = true;

                // Add the SameSite attribute
                sameSiteCookie.SameSite = SameSiteMode.None;

                // Add the cookie to the response cookie collection
                httpResponseBase.Cookies.Add(sameSiteCookie);
            }
        }
    }
}