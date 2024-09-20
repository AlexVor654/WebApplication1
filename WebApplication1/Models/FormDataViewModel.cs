using Microsoft.VisualBasic.FileIO;

namespace WebApplication1.Models
{
    public class FormDataViewModel
    {
        public List<Categoryes> Categories { get; set; } = new List<Categoryes>();
        public List<User> Users { get; set; } = new List<User>();
        public List<Field> Fields { get; set; } = new List<Field>();
        public List<Field_type> Field_types { get; set; } = new List<Field_type>();
        public List<Requeset> Requests { get; set; } = new List<Requeset>();
    }

    public class Categoryes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Field_type
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Requeset
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

}
