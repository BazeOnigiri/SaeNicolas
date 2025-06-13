using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nicolas.UCs;
using System.Windows;
using System.Windows.Controls;

namespace Nicolas.Tests.UCs
{
    [TestClass]
    public class TestUCLogin
    {
        private UCLogin ucLogin;

        [TestInitialize]
        public void Initialiser()
        {
            ucLogin = new UCLogin();
        }

        
    }
}