namespace WebApplication1.Models
{
    public class Request
    {
        public int P_CATEGORY_ID { get; set; }
        public string P_TITLE { get; set; } = string.Empty;
        public string P_DESCRIPTION { get; set; } = string.Empty;
        public int P_FIELD_ID { get; set; }
        public int P_REQUEST_ID { get; set; }
        public string P_FIELD_VALUE { get; set; } = string.Empty;
        

    }
}
