using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorningEarWorm.LastFM;
using Xamarin.Forms;

namespace MorningEarWorm.Mobile
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;

        public MainPage(INativePages native)
        {
            InitializeComponent();
            this.BindingContext = vm = new MainPageViewModel(native);
        }

        public void Tweet(object sender, SelectedItemChangedEventArgs e)
        {
            var track = e.SelectedItem as LastFMTrack;
            if (track != null)
                vm.Tweet(track);

        }
        public void Search(object sender, EventArgs e)
        {
            vm.Search();
        }
        public void SearchYT(object sender, EventArgs e)
        {
            vm.SearchYT();
        }

    }
}
