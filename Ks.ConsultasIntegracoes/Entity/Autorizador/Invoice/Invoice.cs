
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.Autorizador.Invoice
{
    public class Invoice
    {
        public string processed_at { get; set; }

        public string status { get; set; }

        public string issue_date { get; set; }

        public string number { get; set; }

        public string danfe { get; set; }

        public double value { get; set; }

        public string discount_value { get; set; }

        public double products_total_value { get; set; }

        public IList<Product> products { get; set; }
    }
}
