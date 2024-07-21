using Movies.Data.Models;

namespace Movies.Data.Interfaces
{
    public enum OrderBy
    {
        ASC, 
        DESC
    }

    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie GetMovieById(int id);
        Movie InsertMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
        Movie DeleteMovie(int id);

        IEnumerable<Movie> QueryStringFilter(string filterValue, string orderBy, int perPage);

        //IEnumerable<Movie> QueryStringFilter(string filterValue, OrderBy orderBy, int perPage);
    }
}
