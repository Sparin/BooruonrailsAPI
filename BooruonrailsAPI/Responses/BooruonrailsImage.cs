using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    public class BooruonrailsImage
    {
        [JsonObject(MemberSerialization.OptIn)]
        public struct RepresentationsStruct
        {
            [JsonProperty("thumb_tiny")]
            public string ThumbnailTiny { get; private set; }

            [JsonProperty("thumb_small")]
            public string ThumbnailSmall { get; private set; }

            [JsonProperty("thumb")]
            public string Thumbnail { get; private set; }

            [JsonProperty("small")]
            public string Small { get; private set; }

            [JsonProperty("medium")]
            public string Medium { get; private set; }

            [JsonProperty("large")]
            public string Large { get; private set; }

            [JsonProperty("tall")]
            public string Tall { get; private set; }

            [JsonProperty("full")]
            public string Full { get; private set; }
        }

        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; private set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; private set; }

        [JsonProperty("duplicate_reports")]
        public BooruonrailsDuplicateReport[] DuplicateReports { get; private set; }

        [JsonProperty("first_seen_at")]
        public string FirstSeenAt { get; private set; }

        [JsonProperty("uploader_id")]
        public int? UploaderID { get; private set; }

        [JsonProperty("file_name")]
        public string Filename { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("uploader")]
        public string Uploader { get; private set; }

        [JsonProperty("image")]
        public string Image { get; private set; }

        [JsonProperty("score")]
        public int Score { get; private set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; private set; }

        [JsonProperty("downvotes")]
        public int Downvotes { get; private set; }

        [JsonProperty("faves")]
        public int Faves { get; private set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; private set; }

        [JsonProperty("tags")]
        public string Tags { get; private set; }

        [JsonProperty("tag_ids")]
        public string[] TagIDs { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }

        [JsonProperty("aspect_ratio")]
        public double AspectRatio { get; private set; }

        [JsonProperty("original_format")]
        public string OriginalFormat { get; private set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; private set; }

        [JsonProperty("sha512_hash")]
        public string SHA512 { get; private set; }

        [JsonProperty("orig_sha512_hash")]
        public string OriginalSHA512 { get; private set; }

        [JsonProperty("source_url")]
        public string SourceURL { get; private set; }

        [JsonProperty("comments")]
        public BooruonrailsComment[] Comments { get; private set; }

        [JsonProperty("representations")]
        public RepresentationsStruct Representations { get; private set; }

        [JsonProperty("is_rendered")]
        public bool IsRendered { get; private set; }

        [JsonProperty("is_optimized")]
        public bool IsOptimized { get; private set; }

        private BooruonrailsInteraction[] interactions = new BooruonrailsInteraction[0];
        [JsonProperty("interactions")]
        public BooruonrailsInteraction[] Interactions { get { return interactions; } internal set { interactions = value; } }
    }
}
