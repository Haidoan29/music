using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;


namespace MyMvcApp.Controllers
{
    public class SongsController : Controller
    {
        private readonly MusicDbContext _db;
        public SongsController(MusicDbContext db) => _db = db;

        // GET: /Songs
        public async Task<IActionResult> Index()
        {
            var songs = await _db.Songs.OrderByDescending(s => s.CreatedAt).ToListAsync();
            return View(songs);
        }

        // GET: /Songs/Create
        public IActionResult Create() => View();

        // POST: /Songs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Song song, IFormFile MusicFile, IFormFile? CoverFile)
        {
            if (!ModelState.IsValid)
            {
                // debug ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("ModelState errors: " + string.Join(", ", errors));

                // debug file
                Console.WriteLine("MusicFile: " + MusicFile?.FileName);
                Console.WriteLine("CoverFile: " + CoverFile?.FileName);

                return View(song);
            }
            // Xử lý file nhạc
            if (MusicFile != null && MusicFile.Length > 0)
            {
                var musicFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/music");
                if (!Directory.Exists(musicFolder))
                    Directory.CreateDirectory(musicFolder);

                var musicFileName = Guid.NewGuid() + Path.GetExtension(MusicFile.FileName);
                var musicPath = Path.Combine(musicFolder, musicFileName);

                using (var stream = new FileStream(musicPath, FileMode.Create))
                {
                    await MusicFile.CopyToAsync(stream);
                }

                song.FileName = musicFileName;
            }

            // Xử lý file ảnh cover
            if (CoverFile != null && CoverFile.Length > 0)
            {
                var coverFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(coverFolder))
                    Directory.CreateDirectory(coverFolder);

                var coverFileName = Guid.NewGuid() + Path.GetExtension(CoverFile.FileName);
                var coverPath = Path.Combine(coverFolder, coverFileName);

                using (var stream = new FileStream(coverPath, FileMode.Create))
                {
                    await CoverFile.CopyToAsync(stream);
                }

                song.CoverImage = coverFileName;
            }

            _db.Songs.Add(song);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: /Songs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _db.Songs.FindAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        // POST: /Songs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Song song)
        {
            if (id != song.SongID) return BadRequest();
            if (!ModelState.IsValid) return View(song);

            _db.Entry(song).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Songs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _db.Songs.FindAsync(id);
            if (s == null) return NotFound();
            _db.Songs.Remove(s);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Songs/Player/5
        public async Task<IActionResult> Player(int id)
        {
            var song = await _db.Songs.FindAsync(id);
            if (song == null) return NotFound();
            // get list of all songs for next/prev navigation
            var all = await _db.Songs.OrderBy(s => s.SongID).ToListAsync();
            ViewData["AllSongs"] = all;
            return View(song);
        }
    }
}
