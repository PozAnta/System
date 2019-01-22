using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MyWPF
{
    class SQLdata
    {
       
        public int SelectDataBase(string database, string table, string column, List<string> data)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            int count = 0;

            connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
            connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandText = "select * from [" + table + "];";

            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(reader[column].ToString());
                    count++;
                }

            }
            connection.Close();
            return (count);

        }

        public void SelectDataBase_columnList(string database, string table, List<string> column, List<Setup> data)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;

            var list = new List<Setup>();
            
            connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
            connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandText = "select * from [" + table + "];";

            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreach (string key in column)
                    {
                        reader[key].ToString();
                    }
                    
                }

            }
            connection.Close();

        }

        public int SelectDataBaseInt(string database, string table, string column, List<int> data)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            //List<Drive> dr = new List<Drive>();
            int count = 0;

            connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
            connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandText = "select * from [" + table + "];";

            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(Convert.ToInt16(reader[column]));
                    count++;
                }

            }
            connection.Close();
            return (count);

        }

        public int CountInt(string database, string table, string column)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            //List<Drive> dr = new List<Drive>();
            int count = 0;

            connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
            connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandText = "select * from [" + table + "];";
            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    count++;
                }
            }
            connection.Close();
            return (count);
        }

        public void InsertDataBase(string database, string table, string [] column, List<string> data)
        {
            //SQLiteConnection connection;

            
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    string cmd_string = "";
                    foreach(string st in column)
                    {
                        cmd_string += st;
                        cmd_string += ",";
                    }
                    cmd_string = cmd_string.Substring(0,cmd_string.Length - 1);

                    string data_string = "";
                    foreach (string st in data)
                    {
                        data_string = data_string + "'" + st + "'" + ",";
                    }
                    data_string = data_string.Substring(0, data_string.Length - 1);

                    //create new command
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.CommandText = "insert into " + table + "(" + cmd_string + ")" + " values " + "(" + data_string + ")" + ";";
                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            

        }

        public void UpdateDataBase(string database, string table, string column, string data, string raw)
        {
           
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
            {
                
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandText = "update " + table + " set " + column + "=" + data + " where " + "id=" + raw;
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }


        }

        public List<string> Select(string database, string tablename, string column, List<string> data, string where_id_col, string where_value)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;

            try
            {
                connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
                connection.Open();

                cmd = connection.CreateCommand();
                cmd.CommandText = "select * from [" + tablename + "] where " + where_id_col + "=" + where_value + ";";

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(reader[column].ToString());                        
                    }

                }

                connection.Close();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return (data);
        }

        public int SelectQnt(string database, string tablename, string column, string where_id_col, string where_value)
        {
            SQLiteConnection connection;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            List<string> data = new List<string>();

            try
            {
                connection = new SQLiteConnection("Data Source=" + database + "; Version=3;");
                connection.Open();

                cmd = connection.CreateCommand();
                cmd.CommandText = "select * from [" + tablename + "] where " + where_id_col + "=" + where_value + ";";

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(reader[column].ToString());
                    }

                }

                connection.Close();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return (data.Count);


        }
        
    }
}
