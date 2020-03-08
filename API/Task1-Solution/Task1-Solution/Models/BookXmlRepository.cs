using System.Xml.XPath;
using System.Xml.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;
using System.Linq;

namespace BooksService.Models
{
    public class BookXmlRepository : IBookRepository
    {
        private readonly string _xmlFile;

        public BookXmlRepository(IHostingEnvironment env)
        {
            _xmlFile = env.ContentRootPath + "/data.xml";
        }

        public static string XmlFile = "./data.xml";

        public Book[] GetAll()
        {
            var serializer = new XmlSerializer(typeof(Database));
            using (var fs = new FileStream(_xmlFile, FileMode.Open))
            {
                return ((Database)serializer.Deserialize(fs)).Books;
            }
        }

        public Book GetById(int id)
        {
            XDocument doc = XDocument.Load(_xmlFile);            
            XElement book = (doc.XPathSelectElement("//Books/Book[Id=" + id + "]"));

            if (book == null)
            {
                return null;
            }
            
            var serializer = new XmlSerializer(typeof(Book));
            return (Book)serializer.Deserialize(new StringReader(book.ToString()));
        }

        public int Create(Book book)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var books = database.Books;

            var nextId = books.Length > 0 ? (books.Select(b => b.Id).Max() + 1) : 1;
            book.Id = nextId;

            database.Books = books.Append(book).ToArray();

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);

            return nextId;
        }

        public void Update(Book book)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var books = database.Books.Where(b => b.Id != book.Id);

            database.Books = books.Append(book).ToArray();

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);
        }

        public void Delete(int id)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var books = database.Books.Where(b => b.Id != id);

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);
        }
    }
}