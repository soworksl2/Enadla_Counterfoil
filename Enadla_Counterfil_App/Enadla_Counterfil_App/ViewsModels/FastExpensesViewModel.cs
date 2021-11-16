using Enadla_Counterfil_App.Views;
using Enadla_Counterfoil;
using Enadla_Counterfoil.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class FastExpensesViewModel : ObservableObject
    {
        private EnadlaCounterfoil currentCounterfoil;

        private bool isAllExpenses;
        private DateTime dateFrom, dateTo;
        private List<FastExpense> fastExpensesCollection = new List<FastExpense>();
        private FastExpense selectedFastExpense;
        private MvvmHelpers.Commands.Command cmdAddNewExpense,
                                             cmdEditExpense;
        private MvvmHelpers.Commands.AsyncCommand cmdDeleteExpense;

        public List<FastExpense> FastExpensesCollection => this.fastExpensesCollection;
        public FastExpense SelectedFastExpense
        {
            get => this.selectedFastExpense;
            set
            {
                this.SetProperty<FastExpense>(ref this.selectedFastExpense, value, nameof(this.SelectedFastExpense));
                try
                {
                    this.cmdDeleteExpense.RaiseCanExecuteChanged();
                    this.CmdEditExpense.RaiseCanExecuteChanged();
                }
                catch (ArgumentNullException e)
                {

                }
            }
        }
        public bool IsAllExpenses
        {
            get => this.isAllExpenses;
            set
            {
                this.SetProperty<bool>(ref this.isAllExpenses, value, nameof(this.IsAllExpenses));
            }
        }
        public DateTime DateFrom
        {
            get { return this.dateFrom; }
            set
            {
                this.SetProperty<DateTime>(ref this.dateFrom, value, nameof(DateFrom));
                this.UpdateFastExpenses();
            }
        }
        public DateTime DateTo
        {
            get => this.dateTo;
            set
            {
                this.SetProperty<DateTime>(ref this.dateTo, value, nameof(this.DateTo));
                this.UpdateFastExpenses();
            }
        }
        public MvvmHelpers.Commands.Command CmdAddNewExpense => cmdAddNewExpense;
        public MvvmHelpers.Commands.Command CmdEditExpense => cmdEditExpense;
        public MvvmHelpers.Commands.AsyncCommand CmdDeleteExpense => cmdDeleteExpense;

        public FastExpensesViewModel()
        {
            this.currentCounterfoil = (App.Current as App).CurrentCounterfoil;

            this.currentCounterfoil.OnChanged += (sender, argument) => { this.UpdateFastExpenses(); };

            #region initializing commands
            this.cmdAddNewExpense = new MvvmHelpers.Commands.Command(OnAddNewExpense);
            this.cmdDeleteExpense = new MvvmHelpers.Commands.AsyncCommand(OnDeleteExpense, CanExecuteOnDeleteExpense);
            this.cmdEditExpense = new MvvmHelpers.Commands.Command(OnEditExpense, CanExecuteOnEditExpense);
            #endregion
        }

        private void OnAddNewExpense()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateFastExpense(), true);
        }

        private void OnEditExpense()
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new AddOrUpdateFastExpense(this.selectedFastExpense));
        }
        private bool CanExecuteOnEditExpense() => this.selectedFastExpense != null;

        private async Task OnDeleteExpense()
        {
            bool wantDeleteTheRegister = await (App.Current.MainPage as NavigationPage).CurrentPage.DisplayAlert("Advertencia", "Estas seguro de querer borrar este registro", "Si", "No");

            if (wantDeleteTheRegister)
            {
                (App.Current as App).CurrentCounterfoil.Delete(this.selectedFastExpense);
            }
        }
        private bool CanExecuteOnDeleteExpense(object e) => this.selectedFastExpense != null;

        private void LoadFastExpensesByFilters()
        {
            if (isAllExpenses)
            {
                List<FastExpense> _fastExpensesFound = this.currentCounterfoil.GetTable<FastExpense>().ToList();
                this.SetProperty<List<FastExpense>>(ref this.fastExpensesCollection, _fastExpensesFound, nameof(this.FastExpensesCollection));
                return;
            }

            DateTime lastTimeOfTheDayTo = this.dateTo.AddHours(23);

            List<FastExpense> fastExpensesFound = this.currentCounterfoil.GetTable<FastExpense>()
                .Where(x => x.Date >= this.dateFrom && x.Date <= lastTimeOfTheDayTo)
                .OrderBy(x => x.Date)
                .ToList();

            this.SetProperty<List<FastExpense>>(ref this.fastExpensesCollection, fastExpensesFound, nameof(this.FastExpensesCollection));
        }

        public void UpdateFastExpenses()
        {
            this.LoadFastExpensesByFilters();
        }
    }
}
