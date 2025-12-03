using System;
using System.IO;

namespace FreeSCADA.Common
{
    /// <summary>
    /// 日志记录器（简单实现）。
    /// 说明：该实现同时向控制台输出并追加写入到临时文件（FreeSCADA2_log_file.txt）。
    /// 仅作示例，生产环境建议使用成熟的日志框架（如 NLog、log4net、Serilog）。
    /// </summary>
    public class Logger
    {
        public enum Severity
        {
            Information,
            Warning,
            Error
        }

        /// <summary>
        /// 记录一条日志（级别 + 文本）。
        /// </summary>
        /// <param name="severity">日志级别</param>
        /// <param name="message">日志消息</param>
        public virtual void Log(Severity severity, string message)
        {
            // 控制台输出，便于调试时观察
            Console.WriteLine(string.Format("{0}: {1}", severity.ToString(), message));

            try
            {
                // 将日志追加到系统临时目录下的文件（非线程安全的简单实现）
                string tmpFile = Path.Combine(Path.GetTempPath(), "FreeSCADA2_log_file.txt");
                using (Stream stream = new FileStream(tmpFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    // 写入时间戳、级别、消息
                    writer.WriteLine(string.Format("[{0}] {1}: {2}", DateTime.Now, severity.ToString(), message));
                    writer.Flush();
                }
            }
            catch
            {
                // 忽略日志写入时的任何异常，避免因日志导致主流程失败
            }
        }

        /// <summary>
        /// 记录信息级别日志
        /// </summary>
        /// <param name="message">信息内容</param>
        public void LogInfo(string message)
        {
            Log(Severity.Information, message);
        }

        /// <summary>
        /// 记录警告级别日志
        /// </summary>
        /// <param name="message">警告内容</param>
        public void LogWarning(string message)
        {
            Log(Severity.Warning, message);
        }

        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        /// <param name="message">错误内容</param>
        public void LogError(string message)
        {
            Log(Severity.Error, message);
        }
    }
}