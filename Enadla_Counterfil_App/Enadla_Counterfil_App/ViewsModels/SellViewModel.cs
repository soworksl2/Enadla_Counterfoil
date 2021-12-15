using Enadla_Counterfoil.Models;
using MvvmHelpers;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Enadla_Counterfil_App.Views;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class SellViewModel : ObservableObject
    {
        private DateTime fromDate, toDate;
        private Sell selectedSell;
        private List<Sell> sells = new List<Sell>();
        private MvvmHelpers.Commands.Command cmdClickOnAdd,
                                             cmdClickOnUpdate;
        private MvvmHelpers.Commands.AsyncCommand cmdClickOnDelete;

        public DateTime FromDate
        {
            get => this.fromDate;
            set
            {
                this.SetProperty<DateTime>(ref this.fromDate, value, nameof(this.FromDate));

                this.FindAllSellByFilter();
            }
        }
        public DateTime ToDate
        {
            get => this.toDate;
            set
            {
                this.SetProperty<DateTime>(ref this.toDate, value, nameof(this.ToDate));

                FindAllSellByFilter();
            }
        }
        public Sell SelectedSell
        {
            get => this.selectedSell;
            set
            {
                this.SetProperty<Sell>(ref selectedSell, value, nameof(this.SelectedSell));
                this.cmdClickOnDelete.RaiseCanExecuteChanged();
                this.cmdClickOnUpdate.RaiseCanExecuteChanged();
            }
        }
        public List<Sell> Sells
        {
            get => this.sells;
            set
            {
                this.SetProperty<List<Sell>>(ref this.sells, value, nameof(this.Sells));
            }
        }
        public MvvmHelpers.Commands.Command CmdClickOnAdd => this.cmdClickOnAdd;
        public MvvmHelpers.Commands.AsyncCommand CmdClickOnDelete => this.cmdClickOnDelete;
        public MvvmHelpers.Commands.Command CmdClickOnUpdate => this.cmdClickOnUpdate;

        public SellViewModel()
        {


            #region Assingin Commands

            this.cmdClickOnAdd = new MvvmHelpers.Commands.Command(this.ClickOnAdd);
            this.cmdClickOnDelete = new MvvmHelpers.Commands.AsyncCommand(this.ClickOnDelete, this.CanExecuteClickOnDelete);
            this.cmdClickOnUpdate = new MvvmHelpers.Commands.Command(this.ClickOnUpdate, this.CanExecuteClickOnUpdate);

            #endregion
        }

        private void FindAllSellByFilter()
        {
            DateTime toDateProcessed = this.toDate.AddHours(23);

            this.Sells = (App.Current as App).CurrentCounterfoil.GetConnection().GetAllWithChildren<Sell>(s => s.Date >= this.fromDate && s.Date <= toDateProcessed, true);
        }

        #region methods to commands

        private void ClickOnAdd()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateSell());
        }

        private async Task ClickOnDelete()
        {
            bool canDeleteElement = await (App.Current.MainPage as NavigationPage).CurrentPage.DisplayAlert("Advertencia", "Estas seguro que deseas eliminar este elemento", "Si", "No");

            if (canDeleteElement)
            {
                (App.Current as App).CurrentCounterfoil.GetConnection().Delete(this.SelectedSell, true);

                this.Sells.Remove(SelectedSell);
                this.Sells = new List<Sell>(Sells);
            }
        }
        private bool CanExecuteClickOnDelete(object o)
        {
            return this.selectedSell != null;
        }

        private void ClickOnUpdate()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateSell(this.selectedSell));
        }
        private bool CanExecuteClickOnUpdate()
        {
            return this.selectedSell != null;
        }

        #endregion
    }
}
