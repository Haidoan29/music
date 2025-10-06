using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Song
    {
        [Key]
        public int SongID { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }

        // store only file name, e.g. "song1.mp3" and files are in wwwroot/music/
        public string FileName { get; set; } = "";

        public string? Lyrics { get; set; }

        // duration in seconds (optional)
        public int? Duration { get; set; }

        // store cover image filename in wwwroot/images/
        public string? CoverImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
