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
        private decimal marketCost,
                        lastSellPrice;
        private bool isChecked;

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
        public string InfoAndAlias
        {
            get
            {
                string output = string.Empty;

                if (!this.isChecked)
                    output += "!*;";

                if (!string.IsNullOrWhiteSpace(this.alias))
                    output += this.alias;

                return output;
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
        public decimal LastSellPrice
        {
            get => this.lastSellPrice;
            set
            {
                this.lastSellPrice = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.LastSellPrice)));
            }
        }
        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsChecked)));
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
