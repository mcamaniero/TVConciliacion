using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace TvCable.Conciliacion.Libs
{
    public class MySqlHelper
    {
        
        public static DataSet GetDataset(string inputsql)
        {
            using (MySqlConnection cn = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionStringTuves"]))
            {
                using (MySqlCommand myAccessCommand = new MySqlCommand(inputsql, cn))
                {
                    DataSet ds = new DataSet();
                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myAccessCommand);
                    myDataAdapter.Fill(ds);
                    return ds;
                }
            }

        }

        public static DataSet GetDataset(string inputsql, params MySqlParameter[] commandParameters)
        {

            using (MySqlConnection cn = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionStringTuves"]))
            {
                using (MySqlCommand myAccessCommand = new MySqlCommand(inputsql, cn))
                {
                    DataSet ds = new DataSet();

                    foreach (MySqlParameter p in commandParameters)
                    {
                        if (p != null)
                        {
                            myAccessCommand.Parameters.Add(p);
                        }
                    }

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myAccessCommand);
                    myDataAdapter.Fill(ds);
                    return ds;
                }

            }
        }

        public static void ExecuteSql(string inputsql, params MySqlParameter[] commandParameters)
        {
            using (MySqlConnection cn = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionStringTuves"]))
            {
                cn.Open();
                MySqlCommand myAccessCommand1 = new MySqlCommand(inputsql, cn);
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        myAccessCommand1.Parameters.Add(p);
                    }
                }
                myAccessCommand1.ExecuteNonQuery();
                cn.Close();
            }

        }

        public static void ExecuteSql(string inputsql)
        {
            using (MySqlConnection cn = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionStringTuves"]))
            {
                cn.Open();
                MySqlCommand myAccessCommand1 = new MySqlCommand(inputsql, cn);
                myAccessCommand1.ExecuteNonQuery();
                cn.Close();
            }

        }
          
    }
}
