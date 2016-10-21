using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MasterCard.Core.Model
{
	public class RequestMap : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		private static readonly Regex arrayIndexPattern = new Regex("(.*)\\[(.*)\\]");

		private Dictionary<string, object> __storage;

		public int Count
		{
			get
			{
				return this.__storage.Count;
			}
		}

		public object this[string key]
		{
			get
			{
				return this.Get(key);
			}
			set
			{
				this.Add(key, value);
			}
		}

		ICollection<string> IDictionary<string, object>.Keys
		{
			get
			{
				return this.__storage.Keys;
			}
		}

		ICollection<object> IDictionary<string, object>.Values
		{
			get
			{
				return this.__storage.Values;
			}
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<string, object>>)this.__storage).IsReadOnly;
			}
		}

		public RequestMap()
		{
			this.__storage = new Dictionary<string, object>();
		}

		public RequestMap(RequestMap bm)
		{
			this.__storage = bm.__storage;
		}

		public RequestMap(IDictionary<string, object> map)
		{
			this.__storage = new Dictionary<string, object>();
			this.AddAll(map);
		}

		public RequestMap(string jsonMapString)
		{
			this.__storage = new Dictionary<string, object>();
			this.AddAll(RequestMap.AsDictionary(jsonMapString));
		}

		public RequestMap(string key, object value)
		{
			this.__storage = new Dictionary<string, object>();
			this.__storage.Add(key, value);
		}

		protected internal void UpdateFromBaseMap(RequestMap baseMapToSet)
		{
			this.__storage = baseMapToSet.__storage;
		}

		public RequestMap Clone()
		{
			return new RequestMap(this.__storage);
		}

		public void Clear()
		{
			this.__storage.Clear();
		}

		public void AddAll(IDictionary<string, object> data)
		{
			foreach (string current in data.Keys)
			{
				this.Add(current, data[current]);
			}
		}

		private bool IsListKey(string key)
		{
			return key.Contains("[");
		}

		private string ExtractKeyName(string key)
		{
			return key.Substring(0, key.IndexOf("["));
		}

		private int ExtractKeyIndex(string key)
		{
			Match match = RequestMap.arrayIndexPattern.Match(key);
			if (match.Success && !"".Equals(match.Groups[2].ToString()))
			{
				return int.Parse(match.Groups[2].ToString());
			}
			return -1;
		}

		public void Add(string keyPath, object value)
		{
			string[] array = keyPath.Split(new char[]
			{
				'.'
			});
			Dictionary<string, object> dictionary = this.__storage;
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = i + 1 == array.Length;
				string key = array[i];
				if (this.IsListKey(key))
				{
					string key2 = this.ExtractKeyName(key);
					int num = this.ExtractKeyIndex(key);
					if (dictionary.ContainsKey(key2))
					{
						if (flag)
						{
							List<object> list = (List<object>)dictionary[key2];
							if (num > -1 && num < list.Count)
							{
								list[num] = value;
								return;
							}
							list.Add(value);
							return;
						}
						else
						{
							List<Dictionary<string, object>> list2 = (List<Dictionary<string, object>>)dictionary[key2];
							if (num > -1 && num < list2.Count)
							{
								dictionary = list2[num];
							}
							else
							{
								list2.Add(new Dictionary<string, object>());
								List<Dictionary<string, object>> expr_CE = list2;
								dictionary = expr_CE[expr_CE.Count - 1];
							}
						}
					}
					else
					{
						if (flag)
						{
							dictionary[key2] = new List<object>();
							((List<object>)dictionary[key2]).Add(value);
							return;
						}
						dictionary[key2] = new List<Dictionary<string, object>>();
						List<Dictionary<string, object>> expr_11F = (List<Dictionary<string, object>>)dictionary[key2];
						expr_11F.Add(new Dictionary<string, object>());
						dictionary = expr_11F[expr_11F.Count - 1];
					}
				}
				else if (dictionary.ContainsKey(key))
				{
					if (flag)
					{
						dictionary[key] = value;
						return;
					}
					dictionary = (Dictionary<string, object>)dictionary[key];
				}
				else
				{
					if (flag)
					{
						dictionary[key] = value;
						return;
					}
					dictionary[key] = new Dictionary<string, object>();
					dictionary = (Dictionary<string, object>)dictionary[key];
				}
			}
		}

		public virtual RequestMap Set(string key, object value)
		{
			this.Add(key, value);
			return this;
		}

		public object Get(string keyPath)
		{
			string[] array = keyPath.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				Match match = RequestMap.arrayIndexPattern.Match(array[0]);
				if (!match.Success)
				{
					object result;
					this.__storage.TryGetValue(array[0], out result);
					return result;
				}
				string text = match.Groups[1].ToString();
				object obj;
				this.__storage.TryGetValue(text, out obj);
				if (!(obj is IList))
				{
					throw new ArgumentException("Property '" + text + "' is not an array");
				}
				IList list = (IList)obj;
				int? num = new int?(list.Count - 1);
				if (!"".Equals(match.Groups[2].ToString()))
				{
					num = new int?(int.Parse(match.Groups[2].ToString()));
				}
				return list[num ?? 0];
			}
			else
			{
				IDictionary<string, object> dictionary = this.FindLastMapInKeyPath(keyPath);
				string[] expr_108 = array;
				string text2 = expr_108[expr_108.Length - 1];
				Match match2 = RequestMap.arrayIndexPattern.Match(text2);
				if (!match2.Success)
				{
					return dictionary[text2];
				}
				string text3 = match2.Groups[1].ToString();
				object obj2;
				dictionary.TryGetValue(text3, out obj2);
				if (!(obj2 is IList))
				{
					throw new ArgumentException("Property '" + text3 + "' is not an array");
				}
				IList list2 = (IList)obj2;
				int? num2 = new int?(list2.Count - 1);
				if (!"".Equals(match2.Groups[2].ToString()))
				{
					num2 = new int?(int.Parse(match2.Groups[2].ToString()));
				}
				return list2[num2 ?? 0];
			}
		}

		public bool ContainsKey(string keyPath)
		{
			string[] array = keyPath.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				Match match = RequestMap.arrayIndexPattern.Match(array[0]);
				if (!match.Success)
				{
					return this.__storage.ContainsKey(array[0]);
				}
				string text = match.Groups[1].ToString();
				object obj;
				this.__storage.TryGetValue(text, out obj);
				if (!(obj is IList))
				{
					throw new ArgumentException("Property '" + text + "' is not an array");
				}
				List<Dictionary<string, object>> list = (List<Dictionary<string, object>>)obj;
				int? num = new int?(list.Count - 1);
				if (!"".Equals(match.Groups[2].ToString()))
				{
					num = new int?(int.Parse(match.Groups[2].ToString()));
				}
				return num >= 0 && num < list.Count;
			}
			else
			{
				IDictionary<string, object> dictionary = this.FindLastMapInKeyPath(keyPath);
				if (dictionary == null)
				{
					return false;
				}
				IDictionary<string, object> arg_12A_0 = dictionary;
				string[] expr_124 = array;
				return arg_12A_0.ContainsKey(expr_124[expr_124.Length - 1]);
			}
		}

		public bool Remove(string keyPath)
		{
			string[] array = keyPath.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				Match match = RequestMap.arrayIndexPattern.Match(array[0]);
				if (!match.Success)
				{
					return this.__storage.Remove(array[0]);
				}
				string text = match.Groups[1].ToString();
				object obj;
				this.__storage.TryGetValue(text, out obj);
				if (!(obj is IList))
				{
					throw new ArgumentException("Property '" + text + "' is not an array");
				}
				List<Dictionary<string, object>> list = (List<Dictionary<string, object>>)obj;
				int? num = new int?(list.Count - 1);
				if (!"".Equals(match.Groups[2].ToString()))
				{
					num = new int?(int.Parse(match.Groups[2].ToString()));
				}
				if (num.HasValue)
				{
					list.RemoveAt(num ?? 0);
				}
			}
			IDictionary<string, object> arg_106_0 = this.FindLastMapInKeyPath(keyPath);
			string[] expr_100 = array;
			return arg_106_0.Remove(expr_100[expr_100.Length - 1]);
		}

		private IDictionary<string, object> FindLastMapInKeyPath(string keyPath)
		{
			string[] array = keyPath.Split(new char[]
			{
				'.'
			});
			IDictionary<string, object> dictionary = null;
			for (int i = 0; i <= array.Length - 2; i++)
			{
				Match match = RequestMap.arrayIndexPattern.Match(array[i]);
				string text = array[i];
				if (match.Success)
				{
					text = match.Groups[1].ToString();
					object obj = null;
					if (dictionary == null)
					{
						this.__storage.TryGetValue(text, out obj);
					}
					else
					{
						obj = dictionary[text];
					}
					if (!(obj is IList))
					{
						throw new ArgumentException("Property '" + text + "' is not an array");
					}
					IList list = (IList)obj;
					int? num = new int?(list.Count - 1);
					if (!"".Equals(match.Groups[2].ToString()))
					{
						num = new int?(int.Parse(match.Groups[2].ToString()));
					}
					dictionary = (IDictionary<string, object>)list[num ?? 0];
				}
				else
				{
					if (dictionary == null)
					{
						try
						{
							dictionary = (IDictionary<string, object>)this.__storage[text];
							goto IL_133;
						}
						catch
						{
							return null;
						}
					}
					dictionary = (IDictionary<string, object>)dictionary[text];
				}
				IL_133:;
			}
			return dictionary;
		}

		public static IDictionary<string, object> AsDictionary(string json)
		{
			IDictionary<string, object> result;
			try
			{
				result = RequestMap.ParseDictionary(JsonConvert.DeserializeObject<IDictionary<string, object>>(json));
			}
			catch (Exception)
			{
				IList<object> input = JsonConvert.DeserializeObject<List<object>>(json);
				result = new Dictionary<string, object>
				{
					{
						"list",
						RequestMap.ParseListOfDictionary(input)
					}
				};
			}
			return result;
		}

    public static IDictionary<string, object> AsDictionaryFromXml(string xml) {
      string jsonText;
      try {
#if NET461
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        jsonText = JsonConvert.SerializeXmlNode(doc);
#else
        XDocument doc = XDocument.Parse(xml);
        jsonText = JsonConvert.SerializeXNode(doc);
#endif
      } catch(Exception ex) {
        jsonText = JsonConvert.SerializeObject(new {
          Errors = new {
            Error = new {
              Source = "SDK.APICONTROLLER",
              ReasonCode = "INVALID_XML_RESPONSE",
              Description = "Error parsing XML response: " + ex.Message,
              Recoverable = "FALSE",
              RawData = xml
            }
          }
        });
      }

      return AsDictionary(jsonText);
    }

    private static List<object> ParseListOfObjects(IList<object> input) {
      List<object> list = new List<object>();
      foreach(object current in input) {
        object item;
        if(current is IDictionary) {
          item = RequestMap.ParseDictionary((Dictionary<string, object>)current);
        } else if(current is JObject) {
          item = RequestMap.ParseDictionary(((JObject)current).ToObject<Dictionary<string, object>>());
        } else {
          item = current;
        }
        list.Add(item);
      }
      return list;
    }

    private static List<Dictionary<string, object>> ParseListOfDictionary(IList<object> input) {
      List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
      foreach(object current in input) {
        Dictionary<string, object> item = null;
        if(current is IDictionary) {
          item = RequestMap.ParseDictionary((IDictionary<string, object>)current);
        } else if(current is JObject) {
          item = RequestMap.ParseDictionary(((JObject)current).ToObject<Dictionary<string, object>>());
        }
        list.Add(item);
      }
      return list;
    }

    private static Dictionary<string, object> ParseDictionary(IDictionary<string, object> input) {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach(KeyValuePair<string, object> current in input) {
        object value;
        if(current.Value is IDictionary) {
          value = RequestMap.ParseDictionary((IDictionary<string, object>)current.Value);
        } else if(current.Value is JObject) {
          value = RequestMap.ParseDictionary(((JObject)current.Value).ToObject<IDictionary<string, object>>());
        } else if(current.Value is JArray) {
          JToken jToken = ((JArray)current.Value)[0];
          if(jToken is JObject || jToken is IDictionary) {
            value = RequestMap.ParseListOfDictionary(((JArray)current.Value).ToObject<List<object>>());
          } else {
            value = RequestMap.ParseListOfObjects(((JArray)current.Value).ToObject<List<object>>());
          }
        } else {
          value = current.Value;
        }
        dictionary.Add(current.Key, value);
      }
      return dictionary;
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return this.__storage.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.__storage.GetEnumerator();
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			value = this.Get(key);
			return true;
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			this.Add(item.Key, item.Value);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			this.__storage.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return this.__storage.ContainsKey(item.Key);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)this.__storage).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)this.__storage).Remove(item);
		}
	}
}
