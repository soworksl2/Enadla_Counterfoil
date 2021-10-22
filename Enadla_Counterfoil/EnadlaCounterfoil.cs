using SQLite;
using System.IO;

namespace Enadla_Counterfoil
{
    public class EnadlaCounterfoil
    {
        private SQLiteConnection sourceDb;

        public EnadlaCounterfoil(string path)
        {
            bool isNewCounterfoil = File.Exists(path);
            sourceDb = new SQLiteConnection(path);
            if (isNewCounterfoil)
            {
                //Crear todas las tablas y poner la version de la base de dato
            }
        }
    }
}
