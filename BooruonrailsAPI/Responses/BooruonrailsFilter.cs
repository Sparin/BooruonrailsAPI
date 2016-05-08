using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    public class BooruonrailsFilter
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("hidden_tag_ids")]
        public int[] HiddenTagIDs { get; private set; }

        [JsonProperty("spoilered_tag_ids")]
        public int[] SpoileredTagIDs { get; private set; }

        [JsonProperty("spoilered_tags")]
        public string SpoileredTags { get; private set; }

        [JsonProperty("hidden_tags")]
        public string HiddenTags { get; private set; }

        [JsonProperty("hidden_complex")]
        public string HiddenComplex { get; private set; }

        [JsonProperty("spoilered_complex")]
        public string SpoileredComplex { get; private set; }

        [JsonProperty("public")]
        public bool Public { get; private set; }

        [JsonProperty("system")]
        public bool System { get; private set; }

        [JsonProperty("user_count")]
        public int UserCount { get; private set; }

        [JsonProperty("user_id")]
        public int? UserID { get; private set; }
    }    
}
