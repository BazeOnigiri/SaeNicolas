using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nicolas.Classes
{
    internal class DataAccess
    {
        private static DataAccess instance;
        private string connectionString;
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    // Utiliser des credentials par défaut si aucune initialisation n'a été faite
                    // ATTENTION: Ceci est temporaire pour éviter l'erreur, mais pas sécurisé
                    instance = new DataAccess("postgres", "postgres");
                }
                return instance;
            }
        }

        // Propriété pour vérifier si DataAccess a été initialisé avec des credentials utilisateur
        public static bool IsInitializedWithUserCredentials { get; private set; } = false;

        // Méthode statique pour initialiser DataAccess avec les credentials
        public static void Initialize(string username, string password)
        {
            // Fermer l'ancienne connexion si elle existe
            if (instance != null)
            {
                instance.CloseConnection();
            }

            instance = new DataAccess(username, password);
            IsInitializedWithUserCredentials = true;
        }

        // Constructeur privé qui prend les credentials en paramètre
        private DataAccess(string username, string password)
        {
            connectionString = $"Host=localhost;Port=5432;Username={username};Password={password};Database=SAE201;Options='-c search_path=public'";

            try
            {
                connection = new NpgsqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                throw;
            }
        }

        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                    throw;
                }
            }

            return connection;
        }

        // pour requêtes SELECT et retourne un DataTable ( table de données en mémoire)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL");
                throw;
            }
            return dataTable;
        }

        // pour requêtes INSERT et renvoie l'ID généré
        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        // pour requêtes UPDATE, DELETE
        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        // pour requêtes avec une seule valeur retour  (ex : COUNT, SUM) 
        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;
        }

        // Fermer la connexion 
        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Méthode pour réinitialiser l'instance (utile pour les déconnexions)
        public static void Reset()
        {
            if (instance != null)
            {
                instance.CloseConnection();
                instance = null;
                IsInitializedWithUserCredentials = false;
            }
        }
    }
}