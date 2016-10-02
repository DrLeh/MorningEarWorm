using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningEarWorm.LastFM
{

    public class Track
    {
        public string Artist { get; set; }
        public string Name { get; set; }
        public DateTime PlayDate { get; set; }

        public override string ToString()
        {
            return $"{Artist} - {Name} - {PlayDate:MM/dd/yyyy hh:mm tt}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Track))
                return false;

            var other = obj as Track;
            return other.Artist == this.Artist
                && other.Name == this.Name
                && other.PlayDate == this.PlayDate;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Artist.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + PlayDate.GetHashCode();
            return hash;
        }
    }
}
