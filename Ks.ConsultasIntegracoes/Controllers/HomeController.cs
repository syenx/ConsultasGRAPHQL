
using KRAFTSALES.DATAENTITIES.Entidades;
using Ks.ConsultasIntegracoes.Models;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;

namespace Ks.ConsultasIntegracoes.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewModelLogin login = new ViewModelLogin();
            if (this.Session.Keys.Count != 0)
            {
                login.usuario = this.Session[0].ToString();
                login.senha = this.Session[1].ToString();
                if (this.VerificaSeLoginExiste(login))
                    return (ActionResult)this.View(nameof(Index), (object)new ViewModelCosultar());
                this.Response.Redirect("~/Login/Logar");
                return (ActionResult)this.View("Login");
            }
            this.Response.Redirect("~/Login/Logar");
            return (ActionResult)this.View("Login");
        }

        public ActionResult CreateAutorizador() => (ActionResult)this.View("Index", (object)new ViewModelCosultar());

        [HttpPost]
        public ActionResult CreateAutorizador(ViewModelCosultar model)
        {
            model.OrdersResponseAutorizador = model.BuscarResultadoAutorizador();
            return (ActionResult)this.View("Index", (object)model);
        }

        public ActionResult CreateWholeSaler() => (ActionResult)this.View("Index", (object)new ViewModelCosultar());

        [HttpPost]
        public ActionResult CreateWholeSaler(ViewModelCosultar model)
        {
            if (string.IsNullOrEmpty(model.numero_invoice) || string.IsNullOrEmpty(model.insdutryCode))
                return (ActionResult)this.View("Index", (object)new ViewModelCosultar());
            model.OrdersResponseWholeSaler = model.BuscarResultadoWholeSaler();
            return (ActionResult)this.View("Index", (object)model);
        }

        private bool VerificaSeLoginExiste(ViewModelLogin login)
        {
            try
            {
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new Usuario()
                {
                    usuarioLogin = login.usuario,
                    usuarioSenha = FormsAuthentication.HashPasswordForStoringInConfigFile(login.senha, "SHA1")
                }.ListarLogin();
                return dataTable2 != null && dataTable2.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
