using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BooruonrailsModifier
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("avatar")]
        public string Avatar { get; private set; }

        [JsonProperty("creation_date")]
        public string CreationDate { get; private set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; private set; }

        [JsonProperty("uploads_count")]
        public int UploadsCount { get; private set; }

        [JsonProperty("post_count")]
        public int PostCount { get; private set; }

        [JsonProperty("topic_count")]
        public int TopicCount { get; private set; }
    }
}
