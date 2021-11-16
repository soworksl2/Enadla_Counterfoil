using Enadla_Counterfoil.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Enadla_Counterfil_App.ViewsModels
{
    public class AddOrUpdateFastExpenseViewModel : ObservableObject
    {
        private string correctTitle;
        private bool isToEdit;
        private FastExpense currentFastExpense;
        private MvvmHelpers.Commands.AsyncCommand cmdOnAccept;

        public string CorrectTitle
        {
            get { return this.correctTitle; }
            set
            {
                this.SetProperty(ref this.correctTitle, value, nameof(this.CorrectTitle));
            }
        }
        public FastExpense CurrentFastExpense
        {
            get => this.currentFastExpense;
        }
        public MvvmHelpers.Commands.AsyncCommand CmdOnAccept { get => this.cmdOnAccept; }

        public AddOrUpdateFastExpenseViewModel(FastExpense fastExpenseToEdit = null)
        {
            if (fastExpenseToEdit == null)
            {
                this.CorrectTitle = "Agregar Gasto Rapido";
                isToEdit = false;
                currentFastExpense = new FastExpense() { Concept = "General", Date = System.DateTime.Now };
            }
            else
            {
                this.CorrectTitle = "Modificar Gasto Rapido";
                isToEdit = true;
                currentFastExpense = new FastExpense() 
                {
                    IdFastExpense = fastExpenseToEdit.IdFastExpense,
                    Amount = fastExpenseToEdit.Amount,
                    Concept = fastExpenseToEdit.Concept,
                    Date = fastExpenseToEdit.Date
                };
            }

            this.currentFastExpense.PropertyChanged += (sender, objectArgument) =>
            {
                this.OnPropertyChanged(nameof(objectArgument.PropertyName));
                this.cmdOnAccept.RaiseCanExecuteChanged();
            };

            #region initializing commands

            this.cmdOnAccept = new MvvmHelpers.Commands.AsyncCommand(OnClickOnAccept, CanExecuteOnClickOnAccept);

            #endregion
        }

        private async Task OnClickOnAccept()
        {
            bool canBack = false;
            App currentApp = (App)App.Current;
            NavigationPage mainPage = (NavigationPage)currentApp.MainPage;

            if (!isToEdit)
            {
                currentApp.CurrentCounterfoil.Insert(this.currentFastExpense);
                canBack = true;
            }
            else
            {
                bool confirmation = await mainPage.CurrentPage.DisplayAlert("Confirmacion", "estas seguro que deseas editar este registro", "Si", "No");

                if(confirmation)
                {
                    currentApp.CurrentCounterfoil.Update(this.currentFastExpense);
                    canBack = true;
                }
            }

            if(canBack)
                mainPage.PopAsync(true);
        }

        private bool CanExecuteOnClickOnAccept(object e)
        {
            if (string.IsNullOrWhiteSpace(this.currentFastExpense.Concept))
                return false;

            if (this.currentFastExpense.Amount <= 0m)
                return false;

            return true;
        }
    }
}
