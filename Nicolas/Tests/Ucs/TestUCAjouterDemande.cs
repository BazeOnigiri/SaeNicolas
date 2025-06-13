using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.UCs;
using System;
using System.Collections.Generic;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCAjouterDemande
    {
        private UCAjouterDemande ucAjouterDemande;
        private Vin vinTest;

        [TestInitialize]
        public void Initialiser()
        {
            vinTest = new Vin(1, 1, 1, 1, "Test Vin", 10.0, "Description", 2020);
            ucAjouterDemande = new UCAjouterDemande(vinTest);
        }

        [TestMethod]
        public void ValiderChamps_QuantiteVide_RetourneFaux()
        {
            var propriete = typeof(UCAjouterDemande).GetProperty("txtQuantite",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propriete.SetValue(ucAjouterDemande, string.Empty);

            var resultat = ucAjouterDemande.ValiderChamps();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ValiderChamps_QuantiteNegative_RetourneFaux()
        {
            var propriete = typeof(UCAjouterDemande).GetProperty("txtQuantite",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propriete.SetValue(ucAjouterDemande, "-1");

            var resultat = ucAjouterDemande.ValiderChamps();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ValiderChamps_ClientNonSelectionne_RetourneFaux()
        {
            var propriete = typeof(UCAjouterDemande).GetProperty("txtClient",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propriete.SetValue(ucAjouterDemande, string.Empty);

            var resultat = ucAjouterDemande.ValiderChamps();

            Assert.IsFalse(resultat);
        }
    }
}