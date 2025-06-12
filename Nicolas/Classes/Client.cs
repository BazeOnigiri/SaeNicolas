using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Client
    {
        private int numClient;
        private string? nomClient;
        private string? prenomClient;
        private string? mailClient;

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public string? NomClient
        {
            get
            {
                return nomClient;
            }

            set
            {
                nomClient = value;
            }
        }

        public string? PrenomClient
        {
            get
            {
                return prenomClient;
            }

            set
            {
                prenomClient = value;
            }
        }

        public string? MailClient
        {
            get
            {
                return mailClient;
            }

            set
            {
                mailClient = value;
            }
        }
        public Client() { }

        public Client(int numClient, string? nomClient, string? prenomClient, string? mailClient)
        {
            NumClient = numClient;
            NomClient = nomClient;
            PrenomClient = prenomClient;
            MailClient = mailClient;
        }

        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from client;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numClient = dr["numClient"] != DBNull.Value ? Convert.ToInt32(dr["numClient"]) : 0;
                    string? nomClient = dr["nomClient"] != DBNull.Value ? dr["nomClient"].ToString() : null;
                    string? prenomClient = dr["prenomClient"] != DBNull.Value ? dr["prenomClient"].ToString() : null;
                    string? mailClient = dr["mailClient"] != DBNull.Value ? dr["mailClient"].ToString() : null;

                    lesClients.Add(new Client(numClient, nomClient, prenomClient, mailClient));
                }
            }
            return lesClients;
        }
    }
}

