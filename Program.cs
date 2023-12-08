using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;

namespace KovacsBenceDolgozat
{
    internal class Program
    {
        static List<Tagok> tagLista = new List<Tagok>();
        static MySqlConnection connection = null;
        static MySqlCommand command = null;
        static void Main(string[] args)
        {
            readingAndListing();
            addNewTag();
            deleteNewTag();
            Console.ReadLine();

        }
        private static void readingAndListing()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Clear();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "tagdij";
            sb.CharacterSet = "utf8";
            MySqlConnection connection = new MySqlConnection(sb.ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM `ugyfel`";
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tagok ugyfel = new Tagok(dr.GetInt32("azon"), dr.GetString("nev"), dr.GetInt32("szulev"), dr.GetInt32("irszam"), dr.GetString("orsz"));
                        tagLista.Add(ugyfel);
                        Console.WriteLine(ugyfel);
                    }
                }
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);

            }
        }
        private static void addNewTag()
        {
            Tagok ugyfel = new Tagok(1014, "Kis Klaus", 1998, 6800, "Mo");
            command.CommandText = "INSERT INTO `ugyfel`(`azon`, `nev`, `szulev`, `irszam`, `orsz`) VALUES (@azon,@nev,@szulev,@irszam,@orsz)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", ugyfel.azon);
            command.Parameters.AddWithValue("@nev", ugyfel.nev);
            command.Parameters.AddWithValue("@szulev", ugyfel.szulev);
            command.Parameters.AddWithValue("@irszam", ugyfel.irszam);
            command.Parameters.AddWithValue("@orsz", ugyfel.orsz);
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
        private static void deleteNewTag()
        {
            command.CommandText = "DELETE FROM `ugyfel` WHERE `azon` = 1014";
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }


        }
    }
}
