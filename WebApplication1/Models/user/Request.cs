namespace WebApplication1.Models
{
    public class Request
    {
        public int CATEGORY_ID { get; set; }
        public int USER_ID { get; set; }
        public string TITLE { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public int REQUEST_ID { get; set; }
        public DateTime? EXECUTE_DATE { get; set; }
        public int FIELD_ID { get; set; }
        public string FIELD_VALUE { get; set; } = string.Empty;



    }
}
