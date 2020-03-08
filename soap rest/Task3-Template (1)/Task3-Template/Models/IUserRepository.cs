namespace BookmarkService.Models
{
    public interface IUserRepository
    {
        User[] GetAll();
        User GetById(int id);
        int Create(User user);
        void Update(User user);
        void Delete(int id);
    }
}