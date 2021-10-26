using SQLite;

namespace Enadla_Counterfoil.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public decimal MarketCost { get; set; }
    }
}
