using Enadla_Counterfoil.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Enadla_Counterfoil
{
    public class EnadlaCounterfoil : IDisposable
    {
        private const string VERSION_COUNTERFOIL = "1.0.0";
        public const string RESTRICTED_DATA_KEYS = "version";
        public const string DEFAULT_EXTENSION = "ecf";

        private SQLiteConnection counterfoilDbSourceConnection;
        private bool isBlockedInternalData = false;
        private IEnumerable<object> lastModelsOperating;

        public bool IsOpen { get; private set; }
        public string SavePath
        {
            get { return this.counterfoilDbSourceConnection.DatabasePath; }
        }
        public IEnumerable<object> LastModelsOperating => this.lastModelsOperating;

        public EnadlaCounterfoil(string savePath, bool createIfNoExist = true)
        {
            bool isCreatingNewDb = !File.Exists(savePath);

            if (!createIfNoExist && isCreatingNewDb)
                throw new Exception("the counterfoil cannot be load because it not exist and CreateIfNotExist is false");

            this.counterfoilDbSourceConnection = new SQLiteConnection(savePath);

            if (isCreatingNewDb)
            {
                CreateAllTables();

                this.SetData("version", VERSION_COUNTERFOIL);
            }

            this.counterfoilDbSourceConnection.TableChanged += CounterfoilDbSourceConnection_TableChanged;

            isBlockedInternalData = true;
        }

        private void CounterfoilDbSourceConnection_TableChanged(object sender, NotifyTableChangedEventArgs e)
        {
            OnChanged?.Invoke(sender, e);
        }

        private void CreateAllTables()
        {
            this.counterfoilDbSourceConnection.CreateTable<CounterfoilDictionaryTable>();
            this.counterfoilDbSourceConnection.CreateTable<Product>();
            this.counterfoilDbSourceConnection.CreateTable<Sell>();
            this.counterfoilDbSourceConnection.CreateTable<IndividualSelledProduct>();
            this.counterfoilDbSourceConnection.CreateTable<FastExpense>();
        }

        #region Public methods
        public void Dispose()
        {
            this.counterfoilDbSourceConnection.Dispose();
        }

        public string GetData(string key)
        {
            bool existKey = this.counterfoilDbSourceConnection.Table<CounterfoilDictionaryTable>()
                     .Where(rows => rows.Key == key)
                     .Count() >= 1;

            if (!existKey)
                return null;
            return this.counterfoilDbSourceConnection.Get<CounterfoilDictionaryTable>(key).Content;
        }

        public void SetData(string key, string content)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "the key cannot be null");

            if (isBlockedInternalData && RESTRICTED_DATA_KEYS.Contains(key))
                throw new ArgumentException(nameof(key), $"{key} isn't a valid key to modify");

            if (content == null) //want delete the key and its content if exist
            {
                bool existKey = this.counterfoilDbSourceConnection.Table<CounterfoilDictionaryTable>()
                    .Where(rows => rows.Key == key)
                    .Count() >= 1;
                if (existKey)
                    this.counterfoilDbSourceConnection.Delete<CounterfoilDictionaryTable>(key);
            }
            else
            { //want create or modify the key
                CounterfoilDictionaryTable elementToAddOrModify = new CounterfoilDictionaryTable()
                {
                    Key = key,
                    Content = content
                };

                this.counterfoilDbSourceConnection.InsertOrReplace(elementToAddOrModify);
            }
        }

        public void SetDataWithoutRestriction(string key, string content)
        {
            this.isBlockedInternalData = false;
            SetData(key, content);
            this.isBlockedInternalData = true;
        }

        public TableQuery<T> GetTable<T>() where T : new()
        {
            return this.counterfoilDbSourceConnection.Table<T>();
        }

        public void Insert(object modelToInsert)
        {
            this.counterfoilDbSourceConnection.Insert(modelToInsert);
        }

        public void Update(object modelToUpdate)
        {
            this.counterfoilDbSourceConnection.Update(modelToUpdate);
        }

        public void Delete(object modelToDelete)
        {
            this.counterfoilDbSourceConnection.Delete(modelToDelete);
        }
        #endregion

        #region events
        public event EventHandler<NotifyTableChangedEventArgs> OnChanged;
        #endregion

        private class CounterfoilDictionaryTable
        {
            [PrimaryKey]
            public string Key { get; set; }
            public string Content { get; set; }
        }
    }
}
