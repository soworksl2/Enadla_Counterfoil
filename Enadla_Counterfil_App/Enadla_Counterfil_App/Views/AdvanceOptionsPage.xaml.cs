using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enadla_Counterfil_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvanceOptionsPage : ContentPage
    {
        public AdvanceOptionsPage()
        {
            InitializeComponent();
        }

        private void ClickOnProductManager(object sender, EventArgs e)
        {
            (App.Current.MainPage as NavigationPage).PushAsync(new ProductManagerPage(), true);
        }
    }
}