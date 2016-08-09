using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MorningEarWorm.Core;
using MorningEarWorm.LastFM;
using Xamarin.Forms;

namespace MorningEarWorm.Xamarin
{
    public class App : Application
    {
        Label tbResult;
        Label lblProgress;
        TextCell artist;
        TextCell track;
        Button button;
        IConfiguration config;

        public App()
        {
            config = new StaticConfiguration();
            tbResult = new Label { HorizontalTextAlignment = TextAlignment.Center };
            //var view = new View()
            //{

            //};
            lblProgress = new Label { HorizontalTextAlignment = TextAlignment.Center };
            artist = new TextCell { Text = "periphery" };
            track = new TextCell { Text = "" };
            button = new Button
            {
                Text = "Search"
            };
            button.Clicked += (s, e) =>
            {
                lblProgress.Text = "Searching...";
                //ThreadPool.QueueUserWorkItem(o => Search());
                Search();
            };

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Morning Ear Worm"
                        },
                        //artist,
                        button,
                        lblProgress,
                        tbResult
                    }
                }
            };


        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void Search()
        {
            var client = new LastFMClient(config, "thelehmanlip");
            var results = client.FindSongPlays(artist.Text, track.Text);

            var resultString = string.Join("\n", results);

            //RunOnUiThread(() =>
            //{
                tbResult.Text = resultString;
                lblProgress.Text = "Tweeting it!";
            //});

            //ThreadPool.QueueUserWorkItem(o => Tweet(results.First()));
            Tweet(results.First());
        }

        public void Tweet(LastFMTrack track)
        {
            new Tweeter(config).SendTweet(track.Artist, track.Track, (DateTime.Today - track.PlayDate).Days);
            //RunOnUiThread(() =>
            //{
            lblProgress.Text = "Twote it!";
            //});
        }
    }
}
