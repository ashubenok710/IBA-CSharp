using CinemaPortal.Web.Data;
using CinemaPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CinemaPortal.Web.Controllers;
public class MoviesController : Controller
{

    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GetData()
    {
        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        int skip = start != null ? Convert.ToInt32(start) : 0;
        int recordsTotal = 0;

        var movieItems = _movieRepository.SetQueryable();

        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        {
            movieItems = movieItems.OrderBy(sortColumn + " " + sortColumnDirection);
        }
        if (!string.IsNullOrEmpty(searchValue))
        {
            movieItems = movieItems.Where(m => m.Name.Contains(searchValue)
                                        || m.ProductionDate.Contains(searchValue)
                                        || m.Raiting.Contains(searchValue));
        }

        recordsTotal = await movieItems.CountAsync();

        var jsonData = new
        {
            draw = draw,
            recordsFiltered = recordsTotal,
            recordsTotal = recordsTotal,
            data = await movieItems
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync()
        };


        return Ok(jsonData);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult<List<Movie>>> GetMovies()
    {
        return Ok(await _movieRepository.GetMovies());
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

    [HttpPost, ActionName("Delete")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieRepository.RemoveAsync(id);
        return Json(new { success = true });
    }

    public async Task<IActionResult> AddOrEdit(int id = 0)
    {

        if (id == 0)
        {
            return View(new Movie());
        }
        else
        {
            return View(await _movieRepository.GetByIdAsync(id));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrEdit(Movie movie)
    {
        if (movie.Id == 0)
        {
            await _movieRepository.CreateAsync(movie);

            TempData["AlertMessage"] = "Created Successfully";
        }
        else
        {
            await _movieRepository.UpdateAsync(movie);
            TempData["AlertMessage"] = "Updated Successfully";
        }

        return Json(new { success = true });
    }

}