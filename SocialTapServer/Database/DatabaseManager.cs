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
            //InitializeFile();
            //_connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", _dbFileName));
            //_connection.Open();
            //InitializeDatabase();
        }
        /// <summary>
        /// Checks if the database exists. If not, initializes a new database file.
        /// </summary>
        private void InitializeFile()
        {
            /*
            if(!File.Exists(_dbFileName))
            {
                SQLiteConnection.CreateFile(_dbFileName);
            }
            */
        }

        /// <summary>
        /// Generates tables that weren't in the previous build. Upgrade scripts (e.g. table/column rename, add new column, etc.) should also go here.
        /// </summary>
        private void InitializeDatabase()
        {
            /*
            //Initialize (create new tables)
            using (SQLiteCommand command = new SQLiteCommand(File.ReadAllText("Initialize.sql"), _connection))
            {
                command.ExecuteNonQuery();
            }
            //Upgrade (add/rename columns, rename tables, etc.)
            using (SQLiteCommand command = new SQLiteCommand(File.ReadAllText("Upgrade.sql"), _connection))
            {
                command.ExecuteNonQuery();
            }
            */
        }

        public async Task<IEnumerable<Bar>> GetBars(Coordinate location, int count)
        {
            var tmp = await Execute(() => db.Bars);
            //neklauskit kaip veikia. kazkaip.
            var tmp2 = tmp.OrderBy(x => 12742 * SqlFunctions.Asin(SqlFunctions.SquareRoot(SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Location.Latitude - location.Latitude)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Location.Latitude - location.Latitude)) / 2) +
                                    SqlFunctions.Cos((SqlFunctions.Pi() / 180) * location.Latitude) * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (x.Location.Latitude)) *
                                    SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Location.Longitude - location.Longitude)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Location.Longitude - location.Longitude)) / 2))));
            //var tmp2 = tmp.OrderBy(x => x.Location.GetDistanceTo(location)).ToList();
            return tmp2.Take(count);
            //return (await Execute(() => db.Bars)).OrderBy(x => x.Location.GetDistanceTo(location)).Take(count);
            //return cache.GetBars(location, count);
        }
        
        public async Task<Bar> GetBar(Guid id)
        {
            return await Execute(() => db.Bars.Find(id));
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
            var res = await Execute(() => (from bar in db.Bars where bar.Id == barId select bar.Comments).First().OrderByDescending(x=>x.Date).Skip(index).Take(count));
            return res.ToList();
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