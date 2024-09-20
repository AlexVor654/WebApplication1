namespace MvcApp.Models
{
    public class CalculatorModel
    {
        public double FirstNumber { get; set; }
        public double SecondNumber { get; set; }
        public string Operation { get; set; }
        public double Result { get; set; }
        public CalculatorModel()
    {
        Operation = "plus"; // например, инициализация значением по умолчанию
    }
    }
    
}