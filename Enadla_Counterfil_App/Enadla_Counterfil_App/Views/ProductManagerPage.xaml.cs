using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enadla_Counterfil_App.ViewsModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductManagerPage : ContentPage
    {
        public ProductManagerPage()
        {
            this.BindingContext = new ProductManagerViewModel();
            InitializeComponent();
            (this.BindingContext as ProductManagerViewModel).LoadAllProductsByFilters();
        }
    }
}