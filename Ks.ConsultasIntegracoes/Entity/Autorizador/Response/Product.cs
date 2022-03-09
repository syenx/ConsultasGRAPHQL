
namespace Ks.ConsultasIntegracoes.Entity.Autorizador.Response
{
    public class Product
    {
        public string ean { get; set; }

        public int response_quantity { get; set; }

        public int percent_discount { get; set; }

        public int unit_discount_price { get; set; }

        public int unit_net_price { get; set; }

        public string product_status { get; set; }

        public string product_reason { get; set; }

        public bool monitored { get; set; }

        public object wholesaler_reason { get; set; }

        public string importation_outcome { get; set; }
    }
}
