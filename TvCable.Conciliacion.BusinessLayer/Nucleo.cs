using System;
using System.Data;
using TvCable.Conciliacion.DTO.Entities;

namespace TvCable.Conciliacion.BusinessLayer
{
    public class Nucleo : IDisposable
    {
        /// <summary>
        /// Obtiene los datos de Item catalogo a partir del  codigo del Catalogo
        /// </summary>
        /// <param name="codigoCatalogo">Codigo de catalogo</param>
        /// <returns></returns>
        public ECatalogoData GetCatalogoPorCodigoCatalogo(string codigoCatalogo)
        {
            var response = new ECatalogoData();
            var indexItem = 0;
            try
            {
                DataSet dsCatalogo;
                using (var objNucleo = new Data.Nucleo())
                {
                    dsCatalogo = objNucleo.GetCatalogoporCodigoCatalogo(codigoCatalogo);
                }

                if (dsCatalogo != null)
                {
                    if (dsCatalogo.Tables.Count > 0 && dsCatalogo.Tables[0].Rows.Count > 0)
                    {
                        // Datos del catalogo
                        response.ResponseCode = 0;
                        response.ResponseDescription = "Ok";
                        response.IdCatalogo = (int)dsCatalogo.Tables[0].Rows[0]["CAT_ID"];
                        response.CodigoCatalogo = dsCatalogo.Tables[0].Rows[0]["CAT_CODIGO"].ToString();
                        response.NombreCatalogo = dsCatalogo.Tables[0].Rows[0]["CAT_NOMBRE"].ToString();
                        response.DescripcionCatalogo = dsCatalogo.Tables[0].Rows[0]["CAT_DESCRIPCION"].ToString();
                        response.EstadoCatalogoId = (int)dsCatalogo.Tables[0].Rows[0]["CAT_ESTADO"];

                        // Datos de los Items de catalogo
                        var dtItemsCatalogo = dsCatalogo.Tables[1];
                        var itemsCatalogo = new EItemCatalogoData[dtItemsCatalogo.Rows.Count];
                        foreach (DataRow row in dtItemsCatalogo.Rows)
                        {
                            itemsCatalogo[indexItem] = new EItemCatalogoData
                            {
                                IdItemCatalogo = (int)row["ITC_ID"],
                                IdCatalogo = (int)row["CAT_ID"],
                                CodigoItemCatalogo = row["ITC_CODIGO"].ToString(),
                                NombreItemCatalogo = row["ITC_NOMBRE"].ToString(),
                                ValorItemCatalogo = row["ITC_VALOR"].ToString(),
                                IdEstadoItemCatalogo = (int)row["ITC_ESTADO"],
                                DescripcionItemCatalogo = row["ITC_DESCRIPCION"].ToString()
                            };
                            indexItem++;
                        }
                        response.ItemCatalogo = itemsCatalogo;
                    }
                    else
                    {
                        response.ResponseCode = -1;
                        response.ResponseDescription = "Error al obtener los datos del catalogo";
                    }
                }
                else
                {
                    response.ResponseCode = -1;
                    response.ResponseDescription = "Error al obtener los datos del catalogo";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = -1;
                response.ResponseDescription = "Error al obtener los datos del catalogo";
                return response;
            }
        }



        /// <summary>
        /// Obtiene las funcionalidades a partir del usuario
        /// </summary>
        /// <param name="usuario">usuario</param>
        /// <param name="rol">rol</param>
        /// <returns></returns>
        public EUsuario GetFuncionalidadPorUsuario(string usuario, string rol)
        {
            var response = new EUsuario();
            var indexItem = 0;
            try
            {
                DataSet dsFuncionalidad;
                using (var objNucleo = new Data.Conciliacion())
                {
                    dsFuncionalidad = objNucleo.GetUsuarioFuncionalidades(usuario, rol);
                }

                if (dsFuncionalidad != null)
                {
                    if (dsFuncionalidad.Tables.Count > 0 && dsFuncionalidad.Tables[0].Rows.Count > 0)
                    {
                        // Datos del catalogo
                        response.ResponseCode = 0;
                        response.ResponseDescription = "Ok";
                        response.Usuario = dsFuncionalidad.Tables[0].Rows[0]["USR_USUARIO"].ToString();
                        response.Rol = rol;
                        // Datos de los Items de catalogo
                        var dtItemsCatalogo = dsFuncionalidad.Tables[1];
                        var itemsFuncionalidad = new EFuncionalidadUsuario[dtItemsCatalogo.Rows.Count];
                        foreach (DataRow row in dtItemsCatalogo.Rows)
                        {
                            itemsFuncionalidad[indexItem] = new EFuncionalidadUsuario { Elemento = row["FUN_ELEMENTO"].ToString() };
                            indexItem++;
                        }
                        response.ItemFuncionalidad = itemsFuncionalidad;
                    }
                    else
                    {
                        response.ResponseCode = -1;
                        response.ResponseDescription = "Error al obtener los datos del funcionalidad";
                    }
                }
                else
                {
                    response.ResponseCode = -1;
                    response.ResponseDescription = "Error al obtener los datos del funcionalidad";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = -1;
                response.ResponseDescription = "Error al obtener los datos del funcionalidad";
                return response;
            }
        }



        /// <summary>
        /// Obtiene los datos de un Item catalogo a partir del Codigo de Item y codigo del Catalogo
        /// </summary>
        /// <param name="codigoItemCatalogo">codigoItemCatalogo</param>
        /// <param name="codigoCatalogo">codigoCatalogo</param>
        /// <returns></returns>
        public EItemCatalogoData GetItemCatalogoPorCodigoItemCodigoCatalogo(string codigoItemCatalogo, string codigoCatalogo)
        {
            var objResponse = new EItemCatalogoData();
            try
            {
                DataSet dsItemCatalogo;
                using (var objNucleo = new Data.Nucleo())
                {
                    dsItemCatalogo = objNucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(codigoItemCatalogo, codigoCatalogo);
                }
                if (dsItemCatalogo != null)
                {
                    if (dsItemCatalogo.Tables.Count > 0 && dsItemCatalogo.Tables[0].Rows.Count > 0)
                    {
                        objResponse.IdItemCatalogo = (int)dsItemCatalogo.Tables[0].Rows[0]["ITC_ID"];
                        objResponse.IdCatalogo = (int)dsItemCatalogo.Tables[0].Rows[0]["CAT_ID"];
                        objResponse.CodigoItemCatalogo = codigoItemCatalogo;
                        objResponse.NombreItemCatalogo = dsItemCatalogo.Tables[0].Rows[0]["ITC_NOMBRE"].ToString();
                        objResponse.ValorItemCatalogo = dsItemCatalogo.Tables[0].Rows[0]["ITC_VALOR"].ToString();
                        //objResponse.IdPadreItemCatalogo = (int)dsItemCatalogo.Tables[0].Rows[0]["ITC_PADRE"];
                        objResponse.IdEstadoItemCatalogo = (int)dsItemCatalogo.Tables[0].Rows[0]["ITC_ESTADO"];
                        objResponse.DescripcionItemCatalogo = dsItemCatalogo.Tables[0].Rows[0]["ITC_DESCRIPCION"].ToString();
                        objResponse.CodigoCatalogo = codigoItemCatalogo;
                    }
                    else
                    {
                        objResponse.IdItemCatalogo = -1;
                        objResponse.IdCatalogo = -1;
                        objResponse.CodigoItemCatalogo = codigoItemCatalogo;
                        objResponse.NombreItemCatalogo = string.Empty;
                        objResponse.ValorItemCatalogo = string.Empty;
                        objResponse.IdPadreItemCatalogo = -1;
                        objResponse.IdEstadoItemCatalogo = -1;
                        objResponse.DescripcionItemCatalogo = string.Empty;
                        objResponse.CodigoCatalogo = codigoItemCatalogo;
                    }
                }
                return objResponse;
            }
            catch (Exception ex)
            {
                objResponse.IdItemCatalogo = -1;
                objResponse.IdCatalogo = -1;
                objResponse.CodigoItemCatalogo = codigoItemCatalogo;
                objResponse.NombreItemCatalogo = string.Empty;
                objResponse.ValorItemCatalogo = string.Empty;
                objResponse.IdPadreItemCatalogo = -1;
                objResponse.IdEstadoItemCatalogo = -1;
                objResponse.DescripcionItemCatalogo = string.Empty;
                objResponse.CodigoCatalogo = codigoItemCatalogo;
                return objResponse;
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
