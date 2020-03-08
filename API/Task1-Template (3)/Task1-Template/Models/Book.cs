namespace BooksService.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public string Url {
            get {
                return "/books/" + Id;
            }
        }

        public string BookUrl {
            get {
                return "/books/" + Id + "/book";
            }
        }
    }
}