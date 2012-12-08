namespace Put.io.Api.ResponseObjects.Files
{
    public class File : BaseObject
    {
        public bool is_shared { get; set; }
        public string name { get; set; }
        public string screenshot { get; set; }
        public string created_at { get; set; } //TODO: Move to datetime?
        public int parent_id { get; set; }
        public bool is_mp4_available { get; set; }
        public string content_type { get; set; }
        public string icon { get; set; }
        public int id { get; set; }
        public long size { get; set; }
    }
}