using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System.Collections.ObjectModel;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCVisualiserDemandes
    {
        private UCVisualiserDemandes ucVisualiserDemandes;

        [TestInitialize]
        public void Initialiser()
        {
            ucVisualiserDemandes = new UCVisualiserDemandes();
        }

        [TestMethod]
        public void ChargeData_DonneesChargees()
        {
            ucVisualiserDemandes.ChargeData();

            var source = ucVisualiserDemandes.dgDemande.ItemsSource as ObservableCollection<Demande>;
            Assert.IsNotNull(source);
        }
    }
}