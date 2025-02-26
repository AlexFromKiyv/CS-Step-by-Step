namespace LazyObjectInstantiation;
// The MediaPlayer has-an AllTracks object.
class MediaPlayer
{
    // Assume these methods do something useful.
    public void Play() { /* Play a song */ }
    public void Pause() { /* Pause the song */ }
    public void Stop() { /* Stop playback */ }

    //private AllTracks _allSongs = new AllTracks();

    //public AllTracks GetAllTracks()
    //{
    //    // Return all of the songs.
    //    return _allSongs;
    //}

    //private Lazy<AllTracks> _allTracks = new Lazy<AllTracks>();

    public AllTracks GetAllTracks()
    {
        // Return all of the songs.
        return _allTracks.Value;
    }

    private Lazy<AllTracks> _allTracks = new Lazy<AllTracks>( () =>
    {
        Console.WriteLine("Creating AllTracks object!");
        return new AllTracks();
    });
}
