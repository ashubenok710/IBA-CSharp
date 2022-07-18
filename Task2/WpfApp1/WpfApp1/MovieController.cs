using System;
using WpfApp1.Model;
using WpfApp1.Pagination;

namespace WpfApp1
{
    internal class MovieController
    {
        private readonly DBMoviesContext _context;

        public MovieController(DBMoviesContext context)
        {
            _context = context;
        }

        public async void GetMovies(string orderBy, bool orderByDesc, int page, int size)
        {
            var query = _context.Movies.AsQueryable();
            try
            {
                var list = await PaginationService.GetPagination(query, page, orderBy, orderByDesc, size);

            }
            catch (Exception e)
            {

            }
        }


    }
}
