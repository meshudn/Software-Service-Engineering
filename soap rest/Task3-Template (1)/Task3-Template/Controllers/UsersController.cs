using BookmarkService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkService.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookmarkRepository _bookmarkRepository;
        
        public UsersController(IUserRepository userRepository, IBookmarkRepository bookmarkRepository)
        {
            _userRepository = userRepository;
            _bookmarkRepository = bookmarkRepository;
        }

        // GET users
        [HttpGet]
        public User[] Get()
        {
            return _userRepository.GetAll();
        }

        // GET users/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        // POST users
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            var id = _userRepository.Create(user);
            return Created("http://localhost:5000/users/" + id, user);
        }

        // PUT users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]User user)
        {
            _userRepository.Update(user);
        }

        // DELETE users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        // GET users/5/bookmarks
        [HttpGet("{id}/bookmarks")]
        public IActionResult GetBookmarks(string id)
        {
            // TODO
            return NotFound();
        }

        // POST users/5/bookmarks
        [HttpPost("{id}/bookmarks")]
        public IActionResult Post(string id, [FromBody]Bookmark bookmark)
        {
            // TODO
            return NotFound();
        }
    }
}
