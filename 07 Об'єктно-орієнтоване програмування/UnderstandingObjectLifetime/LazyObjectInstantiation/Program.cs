
using LazyObjectInstantiation;

static void CreateMediaPlayer()
{
    // This caller does not care about getting all songs,
    // but indirectly created 10,000 objects!
    MediaPlayer mediaPlayer = new MediaPlayer();
    mediaPlayer.Play();
    Console.WriteLine(GC.GetTotalMemory(false));
}
//CreateMediaPlayer();

static void UsingLazy()
{
    // No allocation of AllTracks object here!
    MediaPlayer mediaPlayer = new();
    mediaPlayer.Play();
    Console.WriteLine(GC.GetTotalMemory(false));

    // Allocation of AllTracks happens when you call GetAllTracks().
    _ = mediaPlayer.GetAllTracks();
    Console.WriteLine(GC.GetTotalMemory(false));
}
//UsingLazy();
