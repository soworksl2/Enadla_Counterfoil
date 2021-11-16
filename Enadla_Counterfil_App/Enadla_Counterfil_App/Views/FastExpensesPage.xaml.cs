using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Enadla_Counterfil_App.ViewsModels;
using Enadla_Counterfoil.Models;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FastExpensesPage : ContentPage
    {
        private FastExpensesViewModel castedBindingContext;

        public FastExpensesPage()
        {
            this.BindingContext = new FastExpensesViewModel();
            InitializeComponent();

            this.castedBindingContext = (FastExpensesViewModel)this.BindingContext;
            this.castedBindingContext.UpdateFastExpenses();
        }

        private void OnIsAllExpensesChange(object sender, CheckedChangedEventArgs e)
        {
            this.DateFromPicker.IsEnabled = !e.Value;
            this.DateToPicker.IsEnabled = !e.Value;
            this.castedBindingContext.UpdateFastExpenses();
        }

        private void dgvFastExpenses_ItemsSourceChanged(object sender, Syncfusion.SfDataGrid.XForms.GridItemsSourceChangedEventArgs e)
        {
            if (e.OldView != null)
            {
                foreach (var column in this.dgvFastExpenses.Columns)
                    this.dgvFastExpenses.GridColumnSizer.ResetAutoWidth(column);
                this.dgvFastExpenses.GridColumnSizer.Refresh(true);
            }

            this.dgvFastExpenses_SelectionChanged(this, new Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs(this.dgvFastExpenses));
        }

        private void dgvFastExpenses_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            (this.BindingContext as FastExpensesViewModel).SelectedFastExpense = (FastExpense)this.dgvFastExpenses.SelectedItem;
        }
    }
}