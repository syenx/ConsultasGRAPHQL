
using System;

namespace Ks.ConsultasIntegracoes.Entity.Usuarios
{
    public class UserDataInfo
    {
        public string UserContent { get; set; }

        public string UserID => this.UserContent.Split(';')[0].ToString();

        public string UserLogin { get; set; }

        public string UserName { get; set; }

        public string UserNameComplete { get; set; }

        public bool UserIsFisrtAccess { get; set; }

        public DateTime? UserUltimoAcesso
        {
            get
            {
                if (string.IsNullOrEmpty(this.UserContent))
                    return new DateTime?();
                if (string.IsNullOrEmpty(this.UserContent.Split(';')[3].ToString()))
                    return new DateTime?();
                return new DateTime?(DateTime.Parse(this.UserContent.Split(';')[3].ToString()));
            }
        }

        public string UserPerfilAcessoNome { get; set; }

        public int UserCasasDecimais => 2;

        public bool IsAdmVendas
        {
            get
            {
                if (string.IsNullOrEmpty(this.UserContent))
                    return false;
                if (string.IsNullOrEmpty(this.UserContent.Split(';')[8].ToString()))
                    return false;
                return bool.Parse(this.UserContent.Split(';')[8].ToString());
            }
        }
    }
}
