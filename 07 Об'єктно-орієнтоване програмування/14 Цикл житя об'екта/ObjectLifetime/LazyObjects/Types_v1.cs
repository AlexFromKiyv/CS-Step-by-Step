using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyObjects
{
    // Represents a single song.
	class Song
	{
        public string Artist { get; set; }
        public string TrackName { get; set; }
        public double TrackLength { get; set; }
    }

    //Represents all songs on a player.
    class AllTracks
    {
        private Song[] _allSongs = new Song[10000];

        public AllTracks()
        {
            // Fill up array 
            Console.WriteLine("Filling up the songs!");
        }
    }

    class MediaPlayer_v1
    {
        public void Play() { /* Play a song */ }
        public void Pause() { /* Pause the song */ }
        public void Stop() { /* Stop playback */}
        
        private AllTracks _allTracks = new AllTracks();

        public AllTracks GetAllTracks()
        {
            return _allTracks;
        }
    }

    class MediaPlayer_v2
    {
        public void Play() { /* Play a song */ }
        public void Pause() { /* Pause the song */ }
        public void Stop() { /* Stop playback */}

        private Lazy<AllTracks> _allTracks = new Lazy<AllTracks>();

        public AllTracks GetAllTracks()
        {
            return _allTracks.Value;
        }
    }

    class MediaPlayer_v3
    {
        public void Play() { /* Play a song */ }
        public void Pause() { /* Pause the song */ }
        public void Stop() { /* Stop playback */}

        private Lazy<AllTracks> _allTracks =
            new Lazy<AllTracks>(() =>
            {
                Console.WriteLine("I do something important here.");
                return new AllTracks();
            }
            );

        public AllTracks GetAllTracks()
        {
            return _allTracks.Value;
        }
    }
}
