using System.Xml.XPath;
using System.Xml.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;
using System.Linq;

namespace BookmarkService.Models
{
    public class UserXmlRepository : IUserRepository
    {
        private readonly string _xmlFile;

        public UserXmlRepository(IHostingEnvironment env)
        {
            _xmlFile = env.ContentRootPath + "/data.xml";
        }

        public static string XmlFile = "./data.xml";

        public User[] GetAll()
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            return ((Database)serializer.Deserialize(fs)).Users;
        }

        public User GetById(int id)
        {
            XDocument doc = XDocument.Load(_xmlFile);            
            XElement user = (doc.XPathSelectElement("//users/user[id=" + id + "]"));

            if (user == null)
            {
                return null;
            }
            
            var serializer = new XmlSerializer(typeof(User));
            return (User)serializer.Deserialize(new StringReader(user.ToString()));
        }

        public int Create(User user)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var users = database.Users;

            var nextId = users.Length > 0 ? (users.Select(b => b.Id).Max() + 1) : 1;
            user.Id = nextId;

            database.Users = users.Append(user).ToArray();

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);

            return nextId;
        }

        public void Update(User user)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var users = database.Users.Where(b => b.Id != user.Id);

            database.Users = users.Append(user).ToArray();

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);
        }

        public void Delete(int id)
        {
            var serializer = new XmlSerializer(typeof(Database));
            var fs = new FileStream(_xmlFile, FileMode.Open);
            var database = (Database)serializer.Deserialize(fs);
            var users = database.Users.Where(b => b.Id != id);

            serializer.Serialize(new FileStream(_xmlFile, FileMode.Create), database);
        }
    }
}