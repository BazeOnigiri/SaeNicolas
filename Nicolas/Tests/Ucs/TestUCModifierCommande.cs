using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System;
using System.Collections.ObjectModel;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCModifierCommande
    {
        private UCModifierCommande ucModifierCommande;
        private Commande commandeTest;

        [TestInitialize]
        public void Initialiser()
        {
            commandeTest = new Commande(1, 1, DateTime.Now, false, 100);
            ucModifierCommande = new UCModifierCommande(commandeTest);
        }

        [TestMethod]
        public void Constructeur_CommandeNull_LanceException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new UCModifierCommande(null));
        }

        [TestMethod]
        public void RecalculerTotal_SansDemandesAssociees_TotalZero()
        {
            ucModifierCommande.RecalculerTotal();

            Assert.AreEqual(0, ucModifierCommande.NouveauTotal);
            Assert.AreEqual("(-100,00 €)", ucModifierCommande.DifferenceMessage);
        }

        [TestMethod]
        public void RecalculerTotal_AvecDemandesAssociees_CalculCorrect()
        {
            var demandes = new ObservableCollection<DemandeAffichage>
            {
                new DemandeAffichage { PrixVin = 50, QuantiteDemande = 2 },
                new DemandeAffichage { PrixVin = 25, QuantiteDemande = 1 }
            };

            var propriete = typeof(UCModifierCommande).GetProperty("demandesAssociees",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propriete.SetValue(ucModifierCommande, demandes);

            ucModifierCommande.RecalculerTotal();

            Assert.AreEqual(125m, ucModifierCommande.NouveauTotal);
            Assert.AreEqual("(+25,00 €)", ucModifierCommande.DifferenceMessage);
        }
    }
}