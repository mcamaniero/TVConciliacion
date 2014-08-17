using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.Data
{
    public class Nucleo : IDisposable
    {
        /// <summary>
        /// Obtiene los datos de un Item catalogo a partir del Codigo de Item y codigo del Catalogo
        /// </summary>
        /// <param name="codigoItemCatalogo">codigoItemCatalogo</param>
        /// <param name="codigoCatalogo">codigoCatalogo</param>
        /// <returns></returns>
        public DataSet GetItemCatalogoPorCodigoItemCodigoCatalogo(string codigoItemCatalogo, string codigoCatalogo)
        {
            try
            {
                DataSet dsItemCatalogo = null;
                var parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@i_codigoItem", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = codigoItemCatalogo;

                parametros[1] = new SqlParameter("@i_codigoCatalogo", SqlDbType.VarChar, 50);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = codigoCatalogo;

                var objDatos = new ClsSqlClientHelper();
                dsItemCatalogo = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetItemCatalogoPorCodigoItemCatalogo, parametros, Constants.DatasetItemCatalogo);
                return dsItemCatalogo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Obtiene los datos de un Item catalogo a partir del Codigo de Item 
        /// </summary>
        /// <param name="catcodigo">item catalogo</param>
        /// <returns></returns>
        public DataSet GetCatalogoporCodigoCatalogo(string codigoCatalogo)
        {
            try
            {
                DataSet dsItemCatalogo = null;
                var parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@i_codigoCatalogo", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = codigoCatalogo;

                var objDatos = new ClsSqlClientHelper();
                dsItemCatalogo = objDatos.DSExecuteQueryStoredProcedure(Constants.SpCatalogoPorCodigoCatalogo, parametros, string.Empty);
                return dsItemCatalogo;

            }
            catch (Exception ex)
            {
                throw ex;
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
