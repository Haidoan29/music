using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyMvcApp.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistID { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PlaylistSong>? PlaylistSongs { get; set; }
    }
}
