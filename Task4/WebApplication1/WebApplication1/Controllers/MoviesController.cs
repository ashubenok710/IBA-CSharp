using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            return Ok(await _movieRepository.GetMovies());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Movie>>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetMovieByID(id);
            if (movie == null)
                return NotFound("No movie here. :/");
            return Ok(movie);
        }

        [HttpPost]
        public void InsertMovie(Movie movie)
        {
            _movieRepository.InsertMovie(movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Movie>>> UpdateMovie(Movie movie, int id)
        {
            var dbMovie = await _movieRepository.GetMovieByID(id);
            //_context.Movies.FindAsync(id);
            if (dbMovie == null)
                return NotFound("No movie here. :/");

            dbMovie.Name = movie.Name;
            dbMovie.Raiting = movie.Raiting;
            dbMovie.ProductionDate = movie.ProductionDate;
            dbMovie.DirectorId = movie.DirectorId;

            _movieRepository.Save();
            //_context.SaveChangesAsync();
            return Ok(await _movieRepository.GetMovies());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Movie>>> DeleteMovie(int id)
        {
            var dbMovie = await _movieRepository.GetMovieByID(id);
            if (dbMovie == null)
                return NotFound("No movie here. :/");

            _movieRepository.DeleteMovie(dbMovie.Id);

            return Ok(await _movieRepository.GetMovies());
        }
    }
}
