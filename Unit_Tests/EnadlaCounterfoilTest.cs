using Enadla_Counterfoil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using SQLite;

namespace Unit_Tests
{
    [TestClass]
    public class EnadlaCounterfoilTest
    {
        const string COUNTERFOIL_EXTENSION = "ecf";

        private string UnitTestFolder
        {
            get
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string unitTestFolderName = "Unit_Tests";
                return Path.Combine(desktopPath, unitTestFolderName);
            }
        }

        private void CreateUnitFolderIfNotExist()
        {
            Directory.CreateDirectory(UnitTestFolder);
        }

        [TestMethod("To Create New Counterfoil")]
        public void CreatingNewEnadlaCounterfoil()
        {
            CreateUnitFolderIfNotExist();
            string specificFolderForTest = Path.Combine(UnitTestFolder, "Creating new Counterfoil");

            if (Directory.Exists(specificFolderForTest))
                Directory.Delete(specificFolderForTest, true);

            Directory.CreateDirectory(specificFolderForTest);

            string fullPathCounterFoil = Path.Combine(specificFolderForTest, $"Talonario.{COUNTERFOIL_EXTENSION}");

            EnadlaCounterfoil testCounterFoil = new EnadlaCounterfoil(fullPathCounterFoil);

            testCounterFoil.Dispose();

            Assert.IsTrue(File.Exists(fullPathCounterFoil));
        }

        [TestMethod("Get data from the Counterfoil Dictionary")]
        [DataRow(true, DisplayName = "Get data from the counterfoil dictionary")]
        [DataRow(false, DisplayName = "Get inexistence data from the counteroil dictionary")]
        public void GetDataFromTheCounterfoilDictionary(bool isGetWithElement)
        {
            CreateUnitFolderIfNotExist();
            string specificFolderForTest = Path.Combine(UnitTestFolder, "Getting data");

            if (Directory.Exists(specificFolderForTest))
                Directory.Delete(specificFolderForTest, true);

            Directory.CreateDirectory(specificFolderForTest);

            string fullPathCounterFoil = Path.Combine(specificFolderForTest, $"Talonario.{COUNTERFOIL_EXTENSION}");

            EnadlaCounterfoil counterfoil = new EnadlaCounterfoil(fullPathCounterFoil);

            if (isGetWithElement)
                counterfoil.SetData("testing", "Test");

            string getedContent = counterfoil.GetData("testing");

            counterfoil.Dispose();

            if (isGetWithElement)
                Assert.AreEqual("Test", getedContent, false);
            else
                Assert.IsNull(getedContent);
        }

        [TestMethod("Set data to the counterfoil")]
        [DataRow("Test", "Testing", true, DisplayName = "Set data to create new data")]
        [DataRow("Test", "Testing", false, DisplayName = "Set data to modifing data")]
        [DataRow("version", "this is not possible", true, DisplayName ="Set data trying to create an restricted key")]
        [DataRow("version", "this is not possible", false, DisplayName = "Set data trying to modifing an restricted key")]
        [DataRow("Test", null, false, DisplayName = "Set data for delete data")]
        [DataRow("Test", null, true, DisplayName = "Set dara for delete inexcistence data")]
        public void SetDataToTheCounterfoil(string key, string content, bool isToCreateNew)
        {
            CreateUnitFolderIfNotExist();
            string specificFolderForTest = Path.Combine(UnitTestFolder, "setting data");

            if (Directory.Exists(specificFolderForTest))
                Directory.Delete(specificFolderForTest, true);

            Directory.CreateDirectory(specificFolderForTest);

            string fullPathCounterFoil = Path.Combine(specificFolderForTest, $"Talonario.{COUNTERFOIL_EXTENSION}");

            EnadlaCounterfoil counterfoil = new EnadlaCounterfoil(fullPathCounterFoil);

            if (!isToCreateNew)
                counterfoil.SetData(key, "bvnw38c99dc92n9qd90a0asjd2ndsoijdsap0dpinuv9w80fjucds9a39duxa9nhc348fhu84nvs");

            if(key == "version")
            {
                Assert.ThrowsException<ArgumentException>(() => counterfoil.SetData(key,content));
            }
            else
            {
                counterfoil.SetData(key, content);

                if (content == null)
                {

                    bool isThereData = counterfoil.GetData(key) != null;

                    Assert.IsFalse(isThereData);
                }
                else
                {
                    string actualContent = counterfoil.GetData(key);

                    Assert.AreEqual(content, actualContent);
                }
            }

            counterfoil.Dispose();
        }

    }
}
