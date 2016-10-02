using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using DLeh.Util;
using System.Configuration;
using MorningEarWorm.Core;

namespace MorningEarWorm.LastFM
{
    public class LastFMRepository : IScrobbleRepository
    {
        public LastFMRepository(IConfiguration config, string userName)
        {
            _config = config;
            UserName = userName;
        }


        IConfiguration _config;
        string secret => _config.GetAppSetting("LastFMSecret");
        string apiKey => _config.GetAppSetting("LastFMApiKey");

        //http://www.last.fm/api/show/user.getArtistTracks
        const string userUrl = "http://ws.audioscrobbler.com/2.0/?method=user.getartisttracks&user={0}&artist={1}&api_key=676c7cda460a2e12ea63a54fb469feef&format=json&page={2}";

        string UserName { get; set; }

        public event DataEventHandler<int> PageSearched;
        public void OnPageSearched(int pageNumber)
        {
            PageSearched?.Invoke(this, pageNumber);
        }

        //todo: make async version
        public IEnumerable<Track> GetTracksByArtist(string artist, int pageNumber = 1)
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

        public IEnumerable<Track> FindSongPlays(string artist, string trackName, int max = 5)
        {
            if (trackName == null)
                trackName = "";

            var pageNumber = 1;
            bool keepSearching = true;
            var counter = 0;

            var allTracks = new List<Track>();
            while (keepSearching)
            {
                //todo: get the tracks async and cancel other retreivals once it's found
                var tracks = GetTracksByArtist(artist, pageNumber);
                if (tracks == null || !tracks.Any())
                    break;

                //foreach(var track in tracks)
                //    Console.WriteLine(track);

                var matchingTracks = tracks.Where(x => x.Name.ToLower().Contains(trackName.ToLower()));
                if (matchingTracks.Any())
                {
                    foreach (var track in matchingTracks.OrderByDescending(x => x.PlayDate))
                    {
                        counter++;
                        if (counter > max)
                            yield break;

                        //if (allTracks.Any(x => x.Equals(track)))
                        //{
                        //    Console.WriteLine("Found dupe, done searching");
                        //    keepSearching = false;
                        //    break;
                        //}

                        allTracks.Add(track);
                        yield return track;
                    }
                }

                OnPageSearched(pageNumber);

                pageNumber++;
            }
        }
    }
}
