using Moq;
using MorningEarWorm.Core;
using MorningEarWorm.LastFM;
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
            RunReal();
            //RunMocked();
        }
        public static void RunReal()
        {
            var client = new LastFMClient("thelehmanlip");
            DoStuff(client);
        }

        public static void RunMocked()
        {
            var client = new Mock<ILastFMClient>();
            var res = new[]
            {
                new LastFMTrack
                {
                    Artist = "Periphery",
                    Track = "Flatline",
                    PlayDate = DateTime.Today.AddDays(-10)
                }
            };
            client.Setup(x => x.FindSongPlays(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(res);
            DoStuff(client.Object);
        }

        public static void DoStuff(ILastFMClient client)
        {

            Console.WriteLine("Artist:");
            var artist = Console.ReadLine();
            Console.WriteLine("Track:");
            var track = Console.ReadLine();
            client.PageSearched += (sender, a) =>
            {
                //Console.WriteLine($"Searched page: {a.Data}");
            };
            var trackPlays = client.FindSongPlays(artist, track, 9);
            if (!trackPlays.Any())
            {
                Console.WriteLine("None found!");
            }
            else
            {
                var counter = 1;
                var list = new List<LastFMTrack>();
                foreach (var trackPlay in trackPlays)
                {
                    Console.WriteLine($"{counter++}: Track found: {trackPlay}");
                    list.Add(trackPlay);
                }
                Console.WriteLine("Select track to tweet.");
                var t = Console.ReadKey();
                Console.WriteLine();

                int which;
                if (int.TryParse(t.KeyChar.ToString(), out which))
                {
                    var trackPlay = list[which - 1];
                    Console.WriteLine("Tweeting it!");
                    Tweeter.SendTweet(trackPlay.Artist, trackPlay.Track, (DateTime.Today - trackPlay.PlayDate).Days);
                }else
                {
                    Console.WriteLine("Skipping tweet");
                    if (t.Key == ConsoleKey.Q)
                        return;
                }
            }
            Console.WriteLine();
            DoStuff(client);
        }
    }
}
