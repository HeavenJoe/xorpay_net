using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Web;

namespace XorPay.Web
{
    public class SqlHelper
    {
        private static string dataPath = "Data Source =" + AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\database.db";
        private static SQLiteConnection conn = new SQLiteConnection(dataPath);
        private static SQLiteCommand cmd = null;
        private static SQLiteDataReader sdr = null;
        private static readonly object obj = new object();
        private static int status = -1;



        /// <summary>
        /// <summary>
        /// 获取连接结果，未连接打开连接
        /// </summary>
        /// <returns>连接结果</returns>
        private static SQLiteConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
        /// <summary>
        /// 该方法执行传入的增删改SQL语句
        /// </summary>
        /// <param name="sql">要执行的增删改SQL语句</param>
        /// <returns>返回更新的记录数</returns>
        public static int ExecuteNonQuery(string sql)
        {
            int res;
            try
            {
                cmd = new SQLiteCommand(sql, GetConn());
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                res = -1;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;
        }
        /// <summary>
        /// 执行带参数的SQL增删改语句
        /// </summary>
        /// <param name="sql">SQL增删改语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回更新的记录数</returns>
        public static int ExecuteNonQuery(string sql, SQLiteParameter[] paras)
        {
            int res = -1;
            bool mylock = true;
            while (mylock)
            {
                if (conn.State == ConnectionState.Open)
                {
                    Thread.Sleep(100);
                    continue;
                }
                using (cmd = new SQLiteCommand(sql, GetConn()))
                {
                    cmd.Parameters.AddRange(paras);
                    try
                    {
                        res = cmd.ExecuteNonQuery();
                        mylock = false;
                    }
                    catch
                    {
                        mylock = true;
                    }
                }
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return res;
        }
        /// <summary>
        /// 该方法执行传入的SQL查询语句
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns> 返回数据集合</returns>
        public static DataTable ExecuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            cmd = new SQLiteCommand(sql, GetConn());
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                if (!sdr.HasRows)
                    return null;
                dt.Load(sdr);
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的SQL查询语句
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回数据集合</returns>
        public static DataTable ExecuteQuery(string sql, SQLiteParameter[] paras)
        {
            DataTable dt = new DataTable();
            cmd = new SQLiteCommand(sql, GetConn());
            cmd.Parameters.AddRange(paras);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的SQL查询判断语句
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="paras">参数集合</param>
        /// <returns>返回是否为空</returns>
        public static bool BoolExecuteQuery(string sql, SQLiteParameter[] paras)
        {
            DataTable dt = new DataTable();
            cmd = new SQLiteCommand(sql, GetConn());
            cmd.Parameters.AddRange(paras);
            try
            {
                using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            DataRow[] rows = dt.Select();
            bool temp = false;
            if (rows.Length > 0)
            {
                temp = true;
            }
            return temp;
        }
        /// <summary>
        /// 该方法执行传入的SQL查询判断语句
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns>返回是否为空</returns>
        public static bool BoolExecuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            cmd = new SQLiteCommand(sql, GetConn());
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            DataRow[] rows = dt.Select();
            bool temp = false;
            if (rows.Length > 0)
            {
                temp = true;
            }
            return temp;
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sqlList">sql列表</param>
        /// <returns>是否成功</returns>
        public static bool ExecuteTranByList(List<string> sqlList)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SQLiteTransaction sqlTran = conn.BeginTransaction())
            {
                string sql = "";
                try
                {
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        sql = sqlList[i];
                        cmd = new SQLiteCommand(sql, GetConn());
                        int res = cmd.ExecuteNonQuery();
                    }
                    sqlTran.Commit();
                    return true;
                }
                catch
                {
                    sqlTran.Rollback();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public static bool ExecuteTran(List<CommandInfo> commondInfoList)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SQLiteTransaction sqlTran = conn.BeginTransaction())
            {
                string sql = "";
                SQLiteParameter[] paras = null;
                try
                {
                    for (int i = 0; i < commondInfoList.Count; i++)
                    {
                        sql = commondInfoList[i].CommandText;
                        paras = commondInfoList[i].Parameters;
                        using (cmd = new SQLiteCommand(sql, GetConn()))
                        {
                            cmd.Parameters.AddRange(paras);
                            int res = cmd.ExecuteNonQuery();
                        }
                    }
                    sqlTran.Commit();
                    return true;
                }
                catch
                {
                    sqlTran.Rollback();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 返回单值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleValue(string sql)
        {
            DataTable dt = ExecuteQuery(sql);
            if (dt == null || dt.Rows.Count == 0)
                return null;
            else
                return dt.Rows[0][0];
        }
    }


    public enum EffentNextType
    {
        /// <summary>
        /// 对其他语句无任何影响 
        /// </summary>
        None,
        /// <summary>
        /// 当前语句必须为"select count(1) from .."格式，如果存在则继续执行，不存在回滚事务
        /// </summary>
        WhenHaveContine,
        /// <summary>
        /// 当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        WhenNoHaveContine,
        /// <summary>
        /// 当前语句影响到的行数必须大于0，否则回滚事务
        /// </summary>
        ExcuteEffectRows,
        /// <summary>
        /// 引发事件-当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        SolicitationEvent
    }
    public class CommandInfo
    {
        public object ShareObject = null;
        public object OriginalData = null;
        event EventHandler _solicitationEvent;
        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }
        public void OnSolicitationEvent()
        {
            if (_solicitationEvent != null)
            {
                _solicitationEvent(this, new EventArgs());
            }
        }
        public string CommandText;
        public SQLiteParameter[] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;
        public CommandInfo()
        {

        }
        public CommandInfo(string sqlText, SQLiteParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }
        public CommandInfo(string sqlText, SQLiteParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }
    }
}