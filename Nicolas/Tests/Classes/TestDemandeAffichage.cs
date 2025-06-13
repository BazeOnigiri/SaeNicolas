using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.UCs;
using System;

namespace Nicolas.Tests.Classes
{
    [TestClass]
    public class TestDemandeAffichage
    {
        [TestMethod]
        public void Constructeur_ProprietesInitialisees()
        {
            var demande = new DemandeAffichage
            {
                NumeroDemande = 1,
                NomVin = "Château Test",
                QuantiteDemande = 5,
                PrixVin = 50.0,
                DateDemande = DateTime.Now.ToString("dd/MM/yyyy"),
                NumeroVin = 1
            };

            Assert.AreEqual(1, demande.NumeroDemande);
            Assert.AreEqual("Château Test", demande.NomVin);
            Assert.AreEqual(5, demande.QuantiteDemande);
            Assert.AreEqual(50.0, demande.PrixVin);
            Assert.AreEqual(1, demande.NumeroVin);
            Assert.IsNotNull(demande.DateDemande);
        }
    }
}