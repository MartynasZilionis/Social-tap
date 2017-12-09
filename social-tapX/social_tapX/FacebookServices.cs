using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace social_tapX
{
    public class FacebookServices
    {
        public async Task<RestModels.User> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages&access_token="
                + accessToken;

            var httpClient = new HttpClient();
                var userJson = await httpClient.GetStringAsync(requestUrl);
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
            //cia pagal ideja turetu vykti tikrinimas su DB ar yra toks vartotojas
            string id = facebookProfile.Id;
            if (id != null)
            {
                RestModels.User user = App.WebSvc.GetListOfUsers(10).Find(e => e.Id == id);
                if (user != null)
                {
                    return user;
                }
                else return new RestModels.User();
            }
            else return null;
            //tikrinimo pabaiga
            //return facebookProfile;
            
        }
    }
}
