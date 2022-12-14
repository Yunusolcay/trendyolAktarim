using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trendyolAktarim.Models;
using BL;

namespace trendyolAktarim
{
    public partial class siparisListe : Form
    {
        
        public siparisListe()
        {
            InitializeComponent();
        }

        public string Sorgu = "";
        private void siparisListe_Load(object sender, EventArgs e)
        {
            DBHelper obj = new DBHelper("A_TRENDYOL", "A_TRENDYOL");
            dataGridView1.DataSource = obj.SelectDataTable(Sorgu);
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void siparisListe_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
