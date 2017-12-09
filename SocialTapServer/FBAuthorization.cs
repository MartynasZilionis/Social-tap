using Newtonsoft.Json;
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

        public async Task ValidateUser(string authToken)
        {
            var requestUrl =
                "https://graph.facebook.com/debug_token?input_token="
                + authToken
                + "&access_token="
                + AppToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            dynamic obj = JsonConvert.DeserializeObject(userJson);
            string isValid = obj.data.is_valid;
            if (isValid == "true")
            {
                await GetFacebookProfileAsync(authToken);
            }
        }
        public async Task<Role> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=first_name&access_token="
                + accessToken;

            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            dynamic obj = JsonConvert.DeserializeObject(userJson);
            string Id = obj.id;
            string firstName = obj.first_name;
            var user = true; // <- Gets user from DB
            if (user == false)
            {
                return Role.Anonymous;
            }
            else if (user == true)
            {
                return Role.Admin;
            }
            else return Role.Anonymous;
        }
    }
}