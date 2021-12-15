using MvvmHelpers;
using Enadla_Counterfoil.Models;
using Xamarin.Forms;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class AddOrUpdateProductsViewModel : ObservableObject
    {
        private Product currentProduct;
        private bool isEditMode;
        private string correctTitle;
        private MvvmHelpers.Commands.Command cmdClickOnAcceptAndContinue,
                                             cmdClickOnAccept,
                                             cmdClickOnCancel;

        public string CorrectTitle => this.correctTitle;
        public Product CurrentProduct
        {
            get => this.currentProduct;
            private set
            {
                this.SetProperty<Product>(ref this.currentProduct, value, nameof(this.CurrentProduct));
                this.currentProduct.PropertyChanged += (sender, e) => { 
                    this.cmdClickOnAcceptAndContinue?.RaiseCanExecuteChanged();
                    this.cmdClickOnAccept?.RaiseCanExecuteChanged();
                };
                this.cmdClickOnAcceptAndContinue?.RaiseCanExecuteChanged();
                this.cmdClickOnAccept?.RaiseCanExecuteChanged();
            }
        }
        public MvvmHelpers.Commands.Command CmdClickOnAcceptAndContinue => this.cmdClickOnAcceptAndContinue;
        public MvvmHelpers.Commands.Command CmdClickOnAccept => this.cmdClickOnAccept;
        public MvvmHelpers.Commands.Command CmdClickOnCancel => this.cmdClickOnCancel;


        public AddOrUpdateProductsViewModel(Product productToEdit = null)
        {
            if(productToEdit == null)
            {
                this.CurrentProduct = new Product();
                this.currentProduct.IsChecked = true;
                this.isEditMode = false;
                this.correctTitle = "Crear producto";
            }
            else
            {
                this.CurrentProduct = (Product)productToEdit.Clone();
                this.currentProduct.IsChecked = true;
                this.isEditMode = true;
                this.correctTitle = "editar producto";
            }

            #region Initializing all commands
            this.cmdClickOnAcceptAndContinue = new MvvmHelpers.Commands.Command(this.ClickOnAcceptAndContinue, this.CanExecuteClickOnAcceptAndContinue);
            this.cmdClickOnAccept = new MvvmHelpers.Commands.Command(this.ClickOnAccept, this.CanExecuteClickOnAccept);
            this.cmdClickOnCancel = new MvvmHelpers.Commands.Command(this.ClickOnCancel);
            #endregion
        }

        private bool CanSaveTheCurrentProduct()
        {
            return !string.IsNullOrWhiteSpace(this.currentProduct.Name);
        }

        #region Methods for commands

        private void ClickOnAcceptAndContinue()
        {
            (App.Current as App).CurrentCounterfoil.Insert(this.currentProduct);
            this.CurrentProduct = new Product()
            {
                IsChecked = true
            };
        }
        private bool CanExecuteClickOnAcceptAndContinue()
        {
            return (this.isEditMode == false && this.CanSaveTheCurrentProduct());
        }

        private void ClickOnAccept()
        {
            if (!isEditMode)
                (App.Current as App).CurrentCounterfoil.Insert(this.currentProduct);
            else
                (App.Current as App).CurrentCounterfoil.Update(this.currentProduct);
            (App.Current.MainPage as NavigationPage).PopAsync(true);
        }
        private bool CanExecuteClickOnAccept() => this.CanSaveTheCurrentProduct();

        private void ClickOnCancel()
        {
            (App.Current.MainPage as NavigationPage).PopAsync(true);
        }

        #endregion

    }
}
