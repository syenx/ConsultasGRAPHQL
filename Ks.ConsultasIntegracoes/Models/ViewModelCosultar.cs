// Decompiled with JetBrains decompiler
// Type: Ks.ConsultasIntegracoes.Models.ViewModelCosultar
// Assembly: Ks.ConsultasIntegracoes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 27A17DC9-30B3-4122-983F-D3BC69F2F9EE
// Assembly location: C:\Users\redbu\OneDrive\Área de Trabalho\FidelizeConsultas\bin\Ks.ConsultasIntegracoes.dll

using Ks.ConsultasIntegracoes.Entity.Autorizador;
using Ks.ConsultasIntegracoes.Entity.WholeSaler;

using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Models
{
    public class ViewModelCosultar
    {
        public List<string> CNPJs { get; set; }

        public string numero_order { get; set; }

        public string CNPJ { get; set; }

        public string numero_invoice { get; set; }

        public string insdutryCode { get; set; }

        public List<Example> OrdersResponseAutorizador { get; set; }

        public List<Root> OrdersResponseWholeSaler { get; set; }

        public List<Example> BuscarResultadoAutorizador()
        {
            List<Example> exampleList = new List<Example>();
            if (!string.IsNullOrEmpty(this.numero_order))
            {
                string numeroOrder = this.numero_order;
                char[] chArray = new char[1] { ',' };
                foreach (string orderId in numeroOrder.Split(chArray))
                {
                    Example example = new IntegracaoFidelizeNovo().IntegrarAutorizador(orderId, this.CNPJ, this.numero_invoice);
                    if (example != null && example.groupedOrder != null)
                        exampleList.Add(example);
                }
            }
            return exampleList;
        }

        public List<Root> BuscarResultadoWholeSaler()
        {
            List<Root> rootList = new List<Root>();
            if (!string.IsNullOrEmpty(this.numero_invoice))
            {
                string numeroInvoice = this.numero_invoice;
                char[] chArray = new char[1] { ',' };
                foreach (string orderId in numeroInvoice.Split(chArray))
                {
                    Root root = new IntegracaoFidelizeNovo().IntegrarWhoSaler(orderId, this.insdutryCode);
                    if (root != null && root.order != null)
                        rootList.Add(root);
                }
            }
            return rootList;
        }
    }
}
