using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningEarWorm.LastFM
{

    public class LastFMTrack
    {
        public string Artist { get; set; }
        public string Track { get; set; }
        public DateTime PlayDate { get; set; }

        public override string ToString()
        {
            return $"{Artist} - {Track} - {PlayDate:MM/dd/yyyy hh:mm tt}";
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
