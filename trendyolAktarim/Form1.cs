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
        }

        private async void btnSiparis_Click(object sender, EventArgs e)
        {
            TrendOrderList tdList = new TrendOrderList();
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
            frm.Show();
            this.Hide();
        }

        private void btnSqlKontrol_Click(object sender, EventArgs e)
        {
            
            if (obj.sqlKontrol("","","") > 0)
            {
                obj.CreateDatabaseIfNotExists(obj.ConnString, localDB);
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ITEMS(), "A_ITEMS");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ORDER(), "A_ORDER");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_PAZARYERI(), "A_PAZARYERI");
                obj.CreateTableIfNoExists(obj.ConnString, dbc.A_ORDERLINE(), "A_ORDERLINE");
                obj.CreateProcedureIfNoExists(obj.ConnString, dbc.P_URUNKAYDET(), "P_URUNKAYDET");
                btnSiparis.Show();
                btnSqlKontrol.Hide();
            } else
            {
                MessageBox.Show("Sql bağlantısı başarısız !!!!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
