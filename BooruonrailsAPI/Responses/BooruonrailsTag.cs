using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    public class BooruonrailsTag
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("slug")]
        public string Slug { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("namespace")]
        public string Namespace { get; private set; }

        [JsonProperty("name_in_namespace")]
        public string NameInNamespace { get; private set; }

        [JsonProperty("images")]
        public int ImagesCount { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; private set; }

        [JsonProperty("aliased_to")]
        public string AliasedTag { get; private set; }

        [JsonProperty("aliased_to_id")]
        public int? AliasedTagID { get; private set; }

        [JsonProperty("spoiler_image_uri")]
        public string SpoilerImageUri { get; private set; }

        [JsonProperty("implied_tags")]
        public string ImpliedTags { get; private set; }

        [JsonProperty("implied_tag_ids")]
        public int[] ImpliedTagIDs { get; private set; }
    }
}
