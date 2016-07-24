using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningEarWorm.LastFM
{
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
}
