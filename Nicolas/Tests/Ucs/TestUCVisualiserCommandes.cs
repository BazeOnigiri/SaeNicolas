using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System;
using System.Collections.Generic;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCVisualiserCommandes
    {
        private UCVisualiserCommandes ucVisualiserCommandes;

        [TestInitialize]
        public void Initialiser()
        {
            ucVisualiserCommandes = new UCVisualiserCommandes();
        }

        [TestMethod]
        public void Constructeur_InitialiseLesListes()
        {
            Assert.IsNotNull(ucVisualiserCommandes.ListeAppelations);
            Assert.IsNotNull(ucVisualiserCommandes.ListeTypesVin);
        }

        [TestMethod]
        public void RechercheMotClef_TexteVide_RetourneVrai()
        {
            var commande = new Commande(1, 1, DateTime.Now, false, 100);
            var resultat = ucVisualiserCommandes.RechercheMotClef(commande);

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void btnClearFiltres_ReinitialiseLesFiltres()
        {
            ucVisualiserCommandes.Recherche = "test";
            ucVisualiserCommandes.Millesime = "2020";
            ucVisualiserCommandes.TypeVin = "1";
            ucVisualiserCommandes.PrixMin = "10";
            ucVisualiserCommandes.PrixMax = "100";
            ucVisualiserCommandes.DateCommande = DateTime.Now;

            ucVisualiserCommandes.btnClearFiltres_Click(null, null);

            Assert.IsNull(ucVisualiserCommandes.Recherche);
            Assert.IsNull(ucVisualiserCommandes.Millesime);
            Assert.IsNull(ucVisualiserCommandes.TypeVin);
            Assert.IsNull(ucVisualiserCommandes.PrixMin);
            Assert.IsNull(ucVisualiserCommandes.PrixMax);
            Assert.IsNull(ucVisualiserCommandes.DateCommande);
        }

        [TestMethod]
        public void dataGridCommandes_SelectionChanged_AfficheDetails()
        {
            var commande = new Commande(1, 1, DateTime.Now, false, 100);
            ucVisualiserCommandes.dataGridCommandes_SelectionChanged(null, null);

            Assert.AreEqual("Sélectionnez une commande pour voir les détails.",
                ucVisualiserCommandes.textBlockDetails.Text);
        }
    }
}