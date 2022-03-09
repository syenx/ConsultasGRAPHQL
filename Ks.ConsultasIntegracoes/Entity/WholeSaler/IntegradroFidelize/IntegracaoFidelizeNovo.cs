using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using DataBaseAccessLib;
using KRAFTSALES.DATAENTITIES;
using Ks.ConsultasIntegracoes.Entity;
using Ks.ConsultasIntegracoes.Entity.Autorizador;
using Ks.ConsultasIntegracoes.Entity.WholeSaler;

using Newtonsoft.Json;

public class IntegracaoFidelizeNovo
{
	public static string TRACK = ((ConfigurationManager.AppSettings["TRACK"] == null) ? (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track\\{0}\\Track_{0}.txt").Replace("file:\\", string.Empty) : ((!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TRACK"].ToString())) ? (ConfigurationManager.AppSettings["TRACK"].ToString() + "\\{0}\\Track_{0}.txt") : (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track\\{0}\\Track_{0}.txt").Replace("file:\\", string.Empty)));

	public static string TRACKDIRECTORY = ((ConfigurationManager.AppSettings["TRACKDIRECTORY"] == null) ? (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track").Replace("file:\\", string.Empty) : ((!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TRACKDIRECTORY"].ToString())) ? ConfigurationManager.AppSettings["TRACKDIRECTORY"].ToString() : (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track").Replace("file:\\", string.Empty)));

	public static string TRACKDIRECTORYDATE = ((ConfigurationManager.AppSettings["TRACKDIRECTORYDATE"] == null) ? (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track\\{0}").Replace("file:\\", string.Empty) : ((!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TRACKDIRECTORYDATE"].ToString())) ? (ConfigurationManager.AppSettings["TRACKDIRECTORYDATE"].ToString() + "\\{0}") : (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Track\\{0}").Replace("file:\\", string.Empty)));

	public Example IntegrarAutorizador(string orderId, string industryCode, string invoice_number)
	{
		if (VerificaSePedidoAutenticadorExiste(orderId))
		{
			IntegracaoFidelizeNovo integraWs = new IntegracaoFidelizeNovo();
			string usuario = GetParametroValue("FidelizeUsuario");
			string Senha = GetParametroValue("FidelizeSenha");
			string Token = integraWs.GetToken(usuario, Senha);
			return integraWs.groupedOrderAutenticador(Token, orderId);
		}
		return null;
	}

	public Root IntegrarWhoSaler(string orderId, string insdutryCode)
	{
		if (VerificaSePedidoWholeSalerExiste(orderId))
		{
			IntegracaoFidelizeNovo integraWs = new IntegracaoFidelizeNovo();
			string usuario = GetParametroValue("FidelizeUsuario");
			string Senha = GetParametroValue("FidelizeSenha");
			string Token = integraWs.GetToken(usuario, Senha);
			return integraWs.groupedOrderWholeSaler(Token, orderId, insdutryCode.ToUpper().Trim());
		}
		return null;
	}

	private string BuscaIndustryCode(string orderId)
	{
		string industryCode = string.Empty;
		DataBaseAccess da = new DataBaseAccess();
		if (!da.open())
		{
			throw new Exception(da.LastMessage);
		}
		string sSQL = $"- {orderId}";
		DataTable dt = da.getDataTable(sSQL, this);
		if (dt != null && dt.Rows.Count > 0)
		{
			industryCode = dt.Rows[0]["industry_code"].ToString();
		}
		return industryCode;
	}

	public bool VerificaSePedidoWholeSalerExiste(string orderId)
	{
		string industryCode = string.Empty;
		DataBaseAccess da = new DataBaseAccess();
		if (!da.open())
		{
			throw new Exception(da.LastMessage);
		}
		string sSQL = $"select count(order_code) qtd from ksPedidoFidelize where order_code =  {orderId}";
		return da.existData(sSQL, this);
	}

	public bool VerificaSePedidoAutenticadorExiste(string orderId)
	{
		string industryCode = string.Empty;
		DataBaseAccess da = new DataBaseAccess();
		if (!da.open())
		{
			throw new Exception(da.LastMessage);
		}
		string sSQL = $"select count(id)  from KSPrePedidoFidelize where id = {orderId}";
		return da.existData(sSQL, this);
	}

	public void GravaArquivo(string Arquivo)
	{
		try
		{
			if (!Directory.Exists(TRACKDIRECTORY))
			{
				Directory.CreateDirectory(TRACKDIRECTORY);
			}
			if (!Directory.Exists(string.Format(TRACKDIRECTORYDATE, DateTime.Now.ToString("dd-MM-yyyy"))))
			{
				Directory.CreateDirectory(string.Format(TRACKDIRECTORYDATE, DateTime.Now.ToString("dd-MM-yyyy")));
			}
			StreamWriter sWriter = new StreamWriter(string.Format(TRACK, DateTime.Now.ToString("dd-MM-yyyy")), append: true, Encoding.Default) ;
			sWriter.WriteLine($"********************\r\n\r\nHora: [{DateTime.Now:HH:mm:ss}]");
			sWriter.WriteLine($"\r\nJson: [{Arquivo}]\r\n");
			sWriter.Flush();
			sWriter.Close();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private static string GetParametroValue(string Key)
	{
		string Retorno = string.Empty;
		try
		{
			ParametrosConfig Param = new ParametrosConfig();
			Param.parametroKey = Key;
			DataTable dt = Param.ListarFiltro();
			if (dt != null)
			{
				if (dt.Rows.Count > 0)
				{
					return dt.Rows[0]["parametroValue"].ToString();
				}
				return Retorno;
			}
			return Retorno;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public string GetToken(string Login, string Senha)
	{
		string url = GetParametroValue("FidelizeUrl");
		string token = "";
		GraphQLClient client = new GraphQLClient(url);
		createToken user = new createToken();
		user.login = Login;
		user.password = Senha;
		string login = JsonConvert.SerializeObject(user, Formatting.Indented).Replace("{", "").Replace("}", "");
		string Query = "\r\n                     mutation createToken { \r\n                         createToken(" + login.Replace("\"login\"", "login").Replace("\"password\"", "password") + "\r\n                         ) { \r\n                           token \r\n                         } }\r\n\r\n                           ";
		dynamic result = client.Execute(Query);
		try
		{
			if (result != null)
			{
				if (!((result.errors == null) ? true : false))
				{
					return "ERRO | " + result.errors[0].message;
				}
				return result.data["createToken"].token;
			}
			return "ERRO | Não retornou Token da fidelize";
		}
		catch (Exception)
		{
			throw;
		}
	}

	public Example groupedOrderAutenticador(string Token, string preOrderId)
	{
		List<Example> lista = new List<Example>();
		Example grouped = new Example();
		string Query = "query groupedOrder{\r\ngroupedOrder(\r\nid: " + preOrderId + "\r\n) {\r\nid\r\ngrouped_order_code\r\nclient_identification\r\nwholesaler\r\nclient_code\r\ncommercial_condition\r\nstatus\r\ntotal_products\r\nproducts\r\n{\r\nean\r\nordered_quantity\r\nwholesaler_discount\r\norder_discount\r\nunit_net_price\r\nindustry_order_code\r\nproduct_reason\r\nindustry_abbreviation\r\n}\r\nresponses {\r\nstatus\r\nprocessed_at\r\norder_motive\r\ntotal_value\r\ndiscount_value\r\nproducts {\r\nean\r\nresponse_quantity\r\npercent_discount\r\nunit_discount_price\r\nunit_net_price\r\nproduct_status\r\nproduct_reason\r\nmonitored\r\nwholesaler_reason\r\nimportation_outcome\r\n}\r\n}\r\ninvoices {\r\nprocessed_at\r\nstatus\r\nissue_date\r\nnumber\r\ndanfe\r\nvalue\r\ndiscount_value\r\nproducts_total_value\r\nproducts {\r\nproduct_status\r\nean\r\ninvoice_quantity\r\npercent_discount\r\n  reimbursement_value\r\nimportation_outcome\r\nbatches {\r\nquantity\r\nidentification\r\ndue_date\r\n}\r\n}\r\n}\r\ncancellations {\r\nstatus\r\nproducts {\r\nean\r\nstatus\r\nimportation_outcome\r\n}\r\n}\r\n}\r\n}\r\n";
		string url = GetParametroValue("FidelizeUrl");
		GraphQLClient client = new GraphQLClient(url);
		try
		{
			try
			{
				GravaArquivo("mutation groupedOrder  " + Query);
			}
			catch
			{
			}
			dynamic result = client.Execute(Query, null, new Dictionary<string, string> {
			{
				"authorization",
				"Bearer " + Token
			} });
			if (result.errors == null)
			{
				string resul2 = Convert.ToString(result.data);
				return JsonConvert.DeserializeObject<Example>(resul2);
			}
			string resul = Convert.ToString(result);
			return grouped;
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}

	private string Mock()
	{
		return "{\r\n     \"groupedOrder\": {\r\n    \"id\": 1540569,\r\n    \"grouped_order_code\": 1540569,\r\n    \"client_identification\": \"01554266000300\",\r\n    \"wholesaler\": \"04307650000305\",\r\n    \"client_code\": \"317565\",\r\n    \"commercial_condition\": \"\",\r\n    \"status\": \"INVOICE_RECEIVED\",\r\n    \"total_products\": 2,\r\n    \"products\": [\r\n      {\r\n                \"ean\": \"7896226505879\",\r\n        \"ordered_quantity\": 2,\r\n        \"wholesaler_discount\": 7.9999,\r\n        \"order_discount\": 8,\r\n        \"unit_net_price\": 1492.7276,\r\n        \"industry_order_code\": 31426,\r\n        \"product_reason\": \"ORDER_CREATED_ON_INDUSTRY_OL_PORTAL\",\r\n        \"industry_abbreviation\": \"RCH\"\r\n      },\r\n      {\r\n                \"ean\": \"7896226505855\",\r\n        \"ordered_quantity\": 4,\r\n        \"wholesaler_discount\": 7.9998,\r\n        \"order_discount\": 8,\r\n        \"unit_net_price\": 597.0984,\r\n        \"industry_order_code\": 31426,\r\n        \"product_reason\": \"ORDER_CREATED_ON_INDUSTRY_OL_PORTAL\",\r\n        \"industry_abbreviation\": \"RCH\"\r\n      }\r\n    ],\r\n    \"responses\": [\r\n      {\r\n                \"status\": \"PROCESSED\",\r\n        \"processed_at\": \"2020-04-27 11:15:11\",\r\n        \"order_motive\": \"ORDER_SUCCESSFULLY_ACCEPTED\",\r\n        \"total_value\": 5373.86,\r\n        \"discount_value\": 0,\r\n        \"products\": [\r\n          {\r\n                    \"ean\": \"7896226505855\",\r\n            \"response_quantity\": 4,\r\n            \"percent_discount\": 0,\r\n            \"unit_discount_price\": 0,\r\n            \"unit_net_price\": 0,\r\n            \"product_status\": \"RESPONSE_IMPORTED_BY_THE_INDUSTRY_OL_PORTAL\",\r\n            \"product_reason\": \"PRODUCT_SUCCESSFULLY_ACCEPTED\",\r\n            \"monitored\": false,\r\n            \"wholesaler_reason\": null,\r\n            \"importation_outcome\": \"Importado com sucesso\"\r\n          },\r\n          {\r\n                    \"ean\": \"7896226505879\",\r\n            \"response_quantity\": 2,\r\n            \"percent_discount\": 0,\r\n            \"unit_discount_price\": 0,\r\n            \"unit_net_price\": 0,\r\n            \"product_status\": \"RESPONSE_IMPORTED_BY_THE_INDUSTRY_OL_PORTAL\",\r\n            \"product_reason\": \"PRODUCT_SUCCESSFULLY_ACCEPTED\",\r\n            \"monitored\": false,\r\n            \"wholesaler_reason\": null,\r\n            \"importation_outcome\": \"Importado com sucesso\"\r\n          }\r\n        ]\r\n      }\r\n    ],\r\n    \"invoices\": [\r\n      {\r\n                \"processed_at\": \"2020-04-27 12:37:00\",\r\n        \"status\": \"PROCESSED\",\r\n        \"issue_date\": \"2020-04-27\",\r\n        \"number\": 106319,\r\n        \"danfe\": \"43200404307650000305550120001063191437120587\",\r\n        \"value\": 5373.86,\r\n        \"discount_value\": 0,\r\n        \"products_total_value\": 5373.86,\r\n        \"products\": [\r\n          {\r\n                    \"product_status\": \"INVOICE_IMPORTED_BY_THE_INDUSTRY_OL_PORTAL\",\r\n            \"ean\": \"7896226505879\",\r\n            \"invoice_quantity\": 2,\r\n            \"percent_discount\": 8,\r\n            \"reimbursement_value\": 259.6,\r\n            \"importation_outcome\": \"Importado com sucesso\",\r\n            \"batches\": []\r\n          },\r\n          {\r\n                    \"product_status\": \"INVOICE_IMPORTED_BY_THE_INDUSTRY_OL_PORTAL\",\r\n            \"ean\": \"7896226505855\",\r\n            \"invoice_quantity\": 4,\r\n            \"percent_discount\": 8,\r\n            \"reimbursement_value\": 207.69,\r\n            \"importation_outcome\": \"Importado com sucesso\",\r\n            \"batches\": []\r\n          }\r\n        ]\r\n      }\r\n    ],\r\n    \"cancellations\": []\r\n  }\r\n    }";
	}

	public Root groupedOrderWholeSaler(string Token, string orderCode, string industryCode)
	{
		List<Root> lista = new List<Root>();
		Root grouped = new Root();
		string Query = "query order {\r\n                        order(\r\n                        industry_code:\"" + industryCode + "\"\r\n                        order_code: " + orderCode + "\r\n                        ) {\r\n                                        id\r\n                                        order_code\r\n                        industry_code\r\n                        wholesaler_branch_code\r\n                        customer_code\r\n                        status\r\n                        products {\r\n                                            ean\r\n                                            amount\r\n                        discount_percentage\r\n                        }\r\n                responses {\r\n                    id\r\n                    importation_outcome\r\n                consideration_code\r\n                products {\r\n                        ean\r\n                        invoice_ean\r\n                response_amount\r\n                unit_discount_percentage\r\n                unit_discount_value\r\n                unit_net_value\r\n                monitored\r\n                industry_consideration\r\n                wholesaler_consideration\r\n                }\r\n                }\r\n                invoices{\r\n                    id\r\n                    importation_status\r\n                importation_outcome\r\n                status\r\n                processed_at\r\n                released_on\r\n                danfe_key\r\n                code\r\n                value\r\n                discount\r\n                products_total_value\r\n                products {\r\n                        ean\r\n                        invoice_amount\r\n                unit_discount_percentage\r\n                }\r\n                    reimbursement_values {\r\n                        ean\r\n                        value\r\n                    }\r\n                }\r\n                cancellations{\r\n                    id\r\n                    imported_at\r\n                outcome\r\n                }\r\n            }\r\n        }\r\n\r\n";
		string url = GetParametroValue("FidelizeUrl");
		GraphQLClient client = new GraphQLClient(url);
		try
		{
			try
			{
				GravaArquivo("mutation groupedOrder  " + Query);
			}
			catch
			{
			}
			dynamic result = client.Execute(Query, null, new Dictionary<string, string> {
			{
				"authorization",
				"Bearer " + Token
			} });
			if (result.errors == null)
			{
				string resul2 = Convert.ToString(result.data);
				return JsonConvert.DeserializeObject<Root>(resul2);
			}
			string resul = Convert.ToString(result);
			return grouped;
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}
}
