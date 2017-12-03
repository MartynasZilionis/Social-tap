using Newtonsoft.Json;
using social_tapX.RestModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace social_tapX
{
    public interface IWebService
    {
        Task<IEnumerable<Bar>> GetAllBars();
        Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n);
        Task<IEnumerable<Comment>> GetComments(Guid barId, int from, int count);
        Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count);
        Task UploadBar(Bar bar);
        Task UploadComment(Guid barId, Comment comment);
        Task UploadRating(Guid barId, Rating rating);
    }

    public class WebService : IWebService
    {
        private static HttpClient client = new HttpClient();
        private static string serviceUrl = "http://socialtapx.azurewebsites.net/api";

        /// <summary>
        /// Gets all the <see cref="Bar">Bars</see>. (WIP)
        /// </summary>
        public async Task<IEnumerable<Bar>> GetAllBars()
        {
            return await GetList<Bar>("/Bar");
        }

        /// <summary>
        /// Gets <see cref="Bar">Bars</see> near a location.
        /// </summary>
        /// <param name="loc">Set of coordinates.</param>
        /// <param name="n">How many bars to get.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n)
        {
            return await GetList<Bar>(String.Format("/GetBars/{0};{1}/{2}", loc.Latitude, loc.Longitude, n));
        }

        /// <summary>
        /// Gets <see cref="count"/> comments about a <see cref="Bar"/>.
        /// </summary>
        /// <param name="barId">Id of the <see cref="Bar"/>.</param>
        /// <param name="from">From which <see cref="Comment"/> to start (skips comments before that)?</param>
        /// <param name="count">How many <see cref="Comment">Comments</see> to get?</param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetComments(Guid barId, int from, int count)
        {
            return await GetList<Comment>(string.Format("/Comment/{0}/{1}/{2}", barId, from, count));
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count)
        {
            return await GetList<Rating>(string.Format("/Rating/{0}/{1}/{2}", barId, from, count));
        }

        public async Task UploadBar(Bar bar)
        {
            await Upload(bar, "/Bar");
        }

        public async Task UploadComment(Guid barId, Comment comment)
        {
            await Upload(comment, "/Comment/" + barId);
        }

        public async Task UploadRating(Guid barId, Rating rating)
        {
            await Upload(rating, "/Rating/" + barId);
        }

        private async Task Upload(object obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj);
            using (var content = new StringContent(json))
            {
                using (var response = await client.PostAsync(serviceUrl + path, content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        private async Task<IEnumerable<T>> GetList<T>(string path)
        {
            
            using (var response = await client.GetAsync(serviceUrl + path))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.StatusCode.ToString());
                return JsonConvert.DeserializeObject<List<T>>(await response.Content.ReadAsStringAsync());
            }
            
        }
    }
    
}