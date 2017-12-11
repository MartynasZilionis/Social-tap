using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;
using SocialTapServer.Models;
using System.Device.Location;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity.SqlServer;
using System.Data;
using System.Data.SqlClient;

namespace SocialTapServer.Database
{
    public class DatabaseManager
    {
        #region Singleton

        private static DatabaseManager _instance = null;
        private static object _instanceLock = new object();

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion Singleton
        #if DEBUG
            private static string conStr = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        #else
            private static string conStr = @"Server=tcp:soctapserv.database.windows.net,1433;Initial Catalog=SocialTapDb;Persist Security Info=False;User ID={USERNAME};Password={PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        #endif
        private DatabaseContext db = new DatabaseContext();
        //private SQLiteConnection _connection;
        //private string _dbFileName = "SocialTap.sqlite";
        private DatabaseCache cache = new DatabaseCache();
        private TaskScheduler scheduler;

        private async Task<T> Execute<T>(Func<T> fnc)
        {
            return await Task.Factory.StartNew(fnc, new CancellationToken(), TaskCreationOptions.None, scheduler);
            //var tsk = new Task<T>(fnc);
            //await tsk.Start(scheduler);
            //return tsk.Result;
        }

        private async Task Execute(Action fnc)
        {
            await Task.Factory.StartNew(fnc, new CancellationToken(), TaskCreationOptions.None, scheduler);
        }

        private DatabaseManager()
        {
            var tsk = new Task(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            });
            tsk.Start();
            tsk.Wait();
            #if DEBUG
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            #endif
        }
        /// <summary>
        /// Generates tables that weren't in the previous build. Upgrade scripts (e.g. table/column rename, add new column, etc.) should also go here.
        /// </summary>
        private void InitializeDatabase()
        {
            using(var conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand
               (
                   @"IF OBJECT_ID('Feedback') IS NULL
                        CREATE TABLE [Feedback](
                            [Id] uniqueidentifier default newid(),
                            [Text] varchar(8000) not null,
                            [Date] datetime not null
                        )", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public async Task AddFeedback(string value)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO Feedback (Text, Date) VALUES (@txt, @date)", conn);
                command.Parameters.Add(new SqlParameter("@txt", SqlDbType.VarChar, 8000, "Text"));
                command.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime, 0, "Date"));

