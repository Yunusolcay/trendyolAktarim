using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BL
{
    public class DBHelper
    {

        #region Members
        public string DecSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private SqlConnection con;
        private SqlDataAdapter da;
        private SqlCommand cmd;
        private string DateFormat;
        public string ConnString;
        public string Sorgu;
        public CultureInfo CI = new CultureInfo(@"tr-TR");
        public DataTable NewTable = new DataTable();
        public connection co = new connection();
        //public string localDB = "A_BYKM";
        //static DBSettings dl = new DBSettings();

        //public static string ServerName = "";
        //public static string UserID = "";
        //public static string Password = "";
        //public static string DbName = "";+
        public string localDB = "";



        #endregion
        public DBHelper(string dbName,string localDB_)
        {
            localDB = localDB_;
            co.databaseName = "master";
            co.serverName = "";
            co.userID = "";
            co.userPassword = "";
            try
            {
                co = JsonConvert.DeserializeObject<connection>(File.ReadAllText("json/connection.json"));
            }
            catch(Exception e)
            {
                
            }
            
            if (dbName != "")
            {
                co.databaseName = dbName;
            }

            #region AAC5
            //sql="bismillahirrahmanirrahim";
            #endregion
            DateFormat = "set dateformat DMY ";
            ConnString = @"server=" + co.serverName + ";database=" + co.databaseName + ";user id=" + co.userID + ";password=" + co.userPassword + ";Trusted_Connection=no;connection timeout=300";
            //ConnString = @"server=.\SQL2017;database=WOLVOX8_DENEME_2020_WOLVOX;user id=sa;password=Pwd1234_;Trusted_Connection=no;connection timeout=300";
        }

        public int sqlKontrol(string servername,string username,string password)
        {
            try {
                DateFormat = "set dateformat DMY ";
                if (servername != "")
                {
                    ConnString = @"server=" + servername + ";database=master;user id=" + username + ";password=" + password + ";Trusted_Connection=no;connection timeout=3";
                }
                Sorgu = "SELECT COUNT(database_id) FROM sys.databases";
                con = new SqlConnection(ConnString);
                cmd = con.CreateCommand();
                cmd.CommandText = DateFormat + Sorgu;
                con.Open();
                object Result = "0";
                try
                {
                    Result = cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message + " : " + Sorgu);
                }
                finally
                {
                    con.Close();
                }
                Int32 returnValue = (Result == null || Result == DBNull.Value) ? 0 : Int32.Parse(Result.ToString());
                return returnValue;

            }
            catch(Exception e)
            {

            }
            return 0;
        }

        public void CreateDatabaseIfNotExists(string connectionString, string dbName)
        {
            connectionString = connectionString.Replace(dbName, "master");
            SqlCommand cmd = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (cmd = new SqlCommand($"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateTableIfNoExists(string connectionString, string sql,string tableName)
        {
            connectionString = connectionString.Replace(co.databaseName, localDB);
            SqlCommand cmd = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //string a = $"If((SELECT 1 FROM sysobjects WHERE xtype = 'U' AND name = '" + tableName + "') = 0) EXEC sp_executesql N'" + sql + "'";
                using (cmd = new SqlCommand($"If((SELECT COUNT(*) FROM sysobjects WHERE xtype = 'U' AND name = '"+ tableName + "') = 0) EXEC sp_executesql N'" + sql+"'", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateProcedureIfNoExists(string connectionString, string sql, string tableName)
        {
            connectionString = connectionString.Replace(co.databaseName, localDB);
            SqlCommand cmd = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (cmd = new SqlCommand($"If((SELECT COUNT(*) FROM sysobjects WHERE xtype = 'P' AND name = '" + tableName + "') = 0) EXEC sp_executesql N'" + sql + "'", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable databaseList(string servername, string username, string password)
        {
            DateFormat = "set dateformat DMY ";
            ConnString = @"server=" + servername + ";database=master;user id=" + username + ";password=" + password + ";Trusted_Connection=no;connection timeout=3";
            Sorgu = "SELECT NAME FROM sys.databases where database_id > 4 ORDER BY NAME";
            DataTable dt = new DataTable();
            dt.Locale = CI;
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + Sorgu;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable firmaList(string servername, string username, string password,string database)
        {
            DateFormat = "set dateformat DMY ";
            ConnString = @"server=" + servername + ";database="+database+";user id=" + username + ";password=" + password + ";Trusted_Connection=no;connection timeout=3";
            Sorgu = "select RIGHT('00' + CONVERT(VARCHAR,FIRMNR),3) + '_' + RIGHT('0' + CONVERT(VARCHAR,NR),2) + '-' + " +
                    "(SELECT TOP 1 NAME FROM L_CAPIFIRM WHERE NR = P.FIRMNR) NAME " +
                    "FROM L_CAPIPERIOD P ORDER BY LOGICALREF DESC";
            DataTable dt = new DataTable();
            dt.Locale = CI;
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + Sorgu;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        #region Functions
        public DataTable SelectDataTable(string sql)
        {
            DataTable dt = new DataTable();
            dt.Locale = CI;
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public string ExecNonQuery_OLD(string sql)
        {
            string sonuc = "ok";
            try
            {
                con = new SqlConnection(ConnString);
                cmd = con.CreateCommand();
                cmd.CommandText = DateFormat + sql;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                sonuc = ex.Message;
            }
            finally
            {
                con = null;
                cmd = null;
            }
            return sonuc;
        }

        public string ExecNonQuery(string sql)
        {
            string sonuc = "ok";
            try
            {
                con = new SqlConnection(ConnString);
                cmd = con.CreateCommand();
                cmd.CommandText = DateFormat + sql;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                sonuc = ex.Message;
                throw new Exception(ex.Message);
            }
            finally
            {
                con = null;
                cmd = null;
            }
            return sonuc;
        }

        public int ExecNonQueryReturnIdentity(string sql)
        {
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "select SCOPE_IDENTITY()";
            int Identity = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            con = null;
            cmd = null;
            return Identity;
        }

        public Int32 ExecScalarReturnInt32(string sql)
        {
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            con.Open();
            object Result = "0";
            try
            {
                Result = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message + " : " + sql);
            }
            finally
            {
                con.Close();
            }
            Int32 returnValue = (Result == null || Result == DBNull.Value) ? 0 : Int32.Parse(Result.ToString());
            return returnValue;
        }

        public string test()
        {
            string abc = "";
            abc = ExecScalarReturnString("SELECT CARIKODU FROM CARI WHERE BLKODU='6'");
            return abc;

        }

        public string ExecScalarReturnString(string sql)
        {
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            con.Open();
            object Result = cmd.ExecuteScalar();
            con.Close();
            string returnValue = (Result == null || Result == DBNull.Value) ? "" : Result.ToString();
            return returnValue;
        }

        public byte[] ExecScalarReturnBytes(string sql)
        {
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            con.Open();
            byte[] Result = (byte[])cmd.ExecuteScalar();
            con.Close();
            //string returnValue = (Result == null || Result == DBNull.Value) ? "" : Result.ToString();
            return Result;
        }


        public float ExecScalarReturnFloat(string sql)
        {
            con = new SqlConnection(ConnString);
            cmd = con.CreateCommand();
            cmd.CommandText = DateFormat + sql;
            con.Open();
            object Result = 0;
            try
            {
                Result = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message + " : " + sql);
            }
            finally
            {
                con.Close();
            }
            float returnValue = (Result == null || Result == DBNull.Value) ? 0 : float.Parse(Result.ToString());
            return returnValue;
        }

        public void ExecSp(string SpName, ref SqlParameter[] ParameterList)
        {
            con = new SqlConnection(ConnString);
            con.Open();

            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpName;
            foreach (SqlParameter prm in ParameterList)
            {
                cmd.Parameters.Add(prm);
            }

            cmd.ExecuteNonQuery();
            con.Close();
            con = null;
            cmd = null;
        }

        public float FloataCevir(object obj)
        {
            float sonuc = 0;
            try
            {
                sonuc = float.Parse(obj.ToString().Replace(".", DecSep).Replace(",", DecSep));
            }
            catch { }

            return sonuc;
        }

        public int InteCevir(object obj)
        {
            int sonuc = 0;
            try
            {
                sonuc = int.Parse(obj.ToString());
            }
            catch { }

            return sonuc;
        }

        public string LocalizePrefix(string Prefix)
        {
            Prefix = Prefix.ToLower(CI).Replace(@"i", @"[ıi]").Replace(@"u", @"[uü]").Replace(@"s", @"[sş]");
            Prefix = Prefix.Replace(@"c", @"[cç]").Replace(@"g", @"[gğ]").Replace(@"o", @"[oö]");
            return Prefix;
        }

        public DateTimeOffset DateTimeOffSetOku(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return DateTimeOffset.Parse("01.01.1900");
            }
            return DateTimeOffset.Parse(obj.ToString());
        }

        public int GetTimeNumber(DateTime Date_)
        {
            return (Date_.Second * 256) + (Date_.Minute * 256 * 256) + (Date_.Hour * 256 * 256 * 256);
        }

        public int GetNewLRef(string TableName)
        {
            SqlParameter[] ParameterList = new SqlParameter[1];
            ParameterList[0] = new SqlParameter("@newLRef", SqlDbType.Int);
            ParameterList[0].Direction = ParameterDirection.Output;

            _execSp("LREF_" + TableName, ref ParameterList);
            return (int)ParameterList[0].Value;
        }

        private void _execSp(string SpName, ref SqlParameter[] ParameterList)
        {
            con = new SqlConnection(ConnString);
            con.Open();

            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpName;
            foreach (SqlParameter prm in ParameterList)
            {
                cmd.Parameters.Add(prm);
            }

            cmd.ExecuteNonQuery();
            con.Close();
            con = null;
            cmd = null;
        }

        /// <summary>
        /// kolonun sql server için düzenlenmiş halini döndürür
        /// </summary>
        public string st(object obj)
        {
            if (obj.GetType() == typeof(float))
            {
                return "'" + obj.ToString().Trim().Replace(",", ".") + "'";
            }
            else if (obj.GetType() == typeof(string))
            {
                return "'" + obj.ToString().Trim().Replace("'", "''") + "'";
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                DateTime tarih = (DateTime)obj;
                return "'" + tarih.ToString("dd/MM/yyyy").Trim() + "'";
            }
            else if (obj.GetType() == typeof(DateTimeOffset))
            {
                DateTimeOffset tarih = (DateTimeOffset)obj;
                return "'" + tarih.ToString("HH:mm").Trim() + "'";
            }
            else
            {
                string a = obj.GetType().ToString();
            }

            return obj.ToString().Trim();
        }

        /// <summary>
        /// insert sorgusunun VALUES ten sonraki kısmını döndürür
        /// </summary>
        public string ins(object[] objs)
        {
            string str_ = "";
            foreach (object obj in objs)
            {
                str_ += st(obj) + ",";
            }
            if (str_.Length > 3)
            {
                return str_.Substring(0, str_.Length - 1);
            }
            return "";
        }
        #endregion

    }

    public class connection{
        public string serverName { get; set; }
        public string userID { get; set; }
        public string userPassword { get; set; }
        public string databaseName { get; set; }
        public string firm { get; set; }
    }

    public class localDBName
    {
        public string dbName = "A_BYKM";
    }
}
