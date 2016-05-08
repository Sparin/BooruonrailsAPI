using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BooruonrailsInteraction
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("interaction_type")]
        public string InteractionType { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }

        [JsonProperty("user_id")]
        public int UserID { get; private set; }

        [JsonProperty("image_id")]
        public int ImageID { get; private set; }
    }
}
