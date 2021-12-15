using Enadla_Counterfil_App.ViewsModels;
using Enadla_Counterfoil.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrUpdateSell : ContentPage
    {
        public AddOrUpdateSell(Sell sellToEdit = null)
        {
            this.BindingContext = new AddOrUpdateSellViewModel(sellToEdit);
            InitializeComponent();
        }

        private void dgvIndividualSellProducts_ItemsSourceChanged(object sender, Syncfusion.SfDataGrid.XForms.GridItemsSourceChangedEventArgs e)
        {
            if(e.OldView != null)
            {
                foreach (var column in this.dgvIndividualSellProducts.Columns)
                    this.dgvIndividualSellProducts.GridColumnSizer.ResetAutoWidth(column);
                this.dgvIndividualSellProducts.GridColumnSizer.Refresh(true);
            }
        }

        private async void dgvIndividualSellProducts_GridLongPressed(object sender, Syncfusion.SfDataGrid.XForms.GridLongPressedEventArgs e)
        {
            if (e.RowData == null)
                return;

            string message = $"Estas seguro que deseas borrar este registro '{(e.RowData as IndividualSelledProduct).ProductName}'";

            bool canDeleteRow = await this.DisplayAlert("advertencia", message, "Si", "No");

            if (canDeleteRow)
            {
                (this.BindingContext as AddOrUpdateSellViewModel).IndividualSelledProductsOfCurrentSell.Remove((e.RowData as IndividualSelledProduct));

                (this.BindingContext as AddOrUpdateSellViewModel).IndividualSelledProductsOfCurrentSell = new List<IndividualSelledProduct>((this.BindingContext as AddOrUpdateSellViewModel).IndividualSelledProductsOfCurrentSell);
            }
        }
    }
}