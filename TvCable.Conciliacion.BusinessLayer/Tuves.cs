using System;
using System.Data;

namespace TvCable.Conciliacion.BusinessLayer
{
    public class Tuves : IDisposable
    {
        public DataSet ObtenerTransaccionesPorFechaMdp(string idMdp, DateTime fechaPago)
        {
            try
            {
                using (var objTuves = new Data.Tuves())
                {
                    return objTuves.ObtenerTransaccionesPorFechaMdp(idMdp, fechaPago);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
