using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trendyolAktarim.Models
{
    class DBCreate
    {

        public string A_ITEMS()
        {
            string snc = "CREATE TABLE [dbo].[A_ITEMS]( \r\n" +
            "ID int IDENTITY(1, 1) NOT NULL, \r\n" +
            "urunID nvarchar(250) NULL, \r\n" +
            "productCode nvarchar(150) NULL, \r\n" +
            "brand nvarchar(150) NULL, \r\n" +
            "barcode nvarchar(150) NULL, \r\n" +
            "title nvarchar(500) NULL, \r\n" +
            "categoryName nvarchar(250) NULL,\r\n" +
            "listPrice float NULL, \r\n" +
            "salePrice float NULL, \r\n" +
            "vat float NULL, \r\n" +
            "stockCode nvarchar(150) NULL, \r\n" +
            "stockId nvarchar(150) NULL, \r\n" +
            "resimUrl nvarchar(500) NULL, \r\n" +
            "specode1 nvarchar(100) NULL, \r\n" +
            "specode2 nvarchar(100) NULL, \r\n" +
            "specode3 nvarchar(100) NULL, \r\n" +
            "specode4 nvarchar(100) NULL, \r\n" +
            "specode5 nvarchar(100) NULL, \r\n" +
            "pazarID int NULL \r\n" +
            ") ON[PRIMARY]";
            return snc;
        }

        public string A_ORDER()
        {
            string snc = "CREATE TABLE [dbo].[A_ORDER]( \r\n" +
                        "[ID][int] IDENTITY(1, 1) NOT NULL, \r\n" +
                        "[FISREF] [int] NULL, \r\n" +
                        "[NAME] [nvarchar](100) NULL, \r\n" +
                        "[SURNAME] [nvarchar](100) NULL, \r\n" +
                        "[CARGO] [nvarchar](150) NULL, \r\n" +
                        "[CARGONO] [nvarchar](100) NULL, \r\n" +
                        "[CARGOURL] [nvarchar](500) NULL,  \r\n" +
                        "[EMAIL] [nvarchar](150) NULL, \r\n" +
                        "[CUSID] [nvarchar](150) NULL, \r\n" +
                        "[ORDERID] [nvarchar](150) NULL, \r\n" +
                        "[ORDERNO] [nvarchar](150) NULL, \r\n" +
                        "[TCNO] [nvarchar](50) NULL, \r\n" +
                        "[TAXNO] [nvarchar](50) NULL, \r\n" +
                        "[ADRESS1] [nvarchar](500) NULL, \r\n" +
                        "[ADRESS2] [nvarchar](500) NULL, \r\n" +
                        "[CITY] [nvarchar](100) NULL, \r\n" +
                        "[DISTRICT] [nvarchar](100) NULL, \r\n" +
                        "[POSTALCODE] [nvarchar](50) NULL, \r\n" +
                        "[TOTALPRICE] [float] NULL, \r\n" +
                        "[SPECODE1] [nvarchar](200) NULL, \r\n" +
                        "[SPECODE2] [nvarchar](200) NULL, \r\n" +
                        "[SPECODE3] [nvarchar](200) NULL, \r\n" +
                        "[SPECODE4] [nvarchar](200) NULL, \r\n" +
                        "[SPECODE5] [nvarchar](200) NULL, \r\n" +
                        "[PZRID] [int] NULL, \r\n" +
                        "[FATADRESS1] [nvarchar](500) NULL, \r\n" +
                        "[FATADRESS2] [nvarchar](500) NULL, \r\n" +
                        "[FATCITY] [nvarchar](100) NULL, \r\n" +
                        "[FATDISTRICT] [nvarchar](100) NULL \r\n" +
                        ") ON[PRIMARY]";


            return snc;
        }

        public string A_ORDERLINE()
        {
            string snc = "CREATE TABLE [dbo].[A_ORDERLINE]( \r\n" +
                        "[ID][int] IDENTITY(1, 1) NOT NULL, \r\n" +
                        "[orderID] [nvarchar](150) NULL, \r\n" +
                        "[productCode] [nvarchar](100) NULL, \r\n" +
                        "[merchantSku] [nvarchar](100) NULL, \r\n" +
                        "[barcode] [nvarchar](100) NULL, \r\n" +
                        "[amount] [float] NULL, \r\n" +
                        "[price] [float] NULL, \r\n" +
                        "[productName] [nvarchar](250) NULL, \r\n" +
                        "[productColor] [nvarchar](100) NULL, \r\n" +
                        "[vat] [float] NULL, \r\n" +
                        "[specode1] [nvarchar](100) NULL, \r\n" +
                        "[specode2] [nvarchar](100) NULL, \r\n" +
                        "[specode3] [nvarchar](100) NULL, \r\n" +
                        "[specode4] [nvarchar](100) NULL, \r\n" +
                        "[specode5] [nvarchar](100) NULL, \r\n" +
                        "[resimUrl] [nvarchar](500) NULL \r\n" +
                        ") ON[PRIMARY]";


            return snc;
        }

        public string P_URUNKAYDET()
        {
            string snc = "CREATE PROCEDURE [dbo].[P_URUNKAYDET] \r\n" +
            "@urunID nvarchar(250),@productCode nvarchar(100),@brand nvarchar(100),@barcode nvarchar(100),@title nvarchar(250),@categoryName nvarchar(150),@listPrice float, @salePrice float, \r\n" +
            " @vat float, @stockCode nvarchar(100),@stockID nvarchar(100),@resimUrl nvarchar(500),@specode1 nvarchar(100),@specode2 nvarchar(100),@specode3 nvarchar(100),@specode4 nvarchar(100),@specode5 nvarchar(100), \r\n" +
            "@pazarID int \r\n" +
            "AS \r\n" +
            "DECLARE @kontrol int \r\n" +
            "SET @kontrol = (SELECT COUNT(*) FROM A_ITEMS WHERE urunID = @urunID AND pazarID = @pazarID) \r\n" +
            "IF @kontrol > 0 \r\n" +
            "BEGIN \r\n" +
            "    UPDATE A_ITEMS SET listPrice = @listPrice,salePrice = @salePrice,vat = @vat WHERE urunID = @urunID AND pazarID = @pazarID \r\n" +
            "END \r\n" +
            "ELSE \r\n" +
            "BEGIN \r\n" +
            "    INSERT A_ITEMS VALUES(@urunID, @productCode, @brand, @barcode, @title, @categoryName, @listPrice, @salePrice, @vat, @stockCode, @stockID, @resimUrl, @specode1, @specode2, @specode3, @specode4, @specode5, @pazarID) \r\n" +
            "END";
            return snc;
        }

        public string A_PAZARYERI()
        {
            string snc = "CREATE TABLE [dbo].[A_PAZARYERI]( \r\n" +
                        "[ID][int] IDENTITY(1, 1) NOT NULL, \r\n" +
                        "[TYPE] [int] NULL, \r\n" +
                        "[JSON] [varchar](max)NULL \r\n" +
                        ") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";
            return snc;
        }

    }
}
