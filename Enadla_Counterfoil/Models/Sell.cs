using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enadla_Counterfoil.Models
{
    public class Sell
    {
        private const byte MAX_PRODUCTS_IN_SUMMARY = 3;
        private const byte MAX_SUMMARY_LENGHT = 30;

        [PrimaryKey, AutoIncrement]
        public int SellId { get; set; }
        public DateTime Date { get; set; }
        public string location { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<IndividualSelledProduct> IndividualSelledProducts { get; set; }

        public string Summary
        {
            get
            {
                if (this.IndividualSelledProducts == null)
                    return "...";

                string output = string.Empty;
                byte includedProduct = 0;
                while(includedProduct < MAX_PRODUCTS_IN_SUMMARY && IndividualSelledProducts.Count > includedProduct)
                {
                    output += IndividualSelledProducts[includedProduct].Product.Name;
                    if (includedProduct > 0)
                        output += ';';
                    includedProduct++;
                }

                if (output.Length > MAX_SUMMARY_LENGHT)
                    output = output.Substring(0, 20);

                output += "...";
                return output;
            }
        }

        public decimal Total
        {
            get
            {
                if (this.IndividualSelledProducts == null || this.IndividualSelledProducts.Count <= 0)
                    return 0m;
                return IndividualSelledProducts.Sum(individualSell => (individualSell.Quantity * individualSell.UnitPrice));
            }
        }
    }
}
