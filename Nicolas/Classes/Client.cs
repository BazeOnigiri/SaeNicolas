using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicolas.Interfaces;

namespace Nicolas.Classes
{
    internal class Client : ICrude<Client>
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

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into client (nomClient, prenomClient, mailClient) values (@nomClient, @prenomClient, @mailClient) RETURNING numClient"))
            {
                cmdInsert.Parameters.AddWithValue("nomClient", (object?)this.NomClient ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("prenomClient", (object?)this.PrenomClient ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("mailClient", (object?)this.MailClient ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumClient = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from client where numClient = @numClient;"))
            {
                cmdSelect.Parameters.AddWithValue("numClient", this.NumClient);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomClient = dt.Rows[0]["nomClient"] != DBNull.Value ? dt.Rows[0]["nomClient"].ToString() : null;
                    this.PrenomClient = dt.Rows[0]["prenomClient"] != DBNull.Value ? dt.Rows[0]["prenomClient"].ToString() : null;
                    this.MailClient = dt.Rows[0]["mailClient"] != DBNull.Value ? dt.Rows[0]["mailClient"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update client set nomClient = @nomClient, prenomClient = @prenomClient, mailClient = @mailClient where numClient = @numClient;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomClient", (object?)this.NomClient ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("prenomClient", (object?)this.PrenomClient ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("mailClient", (object?)this.MailClient ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numClient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from client where numClient = @numClient;"))
            {
                cmdDelete.Parameters.AddWithValue("numClient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
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

        public List<Client> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}

