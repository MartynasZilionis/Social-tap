﻿using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialTapServer
{
    public class DatabaseCache
    {
        private Dictionary<Guid, Bar> bars;

        private object barsLock = new object();

        public DatabaseCache()
        {
            Guid guid = Guid.NewGuid();
            bars = new Dictionary<Guid, Bar>
            {
                {
                    guid,
                    new Bar(guid,
                    "Dummy Bar",
                    new Coordinate(0,0),
                    0,
                    0,
                    0f,
                    0f,
                    0f)
                }
            };
        }

        public void AddBar(Bar info)
        {
            lock (barsLock)
            {
                bars.Add(info.Id, info);
            }
        }

        public IEnumerable<Bar> GetBars(Coordinate location, int count)
        {
            IEnumerable<Bar> answer = new List<Bar>();//bars.Select(x => x.Value).OrderBy(x => { return x.Location.GetDistanceTo(location); }).Take(count);
                //(from x in bars
                                       //orderby x.Value.Location.GetDistanceTo(location)
                                       //select (x => { return x.Value; })).Take(count);
            return answer;
        }

        public Bar GetBar(Guid id)
        {
            return bars[id];
        }
        /*
        public IEnumerable<Rating> GetRatings(Guid barId, int index, int count)
        {
            return bars[barId].GetRatings(index, count);
        }

        public IEnumerable<Comment> GetComments(Guid barId, int index, int count)
        {
            return bars[barId].GetComments(index, count);
        }

        public void AddRating(Guid barId, Rating rating)
        {
            bars[barId].AddRating(rating);
        }

        public void AddComment(Guid barId, Comment comment)
        {
            bars[barId].AddComment(comment);
        }
        */
    }
}