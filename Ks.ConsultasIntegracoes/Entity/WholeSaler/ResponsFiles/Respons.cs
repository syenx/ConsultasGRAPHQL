
using System.Collections.Generic;

namespace Ks.ConsultasIntegracoes.Entity.WholeSaler.ResponsFiles
{
    public class Respons
    {
        public string id { get; set; }

        public string importation_outcome { get; set; }

        public string consideration_code { get; set; }

        public List<Product> products { get; set; }
    }
}
