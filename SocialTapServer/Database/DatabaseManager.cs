using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;
using SocialTapServer.Models;
using System.Device.Location;

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

        private SQLiteConnection _connection = null;
        private string _dbFileName = "SocialTap.sqlite";
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
            if(!File.Exists(_dbFileName))
            {
                SQLiteConnection.CreateFile(_dbFileName);
            }
        }

        /// <summary>
        /// Generates tables that weren't in the previous build. Upgrade scripts (e.g. table/column rename, add new column, etc.) should also go here.
        /// </summary>
        private void InitializeDatabase()
        {
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
        }

        public IEnumerable<Bar> GetBars(GeoCoordinate location, int count)
        {
            return cache.GetBars(location, count);
        }
        
        public Bar GetBar(Guid id)
        {
            return cache.GetBar(id);
        }

        public void AddBar(Bar bar)
        {
            cache.AddBar(bar);
        }

        public IEnumerable<Rating> GetRatings(Guid barId)
        {
            return GetRatings(barId, 0, 10);
        }

        public IEnumerable<Rating> GetRatings(Guid barId, int index, int count)
        {
            return cache.GetRatings(barId, index, count);
        }

        public IEnumerable<Comment> GetComments(Guid barId)
        {
            return GetComments(barId, 0, 10);
        }

        public IEnumerable<Comment> GetComments(Guid barId, int index, int count)
        {
            return cache.GetComments(barId, index, count);
        }

        public void AddRating(Guid barId, Rating rating)
        {
            cache.AddRating(barId, rating);
        }

        public void AddComment(Guid barId, Comment comment)
        {
            cache.AddComment(barId, comment);
        }

        ~DatabaseManager()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}