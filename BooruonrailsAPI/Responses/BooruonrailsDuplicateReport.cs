using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BooruonrailsDuplicateReport
    {
        [JsonProperty("id")]
        public int ID { get; private set; }

        [JsonProperty("state")]
        public string State { get; private set; }

        [JsonProperty("reason")]
        public string Reason { get; private set; }

        [JsonProperty("image_id_number")]
        public int ImageIdNumber { get; private set; }

        [JsonProperty("target_image_id_number")]
        public int TargetImageIdNumber { get; private set; }

        [JsonProperty("user_id")]
        public int? UserID { get; private set; }

        [JsonProperty("modifier")]
        public BooruonrailsModifier Modifier { get; private set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; private set; }
    }
}
