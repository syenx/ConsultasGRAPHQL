
namespace Ks.ConsultasIntegracoes.Entity.WholeSaler.ResponsFiles
{
    public class Product
    {
        public string ean { get; set; }

        public int amount { get; set; }

        public int discount_percentage { get; set; }

        public object invoice_ean { get; set; }

        public int response_amount { get; set; }

        public int unit_discount_percentage { get; set; }

        public int unit_discount_value { get; set; }

        public double unit_net_value { get; set; }

        public bool monitored { get; set; }

        public string industry_consideration { get; set; }

        public object wholesaler_consideration { get; set; }

        public int invoice_amount { get; set; }
    }
}
