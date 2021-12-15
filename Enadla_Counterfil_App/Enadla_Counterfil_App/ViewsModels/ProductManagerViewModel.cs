using Enadla_Counterfil_App.Views;
using Enadla_Counterfoil;
using Enadla_Counterfoil.Models;
using LinqKit;
using MvvmHelpers;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class ProductManagerViewModel : ObservableObject
    {
        private EnadlaCounterfoil currentCounterfoil;
        private string searchFilter;
        private bool isJustNotCheckedFilter;
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private Product selectedProduct;
        private MvvmHelpers.Commands.Command cmdClickOnAddProduct,
                                             cmdClickOnUpdateProduct,
                                             cmdClickOnSearch;
        private MvvmHelpers.Commands.AsyncCommand cmdClickOnDeleteProduct;

        public string SearchFilter
        {
            get => this.searchFilter;
            set
            {
                this.searchFilter = value;
            }
        }
        public bool IsJustNotCheckedFilter
        {
            get => this.isJustNotCheckedFilter;
            set
            {
                this.isJustNotCheckedFilter = value;
            }
        }
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
        public MvvmHelpers.Commands.Command CmdClickOnSearch => this.cmdClickOnSearch;
        public MvvmHelpers.Commands.Command CmdClickOnAddProduct => this.cmdClickOnAddProduct;
        public MvvmHelpers.Commands.Command CmdClickOnUpdateProduct => this.cmdClickOnUpdateProduct;
        public MvvmHelpers.Commands.AsyncCommand CmdClickOnDeleteProduct => this.cmdClickOnDeleteProduct;

        public ProductManagerViewModel()
        {
            this.currentCounterfoil = (App.Current as App).CurrentCounterfoil;
            this.currentCounterfoil.OnChanged += CurrentCounterfoil_OnChanged;

            this.cmdClickOnSearch = new MvvmHelpers.Commands.Command(ClickOnSearch);
            this.cmdClickOnAddProduct = new MvvmHelpers.Commands.Command(ClickOnAddProduct);
            this.cmdClickOnUpdateProduct = new MvvmHelpers.Commands.Command(ClickOnUpdateProduct, CanExecuteClickOnUpdateProduct);
            this.cmdClickOnDeleteProduct = new MvvmHelpers.Commands.AsyncCommand(ClickOnDeleteProduct, CanExecuteClickOnDeleteProduct);
        }

        private void CurrentCounterfoil_OnChanged(object sender, NotifyTableChangedEventArgs e)
        {
            if (e.Table.TableName != nameof(Product))
                return;

            this.LoadAllProductsByFilters();
        }

        public void LoadAllProductsByFilters()
        {
            bool isAllProducts = true;
            ExpressionStarter<Product> filter = PredicateBuilder.New<Product>(true);

            if (!string.IsNullOrWhiteSpace(this.searchFilter))
            {
                string procesedSearchFilter = this.searchFilter.ToLower();
                isAllProducts = false;
                filter.And(p => p.Name.ToLower().Contains(procesedSearchFilter) || p.Alias.ToLower().Contains(procesedSearchFilter));
            }
            if (isJustNotCheckedFilter)
            {
                isAllProducts = false;
                filter.And(p => !p.IsChecked);
            }

            TableQuery<Product> productTable = (App.Current as App).CurrentCounterfoil.GetTable<Product>();

            if (!isAllProducts)
            {
                productTable = productTable.Where(filter);
            }

            productTable = productTable.Take(100);

            List<Product> productsFound = productTable.ToList();
            this.SetProperty<ObservableCollection<Product>>(ref this.products, new ObservableCollection<Product>(productsFound), nameof(this.Products));
        }

        #region Method for commands

        private void ClickOnSearch()
        {
            LoadAllProductsByFilters();
        }

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
