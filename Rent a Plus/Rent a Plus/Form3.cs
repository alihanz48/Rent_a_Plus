using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Rent_a_Plus
{
    public partial class Form3 : Form
    {

        /* Bu projeyi yazılıma başladığım ilk zamanlarda geliştirdim.Bu sebeple gereksiz kullanımlarım/hatalarım olabilir*/

        public Form3()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|rentaplus.accdb");
        OleDbCommand komut = new OleDbCommand();

        private void Form3_Load(object sender, EventArgs e)
        {
            araccek();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            araccek();
        }

        public void araccek()
        {
            listView1.Size = new Size(this.Width, this.Height);
            listView1.Location = new Point(0, button1.Size.Height - 2);
            listView1.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select * from araclar";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = oku["id"].ToString();
                item.SubItems.Add(oku["marka"].ToString());
                item.SubItems.Add(oku["model"].ToString());
                item.SubItems.Add(oku["modelyili"].ToString());
                item.SubItems.Add(oku["plaka"].ToString());
                item.SubItems.Add(oku["saseno"].ToString());
                item.SubItems.Add(oku["motor"].ToString());
                item.SubItems.Add(oku["km"].ToString());
                item.SubItems.Add(oku["renk"].ToString());
                item.SubItems.Add(oku["fiyat"].ToString());
                item.SubItems.Add(oku["durum"].ToString());
                listView1.Items.Add(item);
            }
            baglanti.Close();
        }
    }
}
