
using Ks.ConsultasIntegracoes.Entity.Usuarios;
using Ks.ConsultasIntegracoes.Models;
using KS.SimuladorPrecos.DataEntities.Entidades;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;

namespace Ks.ConsultasIntegracoes.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Logar() => (ActionResult)this.View();

        [HttpPost]
        public ActionResult Logar(ViewModelLogin login)
        {
            string url = "~/home/index";
            if (!this.ModelState.IsValid)
                return (ActionResult)this.View((object)login);
            Usuario usuario = this.EfetuaLogin(login);
            if (usuario.usuarioSenha != null)
            {
                if (object.Equals((object)usuario.usuarioSenha, (object)login.senha))
                {
                    FormsAuthentication.SetAuthCookie(usuario.usuarioLogin, false);
                    if (this.Url.IsLocalUrl(url) && url.Length > 1 && (url.StartsWith("/") && !url.StartsWith("//")) && url.StartsWith("/\\"))
                        return (ActionResult)this.Redirect(url);
                    this.Session[nameof(login)] = (object)usuario.usuarioLogin;
                    this.Session["senha"] = (object)usuario.usuarioSenha;
                    return (ActionResult)this.RedirectToAction("Index", "Home");
                }
                this.ModelState.AddModelError("", "Senha informada Inválida!!!");
                return (ActionResult)this.View();
            }
            this.ModelState.AddModelError("", "Usuário sem acesso para usar o sistema!!!");
            return (ActionResult)this.View();
        }

        private Usuario EfetuaLogin(ViewModelLogin login)
        {
            try
            {
                DataTable dataTable1 = new DataTable();
                AcessoCargaSimulador acessoCargaSimulador = new AcessoCargaSimulador();
                DataTable dataTable2 = new Usuario()
                {
                    usuarioLogin = login.usuario,
                    usuarioSenha = FormsAuthentication.HashPasswordForStoringInConfigFile(login.senha, "SHA1")
                }.ListarLogin();
                if (dataTable2 == null)
                    return new Usuario();
                if (dataTable2.Rows.Count <= 0)
                    return new Usuario();
                return new Usuario()
                {
                    usuarioLogin = dataTable2.Rows[0]["usuarioLogin"].ToString(),
                    usuarioSenha = login.senha
                };
            }
            catch
            {
                return new Usuario();
            }
        }
    }
}
