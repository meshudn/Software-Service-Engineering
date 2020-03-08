using BooksService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;
using System.Linq;
using System;
using System.Xml.Linq;

namespace BooksService.Controllers
{
    [FormatFilter]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;

        private const string _atom = @"
            <service xmlns:atom=""http://www.w3.org/2005/Atom"" xmlns=""http://localhost:5000"">
                <atom:link rel=""books"" href=""/v1/books"" />
            </service>
        ";
        
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET /v_
        [HttpGet("v1")]
        [HttpGet("v2")]
        public string Index()
        {
            Response.ContentType = "application/xml";
            return _atom;
        }

        
        public Book[] GetBooksByAuthorTitleGenre([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null)
        {
            var books = _bookRepository.GetAll();

            if (author != null || title != null || genre != null)
            {
                books = books.Where(book => 
                    (author == null || book.Author.ToLowerInvariant().Contains(author.ToLowerInvariant())) &&
                    (title == null || book.Title.ToLowerInvariant().Contains(title.ToLowerInvariant())) &&
                    (genre == null || book.Genre.ToLowerInvariant().Contains(genre.ToLowerInvariant()))
                ).ToArray();
            }

            return books;
        }

        // GET /v1/books.json
        [HttpGet("v1/books.json")]
        public IActionResult GetBooksByAuthorTitleGenreJson([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null)
        {
            return Json(GetBooksByAuthorTitleGenre(author, title, genre));
        }

        // GET /v1/books
        [HttpGet("v1/books")]
        // GET /v1/books.xml
        [HttpGet("v1/books.xml")]
        public IActionResult GetBooksByAuthorTitleGenreXml([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null)
        {
            return new XmlResult(GetBooksByAuthorTitleGenre(author, title, genre));
        }
        
        
        public Book[] GetBooksByAuthorTitleGenrePaged([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null, [FromQuery]int limit = 10, [FromQuery]int offset = 0)
        {
            return GetBooksByAuthorTitleGenre(author, title, genre).Skip(offset).Take(limit).ToArray();
        }
        
        // GET /v2/books.json
        [HttpGet("v2/books.json")]
        public IActionResult GetBooksByAuthorTitleGenrePagedJson([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null, [FromQuery]int limit = 10, [FromQuery]int offset = 0)
        {
            return Json(GetBooksByAuthorTitleGenrePaged(author, title, genre, limit, offset));
        }

        // GET /v2/books
        [HttpGet("v2/books")]
        // GET /v2/books.xml
        [HttpGet("v2/books.xml")]
        public IActionResult GetBooksByAuthorTitleGenrePagedXml([FromQuery]string author = null, [FromQuery]string title = null, [FromQuery]string genre = null, [FromQuery]int limit = 10, [FromQuery]int offset = 0)
        {
            return new XmlResult(GetBooksByAuthorTitleGenrePaged(author, title, genre, limit, offset)) ;
        }

        // GET /v2/daysUntil?date=...
        [HttpGet("v2/daysUntil")]
        [Produces("application/xml")]
        public IActionResult CalculateDaysUntil([FromQuery]string date)
        {
            DateTime endDate;
            if (!DateTime.TryParse(date, out endDate))
            {
                return BadRequest(new XElement("error", "Invalid date"));
            }

            var timespan = endDate - DateTime.Now;
            return Ok(new XElement("daysUntil", new XAttribute("endDate", date), timespan.Days));
        }
    }
}
