using CinemaPortal.Web.Models;

namespace CinemaPortal.Web.Data;

public interface IMovieRepository
{
    Task<List<Movie>> GetMovies();
    Task<Movie> GetMovieByID(int MovieId);
    void InsertMovie(Movie movie);
    void DeleteMovie(int MovieID);

    Task RemoveAsync(int id);

    Task<Movie> CreateAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task<Movie> GetByIdAsync(int id);
    void UpdateMovie(Movie movie);
    void Save();
    IQueryable<Movie> SetQueryable();
}