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
        public bool IsChecked { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public Product Product { get; set; }

        public string ProductName
        {
            get
            {
                return Product == null? string.Empty : this.Product.Name;
            }
        }
        public decimal Total
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}
