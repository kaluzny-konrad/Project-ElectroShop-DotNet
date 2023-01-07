using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroShop.Shared.Domain;

public class Product
{
    public int ProductId { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, 100)]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:c}")]
    public decimal Price { get; set; }

    public int ManufacturerId { get; set; }
}
