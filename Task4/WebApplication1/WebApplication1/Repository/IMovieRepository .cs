namespace WebApplication1.Repository
{
    public interface IMovieRepository : IDisposable
    {
        Task<List<Movie>> GetMovies();
        Task<Movie> GetMovieByID(int MovieId);
        void InsertMovie(Movie movie);
        void DeleteMovie(int MovieID);
        void UpdateMovie(Movie movie);
        void Save();
    }
}
