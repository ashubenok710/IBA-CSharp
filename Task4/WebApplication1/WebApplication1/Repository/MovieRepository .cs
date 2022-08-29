namespace WebApplication1.Repository
{
    public class MovieRepository : IMovieRepository, IDisposable
    {
        private DBMoviesContext _context;

        public MovieRepository()
        {
            this._context = new DBMoviesContext();
        }

        public async Task<List<Movie>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieByID(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public void InsertMovie(Movie movie)
        {
            _context.Movies.Add(movie);
        }

        public void DeleteMovie(int MovieID)
        {
            Movie Movie = _context.Movies.Find(MovieID);
            _context.Movies.Remove(Movie);
        }

        public void UpdateMovie(Movie Movie)
        {
            _context.Entry(Movie).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
