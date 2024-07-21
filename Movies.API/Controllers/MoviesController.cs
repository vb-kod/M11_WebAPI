using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Interfaces;
using Movies.Data.Models;
using Movies.Data.Repository;

namespace Movies.API.Controllers
{
    /// <summary>
    /// API za filmove
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _repo;

        public MoviesController(IMovieRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Movies
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                return Ok(_repo.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // GET: api/Movies/5
        /// <summary>
        /// Dohvat jednog filma po id-ju.
        /// </summary>
        /// <param name="id">Redni broj pod kojim je film spremljen u bazi podataka.</param>
        /// <returns>Film s određene pozicije.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Movie> GetMovie(int id)
        {
            try
            {
                var result = _repo.GetMovieById(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutMovie(int id, Movie movie)
        {
            try
            {
                if (id != movie.Id)
                {
                    return BadRequest("Movie ID missmatch!");
                }

                var movieToUpdate = _repo.GetMovieById(id);

                if (movieToUpdate == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                return Ok(_repo.UpdateMovie(movie));
                //_repo.UpdateMovie(movie);
                //return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostMovie([FromBody] Movie movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMovie = _repo.InsertMovie(movie);

                // return CreatedAtAction(actionName, routeValues, createdResource)
                return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new movie record");
            }
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                var movieToDelete = _repo.GetMovieById(id);

                if (movieToDelete == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                return Ok(_repo.DeleteMovie(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        // GET: api/movies/search
        [HttpGet("search")]
        public ActionResult SearchByQueryString([FromQuery] string filterValue, [FromQuery] string orderBy = "asc", [FromQuery] int perPage = 0)
        {
            try
            {
                var movies = _repo.QueryStringFilter(filterValue, orderBy, perPage);

                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
