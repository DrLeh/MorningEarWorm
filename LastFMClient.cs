using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using DLeh.Util;
using System.Configuration;

namespace MorningEarWorm
{
    public class LastFMClient
    {
        Func<int, bool> asdf => a => true;


        string secret = ConfigurationManager.AppSettings["LastFMSecret"];
        string apiKey = ConfigurationManager.AppSettings["LastFMApiKey"];

        //http://www.last.fm/api/show/user.getArtistTracks
        const string userUrl = "http://ws.audioscrobbler.com/2.0/?method=user.getartisttracks&user={0}&artist={1}&api_key=676c7cda460a2e12ea63a54fb469feef&format=json&page={2}";

        string UserName { get; set; }

        public event DataEventHandler<int> PageSearched;
        public void OnPageSearched(int pageNumber)
        {
            PageSearched?.Invoke(this, pageNumber);
        }

        public LastFMClient(string userName)
        {
            UserName = userName;
        }
        //todo: make async version
        public IEnumerable<LastFMTrack> GetTracksByArtist(string artist, int pageNumber = 1)
        {
            var artistUrl = string.Format(userUrl, UserName, artist, pageNumber);
            var req = WebRequest.Create(artistUrl);
            var response = req.GetResponse();
            var sb = new StringBuilder();
            using (var stream = response.GetResponseStream())
            {
                var streamReader = new StreamReader(stream);

                string line = "";
                int i = 0;

                while (line != null)
                {
                    i++;
                    line = streamReader.ReadLine();
                    if (line != null)
                        sb.Append(line);
                }
            }
            var fullJson = sb.ToString();

            var obj = JsonConvert.DeserializeObject<UserArtistTracks>(fullJson);
            return obj.GetLastFMTracks();
        }

        public IEnumerable<LastFMTrack> FindSongPlays(string artist, string trackName, int max = 5)
        {
            var pageNumber = 1;
            bool keepSearching = true;
            var counter = 0;

            var allTracks = new List<LastFMTrack>();
            while (keepSearching)
            {
                //todo: get the tracks async and cancel other retreivals once it's found
                var tracks = GetTracksByArtist(artist, pageNumber);
                if (tracks == null || !tracks.Any())
                    break;

                //foreach(var track in tracks)
                //    Console.WriteLine(track);

                var matchingTracks = tracks.Where(x => x.Track.ToLower().Contains(trackName.ToLower()));
                if (matchingTracks.Any())
                {
                    foreach (var track in matchingTracks.OrderByDescending(x => x.PlayDate))
                    {
                        counter++;
                        if (counter > max)
                            break;

                        if (allTracks.Any(x => x.Equals(track)))
                        {
                            Console.WriteLine("Found dupe, done searching");
                            keepSearching = false;
                            break;
                        }

                        allTracks.Add(track);
                        yield return track;
                    }
                }

                OnPageSearched(pageNumber);

                pageNumber++;
            }
        }
    }
    public class UserArtistTracks
    {
        public IEnumerable<LastFMTrack> GetLastFMTracks()
        {
            return tracks.tracks.Select(x => x.ToLastFMTrack());
        }

        [JsonProperty(nameof(artisttracks))]
        public artisttracks tracks { get; set; }
        public class artisttracks
        {

            [JsonProperty(nameof(track))]
            public track[] tracks { get; set; }
            public class track
            {
                public string name { get; set; }
                [JsonProperty(nameof(artist))]
                public artist Artist { get; set; }
                [JsonProperty(nameof(date))]
                public date Date { get; set; }

                public class artist
                {
                    [JsonProperty("#text")]
                    public string name { get; set; }
                }
                public class date
                {
                    [JsonProperty("#text")]
                    public DateTime Date { get; set; }
                }

                public LastFMTrack ToLastFMTrack()
                {
                    return new LastFMTrack
                    {
                        Track = name,
                        Artist = Artist.name,
                        PlayDate = Date.Date.ToLocalTime()
                    };
                }
            }
        }
    }

    public class LastFMTrack
    {
        public string Artist { get; set; }
        public string Track { get; set; }
        public DateTime PlayDate { get; set; }

        public override string ToString()
        {
            return $"{Artist} - {Track} - {PlayDate}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LastFMTrack))
                return false;

            var other = obj as LastFMTrack;
            return other.Artist == this.Artist
                && other.Track == this.Track
                && other.PlayDate == this.PlayDate;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Artist.GetHashCode();
            hash = (hash * 7) + Track.GetHashCode();
            hash = (hash * 7) + PlayDate.GetHashCode();
            return hash;
        }
    }
}
