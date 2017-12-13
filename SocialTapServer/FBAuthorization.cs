using Newtonsoft.Json;
using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SocialTapServer
{
    public enum Role { Anonymous, User, Admin };
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class FBAuthorization : AuthorizationFilterAttribute
    {
        private Role[] allowedRoles;
        private string AppToken = "246497305885398|8g4SvEod-Bgb8-l2jHhB8TOF2L0";
        public FBAuthorization(params Role[] roles)
        {
            allowedRoles = roles;
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            try
            {
                string authToken = filterContext.Request.Headers.Where(x => x.Key == "authToken").FirstOrDefault().Value.FirstOrDefault();// <-- tokenas
                var logged = Task.Run(async () => await ValidateUser(authToken)).Result;
                Task.WaitAll();
                if (logged.Item1)/*validuojasi su fb*/
                {
                    if (allowedRoles.Contains(Role.User)) return; //praleidziam
                    if (allowedRoles.Contains(Role.Admin) && logged.Item2) return; //praleidziam
                }
            }
            catch { }
            
                if (allowedRoles.Contains(Role.Anonymous)) return; //praleidziam
                                                                   //jei dar nepraleidom, reiskia kazkas netaip:
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);// <-- meti toki, jei tokenas neteisingas arba neatitinka role
                                                                                        //allowedRoles <-- is cia gauni roles, kurios turi access
                                                                                        //[...]

        }

        public async Task<Tuple<bool, bool>> ValidateUser(string authToken)
        {
            var requestUrl =
                "https://graph.facebook.com/debug_token?input_token="
                + authToken
                + "&access_token="
                + AppToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            dynamic obj = JsonConvert.DeserializeObject(userJson);
            bool isValid = obj.data.is_valid == "True" ? true : false;
            return new Tuple<bool, bool>(isValid, true);
        }
        //Sita funkcija gauna info apie vartotoja.
        public async Task<User> GetFacebookIdAndNameAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=first_name&access_token="
                + accessToken;

            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            dynamic obj = JsonConvert.DeserializeObject(userJson);
            return new User(obj.id, obj.name);
        }
    }
}