using BooruonrailsAPI.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BooruonrailsAPI.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BooruonrailsAPI.Exceptions;

namespace BooruonrailsAPI
{
    public class BooruonrailsClient
    {
        protected CookieContainer m_cookies = new CookieContainer();

        public int Timeout { get; set; }
        public Uri BaseUri { get; private set; }

        public BooruonrailsClient(string site, int timeout = 100000)
        {
            this.Timeout = timeout;
            this.BaseUri = new Uri(site);
        }

        #region Public Methods
        /// <summary>
        /// Authorization on the site.
        /// Caution: It's unsecure method. Login and password encryption supported on protocol level
        /// </summary>
        /// <param name="username">E-mail of username</param>
        /// <param name="password">Password of username</param>
        /// <param name="rememberMe">Remember me option</param>
        public virtual void SignIn(string username, string password, bool rememberMe = false)
        {
            var Request = (HttpWebRequest)HttpWebRequest.Create(new Uri(BaseUri, "/users/sign_in"));
            SetSettings(ref Request);
            Request.Method = "GET";
            Request.Accept = "";

            HttpWebResponse webResponse = (HttpWebResponse)Request.GetResponse();
            SetCookies(webResponse);

            string response_data = string.Empty;
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
                response_data = reader.ReadToEnd();

            string authToken = Regex.Match(response_data, @"<\D*name=""authenticity_token"" [^ ]*/>").Value; //HTML tag parsing
            authToken = Regex.Replace(Regex.Match(authToken, @"value=""[^""]*""").Value, @"value=|""", string.Empty);//Get auth token from HTML tag
            authToken = Uri.EscapeDataString(authToken);

            /*{0} - Authenticity Token from hidden input on the site
              {1} - Username's E-mail
              {2} - Username's Password
              {3} - "Remember me" option (1 or 0)*/
            string dataPattern = "utf8=%E2%9C%93&authenticity_token={0}&user%5Bemail%5D={1}&user%5Bpassword%5D={2}&user%5Bremember_me%5D={3}&commit=Sign+in";
            string data = String.Format(dataPattern, authToken, username, password, Convert.ToInt32(rememberMe));
            byte[] dataArray = Encoding.UTF8.GetBytes(data);

            Request = (HttpWebRequest)HttpWebRequest.Create(new Uri(BaseUri, "/users/sign_in"));
            SetSettings(ref Request);
            Request.Accept = "";
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = dataArray.Length;

            using (Stream dataStream = Request.GetRequestStream())
                dataStream.Write(dataArray, 0, dataArray.Length);

            webResponse = (HttpWebResponse)Request.GetResponse();
            SetCookies(webResponse);
            response_data = string.Empty;
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
                response_data = reader.ReadToEnd();
        }

        /// <summary>
        /// Returns a user's API key for this site. Requires authorization on the site 
        /// </summary>
        /// <returns>Returns a user's API key for this site</returns>
        public virtual string GetAPIKey()
        {
            string result = string.Empty;
            var Request = (HttpWebRequest)HttpWebRequest.Create(new Uri(BaseUri, "/users/edit"));
            SetSettings(ref Request);
            Request.Method = "GET";
            Request.Accept = "";

            HttpWebResponse webResponse = (HttpWebResponse)Request.GetResponse();
            SetCookies(webResponse);

            string response_data = string.Empty;
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
                response_data = reader.ReadToEnd();

            result = Regex.Match(response_data, @"Your API key is <strong>[^<]*</strong>").Value;
            result = Regex.Replace(result, @"Your API key is <strong>|</strong>", string.Empty);
            return result;
        }

        public virtual BooruonrailsFilter[] GetFilters(params string[] query)
        {
            List<BooruonrailsFilter> Result = new List<BooruonrailsFilter>();

            string json = DownloadString(GetUri("/filters.json", query));

            JObject js = JObject.Parse(json);
            foreach (JProperty w in js.Children())
                try
                {
                    foreach (JArray x in w.Children())
                        foreach (JObject m in x.Children<JObject>())
                            Result.Add(JsonConvert.DeserializeObject<BooruonrailsFilter>(m.ToString()));
                }
                catch (InvalidCastException) { }

            return Result.ToArray();
        }

