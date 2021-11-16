using SQLite;
using System;
using System.ComponentModel;

namespace Enadla_Counterfoil.Models
{
    public class Product : ICloneable, INotifyPropertyChanged
    {
        private int productId;
        private string name,
                       alias;
        private decimal marketCost;

        [PrimaryKey, AutoIncrement]
        public int ProductId
        {
            get => this.productId;
            set
            {
                this.productId = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProductId)));
            }
        }
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
            }
        }
        public string Alias
        {
            get => this.alias;
            set
            {
                this.alias = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Alias)));
            }
        }
        public decimal MarketCost
        {
            get => this.marketCost;
            set
            {
                this.marketCost = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MarketCost)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return new Product()
            {
                ProductId = this.ProductId,
                Name = this.Name,
                Alias = this.Alias,
                MarketCost = this.MarketCost
            };
        }
    }
}
