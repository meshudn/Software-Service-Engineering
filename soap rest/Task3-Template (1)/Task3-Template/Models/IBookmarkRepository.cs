namespace BookmarkService.Models
{
    public interface IBookmarkRepository
    {
        Bookmark[] GetAll();
        Bookmark GetById(int id);
        int Create(Bookmark bookmark);
        void Update(Bookmark bookmark);
        void Delete(int id);
        Bookmark[] GetByUser(string user);
    }
}