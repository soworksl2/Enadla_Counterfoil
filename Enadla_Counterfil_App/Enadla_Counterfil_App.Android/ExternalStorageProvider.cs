using Enadla_Counterfil_App.Core;
using System;
using System.IO;

namespace Enadla_Counterfil_App.Droid
{
    public class ExternalStorageProvider : IExternalStorageProvider
    {
        public string GetDirectory(App.DirectoriesApp directory)
        {
            switch (directory)
            {
                case App.DirectoriesApp.CounterfoilVault:
                    string countefoilVaultPath = Path.Combine("storage/emulated/0/Documents", "Enadla_Talonarios");
                    Directory.CreateDirectory(countefoilVaultPath);
                    return countefoilVaultPath;
                default:
                    throw new ArgumentException("The directory is not defined");
            }
        }
    }
}