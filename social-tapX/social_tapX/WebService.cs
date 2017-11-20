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
        void Set_BarAndPercent(string name, int percent);
        void Set_BarAndCommentsAndRating(string name, string comment, int rating);
        void Set_FeedbackAndDate(string feedback, DateTime date);
        List<Rated_Bar> GetListOfBars(int ToNumber);
        List<string> GetListOfComments(string Bar_Name, int ToNumber);
    }

    public class WebService : IWebService
    {
        private static HttpClient client = new HttpClient();
        private static string serviceUrl = "http://localhost:53162/api";
        //DĖL DEBUGINIMO PRIEŽASČIŲ BARP BARCR IR FBD YRA PUBLIC, ŠIAIP TURĖTŪ BŪT PRIVATE


        //Išsaugoma klasė kurioje yra ivertintas baras, tai yra pavadinimas ir procentai
        //Objektas bus sukuriamas kai bus išanalizuota foto. 
        //metode Set_BarAndPercent galima būtų dadėti kodo, kuris iškart nusiųstų duomenis į duombazę ar ws
        //Naudoja {get; private set;} abiem laukam
        public static BarAndPercent BarP = new BarAndPercent("", 0);

        public void Set_BarAndPercent(string name, int percent)
        {
            BarP = new BarAndPercent(name, percent);
        }

        //Išsaugoma kalsė kurioje yra baro pavadinimas, komentaras ir reitingas
        //Objektas bus sukurtas kai bus paspausta ok komentavimo lange
        //Metode Set_Rated_Bar galima dadėt kodą, kuris viską perduotų į db and ws
        //Naudoja {get; private set;} visiems laukams
        public static BarAndCommentsAndRating BarCR = new BarAndCommentsAndRating("", "", 0);

        public void Set_BarAndCommentsAndRating(string name, string comment, int rating)
        {
            BarCR = new BarAndCommentsAndRating(name, comment, rating);
        }

        //Išsaugoma klasė kurioje yra feedback mums ir data kada parašytas
        //Objektą sukurs kai paspaus submit feedbacko dalyje
        //Naudoja {get; private set;} abiems laukams
        public static FeedbackAndDate FBD = new FeedbackAndDate("", new DateTime(2000, 01, 01));
        
        public void Set_FeedbackAndDate(string feedback, DateTime date)
        {
            FBD = new FeedbackAndDate(feedback, date);
        }

        //Šitas metodas turi grąžinti listą barų, kurių tipas yra Rated_Bar. Turi grąžinti jų 20 (bent kolkas) ir grąžinti IKI skaiciaus
        //ToNumber, pvz jei ToNumber = 20, tai grąžina pirmus 20 (0 - 19), jei ToNumber = 40, tai 20-39 and so on.
        //Jeigu baigiasi barų pavadinimai grąžina kiek gali, likusių vietų NEPILDYTI niekuo
        //ir negali grąžinti daugiau negu 20 (bent kolkas), tikslaii nzn kas nutiktų :D 
        public List<Rated_Bar> GetListOfBars(int ToNumber)
        {
            List<Rated_Bar> ListOfBars = new List<Rated_Bar>();

            ListOfBars.Add(new Rated_Bar("Trys Draugai", 85.45, 9.4));
            ListOfBars.Add(new Rated_Bar("Du Draugai", 75.45, 9.4));
            ListOfBars.Add(new Rated_Bar("Vienas Draugas", 25.45, 5.4));
            ListOfBars.Add(new Rated_Bar("(╯°□°）╯︵ ┻━┻", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("A", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("B", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("C", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("D", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("E", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("F", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("G", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("H", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("(╯°□°）", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("(╯°□°）╯", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("︵ ┻━┻", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("I", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("Y", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("J", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar("K", 5.45, 0.4));
            ListOfBars.Add(new Rated_Bar(" (ﾉಥ益ಥ）ﾉ ︵ ┻━┻", 5.45, 0.4));

            return ListOfBars;
        }


        //Šitas metodas turi grąžinti komentarus baro, kurio pavadinimas Bar_Name. Visos kitos taisyklės kaip ir 
        //Kito metodo (public static List<Rated_Bar> GetListOfBars(int ToNumber))
        public List<string> GetListOfComments(string Bar_Name, int ToNumber)
        {
            List<string> Y = new List<string>
            {
                "KJHsjdhfukhsfhuahds kshdfkj sjdf kjhsjkfh ksdgf skjdhf ksdfkj hkhskjgfk sjkdhfkj gsjkhf hkjsdgkfhs dkjfh ",
                "ksdjhfkjshf kjhsdkfjh kjs kjhdskf shfkj hksdkjfh ",
                "Teip",
                "sdfhksdfkjshgisdjfklshdkfh lsdhf kjsjfh usdhf kjgdkjhf sajfdh khskdjfhkj shdkfhkj sdgf sjdhfksgkfhsjkfh lasjouewyt pjr owrsf",
                "Nope",
                "KJHsjdhfukhsfhuahds kshdfkj sjdf kjhsjkfh ksdgf skjdhf ksdfkj hkhskjgfk sjkdhfkj gsjkhf hkjsdgkfhs dkjfh ",
                "ksdjhfkjshf kjhsdkfjh kjs kjhdskf shfkj hksdkjfh ",
                "Teip",
                "sdfhksdfkjshgisdjfklshdkfh lsdhf kjsjfh usdhf kjgdkjhf sajfdh khskdjfhkj shdkfhkj sdgf sjdhfksgkfhsjkfh lasjouewyt pjr owrsf",
                "Nope"
            };
            return Y;
        }
        
        /// <summary>
        /// Gets all the <see cref="Bar">Bars</see>. (WIP)
        /// </summary>
        public async Task<IEnumerable<Bar>> GetAllBars()
        {
            return await GetList<Bar>("/Bar");
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
            return await GetList<Comment>(string.Format("/Comment/{1}/{2}/{3}", barId, from, count));
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId, int from, int count)
        {
            return await GetList<Rating>(string.Format("/Rating/{1}/{2}/{3}", barId, from, count));
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