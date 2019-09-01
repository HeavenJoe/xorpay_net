using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using XorPay.SDK;

namespace XorPay.Web
{
    /// <summary>
    /// 订单数据操作类
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// 是否存在订单
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public static bool Exists(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                return false;
            }
            string sql = "select count(id) from orders where order_no='" + order_no + "'";
            string str = string.Concat(SqlHelper.GetSingleValue(sql)) ?? "";
            int result = 0;
            int.TryParse(str, out result);
            return result > 0;
        }

        /// <summary>
        /// 根据平台订单或XorPay平台单号
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public static string GetAoidByOrderNo(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                return "";
            }
            string sql = "select aoid from orders where order_no='" + order_no + "' limit 0,1";
            return string.Concat(SqlHelper.GetSingleValue(sql)) ?? "";
        }

        /// <summary>
        /// 获取订单支付二维码地址信息
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public static string GetQR(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                return "";
            }
            string sql = "select qr from orders where order_no='" + order_no + "' limit 0,1";
            return string.Concat(SqlHelper.GetSingleValue(sql)) ?? "";
        }

        /// <summary>
        /// 获取jsapi订单的支付参数信息
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public static string GetJsApiInfo(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                return "";
            }
            string sql = "select jsapi_info from orders where order_no='" + order_no + "' limit 0,1";
            return string.Concat(SqlHelper.GetSingleValue(sql)) ?? "";
        }


        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="aoid"></param>
        /// <param name="qr"></param>
        /// <param name="jsapi_info"></param>
        /// <returns></returns>
        public static bool Add(PayRequestModel requestModel, string aoid = "", string qr = "", string jsapi_info = "")
        {
            string sql = "insert into orders(order_no,buyer,price,pay_type,name,more,add_time,aoid,qr,jsapi_info)";
            sql += " values(@order_no,@buyer,@price,@pay_type,@name,@more,@add_time,@aoid,@qr,@jsapi_info)";
            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@order_no", requestModel.order_id),
                new SQLiteParameter("@buyer", requestModel.order_uid),
                new SQLiteParameter("@price", requestModel.price),
                new SQLiteParameter("@pay_type", requestModel.pay_type),
                new SQLiteParameter("@name", requestModel.name),
                new SQLiteParameter("@more", requestModel.more),
                new SQLiteParameter("@add_time", DateTime.Now),
                new SQLiteParameter("@aoid", aoid),
                new SQLiteParameter("@qr", qr),
                new SQLiteParameter("@jsapi_info", jsapi_info)
            };
            return SqlHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool Update(string value, string where)
        {
            string sql = "update orders set " + value + " where " + where;
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
    }
}