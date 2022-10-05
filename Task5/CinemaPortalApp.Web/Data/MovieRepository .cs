using CinemaPortal.Web.DbContexts;
using CinemaPortal.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaPortal.Web.Data;
public class MovieRepository : IMovieRepository
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

    public async Task RemoveAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
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

    public IQueryable<Movie> SetQueryable()
    {
        return _context.Set<Movie>().AsQueryable();
    }

    public async Task<Movie> GetByIdAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        return movie;
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return movie;
    }

    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }
}