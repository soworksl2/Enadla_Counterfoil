using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Enadla_Counterfil_App.Views;
using Enadla_Counterfoil;
using System.IO;
using Enadla_Counterfil_App.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Enadla_Counterfil_App
{
    public partial class App : Application
    {
        private IExternalStorageProvider externalStorageProvider;
        private DataApp dataApp = new DataApp();
        private EnadlaCounterfoil currentCounterfoil;

        public EnadlaCounterfoil CurrentCounterfoil => this.currentCounterfoil;

        public App(IExternalStorageProvider externalStorageProvider)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI3MzcxQDMxMzkyZTMzMmUzMG5pdngvNUprOVlwZHMydU5CcGdsaWx6U0U3ckt4MFdqbUxKR2tlRS9MREk9");

            InitializeComponent();
            this.externalStorageProvider = externalStorageProvider;

            MainPage = new NavigationPage(new MainPage());

            LoadDataApp();

        }

        private async Task GetAllImportantPermissions()
        {
            string alertMessage = "Esta aplicacion necesita los permisos de escritora y lectura en el dispositivo para poder funcionar, de lo contrario no podra hacerlo";

            //Get Write Permission
            PermissionStatus writePermissionStatus, readPermissionStatus;

            writePermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            while(writePermissionStatus != PermissionStatus.Granted ||readPermissionStatus != PermissionStatus.Granted)
            {
                await (this.MainPage as NavigationPage).CurrentPage.DisplayAlert("Permisos necesarios", alertMessage, "Ok");

                writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                readPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
        }

        private void LoadDataApp()
        {
            string dataAppFullPath = GetFullPathFiles(FilesApp.DataApp);
            if (!File.Exists(dataAppFullPath))
            {
                this.dataApp = new DataApp();
                string serializedDataApp = JsonConvert.SerializeObject(this.dataApp);
                File.WriteAllText(dataAppFullPath, serializedDataApp);
                return;
            }

            this.dataApp = JsonConvert.DeserializeObject<DataApp>(File.ReadAllText(dataAppFullPath));
        }

        public string GetDirectoryApp(DirectoriesApp specificDirectorie)
        {
            switch (specificDirectorie)
            {
                case DirectoriesApp.personal:
                    return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                case DirectoriesApp.CounterfoilVault:
                    return this.externalStorageProvider.GetDirectory(specificDirectorie);
                default:
                    throw new ArgumentException("the directories is not defined");
            }
        }

        public string GetFullPathFiles(FilesApp specificFile)
        {
            switch (specificFile)
            {
                case FilesApp.DataApp:
                    return Path.Combine(GetDirectoryApp(DirectoriesApp.personal), $"{nameof(DataApp)}.{DataApp.EXTENSION}");
                default:
                    throw new ArgumentException("the file is not defined");
            }
        }

        public void LoadCounterfoil(string fullPathToCounterfoil)
        {
            if (string.IsNullOrWhiteSpace(fullPathToCounterfoil))
            {
                string mainCounterfoilPath = 
                    Path.Combine(GetDirectoryApp(DirectoriesApp.CounterfoilVault), $"Main.{EnadlaCounterfoil.DEFAULT_EXTENSION}");

                this.currentCounterfoil = new EnadlaCounterfoil(mainCounterfoilPath);
            }
            else
            {
                this.currentCounterfoil = new EnadlaCounterfoil(fullPathToCounterfoil, false);
            }

            OnLoadCounterfoil?.Invoke(this, new OnLoadCounterfoilArguments(this.currentCounterfoil));
        }

        #region events

        public event EventHandler<OnLoadCounterfoilArguments> OnLoadCounterfoil;

        #endregion

        #region main mobile app events

        protected override async void OnStart()
        {
            await Task.Yield(); //Wait until the app is rendered

            await GetAllImportantPermissions();

            LoadCounterfoil(this.dataApp.lastOpenedCounterfoilPath);
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            await Task.Yield();

            await GetAllImportantPermissions();
        }


        #endregion

        public enum DirectoriesApp
        {
            personal, CounterfoilVault
        }

        public enum FilesApp
        {
            DataApp
        }

        public class OnLoadCounterfoilArguments
        {
            public OnLoadCounterfoilArguments(EnadlaCounterfoil enadlaCounterfoil)
            {
                this.NewLoadedCounterfoil = enadlaCounterfoil;
            }

            public EnadlaCounterfoil NewLoadedCounterfoil { get; set; }
        }
    }
}
