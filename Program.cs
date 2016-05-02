using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningEarWorm
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Artist:");
            var artist = Console.ReadLine();
            Console.WriteLine("Track:");
            var track = Console.ReadLine();

            var client = new LastFMClient("thelehmanlip");
            client.PageSearched += (sender, a) =>
             {
                 Console.WriteLine($"Searched page: {a.Data}");
             };
            var trackPlays = client.FindSongPlays(artist, track);
            if (!trackPlays.Any())
            {
                Console.WriteLine("None found!");
            }else
            {
                foreach (var trackPlay in trackPlays)
                {
                    Console.WriteLine($"Track found: {trackPlay}");
                }
            }
            Console.WriteLine();
            
        }
    }
}
