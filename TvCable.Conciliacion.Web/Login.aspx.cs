using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using TvCable.Conciliacion.BusinessLayer;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.Web
{
    public partial class Login : System.Web.UI.Page
    {
        #region
        internal TvCable.Conciliacion.Libs.Base Base = new Base();
        internal BusinessLayer.Nucleo Nucleo = new BusinessLayer.Nucleo();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Método que valida al usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoginTvCableAuthenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {//Se captura las variables del texto de inicio de sesión
                string username = LoginTvcable.UserName;
                string pwd = LoginTvcable.Password;
                string rol = SimulacionWebUser(username);
                var userIedentificatio = new Nucleo();
                var Userfuncionalidad = userIedentificatio.GetFuncionalidadPorUsuario(username, rol);
                if (Userfuncionalidad.ResponseDescription == "Ok")
                {
                    Session["UserAutentication"] = Userfuncionalidad;
                    FormsAuthentication.RedirectFromLoginPage(LoginTvcable.UserName, LoginTvcable.RememberMeSet);
                    Base.WriteLog(Base.ErrorTypeEnum.Start, 1, "Ha iniciado Sesión el usuario :" + username + ", con rol :" + rol);
                }
                else
                {
                    Session["UserAuthentication"] = null;
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 2, "Excepción al verificar el usuario , mensaje" + ex.Message + ". Excepción; " + ex.ToString());
            }
        }

        /// <summary>
        /// Simulacion de WebService
        /// </summary>

        protected string SimulacionWebUser(string usuario)
        {
            string Usuraio1 = "Tvcable";
            string Rol = null;
            string Usuario2 = "TvConciliacion";

            if (usuario.Equals(Usuraio1))
            {
                Rol = "Read only";
            }
            else if (usuario.Equals(Usuario2))
            {
                Rol = "Admin advanced";
            } return Rol;
        }
    }
}