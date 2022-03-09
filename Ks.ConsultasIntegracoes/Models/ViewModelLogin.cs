// Decompiled with JetBrains decompiler
// Type: Ks.ConsultasIntegracoes.Models.ViewModelLogin
// Assembly: Ks.ConsultasIntegracoes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 27A17DC9-30B3-4122-983F-D3BC69F2F9EE
// Assembly location: C:\Users\redbu\OneDrive\Área de Trabalho\FidelizeConsultas\bin\Ks.ConsultasIntegracoes.dll

using DataBaseAccessLib;
using System;

namespace Ks.ConsultasIntegracoes.Models
{
    public class ViewModelLogin
    {
        public string usuario { get; set; }

        public string senha { get; set; }

        public bool Login()
        {
            string empty = string.Empty;
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            if (!dataBaseAccess.open())
                throw new Exception(dataBaseAccess.LastMessage);
            string sql = string.Format("select count(order_code) qtd from ksPedidoFidelize where order_code =  {0}", (object)"");
            return dataBaseAccess.existData(sql, (object)this);
        }
    }
}
