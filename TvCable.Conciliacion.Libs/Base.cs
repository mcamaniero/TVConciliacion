using System;
using System.Globalization;
using System.IO;

namespace TvCable.Conciliacion.Libs
{
    public class Base : IDisposable
    {
        public enum ErrorTypeEnum
        {
            Start,
            Stop,
            Information,
            Error,
            Trace,
            Warning,
            Exception
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="message">Usage mode: ex.ToString()</param>
        /// <param name="typeError">Error type enum </param>
        public void WriteLog(ErrorTypeEnum ErrorType, int idEvento, string message)
        {
            string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileNamePath"] ?? "";
            string logMapping = System.Configuration.ConfigurationManager.AppSettings["LogMapping"].ToLower() ?? "all";
            
            logFilePath = logFilePath.Replace(".txt", string.Format("{0}{1}{2}.txt", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));
            
            // Verifica el nivel de Log a registrar
            if (System.String.Compare(logMapping.ToLower(), "none", System.StringComparison.Ordinal) == 0)
            {}
            else
            {
                if (System.String.Compare(logMapping.ToLower(), "all", System.StringComparison.Ordinal) == 0) // Registra todos los Logs
                    WriteToLogFile(logFilePath, ErrorType.ToString() + " " + idEvento + ": " + message);
                else
                {
                    // Registra unicamente el nivel indicado
                    if (System.String.Compare(ErrorType.ToString().ToLower(), logMapping, System.StringComparison.Ordinal) == 0)
                        WriteToLogFile(logFilePath, ErrorType.ToString() + " " + idEvento + ": " + message);
                }
            }
        }

        private static void WriteToLogFile(string strLogFile, string logMessage)
        {
            string strLogMessage;
            StreamWriter swLog = null;
            try
            {
                string context = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
                //strLogMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [" + context + "] " + logMessage;
                strLogMessage = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff") + " " + logMessage;

                swLog = !File.Exists(strLogFile) ? new StreamWriter(strLogFile) : File.AppendText(strLogFile);
                swLog.WriteLine(strLogMessage);
                //swLog.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error escribiendo en archivo de LOG: " + ex.ToString());
            }
            finally
            {
                if (swLog != null)
                {
                    swLog.Close();
                    swLog.Dispose();
                    swLog = null;
                }
                strLogMessage = null;
            }
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            //this.Dispose();
        }
        #endregion
    }
}
