using Newtonsoft.Json;
using social_tapX.RestModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace social_tapX
{
    public interface IWebService
    {

        List<User> GetListOfUsers(int ToNumber);

        /// <summary>
        /// Gets bars in the database. Not guaranteed to get all of them, might be limited to the first few.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetAllBars(string authToken = null);

        /// <summary>
        /// Gets <see cref="count"/> top-rated <see cref="Bar">Bars</see> based on stars given in <see cref="Comment">Comments</see>
        /// </summary>
        /// <param name="from">From which <see cref="Bar"/> to start (skips bars before that)?</param>
        /// <param name="count">How many <see cref="Bar">Bars</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetTopBars(int from, int count, string authToken = null);

        /// <summary>
        /// Gets <see cref="Bar">Bars</see> near a location.
        /// </summary>
        /// <param name="loc">Set of coordinates.</param>
        /// <param name="n">How many bars to get.</param>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n, string authToken = null);

        /// <summary>
        /// Gets <see cref="count"/> comments about a <see cref="Bar"/>.
        /// </summary>
        /// <param name="barId">Id of the <see cref="Bar"/>.</param>
        /// <param name="from">From which <see cref="Comment"/> to start (skips comments before that)?</param>
        /// <param name="count">How many <see cref="Comment">Comments</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetComments(Guid barId, int from, int count, string authToken = null);

        /// <summary>
        /// Gets <see cref="count"/> <see cref="Rating">Ratings</see> about a <see cref="Bar"/>.
        /// </summary>
        /// <param name="barId">Id of the <see cref="Bar"/>.</param>
        /// <param name="from">From which <see cref="Rating"/> to start (skips ratings before that)?</param>
        /// <param name="count">How many <see cref="Rating">Ratings</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count, string authToken = null);

        /// <summary>
        /// Uploads a new <see cref="Bar"/>.
        /// </summary>
        /// <param name="bar">The bar to upload. Should only include name and location.</param>
        /// <returns></returns>
        Task UploadBar(Bar bar, string authToken = null);

        /// <summary>
        /// Uploads a new <see cref="Comment"/>.
        /// </summary>
        /// <param name="barId">Bar to upload the <see cref="Comment"/> for.</param>
        /// <param name="comment">The <see cref="Comment"/> to upload. Should only include <see cref="Comment.Author"/> and <see cref="Comment.Content"/>.</param>
        /// <returns></returns>
        Task UploadComment(Guid barId, Comment comment, string authToken = null);

        /// <summary>
        /// Uploads a new <see cref="Rating"/>.
        /// </summary>
        /// <param name="barId">Bar to upload the <see cref="Rating"/> for.</param>
        /// <param name="rating">The <see cref="Rating"/> to upload.</param>
        /// <returns></returns>
        Task UploadRating(Guid barId, Rating rating, string authToken = null);
    }

    public class WebService : IWebService
    {
        private static HttpClient client = new HttpClient();
        private static string serviceUrl = "http://socialtapx.azurewebsites.net/api";

        // FB
        public List<User> GetListOfUsers(int toNumber)
        {
            List<User> ListOfUsers = new List<User>
            {
                new User("1805613166138493", "Arūnas", true),
                new User("12313231231313123", "Arūnas", false),
                new User("123123123123", "Arūnas", false),
                new User("148415886123134950593", "Arūnas", false),
                new User("148415881231364950593", "Arūnas", false),
                new User("1484158123864950593", "Arūnas", false),
                new User("148415138864950593", "Arūnas", false),
                new User("148411358864950593", "Arūnas", false),
                new User("1484112358864950593", "Arūnas", false),
                new User("1484112358864950593", "Arūnas", false),
            };

            return ListOfUsers;
        }
        // END OF FB
        public async Task<IEnumerable<Bar>> GetAllBars(string authToken = null)
        {
            return await GetList<Bar>("/Bar", authToken);
        }

        public async Task<IEnumerable<Bar>> GetTopBars(int from, int count, string authToken = null)
        {
            return await GetList<Bar>(String.Format("/GetTopBars/{0}/{1}", from, count), authToken);
        }

        public async Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n, string authToken = null)
        {
            return await GetList<Bar>(String.Format("/GetBars/{0};{1}/{2}", loc.Latitude, loc.Longitude, n), authToken);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid barId, int from, int count, string authToken = null)
        {
            return await GetList<Comment>(string.Format("/Comment/{0}/{1}/{2}", barId, from, count), authToken);
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count, string authToken = null)
        {
            return await GetList<Rating>(string.Format("/Rating/{0}/{1}/{2}", barId, from, count), authToken);
        }

        public async Task UploadBar(Bar bar, string authToken = null)
        {
            await Upload(bar, "/Bar", authToken);
        }

        public async Task UploadComment(Guid barId, Comment comment, string authToken = null)
        {
            await Upload(comment, "/Comment/" + barId, authToken);
        }

        public async Task UploadRating(Guid barId, Rating rating, string authToken = null)
        {
            await Upload(rating, "/Rating/" + barId, authToken);
        }

        private async Task Upload(object obj, string path, string authToken = null)
        {
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, serviceUrl + path);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Add("authToken", authToken);
            //using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                //using (var response = await client.PostAsync(serviceUrl + path, content))
                using (var response = await client.SendAsync(request)) 
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        private async Task<IEnumerable<T>> GetList<T>(string path, string authToken = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + path);
            request.Headers.Add("authToken", authToken);
            //using (var response = await client.GetAsync(serviceUrl + path))
            using (var response = await client.SendAsync(request)) 
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.StatusCode.ToString());
                var json = await response.Content.ReadAsStringAsync();
                
                var res = JsonConvert.DeserializeObject<List<T>>(json);
                return res;
            }
        }
    }
    
}
