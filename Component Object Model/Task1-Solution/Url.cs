using System;
using System.Text.RegularExpressions;

namespace Task3
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
                this.Host = Decode(match.Groups["host"].Value);
                if (match.Groups["port"].Value != "")
                    this.Port = Convert.ToInt32(match.Groups["port"].Value);
                this.Path = Decode(match.Groups["path"].Value);
                this.Query = Decode(match.Groups["query"].Value);
                this.FragmentId = Decode(match.Groups["fragmentid"].Value);
            }
            else
            {
               throw new FormatException("Could not parse URL: " + urlStr);
            }
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
            if (Path != "") url += "/" + Encode(Path.Substring(1));
            if (Query != "")
            {
                var queryParts = Query.Split('&');
                var qEncoded = "";
                foreach ( var queryPart in queryParts )
                {
                    var nameValue = queryPart.Split( '=' );
                    qEncoded += "&" + Encode( nameValue[0] ) + "=" + Encode( nameValue[1] );
                }
                url += "?" + qEncoded.Substring( 1 );
            }
            if (FragmentId != "") url += "#" + Encode(FragmentId);

            return url;
        }

        /// <summary>
        /// Encodes any special characters in the URL with an escaping sequence.
        /// </summary>        
        public static string Encode(string s)
        {
            string result = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (VALID_CHARACTERS.Contains(s[i].ToString()))
                {
                    // allowed character
                    result += s[i];
                }
                else
                {
                    // character has to be encoded as "%" + HexDigit + HexDigit
                    result += "%" + Convert.ToByte(s[i]).ToString("X");

                }
            }
            return result;
        }

        /// <summary>
        /// Decodes any escaping sequence in the URL with the corresponding characters.
        /// </summary>        
        public static string Decode(string s)
        {
            while (s.Contains("%"))
            {
                int pos = s.IndexOf("%");
                byte b = byte.Parse(s.Substring(pos + 1, 2), System.Globalization.NumberStyles.HexNumber);
                s = s.Substring(0, pos) + Convert.ToChar(b) + s.Substring(pos + 3);
            }
            return s;
        }
    }
}
