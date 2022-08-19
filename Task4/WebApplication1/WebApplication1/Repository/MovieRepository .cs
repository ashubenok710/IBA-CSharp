namespace WebApplication1.Repository
{
    public class MovieRepository : IMovieRepository, IDisposable
    {
        private DBMoviesContext context;

        public MovieRepository(DBMoviesContext context)
        {
            this.context = context;
        }

        public IEnumerable<Movie> GetMovies()
        {
            return context.Movies.ToList();
        }

        public Movie GetMovieByID(int id)
        {
            return context.Movies.Find(id);
        }

        public void InsertMovie(Movie Movie)
        {
            context.Movies.Add(Movie);
        }

        public void DeleteMovie(int MovieID)
        {
            Movie Movie = context.Movies.Find(MovieID);
            context.Movies.Remove(Movie);
        }

        public void UpdateMovie(Movie Movie)
        {
            context.Entry(Movie).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
