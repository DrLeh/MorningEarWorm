using System.Collections.Generic;
using DLeh.Util;

namespace MorningEarWorm.LastFM
{
    public interface ILastFMClient
    {
        event DataEventHandler<int> PageSearched;

        IEnumerable<LastFMTrack> FindSongPlays(string artist, string trackName, int max = 5);
        IEnumerable<LastFMTrack> GetTracksByArtist(string artist, int pageNumber = 1);
        void OnPageSearched(int pageNumber);
    }
}