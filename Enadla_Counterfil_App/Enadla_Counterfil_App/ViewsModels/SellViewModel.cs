using Enadla_Counterfoil.Models;
using MvvmHelpers;
using System.Collections.Generic;
using System;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class SellViewModel : ObservableObject
    {
        private DateTime fromDate, toDate;
        private List<Sell> sells = new List<Sell>();

        public DateTime FromDate
        {
            get => this.fromDate;
            set
            {
                this.SetProperty<DateTime>(ref this.fromDate, value, nameof(this.FromDate));
            }
        }
        public DateTime ToDate
        {
            get => this.toDate;
            set
            {
                this.SetProperty<DateTime>(ref this.toDate, value, nameof(this.ToDate));
            }
        }
        public List<Sell> Sells => this.sells;

        public SellViewModel()
        {
            this.sells.Add(new Sell());
        }
    }
}
