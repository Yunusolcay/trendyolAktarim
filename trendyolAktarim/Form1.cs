using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using trendyolAktarim.Models;

namespace trendyolAktarim
{
    public partial class Form1 : Form
    {
        static string localDB = "A_TRENDYOL";
        DBHelper obj = new DBHelper("", localDB);
        DBCreate dbc = new DBCreate();

        public Form1()
        {
            InitializeComponent();
            btnSiparis.Hide();
            btnOrderLine.Hide();
            btnItems.Hide();
        }

        private void btnSqlKontrol_Click(object sender, EventArgs e)
        {

            if (obj.sqlKontrol("", "", "") > 0)
            {
                obj.CreateDatabaseIfNotExists(obj.ConnString, localDB);
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ITEMS(), "A_ITEMS");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ORDER(), "A_ORDER");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_PAZARYERI(), "A_PAZARYERI");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ORDERLINE(), "A_ORDERLINE");
                obj.CreateProcedureIfNoExists(obj.ConnString, dbc.P_URUNKAYDET(), "P_URUNKAYDET");
                btnSiparis.Show();
                btnOrderLine.Show();
                btnItems.Show();
                btnSqlKontrol.Hide();
            }
            else
            {
                MessageBox.Show("Sql bağlantısı başarısız !!!!");
            }
        }

        private async void btnSiparis_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            CheckForIllegalCrossThreadCalls = false;
            int sayfa = 2;
            Trendyol to = new Trendyol();
            for (int i = 1; i <= sayfa; i++)
            {
                TrendOrderList a = await to.getOrdersFromTrendyol(i, 14);
                sayfa = a.totalPages;
                if (a == null || a.content.Count < 1) break;
                foreach (TrendOrder proc in a.content)
                {
                    to.saveOrder(proc);
                }
            }
            siparisListe frm = new siparisListe();
            frm.Sorgu = "SELECT* FROM A_ORDER ORDER BY ID DESC";
            frm.Show();
            this.Hide();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void btnOrderLine_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            CheckForIllegalCrossThreadCalls = false;
            int sayfa = 2;
            Trendyol to = new Trendyol();
            for (int i = 1; i <= sayfa; i++)
            {
                TrendProductList a = await to.getProductList(i);
                sayfa = a.totalPages;
                if (a == null || a.content.Count < 1) break;
                foreach (TrendyolProduct proc in a.content)
                {
                    to.urunKaydet(proc);
                }
            }
            siparisListe frm = new siparisListe();
            frm.Sorgu = "SELECT * FROM A_ORDERLINE ORDER BY ID DESC";
            frm.Show();
            this.Hide();
        }

        private async void btnItems_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            CheckForIllegalCrossThreadCalls = false;
            Trendyol tp = new Trendyol();
            int sayfa = 2;
            for (int i = 0; i <= sayfa; i++)
            {
                TrendProductList a = await tp.getProductList(i);
                sayfa = a.totalPages;
                if (a == null || a.content.Count < 1) break;
                foreach (TrendyolProduct proc in a.content)
                {
                    tp.urunKaydet(proc);
                }
            }
            siparisListe frm = new siparisListe();
            frm.Sorgu = "SELECT * FROM A_ORDERLINE ORDER BY ID DESC";
            frm.Show();
            this.Hide();
        }
    }
}
