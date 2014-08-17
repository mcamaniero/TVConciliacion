using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.WebControls;

namespace TvCable.Conciliacion.Libs
{
    public class Util
    {

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// Convierte un Stream a byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFullyStream(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    input.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Loading items of DropDownList
        /// </summary>
        /// <param name="dwlControl">Control source</param>
        /// <param name="displayMember">Columname</param>
        /// <param name="valueMember">Columname</param>
        /// <param name="data">Items of control</param>
        public static void LoadingDropDownList(DropDownList dwlControl, string displayMember, string valueMember, DataTable data)
        {
            dwlControl.DataSource = data;
            dwlControl.DataTextField = displayMember;
            dwlControl.DataValueField = valueMember;
            dwlControl.DataBind();

        }
        //// <summary>
        /// Loading items of DropDownList
        /// </summary>
        /// <param name="dwlControl">Control source</param>
        /// <param name="displayMember">Columname</param>
        /// <param name="valueMember">Columname</param>
        /// <param name="data">Items of control array</param>
        public static void LoadingDropDownList(DropDownList dwlControl, string displayMember, string valueMember, Array data)
        {
            dwlControl.DataSource = data;
            dwlControl.DataTextField = displayMember;
            dwlControl.DataValueField = valueMember;
            dwlControl.DataBind();

        }

        /// <summary>
        /// Generate the timestamp for the signature       
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Convierte una fecha en String a DateTime segun el formato 
        /// </summary>
        /// <param name="value">Fecha en String</param>
        /// <param name="formato">Formato de fecha ejemplo: yyyyMMdd, dd-MM-yyyy</param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string date, string format)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(date, format, provider);
        }

        

        public static DateTime ConvertStringToDateTime(string date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(date, "dd/MM/yyyy", provider);
        }

        public static DateTime ConvertStringToDateTimeMysql(string date)
        {
            //var isoDateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            //dateValue.ToString(isoDateTimeFormat.UniversalSortableDateTimePattern);

            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.Parse(date);
        }
        /// <summary>
        /// Convierte un String a Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ConvertStringToDecimal(string value)
        {
            var valueClean = value.Replace("$", "").Trim();
            if (!valueClean.Trim().Equals(String.Empty))
            {
                valueClean = valueClean.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                valueClean = valueClean.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                try
                {
                    return Convert.ToDecimal(valueClean);
                }
                catch
                {
                    return Convert.ToDecimal(Convert.ToDouble(valueClean));
                }
            }
            else
                return 0;
        }
        /// <summary>
        /// Convierte un string en int
        /// </summary>
        /// <param name="value">valor string</param>
        /// <returns>valor int</returns>
        public static int ConvertStringToInt(string value)
        {

            try
            {
                if (!value.Trim().Equals(String.Empty))
                {
                    int numVal = Convert.ToInt32(value);

                    return numVal;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public string GeneraCampoConciliacion(int longitud, string cadena, string separador)
        {
            try
            {
                string cadenaFinal = cadena.Trim();
                int lonCadenaActual = cadenaFinal.Length;
                if (longitud > lonCadenaActual)
                {
                    while (longitud > lonCadenaActual)
                    {
                        cadenaFinal = separador + cadenaFinal;
                        lonCadenaActual++;
                    }
                }
                return cadenaFinal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ObtieneCampoConciliacion(string cadena, int posicionInicial, int posicionFinal)
        {
            try
            {
                string cadenaFinal = cadena.Trim();
                var campoFinal = new StringBuilder();
                int lonCadenaActual = cadenaFinal.Length;
                var posFin = 1;

                if (posicionFinal <= lonCadenaActual)
                {
                    for (int i = 0; i < lonCadenaActual; i++)
                    {
                        var posIni = posicionInicial - 1;
                        if ((i >= posIni) && (posFin <= posicionFinal))
                        {
                            campoFinal = campoFinal.Append(cadenaFinal[i]);
                            posFin++;
                        }
                    }
                }
                return campoFinal.ToString();
            }
            catch (Exception ex)
            {
                return cadena;
            }
        }

        public static int SendMail(string fromMail, string toMail, string SubjectMail, string strBody, Stream linkSingleAttachment, string nameFile1, out string resultyDescription)
        {
            try
            {
                string strSMTPServer = ConfigurationManager.AppSettings["servidorSalida"].ToString();
                string strCuentaMail = ConfigurationManager.AppSettings["CuentaCorreoOrigen"].ToString();
                string strPasswordCuentaMail = ConfigurationManager.AppSettings["PasswordCorreoOrigen"].ToString();
                string strServidorMail = ConfigurationManager.AppSettings["servidorMail"];
                string portSmtpServerMail = ConfigurationManager.AppSettings["portSmtpServerMail"].ToString();
                bool useSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["useSsl"]);

                var oMsg = new System.Net.Mail.MailMessage();
                oMsg.From = new MailAddress(fromMail);
                oMsg.To.Add(toMail);
                oMsg.Subject = SubjectMail;
                oMsg.IsBodyHtml = true;
                oMsg.Body = strBody;
                System.Net.Mail.Attachment oAttch;

                if (linkSingleAttachment != null)
                {
                    // Attach file 1
                    oAttch = new Attachment(linkSingleAttachment, nameFile1);
                    oMsg.Attachments.Add(oAttch);
                }
                

                // new Metodo
                var basicCredential = new NetworkCredential(strCuentaMail, strPasswordCuentaMail);
                var smtpClient = new SmtpClient(strSMTPServer, Convert.ToInt32(portSmtpServerMail));
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = useSsl;
                smtpClient.Send(oMsg);

                oMsg = null;
                oAttch = null;
                resultyDescription = "Send mail OK";
                return 1;
            }
            catch (Exception ex)
            {
                resultyDescription = "Se presento un error al enviar el mail: Mensaje: " + ex.Message + ". Exception: " + ex.ToString();
                return -1;
            }
        }
        /// <summary>
        /// Calcula Valor Iva
        /// </summary>
        /// <param name="baseiva">Base Iva</param>
        /// <returns>Valor Iva</returns>
        public decimal ValorIva(decimal baseiva)
        {

            return Convert.ToDecimal(0.12) * baseiva;
        }
        /// <summary>
        /// Calcula Base Iva
        /// </summary>
        /// <param name="valortotal">Valor total</param>
        /// <returns>Base Iva</returns>
        public decimal BaseIva(decimal valortotal)
        {

            return valortotal - ( Convert.ToDecimal(0.12) * valortotal);
        }
        /// <summary>
        /// Calcula Valor Ice
        /// </summary>
        /// <param name="baseice">Base Ice</param>
        /// <returns>Valor Ice</returns>
        public decimal ValorIce(decimal baseice)
        {
            return (Convert.ToDecimal(0.15) * baseice);
        }
        /// <summary>
        /// Calculo Base ICe
        /// </summary>
        /// <param name="baseiva">Base Iva</param>
        /// <returns>Base Ice</returns>
        public decimal BaseIce(decimal baseiva)
        {
            return baseiva - (Convert.ToDecimal(0.15) * baseiva);
        }
        /// <summary>
        /// Calculo Valor Neto
        /// </summary>
        /// <param name="baseiva">Base Iva</param>
        /// <returns>Valor neto</returns>
        public decimal ValorNeto(decimal baseiva)
        {
            return baseiva - (Convert.ToDecimal(0.15) * baseiva);
        }
      



    }
}
