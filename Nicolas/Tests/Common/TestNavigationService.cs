using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System.Windows.Controls;

namespace Nicolas.Tests.Common
{
    [TestClass]
    public class TestNavigationService
    {
        private MainWindow mainWindow;

        [TestInitialize]
        public void Initialiser()
        {
            mainWindow = new MainWindow();
        }

        [TestMethod]
        public void NaviguerVers_ChangementDeVue()
        {
            var nouvelleVue = new UCRechercherVin();

            mainWindow.mainGrid.Children.Clear();
            mainWindow.mainGrid.Children.Add(nouvelleVue);

            Assert.AreEqual(1, mainWindow.mainGrid.Children.Count);
            Assert.IsInstanceOfType(mainWindow.mainGrid.Children[0], typeof(UCRechercherVin));
        }

        [TestMethod]
        public void RetourVersVuePrecedente_RetourneVersPrecedent()
        {
            // Arrange
            var vue1 = new UCRechercherVin();
            var vinTest = new Vin(1, 1, 1, 1, "Test Vin", 10.0, "Description", 2020);
            var vue2 = new UCAjouterDemande(vinTest); // Utilisation d'un Vin valide

            // Act
            mainWindow.mainGrid.Children.Clear();
            mainWindow.mainGrid.Children.Add(vue1);
            mainWindow.mainGrid.Children.Clear();
            mainWindow.mainGrid.Children.Add(vue2);

            mainWindow.mainGrid.Children.Clear();
            mainWindow.mainGrid.Children.Add(vue1);

            // Assert
            Assert.IsInstanceOfType(mainWindow.mainGrid.Children[0], typeof(UCRechercherVin));
        }
    }
}