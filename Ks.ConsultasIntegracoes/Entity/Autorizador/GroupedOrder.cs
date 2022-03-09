
using Ks.ConsultasIntegracoes.Entity.Autorizador.Response;
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.Autorizador
{
    public class GroupedOrder
    {
        public string id { get; set; }

        public string grouped_order_code { get; set; }

        public string client_identification { get; set; }

        public string wholesaler { get; set; }

        public string client_code { get; set; }

        public string commercial_condition { get; set; }

        public string status { get; set; }

        public string total_products { get; set; }

        public IList<Product> products { get; set; }

        public IList<Respons> responses { get; set; }

        public IList<Ks.ConsultasIntegracoes.Entity.Autorizador.Invoice.Invoice> invoices { get; set; }

        public IList<object> cancellations { get; set; }
    }
}
