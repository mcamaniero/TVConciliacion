using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TvCable.Conciliacion.Web
{
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {


            string username = Login2.UserName;
            string pwd = Login2.Password; 

            string s;
            //s = WebConfigurationManager.ConnectionStrings["ChartDatabaseConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(s);
            //con.Open();
            //string sqlUserName;
            //sqlUserName = "SELECT user_name,password FROM Login WHERE user_name ='" + username + "' AND password ='" + pwd + "'";
            // SqlCommand cmd = new SqlCommand(sqlUserName, con);

            // string CurrentName;
            // CurrentName = (string)cmd.ExecuteScalar();

            if (username != null)
            {
                Session["usuario"] = username; // admin
                //Session.Timeout = 1;
                Response.Redirect("MainConciliacion.aspx");
            }
            else
            {
                Session["UserAuthentication"] = "";
            }
        }
    }
}