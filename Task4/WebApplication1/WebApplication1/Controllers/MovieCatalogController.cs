﻿using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DBMoviesContext _context = new DBMoviesContext();

        public MovieController(DBMoviesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Movie>>> GetVideoGame(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound("No movie here. :/");
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<List<Movie>>> CreateVideoGame(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Movie>>> UpdateVideoGame(Movie movie, int id)
        {
            var dbMovie = await _context.Movies.FindAsync(id);
            if (dbMovie == null)
                return NotFound("No movie here. :/");

            dbMovie.Name = movie.Name;
            dbMovie.Raiting = movie.Raiting;
            dbMovie.ProductionDate = movie.ProductionDate;
            dbMovie.DirectorId = movie.DirectorId;

            await _context.SaveChangesAsync();
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Movie>>> DeleteVideoGame(int id)
        {
            var dbMovie = await _context.Movies.FindAsync(id);
            if (dbMovie == null)
                return NotFound("No movie here. :/");

            _context.Movies.Remove(dbMovie);
            await _context.SaveChangesAsync();

            return Ok(await _context.Movies.ToListAsync());
        }
    }
}
