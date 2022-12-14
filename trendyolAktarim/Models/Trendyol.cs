using BL;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace trendyolAktarim.Models
{
    public class Trendyol
    {
        TrendyolSettings td = new TrendyolSettings();
        public async Task<TrendOrderList> getOrdersFromTrendyol(int page, int days)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                long startDate = td.dateTimeToTicks(DateTime.Now.AddDays(-days));
                long endDate = td.dateTimeToTicks(DateTime.Now);
                string parameter = "?size=200&orderByField=CreatedDate=DESC" +
                    "&startDate=" + startDate + "&endDate=" + endDate + "&page=" + page;
                parameter = "";
                var client = new RestClient(td.apiUrl + "suppliers/" + td.auth.supplierID + "/orders" + parameter);
                client.Authenticator = new HttpBasicAuthenticator(td.auth.apiUserName, td.auth.apiPassword);
                var request = new RestRequest(Method.GET);
                RestResponse resp = (RestResponse)client.Execute(request);
                TrendOrderList orderList = (TrendOrderList)JsonConvert.DeserializeObject(resp.Content, typeof(TrendOrderList));
                return await Task.FromResult<TrendOrderList>(((Func<TrendOrderList>)(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    return orderList;
                }))());
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        public void saveOrder(TrendOrder o)
        {
            DBHelper obj = new DBHelper("A_TRENDYOL", "A_TRENDYOL");
            string Sorgu = "SELECT COUNT(*) FROM A_ORDER WHERE ORDERID='" + o.id + "' AND PZRID=1";
            int snc = obj.ExecScalarReturnInt32(Sorgu);
            if (snc == 0)
            {
                if (o.orderNumber == null)
                {
                    o.orderNumber = "";
                }
                if (o.taxNumber == null)
                {
                    o.taxNumber = "";
                }
                Sorgu = "INSERT A_ORDER VALUES (0,'" + o.customerFirstName + "','" + o.customerLastName + "','" + o.cargoProviderName + "','" + o.cargoTrackingNumber + "'," +
                    "'" + o.cargoTrackingLink + "','" + o.customerEmail + "','" + o.customerId + "','" + o.id + "','" + o.orderNumber + "','" + o.tcIdentityNumber + "','" + o.taxNumber + "'," +
                    "'" + o.shipmentAddress.address1.Replace("'", "") + "','" + o.shipmentAddress.address2.Replace("'", "") + "','" + o.shipmentAddress.city + "'," +
                    "'" + o.shipmentAddress.district + "','" + o.shipmentAddress.postalCode + "','" + o.totalPrice.ToString().Replace(",", ".") + "','','','','','',1," +
                    "'" + o.invoiceAddress.address1.Replace("'", "") + "','" + o.invoiceAddress.address2.Replace("'", "") + "','" + o.invoiceAddress.city + "','" + o.invoiceAddress.district + "')";
                int ORDERID = obj.ExecNonQueryReturnIdentity(Sorgu);
                if (ORDERID > 0)
                {
                    foreach (TrendOrderLine s in o.lines)
                    {
                        if (s.productColor == null)
                        {
                            s.productColor = "";
                        }
                        Sorgu = "INSERT A_ORDERLINE VALUES ('" + ORDERID.ToString() + "','" + s.productCode.Replace("'", "''") + "','" + s.merchantSku.Replace("'", "''") + "','" + s.barcode.Replace("'", "''") + "','" + s.quantity.ToString().Replace(",", ".") + "'," +
                            "'" + s.price.ToString().Replace(",", ".") + "','" + s.productName.Replace("'", " ") + "','" + s.productColor.Replace("'", " ") + "','" + s.vatBaseAmount.ToString().Replace(",", ".") + "'," +
                            "'','','','','','" + s.imageUrl + "')";
                        obj.ExecNonQuery(Sorgu);
                    }
                }
            }
        }
    }

    public class TrendyolSettings
    {
        public string apiUrl = "https://api.trendyol.com/sapigw/";
        public TrendAuthInfo auth = new TrendAuthInfo();
        public long dateTimeToTicks(DateTime date)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            TimeSpan ts = date.Subtract(dtDateTime);
            return (long)ts.TotalMilliseconds;
        }
    }
    public class TrendAuthInfo
    {
        public TrendAuthInfo()
        {
            try
            {
                TrendAuth abc = JsonConvert.DeserializeObject<TrendAuth>(File.ReadAllText("json/trendyol.json"));
                supplierID = abc.supplierID;
                apiUserName = abc.apiUserName;
                apiPassword = abc.apiPassword;
                updateStartDays = 14;
            }
            catch (Exception e)
            {

            }
        }
        public string supplierID = "";
        public string apiUserName = "";
        public string apiPassword = "";
        public int updateStartDays = 14;
    }
    public class TrendAuth
    {
        public string supplierID = "";
        public string apiUserName = "";
        public string apiPassword = "";
        public int updateStartDays = 14;
    }

    public class TrendOrderList
    {
        public int page, size, totalPages, totalElements;
        public List<TrendOrder> content = new List<TrendOrder>();
    }

    public class TrendOrder
    {
        public int REF, ORDFICHEREF, INVOICEREF, PRINTCOUNT, Pickid;
        public Int64 id, orderDate, customerId, cargoTrackingNumber,
            estimatedDeliveryStartDate, estimatedDeliveryEndDate, shipmentAddressId, invoiceAddressId;
        public DateTime orderDateD, estimatedDeliveryStartDateD, estimatedDeliveryEndDateD;
        public string orderNumber, taxNumber, customerFirstName, customerLastName,
            tcIdentityNumber, customerEmail, cargoTrackingLink, cargoProviderName, currencyCode,
            shipmentPackageStatus, deliveryType, packageStatusTR, ORDNUMBER, INVNUMBER;
        public float grossAmount, totalDiscount, totalPrice;
        public bool allowSave, allowPrint;

        public TrendAddress shipmentAddress;
        public TrendAddress invoiceAddress;
        public List<TrendOrderHistory> packageHistories = new List<TrendOrderHistory>();
        public List<TrendOrderLine> lines = new List<TrendOrderLine>();


        public TrendOrder()
        { }

        public TrendOrder(DataRow dr)
        {
            id = Int64.Parse(dr["id"].ToString());
            shipmentAddressId = Int64.Parse(dr["shipmentAddressId"].ToString());
            invoiceAddressId = Int64.Parse(dr["invoiceAddressId"].ToString());
            orderDate = Int64.Parse(dr["orderDate"].ToString());
            customerId = Int64.Parse(dr["customerId"].ToString());
            cargoTrackingNumber = Int64.Parse(dr["cargoTrackingNumber"].ToString());
            cargoTrackingLink = dr["cargoTrackingLink"].ToString();
            estimatedDeliveryStartDate = Int64.Parse(dr["estimatedDeliveryStartDate"].ToString());
            estimatedDeliveryEndDate = Int64.Parse(dr["estimatedDeliveryEndDate"].ToString());
            orderDateD = ticksToDateTime(orderDate);
            estimatedDeliveryStartDateD = ticksToDateTime(estimatedDeliveryStartDate);
            estimatedDeliveryEndDateD = ticksToDateTime(estimatedDeliveryEndDate);

            grossAmount = float.Parse(dr["grossAmount"].ToString().Replace(",", "."));
            totalDiscount = float.Parse(dr["totalDiscount"].ToString().Replace(",", "."));
            totalPrice = float.Parse(dr["totalPrice"].ToString().Replace(",", "."));

            orderNumber = dr["orderNumber"].ToString();
            taxNumber = dr["taxNumber"].ToString();
            customerFirstName = dr["customerFirstName"].ToString();
            customerLastName = dr["customerLastName"].ToString();
            tcIdentityNumber = dr["tcIdentityNumber"].ToString();
            customerEmail = dr["customerEmail"].ToString();
            cargoProviderName = dr["cargoProviderName"].ToString();
            currencyCode = dr["currencyCode"].ToString();
            shipmentPackageStatus = dr["shipmentPackageStatus"].ToString();
            deliveryType = dr["deliveryType"].ToString();
            REF = int.Parse("REF");
            ORDFICHEREF = int.Parse("ORDFICHEREF");
            INVOICEREF = int.Parse("INVOICEREF");
            ORDNUMBER = dr["ORDNUMBER"].ToString();
            INVNUMBER = dr["INVNUMBER"].ToString();
            PRINTCOUNT = int.Parse("PRINTCOUNT");
            Pickid = int.Parse("Pickid");

            allowSave = false;
            switch (shipmentPackageStatus)
            {
                case "ReadyToShip":
                    packageStatusTR = "Yeni Sipariş";
                    allowSave = true;
                    allowPrint = true;
                    break;
                case "Picking":
                    packageStatusTR = "Hazırlanıyor";
                    allowSave = true;
                    allowPrint = true;
                    break;
                case "Shipped":
                    allowSave = true;
                    packageStatusTR = "Kargoya Verildi";
                    break;
                case "Cancelled":
                    packageStatusTR = "İptal Edildi";
                    break;
                case "Delivered":
                    allowSave = true;
                    packageStatusTR = "Teslim Edildi";
                    break;
                case "UnDeliveredAndReturned":
                    packageStatusTR = "Teslim edilemedi İade";
                    break;
                case "UnDelivered":
                    packageStatusTR = "Teslim Edilemedi";
                    break;
                case "UnPacked":
                    packageStatusTR = "Bu Kayıt Değişti";
                    break;
                default:
                    packageStatusTR = "Sipariş";
                    break;
            }
            if (ORDFICHEREF > 0 || INVOICEREF > 0) allowSave = false;

            //if (id < 143813700) allowSave = false;

            DateTime ticksToDateTime(double ticks)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
                dtDateTime = dtDateTime.AddMilliseconds(ticks).ToLocalTime();
                return dtDateTime;
            }
        }


    }

    public class TrendAddress
    {
        public Int64 id;
        public string firstName, lastName, company, address1, address2, city, cityCode,
            district, districtId, postalCode, countryCode, fullAddress, fullName;

        public TrendAddress()
        { }

        public TrendAddress(DataRow dr)
        {
            id = Int64.Parse(dr["id"].ToString());
            firstName = dr["firstName"].ToString();
            lastName = dr["lastName"].ToString();
            company = dr["company"].ToString();
            address1 = dr["address1"].ToString();
            address2 = dr["address2"].ToString();
            city = dr["city"].ToString();
            cityCode = dr["cityCode"].ToString();
            district = dr["district"].ToString();
            districtId = dr["districtId"].ToString();
            postalCode = dr["postalCode"].ToString();
            countryCode = dr["countryCode"].ToString();
            fullAddress = dr["fullAddress"].ToString();
            fullName = dr["fullName"].ToString();

        }
    }

    public class TrendOrderLine
    {
        public Int64 id, orderId, productId, merchantId, salesCampaignId;
        public int quantity;
        public string productCode, sku, barcode, productSize, merchantSku, productName,
                     currencyCode, productColor, orderLineItemStatusName, imageUrl;
        public float amount, price, discount, lineItemPrice, vatBaseAmount;

        public List<TrendOrderLineDiscount> discountDetails = new List<TrendOrderLineDiscount>();

        public TrendOrderLine()
        { }

        public TrendOrderLine(DataRow dr)
        {
            id = Int64.Parse(dr["id"].ToString());
            orderId = Int64.Parse(dr["orderId"].ToString());
            productId = Int64.Parse(dr["productId"].ToString());
            merchantId = Int64.Parse(dr["merchantId"].ToString());
            salesCampaignId = Int64.Parse(dr["salesCampaignId"].ToString());
            quantity = int.Parse(dr["quantity"].ToString());
            imageUrl = dr["imageUrl"].ToString();
            sku = dr["sku"].ToString();
            barcode = dr["barcode"].ToString();
            productSize = dr["productSize"].ToString();
            merchantSku = dr["merchantSku"].ToString();
            productName = dr["productName"].ToString();
            productCode = dr["productCode"].ToString();
            currencyCode = dr["currencyCode"].ToString();
            productColor = dr["productColor"].ToString();
            orderLineItemStatusName = dr["orderLineItemStatusName"].ToString();
            amount = float.Parse(dr["amount"].ToString().Replace(",","."));
            price = float.Parse(dr["price"].ToString().Replace(",", "."));
            discount = float.Parse(dr["discount"].ToString().Replace(",", "."));
            lineItemPrice = float.Parse(dr["lineItemPrice"].ToString().Replace(",", "."));
            vatBaseAmount = float.Parse(dr["vatBaseAmount"].ToString().Replace(",", "."));
        }

    }

    public class TrendOrderStatus
    {
        public TrendOrderStatusLine[] lines = new TrendOrderStatusLine[1];
        //public TrendOrderStatusParam @params;// = new TrendOrderStatusParam();
        public string status = "Picking";// "Invoiced";// "Picking";
    }

    public class TrendOrderHistory
    {
        public Int64 createdDate, cargoTrackingNumber;
        public string status = "";
        public DateTime createdDateD;
        public Int64 orderId;
        public string orderNumber = "";

    }

    public class TrendOrderStatusParam
    {
        public string invoiceNumber = "";
    }

    public class TrendOrderStatusLine
    {
        public string lineId = "";
        public int quantity = 1;
    }

    public class TrendOrderLineDiscount
    {
        public float lineItemPrice, lineItemDiscount;
    }

    public class PickInfo
    {
        public int Pickid;
        public string Tarih;
        public string Aciklama;
        public string DosyaAdi;
        public int PrintCount;
        public int SiparisSayisi;
        public int ArasToplam;
        public int ArasSevk;
        public int TrendToplam;
        public int TrendSevk;

        public PickInfo(DataRow dr)
        {
            Pickid = int.Parse(dr["Pickid"].ToString());
            PrintCount = int.Parse(dr["PrintCount"].ToString());
            SiparisSayisi = int.Parse(dr["SiparisSayisi"].ToString());
            Tarih = dr["Tarih"].ToString();
            Aciklama = dr["Aciklama"].ToString();
            DosyaAdi = dr["DosyaAdi"].ToString();
            ArasToplam = int.Parse(dr["ArasToplam"].ToString());
            ArasSevk = int.Parse(dr["ArasSevk"].ToString());
            TrendToplam = int.Parse(dr["TrendToplam"].ToString());
            TrendSevk = int.Parse(dr["TrendSevk"].ToString());
        }
    }
}
