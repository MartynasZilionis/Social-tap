using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
namespace social_tapX
{
    public class FacebookServices
    {
        private string AppToken = "246497305885398|8g4SvEod-Bgb8-l2jHhB8TOF2L0";
        public enum Role { Anonymous, User, Admin };
        public async Task<Role> ValidateUser(string authToken)
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
            if (isValid == "True")
            {
                return await GetFacebookProfileAsync(authToken);
            }
            else return Role.Anonymous;
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
            return Role.User; 

        }
    }
}
