using System.ComponentModel.DataAnnotations;

namespace PI4Daan.Models
{
    public class Collectible
    {
        public int Id { get; set; }  //Primary Key
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 999999.99, ErrorMessage = "Prijs moet tussen 0 en 999999.99 liggen.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Range(0, 999999.99, ErrorMessage = "Waarde moet tussen 0 en 999999.99 liggen.")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }
        public int CategoryId { get; set; }  // Foreign Key
        public Category? Category { get; set; }  // Navigation Property
        public int BrandId { get; set; }  // Foreign Key
        public Brand? Brand { get; set; }  // Navigation Property
        [Range(0, 100.0, ErrorMessage = "Percentage moet tussen 0 en 100 liggen.")]
        public decimal Percentage { get; set; }
        [Range(0, 5.0, ErrorMessage = "Beoordeling moet tussen 0 en 5 liggen.")]
        public double Rating { get; set; }
    }
}
