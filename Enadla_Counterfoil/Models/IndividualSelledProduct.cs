using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Enadla_Counterfoil.Models
{
    public class IndividualSelledProduct
    {
        [PrimaryKey, AutoIncrement]
        public int IndividualSelledProductId { get; set; }
        public decimal Quantity { get; set; }
        [ForeignKey(typeof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(typeof(Sell))]
        public int SellId { get; set; }
        public decimal UnitPrice { get; set; }

        [ManyToOne]
        public Product Product { get; set; }
    }
}
