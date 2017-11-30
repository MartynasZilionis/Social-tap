using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;
using SocialTapServer.Models;
using System.Device.Location;
using System.Threading.Tasks;

namespace SocialTapServer.Database
{
    public class DatabaseManager
    {
        #region Singleton

        private static DatabaseManager _instance;
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

        private DatabaseManager()
        {
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

        public IEnumerable<Bar> GetBars(GeoCoordinate location, int count)
        {
            return db.Bars.OrderBy(x => x.Location.GetDistanceTo(location)).Take(count);
            //return cache.GetBars(location, count);
        }
        
        public async Task<Bar> GetBar(Guid id)
        {
            return await db.Bars.FindAsync(id);
            //return cache.GetBar(id);
        }

        public async Task AddBar(Bar bar)
        {
            db.Bars.Add(bar);
            await db.SaveChangesAsync();
            //cache.AddBar(bar);
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId)
        {
            return await GetRatings(barId, 0, 10);
        }

        public async Task<IEnumerable<Rating>> GetRatings(Guid barId, int index, int count)
        {
            return (await db.Bars.FindAsync(barId)).Ratings.Skip(index).Take(count);
            //return cache.GetRatings(barId, index, count);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid barId)
        {
            return await GetComments(barId, 0, 10);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid barId, int index, int count)
        {
            return (await db.Bars.FindAsync(barId)).Comments.Skip(index).Take(count);
            //return cache.GetComments(barId, index, count);
        }

        public async Task AddRating(Guid barId, Rating rating)
        {
            (await db.Bars.FindAsync(barId)).Ratings.Add(rating);
            await db.SaveChangesAsync();
            //cache.AddRating(barId, rating);
        }

        public async Task AddComment(Guid barId, Comment comment)
        {
            (await db.Bars.FindAsync(barId)).Comments.Add(comment);
            //db.Bars.Where(x => x.Id == barId).FirstOrDefault().Comments.Add(comment);
            await db.SaveChangesAsync();
            //cache.AddComment(barId, comment);
        }

        ~DatabaseManager()
        {
            //_connection.Close();
            //_connection.Dispose();
            db.Dispose();
        }
    }
}