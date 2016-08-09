using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Android.Content;
using MorningEarWorm.Core;
using MorningEarWorm.LastFM;

namespace MorningEarWorm.Mobile
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public void NotifyPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        IConfiguration config;

        INativePages nativePage;
        public MainPageViewModel(INativePages native)
        {
            config = new StaticConfiguration();
            nativePage = native;
        }

        private string _Artist;
        public string Artist { get { return _Artist; } set { _Artist = value; NotifyPropertyChanged(); } }

        private string _Track;
        public string Track { get { return _Track; } set { _Track = value; NotifyPropertyChanged(); } }

        private string _Message;
        public string Message { get { return _Message; } set { _Message = value; NotifyPropertyChanged(); } }

        private ObservableCollection<LastFMTrack> _Results;
        public ObservableCollection<LastFMTrack> Results { get { return _Results; } set { _Results = value; NotifyPropertyChanged(); } }


        public void Search()
        {
            var client = new LastFMClient(config, "thelehmanlip");
            Results = new ObservableCollection<LastFMTrack>(client.FindSongPlays(Artist, Track, 5));
            //Message = "Tweeting it!";
            //Tweet(Results.First());
        }

        public void Tweet(LastFMTrack track)
        {
            new Tweeter(config).SendTweet(track.Artist, track.Track, (DateTime.Today - track.PlayDate).Days);
            Message = "Twote it!";
        }

        public void SearchYT()
        {
            nativePage.SearchYT($"{Artist} {Track}");
        }

    }
}
