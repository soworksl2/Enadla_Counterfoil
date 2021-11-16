using MvvmHelpers;
using Xamarin.Forms;
using Enadla_Counterfil_App.Views;
using System.Collections.ObjectModel;
using Enadla_Counterfoil.Models;
using SQLite;
using System.Collections.Generic;
using Enadla_Counterfoil;
using System.Linq;
using System.Threading.Tasks;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class ProductManagerViewModel : ObservableObject
    {
        private EnadlaCounterfoil currentCounterfoil;
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private Product selectedProduct;
        private MvvmHelpers.Commands.Command cmdClickOnAddProduct,
                                             cmdClickOnUpdateProduct;
        private MvvmHelpers.Commands.AsyncCommand cmdClickOnDeleteProduct;

        public ObservableCollection<Product> Products
        {
            get => this.products;
        }
        public Product SelectedProduct
        {
            get => this.selectedProduct;
            set
            {
                this.SetProperty<Product>(ref this.selectedProduct, value, nameof(this.SelectedProduct));
                this.CmdClickOnUpdateProduct.RaiseCanExecuteChanged();
                this.cmdClickOnDeleteProduct.RaiseCanExecuteChanged();
            }
        }
        public MvvmHelpers.Commands.Command CmdClickOnAddProduct => this.cmdClickOnAddProduct;
        public MvvmHelpers.Commands.Command CmdClickOnUpdateProduct => this.cmdClickOnUpdateProduct;
        public MvvmHelpers.Commands.AsyncCommand CmdClickOnDeleteProduct => this.cmdClickOnDeleteProduct;

        public ProductManagerViewModel()
        {
            this.currentCounterfoil = (App.Current as App).CurrentCounterfoil;
            this.currentCounterfoil.OnChanged += CurrentCounterfoil_OnChanged;

            this.cmdClickOnAddProduct = new MvvmHelpers.Commands.Command(ClickOnAddProduct);
            this.cmdClickOnUpdateProduct = new MvvmHelpers.Commands.Command(ClickOnUpdateProduct, CanExecuteClickOnUpdateProduct);
            this.cmdClickOnDeleteProduct = new MvvmHelpers.Commands.AsyncCommand(ClickOnDeleteProduct, CanExecuteClickOnDeleteProduct);
        }

        private void CurrentCounterfoil_OnChanged(object sender, NotifyTableChangedEventArgs e)
        {
            if(e.Table.TableName != nameof(Product))
                return;

            this.LoadAllProductsByFilters();
        }

        public void LoadAllProductsByFilters()
        {
            TableQuery<Product> productTable = (App.Current as App).CurrentCounterfoil.GetTable<Product>();
            List<Product> productsFound = productTable.ToList();
            this.SetProperty<ObservableCollection<Product>>(ref this.products, new ObservableCollection<Product>(productsFound), nameof(this.Products));
        }

        #region Method for commands

        private void ClickOnAddProduct()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateProducts(), true);
        }

        private bool CanExecuteClickOnUpdateProduct() => this.selectedProduct != null;
        private void ClickOnUpdateProduct()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateProducts(this.selectedProduct), true);
        }

        private async Task ClickOnDeleteProduct()
        {
            string alertMessage = "Estas segura de que deseas eliminar este registro?";
            bool canDelete = await (App.Current.MainPage as NavigationPage).CurrentPage.DisplayAlert("Confirmacion", alertMessage, "Si", "No");

            if (canDelete)
            {
                this.currentCounterfoil.Delete(selectedProduct);
            }
        }
        private bool CanExecuteClickOnDeleteProduct(object e)
        {
            return this.selectedProduct != null;
        }

        #endregion
    }
}
