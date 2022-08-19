namespace WebApplication1.Repository
{
    internal interface IMovieRepository : IDisposable
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovieByID(int MovieId);
        void InsertMovie(Movie movie);
        void DeleteMovie(int MovieID);
        void UpdateMovie(Movie movie);
        void Save();
    }
}
