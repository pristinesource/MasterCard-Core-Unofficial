using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
#if NET461
using System.Web;
#elif NETSTANDARD1_6
using RestSharp.Extensions.MonoHttp;
#endif

namespace MasterCard.Core {
    public static class Util {
        private static readonly string[] URIRFC3986CHARSTOESCAPE = new string[]
        {
            "!",
            "*",
            "'",
            "(",
            ")"
        };

        private static UTF8Encoding encoder = new UTF8Encoding();

        public static string NormalizeUrl(string requestUrl) {
            Uri uri = new Uri(requestUrl);
            return string.Format("{0}{1}{2}{3}", new object[]
            {
                uri.Scheme,
#if NET461
				Uri.SchemeDelimiter,
#elif NETSTANDARD1_6
                "://",
#endif
                uri.Authority,
                uri.AbsolutePath
            });
        }

        public static string GetCurrenyAssemblyPath() {
            return Path.GetDirectoryName(typeof(Util).GetTypeInfo().Assembly.CodeBase).Remove(0, "file:\\".Length);
        }

        public static IDictionary<string, object> SubMap(IDictionary<string, object> inputMap, List<string> inList) {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach(string current in inList) {
                if(inputMap.ContainsKey(current)) {
                    dictionary.Add(current, inputMap[current]);
                    inputMap.Remove(current);
                }
            }
            return dictionary;
        }

        public static string GetReplacedPath(string path, IDictionary<string, object> inputMap) {
            String result = path;

            MatchCollection matches = Regex.Matches(path, @"{(.*?)}");

            // Here we check the Match instance.
            foreach(Match match in matches) {
                GroupCollection groups = match.Groups;
                string key = groups[1].Value;
                if(inputMap.ContainsKey(key)) {
                    //add to the path
                    Object value = "";
                    inputMap.TryGetValue(key, out value);

                    //arizzini: replacing the value in the path
                    result = result.Replace("{" + key + "}", value.ToString());

                    //arizzini: removing the value from the input map
                    inputMap.Remove(key);
                } else {
                    throw new System.ArgumentException("Error, path paramer: '" + key + "' expected but not found in input map");
                }
            }

            return result;
        }

        public static string NormalizeParameters(string requestUrl, SortedDictionary<string, string> oauthParameters) {
            StringBuilder stringBuilder = new StringBuilder();
            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(oauthParameters, StringComparer.Ordinal);
            if(requestUrl.IndexOf('?') > 0) {
                NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(requestUrl.Substring(requestUrl.IndexOf('?')));
                foreach(string text in nameValueCollection) {
                    string[] values = nameValueCollection.GetValues(text);
                    for(int i = 0; i < values.Length; i++) {
                        string value = values[i];
                        sortedDictionary.Add(text, value);
                    }
                }
            }
            foreach(KeyValuePair<string, string> current in sortedDictionary) {
                if(stringBuilder.Length > 0) {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append(Util.UriRfc3986(current.Key)).Append("=").Append(Util.UriRfc3986(current.Value));
            }
            return stringBuilder.ToString();
        }

        public static string UriRfc3986(string stringToEncode) {
            StringBuilder stringBuilder = new StringBuilder(Uri.EscapeDataString(stringToEncode));
            for(int i = 0; i < Util.URIRFC3986CHARSTOESCAPE.Length; i++) {
                stringBuilder.Replace(Util.URIRFC3986CHARSTOESCAPE[i], HexEscape(Util.URIRFC3986CHARSTOESCAPE[i][0]));
            }
            return stringBuilder.ToString();
        }

        public static byte[] Sha1Encode(string input) {
            HashAlgorithm arg_12_0 = SHA1.Create();
            byte[] bytes = Util.encoder.GetBytes(input);
            return arg_12_0.ComputeHash(bytes);
        }

        public static string Base64Encode(byte[] textBytes) {
            return Convert.ToBase64String(textBytes);
        }

#if NETSTANDARD1_6
        private static readonly char[] HexUpperChars = new char[] {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'A',
            'B',
            'C',
            'D',
            'E',
            'F'
        };

        private static void EscapeAsciiChar(char ch, char[] to, ref int pos) {
            int num = pos;
            pos = num + 1;
            to[num] = '%';
            num = pos;
            pos = num + 1;
            to[num] = HexUpperChars[(int)((ch & 'ð') >> 4)];
            num = pos;
            pos = num + 1;
            to[num] = HexUpperChars[(int)(ch & '\u000f')];
        }

#endif

        private static string HexEscape(char character) {
#if NET461
            return Uri.HexEscape(character);
#elif NETSTANDARD1_6
            char[] array = new char[3];
            int num = 0;
            EscapeAsciiChar(character, array, ref num);
            return new string(array);
#endif
        }
    }
}
