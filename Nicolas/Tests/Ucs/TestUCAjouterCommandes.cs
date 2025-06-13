using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System;
using System.Collections.ObjectModel;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCAjouterCommandes
    {
        private UCAjouterCommandes ucAjouterCommandes;

        [TestInitialize]
        public void Initialiser()
        {
            ucAjouterCommandes = new UCAjouterCommandes();
        }

        [TestMethod]
        public void RecalculerTotal_SansDemandes_RetourneZero()
        {
            ucAjouterCommandes.RecalculerTotal();

            Assert.AreEqual(0m, ucAjouterCommandes.PrixTotal);
        }

        [TestMethod]
        public void RecalculerTotal_AvecDemandes_CalculCorrect()
        {
            ucAjouterCommandes.DemandesSelectionnees.Add(new DemandeAffichage
            {
                PrixVin = 50,
                QuantiteDemande = 2
            });
            ucAjouterCommandes.DemandesSelectionnees.Add(new DemandeAffichage
            {
                PrixVin = 25,
                QuantiteDemande = 1
            });

            ucAjouterCommandes.RecalculerTotal();

            Assert.AreEqual(125m, ucAjouterCommandes.PrixTotal);
        }

        [TestMethod]
        public void AjouterDemande_DemandeValide_AjouteeALaListe()
        {
            var demande = new DemandeAffichage
            {
                NumeroDemande = 1,
                NomVin = "Test Vin",
                QuantiteDemande = 2,
                PrixVin = 50
            };

            ucAjouterCommandes.DemandesDisponibles.Add(demande);
            ucAjouterCommandes.BtnAjouter_Click(null, null);

            Assert.AreEqual(1, ucAjouterCommandes.DemandesSelectionnees.Count);
            Assert.AreEqual(0, ucAjouterCommandes.DemandesDisponibles.Count);
        }
    }
}