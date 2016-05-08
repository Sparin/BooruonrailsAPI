using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BooruonrailsComment
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("body")]
        public string Body { get; private set; }

        [JsonProperty("author")]
        public string Author { get; private set; }

        [JsonProperty("image_id")]
        public int ImageID { get; private set; }

        [JsonProperty("posted_at")]
        public string PostedAt { get; private set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; private set; }
    }
}
