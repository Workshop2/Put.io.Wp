namespace Put.io.Api.ResponseObjects.Transfers
{
    public class Transfer : BaseObject
    {
        public long uploaded { get; set; }
        public int? estimated_time { get; set; }
        public int peers_getting_from_us { get; set; }
        public bool extract { get; set; }
        public double current_ratio { get; set; }
        public long size { get; set; }
        public int up_speed { get; set; }
        public int id { get; set; }
        public string source { get; set; }
        public string subscription_id { get; set; }
        public string status_message { get; set; }
        public int down_speed { get; set; }
        public int peers_connected { get; set; }
        public long downloaded { get; set; }
        public long? file_id { get; set; }
        public int peers_sending_to_us { get; set; }
        public int percent_done { get; set; }
        public string tracker_message { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public int save_parent_id { get; set; }
    }
}