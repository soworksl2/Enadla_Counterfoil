using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace Enadla_Counterfoil.Models
{
    public class Sell
    {
        [PrimaryKey, AutoIncrement]
        public int SellId { get; set; }
        public DateTime Date { get; set; }
        public string location { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<IndividualSelledProduct> IndividualSelledProducts { get; set; }
    }
}