                using (SqlDataAdapter da = new SqlDataAdapter("Select Id, Text, Date from Feedback", conn))
                {
                    da.InsertCommand = command;

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Feedback");

                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Text"] = value;
                    newRow["Date"] = DateTime.UtcNow;
                    ds.Tables[0].Rows.Add(newRow);

                    da.Update(ds.Tables[0]);
                    conn.Close();
                }
            }
            //await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Bar>> GetBars(Coordinate location, int count)
        {
            return await Execute(() =>
            (//neklauskit kaip veikia. kazkaip.
            from bar in db.Bars orderby 12742 * SqlFunctions.Asin(SqlFunctions.SquareRoot(SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (bar.Location.Latitude - location.Latitude)) / 2) *
                                                SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (bar.Location.Latitude - location.Latitude)) / 2) +
                                                SqlFunctions.Cos((SqlFunctions.Pi() / 180) * location.Latitude) * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (bar.Location.Latitude)) *
                                                SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (bar.Location.Longitude - location.Longitude)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) *
                                                (bar.Location.Longitude - location.Longitude)) / 2)))
            select new
            {
                Id = bar.Id,
                Name = bar.Name,
                Location = bar.Location,
                AverageFill = bar.Ratings.Select(r => r.FillPercentage).DefaultIfEmpty(0f).Average(),
                AveragePrice = bar.Ratings.Select(r => r.MugPrice / r.MugSize * 1000).DefaultIfEmpty(0f).Average(),
                CommentsCount = bar.Comments.Count(),
                RatingsCount = bar.Ratings.Count(),
                AverageStars = (float) bar.Comments.Select(c => c.Stars).DefaultIfEmpty(0).Average()
            }).Take(count).ToList().Select(x => new Bar(x.Id, x.Name, x.Location, x.CommentsCount, x.RatingsCount, x.AveragePrice, x.AverageFill, x.AverageStars)));
        }

        public async Task<IEnumerable<Bar>> GetTopBars(int index, int count)
        {
            return await Execute(() =>
            (
            from bar in db.Bars
            orderby bar.Comments.Select(c=>c.Stars).DefaultIfEmpty(0).Average() descending
            select new
            {
                Id = bar.Id,
                Name = bar.Name,
                Location = bar.Location,
                AverageFill = bar.Ratings.Select(r => r.FillPercentage).DefaultIfEmpty(0f).Average(),
                AveragePrice = bar.Ratings.Select(r => r.MugPrice / r.MugSize * 1000).DefaultIfEmpty(0f).Average(),
                CommentsCount = bar.Comments.Count(),
                RatingsCount = bar.Ratings.Count(),
                AverageStars = (float)bar.Comments.Select(c => c.Stars).DefaultIfEmpty(0).Average()
            }).Take(count).ToList().Select(x => new Bar(x.Id, x.Name, x.Location, x.CommentsCount, x.RatingsCount, x.AveragePrice, x.AverageFill, x.AverageStars)));
        }
        
        public async Task<Bar> GetBar(Guid id)
        {
            return await Execute(() =>
                (
                from bar in db.Bars where bar.Id == id select
                new
                {
                    Id = bar.Id,
                    Name = bar.Name,
                    Location = bar.Location,
                    AverageFill = bar.Ratings.Select(r => r.FillPercentage).DefaultIfEmpty(0f).Average(),
                    AveragePrice = bar.Ratings.Select(r => r.MugPrice / r.MugSize * 1000).DefaultIfEmpty(0f).Average(),
                    CommentsCount = bar.Comments.Count(),
                    RatingsCount = bar.Ratings.Count(),
                    AverageStars = (float) bar.Comments.Select(c => c.Stars).DefaultIfEmpty(0).Average()
                }
                ).ToList().Select(x=> new Bar(x.Id, x.Name, x.Location, x.CommentsCount, x.RatingsCount, x.AveragePrice, x.AverageFill, x.AverageStars)).First());
            //Bar res = await Execute(() => db.Bars.Find(id));
            //return cache.GetBar(id);
        }

        public async Task AddBar(Bar bar)
        {
            await Execute(() => db.Bars.Add(bar));
            await db.SaveChangesAsync();
            //cache.AddBar(bar);
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId)
        {
            return await GetRatings(barId, 0, 10);
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId, int index, int count)
        {
            return await Execute(() => (from bar in db.Bars where bar.Id == barId select bar.Ratings).First().OrderByDescending(x => x.Date).Skip(index).Take(count).ToList());
            //return await Execute(() => db.Bars.Find(barId).Ratings.Skip(index).Take(count));
            //return cache.GetRatings(barId, index, count);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid barId)
        {
            return await GetComments(barId, 0, 10);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid barId, int index, int count)
        {
            return await Execute(() => (from bar in db.Bars where bar.Id == barId select bar.Comments).First().OrderByDescending(x => x.Date).Skip(index).Take(count).ToList());
            //return await Execute(() => db.Bars.Find(barId).Comments.Skip(index).Take(count));
            //return cache.GetComments(barId, index, count);
        }

        public async Task AddRating(Guid barId, Rating rating)
        {
            await Execute(() => db.Bars.Find(barId).Ratings.Add(rating));
            await db.SaveChangesAsync();
            //cache.AddRating(barId, rating);
        }

        public async Task AddComment(Guid barId, Comment comment)
        {
            (await Execute(() => db.Bars.Find(barId).Comments)).Add(comment);
            //db.Bars.Where(x => x.Id == barId).FirstOrDefault().Comments.Add(comment);
            await db.SaveChangesAsync();
            //cache.AddComment(barId, comment);
        }
        //To be implemented
       
        public async Task<bool> GetUser(string Id)
        {
            bool res = true; // await from DB
            return res; // <- Grąžina ar useris Adminas; 
        }

        public async Task AddUser(string Id, string name)
        {
            //await -> adds user to DB;
        }
            

        

        ~DatabaseManager()
        {
            //_connection.Close();
            //_connection.Dispose();
            var tsk = db.SaveChangesAsync();
            tsk.Start();
            tsk.Wait();
            db.Dispose();
        }
    }
}