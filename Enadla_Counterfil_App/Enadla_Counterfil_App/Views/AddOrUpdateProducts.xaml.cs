using Enadla_Counterfil_App.ViewsModels;
using Enadla_Counterfoil.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrUpdateProducts : ContentPage
    {
        public AddOrUpdateProducts(Product productToEdit = null)
        {
            this.BindingContext = new AddOrUpdateProductsViewModel(productToEdit);
            InitializeComponent();
        }
    }
}