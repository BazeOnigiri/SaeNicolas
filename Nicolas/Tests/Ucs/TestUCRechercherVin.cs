using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System.Collections.Generic;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCRechercherVin
    {
        private UCRechercherVin ucRechercherVin;
        private List<Vin> vinsTest;

        [TestInitialize]
        public void Initialiser()
        {
            ucRechercherVin = new UCRechercherVin();
            vinsTest = new List<Vin>
            {
                new Vin(1, 1, 1, 1, "Château Test", 25.0, "Description 1", 2020),
                new Vin(2, 2, 1, 2, "Domaine Test", 35.0, "Description 2", 2019)
            };
        }

        [TestMethod]
        public void FiltrerVins_RechercheExacte_RetourneVrai()
        {
            ucRechercherVin.Recherche = "Château Test";

            var resultat = ucRechercherVin.FiltrerVins(vinsTest[0]);

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void FiltrerVins_RechercheInexistante_RetourneFaux()
        {
            ucRechercherVin.Recherche = "Inexistant";

            var resultat = ucRechercherVin.FiltrerVins(vinsTest[0]);

            Assert.IsFalse(resultat);
        }
    }
}