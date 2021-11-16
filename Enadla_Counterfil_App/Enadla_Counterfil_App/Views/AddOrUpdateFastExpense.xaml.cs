using Enadla_Counterfoil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Enadla_Counterfil_App.ViewsModels;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrUpdateFastExpense : ContentPage
    {
        public AddOrUpdateFastExpense(FastExpense expenseToEdit = null)
        {
            this.BindingContext = new AddOrUpdateFastExpenseViewModel(expenseToEdit);
            InitializeComponent();
        }

        private void OnConceptEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as Entry).Text))
                (sender as Entry).Text = "General";
        }

        private void OnClickCancelBtn(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PopAsync(true);
        }
    }
}