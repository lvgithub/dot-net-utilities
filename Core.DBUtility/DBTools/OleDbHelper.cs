using System;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace Core.DBUtility
{
    /// <summary>
    /// 常用的Access数据库Sql操作辅助类库
    /// </summary>
    public class OleDbHelper
    {
        private string connectionString = "";
        private const string accessPrefix = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID=Admin;Jet OLEDB:Database Password=;";
        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <returns></returns>
        public static OleDbConnection GetOleDbConnection()
        {
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["db"].ToString()));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessFilePath"></param>
        public OleDbHelper(string accessFilePath)
        {
            connectionString = string.Format(accessPrefix, accessFilePath);
        }

        /// <summary>
        /// 测试数据库是否正常连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            bool result = false;

            using (DbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 执行Sql，并返回成功的数量
        /// </summary>
        /// <param name="sqlList">待执行的Sql列表</param>
        /// <returns></returns>
        public int ExecuteNonQuery(List<string> sqlList)
        {
            int count = 0;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;

                foreach (string sql in sqlList)
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    try
                    {
                        if (command.ExecuteNonQuery() > 0)
                        {
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="cmdText">a</param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, params object[] p)
        {
            OleDbCommand command = new OleDbCommand();

            using (OleDbConnection connection = GetOleDbConnection())
            {
                PrepareCommand(command, connection, cmdText, p);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行无返回值的语句，成功返回True，否则False
        /// </summary>
        /// <param name="sql">待执行的Sql</param>
        /// <returns></returns>
        public bool ExecuteNoQuery(string sql)
        {
            bool result = false;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (command.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行单返回值的语句
        /// </summary>
        /// <param name="sql">待执行的Sql</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            object result = null;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                result = command.ExecuteScalar();
            }
            return result;
        }
        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params object[] p)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection connection = GetOleDbConnection())
            {
                PrepareCommand(cmd, connection, cmdText, p);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行Sql，并返回IDataReader对象。
        /// </summary>
        /// <param name="sql">待执行的Sql</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            IDataReader reader = null;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return reader;
        }
        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string cmdText, params object[] p)
        {
            OleDbCommand command = new OleDbCommand();
            OleDbConnection connection = GetOleDbConnection();
            try
            {
                PrepareCommand(command, connection, cmdText, p);
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 执行Sql并返回DataSet集合
        /// </summary>
        /// <param name="sql">待执行的Sql</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connectionString);
            adapter.Fill(ds);
            return ds;
        }
        public static DataSet ExecuteDataset(string cmdText, params object[] p)
        {
            DataSet ds = new DataSet();
            OleDbCommand command = new OleDbCommand();
            using (OleDbConnection connection = GetOleDbConnection())
            {
                PrepareCommand(command, connection, cmdText, p);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(ds);
            }

            return ds;
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, string cmdText, params object[] p)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;

            if (p != null)
            {
                foreach (object parm in p)
                    cmd.Parameters.AddWithValue(string.Empty, parm);
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cmdText"></param>
        /// <param name="countText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataSet ExecutePager(ref int recordCount, int pageIndex, int pageSize, string cmdText, string countText, params object[] p)
        {
            if (recordCount < 0)
                recordCount = int.Parse(ExecuteScalar(countText, p).ToString());

            DataSet ds = new DataSet();

            OleDbCommand command = new OleDbCommand();
            using (OleDbConnection connection = GetOleDbConnection())
            {
                PrepareCommand(command, connection, cmdText, p);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, "result");
            }
            return ds;
        }

        public static DataRow ExecuteDataRow(string cmdText, params object[] p)
        {
            DataSet ds = ExecuteDataset(cmdText, p);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }

     

    }
}
