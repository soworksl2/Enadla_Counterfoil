using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Enadla_Counterfil_App.Views;
using Enadla_Counterfoil;

namespace Enadla_Counterfil_App
{
    public partial class App : Application
    {
        private EnadlaCounterfoil counterfoil;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
