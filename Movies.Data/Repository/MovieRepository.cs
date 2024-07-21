using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _context;

        public MovieRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.Id == id);
        }

        public Movie InsertMovie(Movie movie)
        {
            var result = _context.Movies.Add(movie);
            _context.SaveChanges();
            return result.Entity;
        }

        public Movie UpdateMovie(Movie movie)
        {
            var result = _context.Movies.FirstOrDefault(m => m.Id == movie.Id);

            if (result != null)
            {
                result.Title = movie.Title;
                result.Genre = movie.Genre;
                result.ReleaseYear = movie.ReleaseYear;

                _context.SaveChanges();

                return result;
            }
            else
            {
                return null;
            }
        }

        public Movie DeleteMovie(int id)
        {
            var result = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (result != null)
            {
                _context.Movies.Remove(result);
                _context.SaveChanges();

                return result;
            }

            return null;
        }

        public IEnumerable<Movie> QueryStringFilter(string filterValue, string orderBy, int perPage)
        {
            var movies = _context.Movies.ToList();

            // parametar: filterValue
            if (!string.IsNullOrEmpty(filterValue))
            {
                // provjera postojanja filterValue u Title ili Genre
                movies = movies.Where(m => m.Title.Contains(filterValue, StringComparison.CurrentCultureIgnoreCase)
                                            || m.Genre.Contains(filterValue, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            // provjera i eventualni izlaz iz metode...
            if (movies.Count > 0)
            {
                // parametar: orderBy
                switch (orderBy.ToLower())
                {
                    case "asc":
                        movies = movies.OrderBy(m => m.Id).ToList();
                        break;
                    case "desc":
                        movies = movies.OrderByDescending(m => m.Id).ToList();
                        break;
                }

                // parametar: perPage
                if (perPage > 0)
                {
                    movies = movies.Take(perPage).ToList();
                }
            }

            return movies;
        }

        //public IEnumerable<Movie> QueryStringFilter(string filterValue, OrderBy orderBy, int perPage)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
