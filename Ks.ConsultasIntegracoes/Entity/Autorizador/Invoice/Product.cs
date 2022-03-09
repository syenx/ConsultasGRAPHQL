
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.Autorizador.Invoice
{
    public class Product
    {
        public string product_status { get; set; }

        public string ean { get; set; }

        public int invoice_quantity { get; set; }

        public string percent_discount { get; set; }

        public string reimbursement_value { get; set; }

        public string importation_outcome { get; set; }

        public List<object> batches { get; set; }
    }
}
