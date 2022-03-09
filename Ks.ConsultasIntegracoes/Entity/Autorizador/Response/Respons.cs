
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.Autorizador.Response
{
    public class Respons
    {
        public string status { get; set; }

        public string processed_at { get; set; }

        public string order_motive { get; set; }

        public double total_value { get; set; }

        public string discount_value { get; set; }

        public IList<Product> products { get; set; }
    }
}
