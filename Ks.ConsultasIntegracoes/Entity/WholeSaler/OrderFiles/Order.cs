
using Ks.ConsultasIntegracoes.Entity.WholeSaler.InvoicesFIles;
using Ks.ConsultasIntegracoes.Entity.WholeSaler.ResponsFiles;
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.WholeSaler.OrderFiles
{
    public class Order
    {
        public string id { get; set; }

        public string order_code { get; set; }

        public string industry_code { get; set; }

        public string wholesaler_branch_code { get; set; }

        public string customer_code { get; set; }

        public string status { get; set; }

        public List<Product> products { get; set; }

        public List<Respons> responses { get; set; }

        public List<Invoice> invoices { get; set; }

        public List<object> cancellations { get; set; }
    }
}
