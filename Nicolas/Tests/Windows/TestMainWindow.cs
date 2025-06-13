using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.UCs;
using System.Windows.Controls;

namespace Nicolas.Tests.Windows
{
    [TestClass]
    public class TestMainWindow
    {
        private MainWindow mainWindow;

        [TestInitialize]
        public void Initialiser()
        {
            mainWindow = new MainWindow();
        }

        [TestMethod]
        public void Constructeur_InitialiseRechercherVin()
        {
            var enfants = mainWindow.mainGrid.Children;
            Assert.AreEqual(1, enfants.Count);
            Assert.IsInstanceOfType(enfants[0], typeof(UCRechercherVin));
        }

        [TestMethod]
        public void MainGrid_ExisteEtInitialise()
        {
            Assert.IsNotNull(mainWindow.mainGrid);
            Assert.IsInstanceOfType(mainWindow.mainGrid, typeof(Grid));
        }
    }
}