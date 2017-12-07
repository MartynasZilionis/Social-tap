using Newtonsoft.Json;
using social_tapX.RestModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace social_tapX
{
    public interface IWebService
    {
        /// <summary>
        /// Gets bars in the database. Not guaranteed to get all of them, might be limited to the first few.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetAllBars();

        /// <summary>
        /// Gets <see cref="count"/> top-rated <see cref="Bar">Bars</see> based on stars given in <see cref="Comment">Comments</see>
        /// </summary>
        /// <param name="from">From which <see cref="Bar"/> to start (skips bars before that)?</param>
        /// <param name="count">How many <see cref="Bar">Bars</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetTopBars(int from, int count);

        /// <summary>
        /// Gets <see cref="Bar">Bars</see> near a location.
        /// </summary>
        /// <param name="loc">Set of coordinates.</param>
        /// <param name="n">How many bars to get.</param>
        /// <returns></returns>
        Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n);

        /// <summary>
        /// Gets <see cref="count"/> comments about a <see cref="Bar"/>.
        /// </summary>
        /// <param name="barId">Id of the <see cref="Bar"/>.</param>
        /// <param name="from">From which <see cref="Comment"/> to start (skips comments before that)?</param>
        /// <param name="count">How many <see cref="Comment">Comments</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetComments(Guid barId, int from, int count);

        /// <summary>
        /// Gets <see cref="count"/> <see cref="Rating">Ratings</see> about a <see cref="Bar"/>.
        /// </summary>
        /// <param name="barId">Id of the <see cref="Bar"/>.</param>
        /// <param name="from">From which <see cref="Rating"/> to start (skips ratings before that)?</param>
        /// <param name="count">How many <see cref="Rating">Ratings</see> to get?</param>
        /// <returns></returns>
        Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count);

        /// <summary>
        /// Uploads a new <see cref="Bar"/>.
        /// </summary>
        /// <param name="bar">The bar to upload. Should only include name and location.</param>
        /// <returns></returns>
        Task UploadBar(Bar bar);

        /// <summary>
        /// Uploads a new <see cref="Comment"/>.
        /// </summary>
        /// <param name="barId">Bar to upload the <see cref="Comment"/> for.</param>
        /// <param name="comment">The <see cref="Comment"/> to upload. Should only include <see cref="Comment.Author"/> and <see cref="Comment.Content"/>.</param>
        /// <returns></returns>
        Task UploadComment(Guid barId, Comment comment);

        /// <summary>
        /// Uploads a new <see cref="Rating"/>.
        /// </summary>
        /// <param name="barId">Bar to upload the <see cref="Rating"/> for.</param>
        /// <param name="rating">The <see cref="Rating"/> to upload.</param>
        /// <returns></returns>
        Task UploadRating(Guid barId, Rating rating);
    }

    public class WebService : IWebService
    {
        private static HttpClient client = new HttpClient();
        private static string serviceUrl = "http://socialtapx.azurewebsites.net/api";
        public async Task<IEnumerable<Bar>> GetAllBars()
        {
            return await GetList<Bar>("/Bar");
        }

        public async Task<IEnumerable<Bar>> GetTopBars(int from, int count)
        {
            return await GetList<Bar>(String.Format("/GetTopBars/{0}/{1}", from, count));
        }

        public async Task<IEnumerable<Bar>> GetNearestBars(Coordinate loc, int n)
        {
            return await GetList<Bar>(String.Format("/GetBars/{0};{1}/{2}", loc.Latitude, loc.Longitude, n));
        }

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
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
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
                var json = await response.Content.ReadAsStringAsync();
                
                var res = JsonConvert.DeserializeObject<List<T>>(json);
                return res;
            }
        }
    }
    
}