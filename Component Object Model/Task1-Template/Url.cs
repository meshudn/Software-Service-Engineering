using System;
using System.Text.RegularExpressions;

namespace Task1
{
    /// <summary>
    /// A class for generating and parsing HTTP-URIs.
    /// </summary>
    public class Url
    {
        const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-_.~";

        public string Scheme = "";
        public string Host = "";
        public int Port = 80;
        public string Path = "";
        public string Query = "";
        public string FragmentId = "";

        /// <summary>
        /// Constructor for parsing URLs.
        /// </summary>
        public Url(string urlStr)
        {
            // TODO
            //Match match = Regex.Match(urlStr, @"", RegexOptions.IgnorePatternWhitespace);
            //if (match.Success)
            //{
            //    ...
            //}
            //else
            //{
            //    throw new FormatException("Could not parse URL: " + urlStr);
            //}
        }

        /// <summary>
        /// Constructor for building URLs from their components.
        /// </summary>
        public Url(string scheme, string host, int port, string path, string query, string fragmentId)
        {
            this.Scheme = scheme;
            this.Host = host;
            this.Port = port;
            this.Path = path;
            this.Query = query;
            this.FragmentId = fragmentId;
        }

        /// <summary>
        /// Returns the string representation of the URL.
        /// </summary>
        public override string ToString()
        {
            // TODO
            string url = "";

            return url;
        }

        /// <summary>
        /// Encodes any special characters in the URL with an escaping sequence.
        /// </summary>
        public static string Encode(string s)
        {
            string result = "";

            // TODO
            return result;
        }

        /// <summary>
        /// Decodes any escaping sequence in the URL with the corresponding characters.
        /// </summary>
        public static string Decode(string s)
        {
            // TODO
            return s;
        }
    }
}
