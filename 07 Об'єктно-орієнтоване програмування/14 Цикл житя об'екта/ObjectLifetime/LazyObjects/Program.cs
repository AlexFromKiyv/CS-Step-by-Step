using LazyObjects;

void ManySongObjects()
{
    // Heare create 10000 Songs object
    MediaPlayer_v1 mediaPlayer_v1 = new MediaPlayer_v1();
    mediaPlayer_v1.Play();
    mediaPlayer_v1.Stop();
}

void WithoutManySongObjects()
{
    // No allocation of AllTracks object here!
    MediaPlayer_v2 mediaPlayer = new MediaPlayer_v2();
    mediaPlayer.Play();
    mediaPlayer.Stop();

    //Allocation of AllTracks happens when you call GetAllTracks().
    mediaPlayer.GetAllTracks();
}