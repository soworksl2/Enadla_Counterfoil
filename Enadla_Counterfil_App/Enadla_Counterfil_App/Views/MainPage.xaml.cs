using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void ClickOnAddFastExpenses(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateFastExpense(), true);
        }

        private void ClickOnAllFastExpenses(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new FastExpensesPage(), true);
        }

        private void ClickOnSell(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new SellPage(), true);
        }

        private void ClickOnAdvanceOptions(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AdvanceOptionsPage(), true);
        }
    }
}