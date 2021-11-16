using System;
using System.Collections.Generic;
using System.Text;

namespace Enadla_Counterfil_App.Core
{
    public interface IExternalStorageProvider
    {
        string GetDirectory(App.DirectoriesApp directory);
    }
}
