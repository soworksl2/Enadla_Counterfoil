using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace Enadla_Counterfoil.Models
{
    public class FastExpense : INotifyPropertyChanged
    {
        private int idFastExpense;
        private DateTime date;
        private string concept;
        private decimal amount;

        [PrimaryKey, AutoIncrement]
        public int IdFastExpense 
        {
            get => this.idFastExpense;
            set
            {
                this.idFastExpense = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IdFastExpense)));
            }
        }
        public DateTime Date
        {
            get => this.date;
            set
            {
                this.date = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Date)));
            }
        }
        public string Concept
        {
            get => this.concept;
            set
            {
                this.concept = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Concept)));
            }
        }
        public decimal Amount
        {
            get => this.amount;
            set
            {
                this.amount = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Amount)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
