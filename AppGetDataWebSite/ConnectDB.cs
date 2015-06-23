using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace AppGetDataWebSite
{
    public class ConnectDB
    {
        private readonly string strConnect = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        private SqlConnection strConn;
        static string mscParamToken = "@";
        public string m_strTableName = string.Empty;
        public string TableName
        {
            get
            {
                return m_strTableName;
            }
            set
            {
                m_strTableName = value;
            }
        }
        public int m_id = 0;
        public int TableId
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        public ConnectDB() {
            strConn = new SqlConnection(strConnect);
        }
        public  void TryOpen( SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        public  void TryClose( SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open) return;
            connection.Close();
            SqlConnection.ClearPool(connection);
        }

        public  void TryAddParameters( SqlCommand command, params SqlParameter[] parameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (parameters == null || parameters.Length == 0) return;
            command.Parameters.AddRange(parameters);
        }

        public int ExecuteNonQuery(string query, CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            try
            {
                TryOpen(strConn);
                SqlCommand command = strConn.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                TryAddParameters(command,parameters);
                return command.ExecuteNonQuery();
            }
            finally
            {
                TryClose(strConn);
            }
        }

        public object ExecuteScalar(string query, CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            try
            {
                TryOpen(strConn);
                SqlCommand command = strConn.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                TryAddParameters(command, parameters);
                return command.ExecuteScalar();
            }
            finally
            {
                TryClose(strConn);
            }
        }

        public SqlDataReader ExecuteReader(string query, CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            try
            {
                TryOpen(strConn);
                SqlCommand command = strConn.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                TryAddParameters(command, parameters);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                TryClose(strConn);
                throw;
            }
            finally
            {
                // strConn.TryClose();
            }
        }

        public DataSet ExecuteData(string query, CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            try
            {
                var dtSet = new DataSet();
                TryOpen(strConn);
                SqlCommand command = strConn.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                TryAddParameters(command, parameters);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(dtSet);
                return dtSet;
            }
            finally
            {
                TryClose(strConn);
            }
        }

        public DataTable ExecuteDataTable(string query, CommandType commandType = CommandType.Text,
           params SqlParameter[] parameters)
        {
            try
            {
                var dt = new DataTable();
                TryOpen(strConn);
                SqlCommand command = strConn.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                TryAddParameters(command, parameters);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
            finally
            {
                TryClose(strConn);
            }
        }

        public bool CheckExist()
        {
            bool flag = true;
            try
            {
                string qry = "select count(id) from " + m_strTableName + " where id="+m_id;
                int i = FC_Convert.ParseInt(ExecuteScalar(qry, CommandType.Text, null));
                if (i > 0)
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = true;
                
            }
            return flag;
        }

        public string Save(List<SqlParameter> lstpr)
        {
            string result = string.Empty;
            try
            {
                string qry = string.Empty;
                if (CheckExist())
                {
                    qry = "Insert_" + m_strTableName;
                }
                else
                {
                    qry = "Update_" + m_strTableName;
                }               
                int i = FC_Convert.ParseInt(ExecuteScalar(qry, CommandType.StoredProcedure, parameters: lstpr.ToArray()));
                if (i > 0)
                {
                    result = FC_Convert.ToString(i);
                }
                else
                {
                    result = "Eror";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
               
            }
            return result;
        }
    }
}
