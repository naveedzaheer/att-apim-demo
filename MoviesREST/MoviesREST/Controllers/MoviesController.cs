using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesREST.Models;

namespace MoviesREST.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        private readonly FilmyContext _context;

        public MoviesController(FilmyContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public IEnumerable<Movie> GetMovies(string name= null, string directorName = null, string rating = null)
        {
            List<Movie> movies = _context.Movies.ToList();

            if (!string.IsNullOrEmpty(name))
                movies = movies.Where(m => m.FilmName.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) > -1).ToList();

            if (!string.IsNullOrEmpty(directorName))
                movies = movies.Where(m => m.DirectorName.IndexOf(directorName, StringComparison.CurrentCultureIgnoreCase) > -1).ToList();

            if (!string.IsNullOrEmpty(rating))
                movies = movies.Where(m => string.Compare(m.Certificate, rating, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();

            Thread.Sleep(200);

            return movies;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovies([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movies = await _context.Movies.SingleOrDefaultAsync(m => m.FilmId == id);

            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovies([FromRoute] int id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.FilmId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> PostMovies([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovies", new { id = movie.FilmId }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovies([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.FilmId == id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.FilmId == id);
        }
    }
}