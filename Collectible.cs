using System.ComponentModel.DataAnnotations;

namespace PI4Daan
{
    public class Collectible
    {
        public int Id { get; set; }  //Primary Key
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 999999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Range(0, 999999.99)]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        [Range(0, 100.0)]
        public decimal Percentage { get; set; }
        [Range(0, 5.0)]
        public double Rating { get; set; }
    }
}
