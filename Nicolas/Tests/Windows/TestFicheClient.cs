using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.Classes;
using Nicolas.Windows;
using System.Windows;

namespace Nicolas.Tests.Windows
{
    [TestClass]
    public class TestFicheClient
    {
        private FicheClient ficheClient;

        [TestInitialize]
        public void Initialiser()
        {
            ficheClient = new FicheClient();
        }

        [TestMethod]
        public void Constructeur_NouveauClient_ProprietesVides()
        {
            Assert.IsNull(ficheClient.Nom);
            Assert.IsNull(ficheClient.Prenom);
            Assert.IsNull(ficheClient.Mail);
        }

        [TestMethod]
        public void Constructeur_ClientExistant_ProprietesInitialisees()
        {
            var client = new Client(1, "Dupont", "Jean", "jean@email.com");
            var fiche = new FicheClient(client);

            Assert.AreEqual("Dupont", fiche.Nom);
            Assert.AreEqual("Jean", fiche.Prenom);
            Assert.AreEqual("jean@email.com", fiche.Mail);
        }

        [TestMethod]
        public void ValiderChamps_ChampsVides_RetourneFaux()
        {
            var resultat = ficheClient.ValiderChamps();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ValiderChamps_EmailInvalide_RetourneFaux()
        {
            ficheClient.txtNom.Text = "Dupont";
            ficheClient.txtPrenom.Text = "Jean";
            ficheClient.txtMail.Text = "emailinvalide";

            var resultat = ficheClient.ValiderChamps();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ValiderChamps_DonneesValides_RetourneVrai()
        {
            ficheClient.txtNom.Text = "Dupont";
            ficheClient.txtPrenom.Text = "Jean";
            ficheClient.txtMail.Text = "jean@email.com";

            var resultat = ficheClient.ValiderChamps();

            Assert.IsTrue(resultat);
            Assert.AreEqual("Dupont", ficheClient.Nom);
            Assert.AreEqual("Jean", ficheClient.Prenom);
            Assert.AreEqual("jean@email.com", ficheClient.Mail);
        }
    }
}