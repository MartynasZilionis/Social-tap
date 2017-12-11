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
        public FacebookServices()
        {

        }
        public async static Task<int> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=first_name&access_token="
                + accessToken;

            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            dynamic obj = JsonConvert.DeserializeObject(userJson);
            string Id = obj.id;
            string firstName = obj.first_name;
            return 0;//await App.WebSvc.GetRole(accessToken); 
        }
    }
    
}
