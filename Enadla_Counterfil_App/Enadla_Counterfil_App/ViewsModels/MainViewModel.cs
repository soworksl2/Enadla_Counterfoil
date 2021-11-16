using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Xamarin.Forms;
using Enadla_Counterfoil.Models;
using Enadla_Counterfoil;
using System.Linq;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class MainViewModel : ObservableObject
    {
        private EnadlaCounterfoil currentCounterfoil;
        private decimal profitOfTheDay;

        public decimal ProfitOfTheDay { get => this.profitOfTheDay; }
        public Color CorrectColorProfit
        {
            get
            {
                if (profitOfTheDay > 0)
                    return Color.LimeGreen;
                else if (profitOfTheDay < 0)
                    return Color.Red;
                else
                    return Color.Gray;
            }
        }

        public MainViewModel()
        {
            App currentApp = (App.Current as App);
            this.currentCounterfoil = currentApp.CurrentCounterfoil;

            currentApp.OnLoadCounterfoil += (executor, objectArgument) => 
            {
                this.currentCounterfoil = objectArgument.NewLoadedCounterfoil;

                this.currentCounterfoil.OnChanged += (sender, e) => { UpdateProfitOfTHeDay(); };

                UpdateProfitOfTHeDay();
            };
        }

        private void UpdateProfitOfTHeDay()
        {
            List<Sell> sellsOfTheDay = this.currentCounterfoil.GetTable<Sell>().Where(sell => sell.Date > DateTime.Today).ToList();
            List<FastExpense> expensesOfTheDay = this.currentCounterfoil.GetTable<FastExpense>().Where(ex => ex.Date >= DateTime.Today).ToList();

            decimal totalSells = (sellsOfTheDay.Count <= 0)? 0m : sellsOfTheDay.Sum(sell => sell.Total);
            decimal totalExpenses = (expensesOfTheDay.Count <= 0)? 0m: expensesOfTheDay.Sum(ex => ex.Amount);

            decimal profit = totalSells - totalExpenses;

            this.SetProperty<decimal>(ref profitOfTheDay, profit, nameof(ProfitOfTheDay));
            this.OnPropertyChanged(nameof(this.CorrectColorProfit));
        }
    }
}
