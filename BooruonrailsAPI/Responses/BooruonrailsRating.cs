using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruonrailsAPI.Responses
{
    public class BooruonrailsRating
    {
        [JsonProperty("score")]
        public int Score { get; private set; }

        [JsonProperty("favourites")]
        public int Favourites { get; private set; }

        [JsonProperty("up_vote_count")]
        public int UpVoteCount { get; private set; }

        [JsonProperty("down_vote_count")]
        public int DownVoteCount { get; private set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; private set; }
    }
}
