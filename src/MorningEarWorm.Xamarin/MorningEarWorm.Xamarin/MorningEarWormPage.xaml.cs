using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorningEarWorm.Core;
using MorningEarWorm.LastFM;
using Xamarin.Forms;

namespace MorningEarWorm.Xamarin
{
    public partial class MorningEarWormPage : ContentPage
    {
        MainPageViewModel vm;
        public MorningEarWormPage()
        {
            //InitializeComponent();
            this.BindingContext = vm = new MainPageViewModel();
        }

        public void btnClick(object sender, EventArgs e)
        {
            vm.Search();
        }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        IConfiguration config;
        public MainPageViewModel()
        {
            config = new StaticConfiguration();
        }

        public string Artist { get; set; }
        public string Track { get; set; }
        public string Message { get; set; }
        public List<LastFMTrack> Results { get; set; }


        public void Search()
        {
            var client = new LastFMClient(config, "thelehmanlip");
            Results = client.FindSongPlays(Artist, Track, 5).ToList();
            Message = "Tweeting it!";
            Tweet(Results.First());
        }

        public void Tweet(LastFMTrack track)
        {
            new Tweeter(config).SendTweet(track.Artist, track.Track, (DateTime.Today - track.PlayDate).Days);
            Message = "Twote it!";
        }

    }
}
