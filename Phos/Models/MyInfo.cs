using Newtonsoft.Json;

namespace Phos.Models
{
    public class MyInfo
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_watching")]
        public int UserWatching { get; set; }

        [JsonProperty("user_completed")]
        public int UserCompleted { get; set; }

        [JsonProperty("user_onhold")]
        public int OnHold { get; set; }

        [JsonProperty("user_dropped")]
        public int Dropped { get; set; }

        [JsonProperty("user_plantowatch")]
        public int PlanToWatch { get; set; }

        [JsonProperty("user_days_spent_watching")]
        public float UserDaysSpentWatching { get; set; }
    }
}