        #region Interactions
        public virtual BooruonrailsRating CallInteractionMethod(InteractionMethod method, params string[] query)
        {
            return JsonConvert.DeserializeObject<BooruonrailsRating>(DownloadString(GetUri(method, query), "PUT"));
        }
        #endregion

        #region Images and Searching
        public virtual BooruonrailsImage GetImage(int id, params string[] query)
        {
            return JsonConvert.DeserializeObject<BooruonrailsImage>(DownloadString(GetUri(id + ".json", query)));
        }

        public virtual BooruonrailsImage[] GetImages(SearchMethod method, params string[] query)
        {
            List<BooruonrailsImage> Result = new List<BooruonrailsImage>();
            List<BooruonrailsInteraction> Interactions = new List<BooruonrailsInteraction>();

            if ((method == SearchMethod.WatchList || method == SearchMethod.Favourites) && !IsAuthenticated(query))
                throw new NotAuthorizedException("Authorization or \"key\" parameter required for this method");

            string json = DownloadString(GetUri(method, query));

            //Legacy from API-based
            JObject js = JObject.Parse(json);
            foreach (JProperty w in js.Children())
            {
                if (w.Name == "search" || w.Name == "images")
                    foreach (JArray x in w.Children())
                        foreach (JObject m in x.Children<JObject>())
                            Result.Add(JsonConvert.DeserializeObject<BooruonrailsImage>(m.ToString()));
                if (w.Name == "interactions")
                    foreach (JArray x in w.Children())
                        foreach (JObject m in x.Children<JObject>())
                            Interactions.Add(JsonConvert.DeserializeObject<BooruonrailsInteraction>(m.ToString()));
            }

            for (int i = 0; i < Interactions.Count; i++)
                for (int j = 0; j < Result.Count; j++)
                    if (Interactions[i].ImageID == Convert.ToInt32(Result[j].ID))
                    {
                        List<BooruonrailsInteraction> temp = new List<BooruonrailsInteraction>();
                        if (Result[j].Interactions != null)
                            temp = Result[j].Interactions.ToList();
                        temp.Add(Interactions[i]);
                        Result[j].Interactions = temp.ToArray();
                    }

            return Result.ToArray();
        }
        #endregion

        #region Tags
        /// <summary>
        /// Returns a tag infromation by ID number
        /// </summary>
        /// <param name="id">ID of tag</param>
        /// <param name="query">Speacial parameters for request query</param>
        /// <returns>Returns a tag infromation by ID</returns>
        public virtual BooruonrailsTag GetTag(int id, params string[] query)
        {
            return this.GetTag(Convert.ToString(id), query);
        }

        /// <summary>
        ///Returns a tag infromation by name
        /// </summary>
        /// <param name="name">Name of tag</param>
        /// <param name="query">Speacial parameters for request query</param>
        /// <returns>Returns a tag infromation by name</returns>
        public virtual BooruonrailsTag GetTag(string name, params string[] query)
        {
            string json = DownloadString(GetUri("/tags/" + name + ".json", query));

            JObject js = JObject.Parse(json);
            foreach (JProperty w in js.Children())
                if (w.Name == "tag" && w.HasValues)
                    foreach (JObject m in w.Children<JObject>())
                        return JsonConvert.DeserializeObject<BooruonrailsTag>(m.ToString());

            return null;
        }

        public virtual BooruonrailsTag[] GetTags(TagsType type, params string[] query)
        {
            List<BooruonrailsTag> Result = new List<BooruonrailsTag>();
            string json = DownloadString(GetUri(type, query));
            Console.WriteLine(json);
            
            JArray x = JArray.Parse(json);
            foreach (JObject m in x.Children<JObject>())
                Result.Add(JsonConvert.DeserializeObject<BooruonrailsTag>(m.ToString()));

            return Result.ToArray();
        }
        #endregion
        #endregion

