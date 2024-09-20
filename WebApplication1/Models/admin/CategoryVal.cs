namespace WebApplication1.Models
{
    public class CategoryVal
    {
        public int FIELD_ID { get; set; }
        public int CATEGORY_ID { get; set; }
        public int FIELD_TYPE_ID { get; set; }
        public string FIELD_NAME { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        
        public string DISPLAY_TEXT { get; set; } = string.Empty;
        public string FIELD_VALUE { get; set; } = string.Empty;
        public string SEARCH_FIELD_VALUE { get; set; } = string.Empty;

    }
}
