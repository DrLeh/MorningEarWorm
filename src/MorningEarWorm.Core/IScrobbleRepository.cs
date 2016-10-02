using System.Collections.Generic;
using DLeh.Util;

namespace MorningEarWorm.LastFM
{
    public interface IScrobbleRepository
    {
        event DataEventHandler<int> PageSearched;

        IEnumerable<Track> FindSongPlays(string artist, string trackName, int max = 5);
        IEnumerable<Track> GetTracksByArtist(string artist, int pageNumber = 1);
        void OnPageSearched(int pageNumber);
    }
}