        #region Protected Service Methods
        protected string GetQuery(params string[] query)
        {
            if (query.Length == 0)
                return string.Empty;
            string queryRow = "?";
            for (int i = 0; i < query.Length; i++)
                if (i + 1 == query.Length)
                    queryRow += query[i];
                else
                    queryRow += query[i] + "&";
            return Uri.EscapeUriString(queryRow);

        }

        protected Uri GetUri(string Path, params string[] query)
        {
            return new Uri(BaseUri, Path + GetQuery(query));
        }

        protected virtual Uri GetUri(InteractionMethod method, params string[] query)
        {
            string queryRow = GetQuery(query);
            switch (method)
            {
                case InteractionMethod.Vote:
                    return new Uri(BaseUri, "/api/v2/interactions/vote.json" + queryRow);
                case InteractionMethod.Favourites:
                default:
                    return new Uri(BaseUri, "/api/v2/interactions/fave.json" + queryRow);
            }
        }

        protected virtual Uri GetUri(SearchMethod method, params string[] query)
        {
            string queryRow = GetQuery(query);
            switch (method)
            {
                case SearchMethod.AllTop:
                    return new Uri(BaseUri, "/lists/all_time_top_scoring.json" + queryRow);
                case SearchMethod.Top:
                    return new Uri(BaseUri, "/lists/top_scoring.json" + queryRow);
                case SearchMethod.WatchList:
                    return new Uri(BaseUri, "/images/watched.json" + queryRow);
                case SearchMethod.Favourites:
                    return new Uri(BaseUri, "/images/favourites.json" + queryRow);
                case SearchMethod.Images:
                    return new Uri(BaseUri, "/images.json" + queryRow);
                case SearchMethod.Search:
                default:
                    return new Uri(BaseUri, "/search.json" + queryRow);
            }
        }

        protected virtual Uri GetUri(TagsType type, params string[] query)
        {
            string queryRow = GetQuery(query);
            switch (type)
            {
                case TagsType.Aliases:
                    return new Uri(BaseUri, "/tags/aliases.json" + queryRow);
                case TagsType.Implied:
                    return new Uri(BaseUri, "/tags/implied.json" + queryRow);
                case TagsType.All:
                default:
                    return new Uri(BaseUri, "/tags.json" + queryRow);
            }
        }

        protected virtual void SetCookies(WebResponse response)
        {
            for (int i = 0; i < response.Headers.Count; i++)
                if (response.Headers.GetKey(i).ToLower() == "set-cookie")
                {
                    string[] values = response.Headers.GetValues(i);
                    foreach (string value in values)
                        m_cookies.SetCookies(BaseUri, value);
                }
        }

        protected virtual void SetSettings(ref HttpWebRequest request)
        {
            request.Timeout = Timeout;
            request.CookieContainer = m_cookies;
            request.Accept = "application/json";
            request.AllowAutoRedirect = false;
        }

        protected virtual string DownloadString(Uri site, string method = "GET")
        {
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(site);
            SetSettings(ref Request);
            Request.ContentLength = 0;
            Request.Method = method;

            HttpWebResponse webResponse = (HttpWebResponse)Request.GetResponse();
            SetCookies(webResponse);

            string response_data = string.Empty;
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
                response_data = reader.ReadToEnd();
            Console.WriteLine(response_data);
            return response_data;
        }

        protected virtual bool IsAuthenticated(params string[] query)
        {
            foreach (string s in query)
                if (Regex.IsMatch(s, "key=.*"))
                    return true;

            if (GetAPIKey() != "")
                return true;

            return false;
        }
        #endregion
    }
}

/* Example of Queries:
    PUT /api/v2/interactions/fave.json?key=MYKEY&value=true&id=2
    PUT /api/v2/notifications/watch.json?key=MYKEY&actor_class=Image&id=2
    GET search.json?key=MYKEY&q=explicit&faves=&upvotes=only&uploads=&watched=&min_score=&max_score=&sf=score&sd=desc*/
