// Course Material "Protokolle Verteilte Systeme"
// (c) 2008 by Professur Verteilte und Selbstorganisierende Rechnersysteme, TUC

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vsr.Teaching.Pvs.Web
{
    /// <summary>
    /// A class for generating and parsing HTTP-URIs.
    /// </summary>
    public class Url
    {
        const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-_@.&+-!*\"'(),/#?:";

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
            urlStr = Url.Decode(urlStr);

            Match match = Regex.Match(urlStr, @"
                ^(?:
                (?<scheme>[^:]*)
                \:\/\/(?<host>[^:^/^?^#]*)
                (?:\:(?<port>\d*))?
                )?
                (?<path>\/[^?^#]*)?
                (?:\?(?<query>[^#]*))?
                (?:\#(?<fragmentid>.*))?$            
            ", RegexOptions.IgnorePatternWhitespace);
            if (match.Success)
            {
                this.Scheme = match.Groups["scheme"].Value.ToLower();
                this.Host = match.Groups["host"].Value;
                if (match.Groups["port"].Value != "")
                    this.Port = Convert.ToInt32(match.Groups["port"].Value);
                this.Path = match.Groups["path"].Value;
                this.Query = match.Groups["query"].Value;
                this.FragmentId = match.Groups["fragmentid"].Value;
            }
            else
                throw new FormatException("Could not parse URL: " + urlStr);
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
            string url = Scheme + "://" + Host;
            if (Port != 80) url += ":" + Port;
            if (Path != "") url += Path;
            if (Query != "") url += "?" + Query;
            if (FragmentId != "") url += "#" + FragmentId;

            return Url.Encode(url);
        }

        /// <summary>
        /// Encodes any special characters in the URL with an escaping sequence.
        /// </summary>        
        public static string Encode(string url)
        {
            string result = "";
            for (int i = 0; i < url.Length; i++)
            {
                if (VALID_CHARACTERS.Contains("" + url[i]))
                {
                    // allowed character
                    result += url[i];
                }
                else
                {
                    // character has to be encoded as "%" + HexDigit + HexDigit
                    result += "%" + Convert.ToByte(url[i]).ToString("X");

                }
            }
            return result;
        }

        /// <summary>
        /// Decodes any escaping sequence in the URL with the corresponding characters.
        /// </summary>        
        public static string Decode(string url)
        {
            while (url.Contains("%"))
            {
                int pos = url.IndexOf("%");
                byte b = byte.Parse(url.Substring(pos + 1, 2), System.Globalization.NumberStyles.HexNumber);
                url = url.Substring(0, pos) + Convert.ToChar(b) + url.Substring(pos + 3);
            }
            return url;
        }
    }
}
