namespace MyMvcApp.Models
{
    public class PlaylistSong
    {
        public int PlaylistID { get; set; }
        public Playlist? Playlist { get; set; }

        public int SongID { get; set; }
        public Song? Song { get; set; }
    }
}
