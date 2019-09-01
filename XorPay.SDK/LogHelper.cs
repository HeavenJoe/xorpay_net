using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorPay.SDK
{
    /// <summary>
    ///  日志输出类
    /// </summary>
    public class LogHelper
    {
        private static readonly NLog.Logger logger;

        static LogHelper()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 输出记录日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void Info(object content)
        {
            logger.Info(content);
        }

        /// <summary>
        /// 输出记录日志(格式化)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="obj">参数</param>
        public static void Info(string content, params object[] obj)
        {
            logger.Info(content, obj);
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void Error(object content)
        {
            logger.Error(content);
        }

        /// <summary>
        /// 输出警告日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void Warn(object content)
        {
            logger.Warn(content);
        }

        /// <summary>
        /// 输出调试日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void Debug(object content)
        {
            logger.Debug(content);
        }

        /// <summary>
        /// 输出简洁错误日志 true是、false否
        /// </summary>
        /// <param name="info"></param>
        public static void Error(string message, Exception ex, bool status = false)
        {
            if (status)
            {
                logger.Error(message, ex.Message);
            }
            else
            {
                logger.Error(ex, message);
            }
        }
    }
}
