using System;
using System.Data;
using MySql.Data.MySqlClient;
using TvCable.Conciliacion.DTO;

namespace TvCable.Conciliacion.Data
{
    public class Tuves : IDisposable
    {
        public DataSet ObtenerTransaccionesPorFechaMdp(string idMdp, DateTime fechaPago)
        {
            try
            {
                // Parametros
                /*
                MySqlParameter[] parametros = new MySqlParameter[1];
                MySqlParameter i_codigoCatalogo = new MySqlParameter("@i_codigoCatalogo", MySqlDbType.VarChar);
                i_codigoCatalogo.Value = codigoCatalogo;
                parametros[0] = i_codigoCatalogo;
                */
                
                var strFechaTrxTuves = fechaPago.ToString("yyyy-MM-dd");
                var sqlTransaction = "SELECT payments.UniqueID, payments.DealerKey, payments.CollectID, payments.CustContUId, payments.CustIdTyp, payments.CustIdent, payments.CustPaym, payments.PayConcept, payments.CouponSet, payments.CouponNum, payments.PayBill, payments.PayDate, payments.PaySegment, payments.DueDate, payments.PreDays, payments.PayDocN, payments.PayAmnt, payments.PayPlanAmnt, payments.PayKitAmnt, payments.OnBillNum, payments.RevDocN, payments.Notes, payments.tsUser, payments.ts FROM payments WHERE  payments.CollectID = '{0}' AND payments.PayDate = '{1}'";
                var exeSql = string.Format(sqlTransaction, idMdp, strFechaTrxTuves);

                return Libs.MySqlHelper.GetDataset(exeSql);

                //return Libs.MySqlHelper.GetDataset(exeSql, parametros);
            }
            catch (Exception)
            {
                return new DataSet();
                throw;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
