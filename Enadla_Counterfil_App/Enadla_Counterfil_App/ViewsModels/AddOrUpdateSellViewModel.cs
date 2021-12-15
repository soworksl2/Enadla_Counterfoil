using Enadla_Counterfoil;
using Enadla_Counterfoil.Models;
using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using SQLiteNetExtensions.Extensions;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class AddOrUpdateSellViewModel : ObservableObject
    {
        private string correctTitle;
        private Sell currentSell;
        private List<IndividualSelledProduct> individualSelledProductsOfCurrentSell;
        private List<Product> inexistentProductsToAdd = new List<Product>();
        private ObservableCollection<Product> allProducts;
        private MvvmHelpers.Commands.Command cmdClickOnCancel,
                                             cmdClickOnInsert,
                                             cmdClickOnConfirmSell;

        public string CorrectTitle => this.correctTitle;
        public Sell CurrentSell => this.currentSell;
        public List<IndividualSelledProduct> IndividualSelledProductsOfCurrentSell 
        {
            get => this.individualSelledProductsOfCurrentSell;
            set
            {
                this.SetProperty<List<IndividualSelledProduct>>(ref this.individualSelledProductsOfCurrentSell, value, nameof(this.IndividualSelledProductsOfCurrentSell));

                this.OnPropertyChanged(nameof(this.Total));
                this.cmdClickOnConfirmSell.RaiseCanExecuteChanged();
            }
        }
        public List<Product> InexistentproductsToAdd => this.inexistentProductsToAdd;
        public decimal Total 
        {
            get => this.individualSelledProductsOfCurrentSell.Sum(i => i.Total);
        }
        public ObservableCollection<Product> AllProducts => this.allProducts;
        public MvvmHelpers.Commands.Command CmdClickOnCancel => this.cmdClickOnCancel;
        public MvvmHelpers.Commands.Command CmdClickOnInsert => this.cmdClickOnInsert;
        public MvvmHelpers.Commands.Command CmdClickOnConfirmSell => this.cmdClickOnConfirmSell;

        #region Insertion Options properties

        private decimal quantity;
        private string nameWishedProduct;
        private decimal selectedPrice;

        private bool isAdvancedInsertion;

        private decimal totalSetQuantity;
        private decimal extraQuantity;
        private bool isUnitPrice;

        public decimal Quantity
        {
            get => this.quantity;
            set
            {
                this.SetProperty<decimal>(ref this.quantity, value, nameof(this.Quantity));
                this.cmdClickOnInsert.RaiseCanExecuteChanged();
            }
        }
        public string NameWishedProduct
        {
            get => this.nameWishedProduct;
            set
            {
                this.SetProperty<string>(ref this.nameWishedProduct, value, nameof(this.NameWishedProduct));
                this.cmdClickOnInsert.RaiseCanExecuteChanged();
            }
        }
        public decimal SelectedPrice
        {
            get => this.selectedPrice;
            set
            {
                this.SetProperty<decimal>(ref this.selectedPrice, value, nameof(this.SelectedPrice));
            }
        }

        public bool IsAdvanceInsertion
        {
            get => this.isAdvancedInsertion;
            set
            {
                this.SetProperty<bool>(ref this.isAdvancedInsertion, value, nameof(this.IsAdvanceInsertion));
                if (!value)
                    this.ClearInsertionForm(false, true);

                this.cmdClickOnInsert.RaiseCanExecuteChanged();
            }
        }

        public decimal TotalSetQuantity
        {
            get => this.totalSetQuantity;
            set
            {
                this.SetProperty<decimal>(ref this.totalSetQuantity, value, nameof(this.TotalSetQuantity));

                this.cmdClickOnInsert.RaiseCanExecuteChanged();
            }
        }
        public decimal ExtraQuantity
        {
            get => this.extraQuantity;
            set
            {
                this.SetProperty<decimal>(ref this.extraQuantity, value, nameof(this.ExtraQuantity));

                this.cmdClickOnInsert.RaiseCanExecuteChanged();
            }
        }
        public bool IsUnitPrice
        {
            get => this.isUnitPrice;
            set
            {
                this.SetProperty<bool>(ref this.isUnitPrice, value, nameof(this.IsUnitPrice));
            }
        }

        #endregion

        public AddOrUpdateSellViewModel(Sell sellToEdit = null)
        {
            if (sellToEdit == null)
            {
                this.correctTitle = "Nueva venta";
                this.currentSell = new Sell()
                {
                    Date = DateTime.Now
                };
            }
            else
            {
                this.correctTitle = "Modificar venta";
                this.currentSell = sellToEdit;
            }

            this.allProducts = new ObservableCollection<Product>(GetAllProducts());

            this.individualSelledProductsOfCurrentSell = FindIndividualSelledProductsOf(this.currentSell);

            //Initializing all commands
            this.cmdClickOnCancel = new MvvmHelpers.Commands.Command(ClickOnCancel);
            this.cmdClickOnInsert = new MvvmHelpers.Commands.Command(ClickOnInsert, CanExecuteClickOnInsert);
            this.cmdClickOnConfirmSell = new MvvmHelpers.Commands.Command(ClickOnConfirmSell, CanExecuteClickOnConfirmSell);
        }

        private void ClearInsertionForm(bool clearBasicInsertionForm = true, bool clearAdvanceInsertionForm = true)
        {
            if (clearBasicInsertionForm)
            {
                this.Quantity = 0m;
                this.NameWishedProduct = string.Empty; 
                this.SelectedPrice = 0m;
            }

            if (clearAdvanceInsertionForm)
            {
                this.TotalSetQuantity = 0m;
                this.ExtraQuantity = 0m;
                this.IsUnitPrice = false;
            }
        }

        private List<IndividualSelledProduct> FindIndividualSelledProductsOf(Sell sell)
        {
            EnadlaCounterfoil currentCounterfoil = (App.Current as App).CurrentCounterfoil;

            TableQuery<IndividualSelledProduct> individualSelledProductSection = currentCounterfoil.GetTable<IndividualSelledProduct>();

            List<IndividualSelledProduct> individualSelledProducts = individualSelledProductSection.Where(s => s.SellId == sell.SellId).ToList();

            foreach (IndividualSelledProduct individual in individualSelledProducts)
            {
                individual.Product = this.AllProducts.First(p => p.ProductId == individual.ProductId);
            }

            return individualSelledProducts;
        }

        private List<Product> GetAllProducts()
        {
            EnadlaCounterfoil currentCounterfoil = (App.Current as App).CurrentCounterfoil;

            return currentCounterfoil.GetTable<Product>().ToList();
        }

        #region methods to commands
        private void ClickOnCancel()
        {
            (App.Current.MainPage as NavigationPage).PopAsync(true);
        }

        private void ClickOnInsert()
        {
            IndividualSelledProduct newIndividualSelledProduct = new IndividualSelledProduct();

            newIndividualSelledProduct.IsChecked = false;

            newIndividualSelledProduct.Quantity = this.Quantity;

            Product newProductToIndividualSelledProduct = null;

            foreach(Product p in allProducts)
            {
                if(p.Name.ToLower() == this.nameWishedProduct.ToLower())
                {
                    newProductToIndividualSelledProduct = p;
                    break;
                }
            }

            if (newProductToIndividualSelledProduct == null)
            {
                newProductToIndividualSelledProduct = new Product()
                {
                    IsChecked = false,
                    Name = this.nameWishedProduct
                };

                this.allProducts.Add(newProductToIndividualSelledProduct);
                this.inexistentProductsToAdd.Add(newProductToIndividualSelledProduct);
            }

            newIndividualSelledProduct.Product = newProductToIndividualSelledProduct;

            newIndividualSelledProduct.UnitPrice = this.selectedPrice;

            if (this.isAdvancedInsertion)
            {
                newIndividualSelledProduct.Quantity = (this.quantity / this.totalSetQuantity) + this.extraQuantity;

                if (this.isUnitPrice)
                {
                    newIndividualSelledProduct.UnitPrice = this.selectedPrice * this.quantity;
                }
            }
            this.individualSelledProductsOfCurrentSell.Add(newIndividualSelledProduct);
            this.IndividualSelledProductsOfCurrentSell = new List<IndividualSelledProduct>(this.IndividualSelledProductsOfCurrentSell);

            ClearInsertionForm();

        }
        private bool CanExecuteClickOnInsert()
        {
            if (this.Quantity <= 0m)
                return false;
            if (string.IsNullOrWhiteSpace(this.NameWishedProduct))
                return false;

            if (this.isAdvancedInsertion)
            {
                if (this.Quantity > this.TotalSetQuantity)
                    return false;

                if (this.extraQuantity < 0)
                    return false;
            }

            return true;
        }

        private void ClickOnConfirmSell()
        {
            if(this.correctTitle == "Modificar venta")
            {
                (App.Current as App).CurrentCounterfoil.GetConnection().DeleteAll(this.currentSell.IndividualSelledProducts);
            }

            this.currentSell.IndividualSelledProducts = this.individualSelledProductsOfCurrentSell;

            (App.Current as App).CurrentCounterfoil.GetConnection().InsertOrReplaceWithChildren(this.currentSell, true);

            (App.Current.MainPage as NavigationPage).PopAsync(true);
        }
        private bool CanExecuteClickOnConfirmSell()
        {
            return this.IndividualSelledProductsOfCurrentSell.Count > 0;
        }
        #endregion
    }
}
