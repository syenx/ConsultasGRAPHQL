
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.WholeSaler.InvoicesFIles
{
    public class Invoice
    {
        public string id { get; set; }

        public string importation_status { get; set; }

        public string importation_outcome { get; set; }

        public string status { get; set; }

        public string processed_at { get; set; }

        public string released_on { get; set; }

        public string danfe_key { get; set; }

        public string code { get; set; }

        public string value { get; set; }

        public string discount { get; set; }

        public object products_total_value { get; set; }

        public List<Product> products { get; set; }

        public List<ReimbursementValue> reimbursement_values { get; set; }
    }
}

