using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace MorningEarWorm.Core
{
    public class Tweeter
    {
        readonly IConfiguration _config;
        public Tweeter(IConfiguration config)
        {
            _config = config;
        }

        public void SendTweet(string artist, string song, int days)
        {
            var data = new TweetData
            {
                Artist = artist,
                Song = song,
                Days = days
            };
            var message = BuildMessage(data);
            Send(message);
        }

        public class TweetData
        {
            public string Artist { get; set; }
            public string Song { get; set; }
            public int Days { get; set; }

            public override string ToString()
            {
                return $"{Artist}{Song}{Days}";
            }

            public void Trim(int maxLength)
            {
                var dayLength = Days.ToString().Length;
                maxLength = maxLength - dayLength - Artist.Length;
                //for now just trim song 
                if (Song.Length > maxLength)
                    Song = Song.Substring(0, maxLength);
            }
        }

        private string BuildMessage(TweetData data)
        {
            var template = "This morning I woke up with {0} - {1} in my head. It has been {2} days since I last heard this song";
            var templateLength = template.Length - 9;
            var maxLength = 140 - templateLength;

            if (data.ToString().Length > maxLength)
            {
                data.Trim(maxLength);
            }
            var tweet = string.Format(template, data.Artist, data.Song, data.Days);
            return tweet;
        }

        private void Send(string tweet)
        {
            var TwitterConsumerKey = _config.GetAppSetting("TwitterConsumerKey");
            var TwitterSecret = _config.GetAppSetting("TwitterSecret");
            var TwitterAccessToken = _config.GetAppSetting("TwitterAccessToken");
            var TwitterAccessTokenSecret = _config.GetAppSetting("TwitterAccessTokenSecret");

            Auth.SetUserCredentials(TwitterConsumerKey, TwitterSecret, TwitterAccessToken, TwitterAccessTokenSecret);
            Tweet.PublishTweet(tweet);
        }

        public class TwitterData
        {
            public string Status { get; set; }
        }
    }
}
