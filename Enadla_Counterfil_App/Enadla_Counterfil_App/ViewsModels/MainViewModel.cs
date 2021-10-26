using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Xamarin.Forms;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class MainViewModel : ObservableObject
    {
        private decimal profitOfTheDay = -20214.99M;

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
    }
}
