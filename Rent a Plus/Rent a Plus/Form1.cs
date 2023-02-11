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
using System.IO;

namespace Rent_a_Plus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      /*-------------------------------------------------------------------------------------------
        -------İlk büyük projem olduğu için gereksiz kullanımlarım ve hatalarım olabilir.----------
        -------------------------------------------------------------------------------------------*/
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|rentaplus.accdb");
        OleDbCommand komut = new OleDbCommand();
        string dgmtrh;
        string kirabas;
        string kirabitis;
        string arcteslimtarh;
        private void Form1_Load(object sender, EventArgs e)
        {
            arackiralapanel.Location = new Point(label10.Location.X, label10.Location.Y);
            araclarpanel.Location= new Point(label10.Location.X, label10.Location.Y);
            aracteslimpanel.Location= new Point(label10.Location.X, label10.Location.Y);
            foreach (var dsylr in Directory.GetFiles("images/dlt"))
            {
                File.Delete(dsylr);
            }
            panel4.Location = new Point(panel1.Size.Width+((this.Size.Width-panel1.Size.Width-panel4.Size.Width)/2),10);
            dataGridView1.Location = new Point(panel1.Size.Width+((this.Size.Width-panel1.Size.Width-dataGridView1.Size.Width)/2),button2.Location.Y);
            panel6.Location = new Point(dataGridView1.Location.X,dataGridView1.Location.Y-panel6.Size.Height);
            
            OleDbDataReader oku;

            baglanti.Open();
            komut.Connection = baglanti;
            string bay = System.DateTime.Now.Month.ToString();
            komut.CommandText = "SELECT Sum(kayitlar.para) AS Toplapara FROM kayitlar WHERE Month([kirabitis]) = "+bay+"; ";
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                label5.Text = oku["Toplapara"].ToString();
            }
            baglanti.Close();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT Count(araclar.durum) AS Saydurum FROM araclar HAVING durum ='dolu'";
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                label6.Text = oku["Saydurum"].ToString();
            }
            baglanti.Close();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select Count(araclar.id) AS sayid from araclar";
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                label7.Text = oku["sayid"].ToString();
            }
            baglanti.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            arackiralapanel.Size = new Size(0, 0);
            aracteslimpanel.Size = new Size(0, 0);
            araclarpanel.Size = new Size(0, 0);
            temizle1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (arackiralapanel.Width == 0)
            {
                arackiralapanel.Size = new Size(this.Width - panel1.Width, panel1.Height);
            }
            arackiralapanel.BringToFront();

            temizle1();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select marka from araclar where durum='bos' GROUP BY marka";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox2.Items.Add(oku["marka"].ToString());
            }
            baglanti.Close();
        }

        void temizle1()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox7.Clear();
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            label21.ForeColor = Color.Black;
            label21.Text = "";
            label20.ForeColor = Color.Black;
            label20.Text = "";
            textBox5.Text = "";
            comboBox8.Items.Clear();
            comboBox8.Text = "";
            textBox6.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox6.Text="";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox24.Text = "";
            comboBox10.Items.Clear();
            comboBox10.Text = "";
            textBox3.Text = "";
            comboBox9.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            temizle1();
            if (aracteslimpanel.Width == 0)
            {
                aracteslimpanel.Size = new Size(this.Width - panel1.Width, panel1.Height);
            }
            aracteslimpanel.BringToFront();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (araclarpanel.Width == 0)
            {
                araclarpanel.Size = new Size(this.Width - panel1.Width, panel1.Height);
            }
            araclarpanel.BringToFront();
            temizle1();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text==""||textBox2.Text==""||textBox4.Text==""||comboBox1.Text==""||comboBox6.Text==""||textBox7.Text=="")
            {
                MessageBox.Show("Alanlardan herhangi birisi boş olamaz");
            }
            else
            {
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "Select id from araclar where plaka='" + comboBox6.Text + "'";
                OleDbDataReader oku = komut.ExecuteReader();
                string aracid = "";
                while (oku.Read())
                {
                    aracid = oku["id"].ToString();
                }
                baglanti.Close();

                dogumtarihi();
                kirabaslangic();
                kirabitisa();

                baglanti.Open();
                try
                {
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into kiralik values('" + textBox1.Text + "','" + textBox2.Text + "','" + dgmtrh + "','" + comboBox1.Text + "','" + textBox4.Text + "','" + kirabas + "','" + kirabitis + "','" + aracid + "'," + textBox7.Text + ")";
                    komut.ExecuteNonQuery();
                    komut.Connection = baglanti;
                    komut.CommandText = "update araclar set durum='dolu' where id=" + aracid + "";
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kiralama Başarılı!");
                }
                catch (Exception)
                {

                    MessageBox.Show("İşlem başarısız!");
                    throw;
                }
                baglanti.Close();
                temizle1();

                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "select marka from araclar where durum='bos' GROUP BY marka";
                oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    comboBox2.Items.Add(oku["marka"].ToString());
                }
                baglanti.Close();
            }
            
        }

        void dogumtarihi()
        {
            string g = (dateTimePicker4.Value.Day.ToString().Length == 1) ? ("0" + dateTimePicker4.Value.Day) : (dateTimePicker4.Value.Day.ToString());
            string a = (dateTimePicker4.Value.Month.ToString().Length == 1) ? ("0" + dateTimePicker4.Value.Month) : (dateTimePicker4.Value.Month.ToString());
            dgmtrh = g + "." + a + "." + dateTimePicker4.Value.Year;
        }

        void kirabaslangic()
        {
            string g = (dateTimePicker1.Value.Day.ToString().Length == 1) ? ("0" + dateTimePicker1.Value.Day) : (dateTimePicker1.Value.Day.ToString());
            string a = (dateTimePicker1.Value.Month.ToString().Length == 1) ? ("0" + dateTimePicker1.Value.Month) : (dateTimePicker1.Value.Month.ToString());
            kirabas = g + "." + a + "." + dateTimePicker1.Value.Year;
        }

        void kirabitisa()
        {
            string g = (dateTimePicker2.Value.Day.ToString().Length == 1) ? ("0" + dateTimePicker2.Value.Day) : (dateTimePicker2.Value.Day.ToString());
            string a = (dateTimePicker2.Value.Month.ToString().Length == 1) ? ("0" + dateTimePicker2.Value.Month) : (dateTimePicker2.Value.Month.ToString());
            kirabitis = g + "." + a + "." + dateTimePicker2.Value.Year;
        }

        void arcteslimtrh()
        {
            string g = (dateTimePicker3.Value.Day.ToString().Length == 1) ? ("0" + dateTimePicker3.Value.Day) : (dateTimePicker3.Value.Day.ToString());
            string a = (dateTimePicker3.Value.Month.ToString().Length == 1) ? ("0" + dateTimePicker3.Value.Month) : (dateTimePicker3.Value.Month.ToString());
            arcteslimtarh = g + "." + a + "." + dateTimePicker3.Value.Year;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select fiyat from araclar where plaka='" + comboBox6.Text + "'";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                textBox7.Text = oku["fiyat"].ToString();
            }
            baglanti.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox7.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select model from araclar where marka='" + comboBox2.Text + "' and durum='bos' GROUP BY model";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox3.Items.Add(oku["model"].ToString());
            }
            baglanti.Close();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox7.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select modelyili from araclar where marka='" + comboBox2.Text + "' and model='" + comboBox3.Text + "' and durum='bos' GROUP BY modelyili";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox7.Items.Add(oku["modelyili"].ToString());
            }
            baglanti.Close();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select renk from araclar where marka='" + comboBox2.Text + "' and model='" + comboBox3.Text + "' and modelyili='" + comboBox7.Text + "' and durum='bos' GROUP BY renk";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox4.Items.Add(oku["renk"].ToString());
            }
            baglanti.Close();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select motor from araclar where marka='" + comboBox2.Text + "' and model='" + comboBox3.Text + "' and modelyili='" + comboBox7.Text + "' and renk='" + comboBox4.Text + "' and durum='bos' GROUP BY motor";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox5.Items.Add(oku["motor"].ToString());
            }
            baglanti.Close();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select plaka from araclar where marka='" + comboBox2.Text + "' and model='" + comboBox3.Text + "' and modelyili='" + comboBox7.Text + "' and renk='" + comboBox4.Text + "' and motor='" + comboBox5.Text + "' and durum='bos' GROUP BY plaka";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox6.Items.Add(oku["plaka"].ToString());
            }
            baglanti.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("ID numarası giriniz");
            }
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists("images/i-pic/" + textBox2.Text + ".jpg"))
                    {
                        File.Move("images/i-pic/" + textBox2.Text + ".jpg", "images/dlt/" + textBox2.Text + ".jpg");
                    }
                    File.Copy(openFileDialog1.FileName, "images/i-pic/" + textBox2.Text + ".jpg");
                    if (File.Exists("images/i-pic/" + textBox2.Text + ".jpg"))
                    {
                        label21.ForeColor = Color.Green;
                        label21.Text = "Resim eklendi.";
                    }
                }
                else
                {
                    label21.ForeColor = Color.Red;
                    label21.Text = "Resim eklenmedi.";
                }
            }

        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("ID numarası giriniz");
            }
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists("images/i-ic/" + textBox2.Text + ".jpg"))
                    {
                        File.Move("images/i-ic/" + textBox2.Text + ".jpg", "images/dlt/" + textBox2.Text + ".jpg");
                    }
                    File.Copy(openFileDialog1.FileName, "images/i-ic/" + textBox2.Text + ".jpg");
                    if (File.Exists("images/i-ic/" + textBox2.Text + ".jpg"))
                    {
                        label20.ForeColor = Color.Green;
                        label20.Text = "Resim eklendi.";
                    }
                }
                else
                {
                    label20.ForeColor = Color.Red;
                    label20.Text = "Resim eklenmedi.";
                }
            }
        }

        Form2 frm2;
        private void button8_Click(object sender, EventArgs e)
        {
            if (File.Exists("images/i-pic/" + textBox2.Text + ".jpg"))
            {
                if (frm2 == null || frm2.IsDisposed)
                {
                    frm2 = new Form2();
                    frm2.Show();
                }
                else
                {
                    frm2.BringToFront();
                }
                frm2.rsmgoster("images/i-pic/" + textBox2.Text + ".jpg");
            }
            else
            {
                MessageBox.Show("Resim Bulunamadı");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (File.Exists("images/i-ic/" + textBox2.Text + ".jpg"))
            {
                if (frm2 == null || frm2.IsDisposed)
                {
                    frm2 = new Form2();
                    frm2.Show();
                }
                else
                {
                    frm2.BringToFront();
                }

                frm2.rsmgoster("images/i-ic/" + textBox2.Text + ".jpg");

            }
            else
            {
                MessageBox.Show("Kimlik fotokopisi bulunamadı");

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            comboBox8.Items.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            label33.Text = "";
            label34.Text = "";
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText="select adsoyad from kiralik where id='"+textBox5.Text+"'";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                comboBox8.Items.Add(oku["adsoyad"].ToString());
            }
            baglanti.Close();

            
        }

        string kirabasd;
        string arcfyt;
        string aracid = "";
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox4.Items.Clear();
            OleDbDataReader oku ;

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "Select * from kiralik where id='" + textBox5.Text + "' and adsoyad='" + comboBox8.Text + "'";
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                listBox4.Items.Add(oku["adsoyad"].ToString());
                listBox1.Items.Add("Ad Soyad : " + listBox4.Items[0].ToString());

                listBox4.Items.Add(oku["id"].ToString());
                listBox1.Items.Add("ID : " + listBox4.Items[1].ToString());

                listBox4.Items.Add(oku["dgmtrh"].ToString());
                listBox1.Items.Add("Doğum Tarihi : " + listBox4.Items[2].ToString());

                listBox4.Items.Add(oku["cinsiyet"].ToString());
                listBox1.Items.Add("Cinsiyet : " + listBox4.Items[3].ToString());

                listBox4.Items.Add(oku["uyruk"].ToString());
                listBox1.Items.Add("Uyruk : " + listBox4.Items[4].ToString());

                listBox4.Items.Add(oku["kirabas"].ToString());
                kirabasd = listBox4.Items[5].ToString();
                listBox1.Items.Add("Kira başlangıcı : " + listBox4.Items[5].ToString());

                listBox4.Items.Add(oku["kirabitis"].ToString());
                listBox1.Items.Add("Kira bitişi : " + listBox4.Items[6].ToString());


                listBox4.Items.Add(oku["aracid"].ToString());
                aracid = listBox4.Items[7].ToString();
                listBox1.Items.Add("Araç ID : " + aracid);

                listBox4.Items.Add(oku["fiyat"].ToString());
                arcfyt = listBox4.Items[8].ToString();
                listBox1.Items.Add("Fiyat(TL/GUN) : " +arcfyt);
            }
            baglanti.Close();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "Select * from araclar where id="+aracid+"";
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                listBox2.Items.Add("Marka : "+oku["marka"].ToString());
                listBox2.Items.Add("Model : "+oku["model"].ToString());
                listBox2.Items.Add("Model yılı : "+oku["modelyili"].ToString());
                listBox2.Items.Add("Plaka : "+oku["plaka"].ToString());
                listBox2.Items.Add("Şase No : "+oku["saseno"].ToString());
                listBox2.Items.Add("Motor : "+oku["motor"].ToString());
                listBox2.Items.Add("Km : "+oku["km"].ToString());
                listBox2.Items.Add("Renk : "+oku["renk"].ToString());
                listBox2.Items.Add("Fiyat(TL/GUN) : "+oku["fiyat"].ToString());
            }
            baglanti.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            
            if (File.Exists("images/i-pic/" + textBox5.Text + ".jpg"))
            {
                if (frm2 == null || frm2.IsDisposed)
                {
                    frm2 = new Form2();
                    frm2.Show();
                }
                else
                {
                    frm2.BringToFront();
                }
                frm2.rsmgoster("images/i-pic/" + textBox5.Text + ".jpg");

            }
            else
            {
                MessageBox.Show("Fotoğraf Bulunamadı");

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            if (File.Exists("images/i-ic/" + textBox5.Text + ".jpg"))
            {
                if (frm2 == null || frm2.IsDisposed)
                {
                    frm2 = new Form2();
                    frm2.Show();
                }
                else
                {
                    frm2.BringToFront();
                }
                frm2.rsmgoster("images/i-ic/" + textBox5.Text + ".jpg");

            }
            else
            {
                MessageBox.Show("Fotoğraf Bulunamadı");

            }
        }

        
        private void button12_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count>2)
            {
                arcteslimtrh();
                int kbasy = Convert.ToInt32(kirabasd[6].ToString() + kirabasd[7].ToString() + kirabasd[8].ToString() + kirabasd[9].ToString());
                int kbasa = Convert.ToInt32(kirabasd[3].ToString() + kirabasd[4].ToString());
                int kbasg = Convert.ToInt32(kirabasd[0].ToString() + kirabasd[1].ToString());
                int kbity = Convert.ToInt32(dateTimePicker3.Value.Year.ToString());
                int kbita = Convert.ToInt32(dateTimePicker3.Value.Month.ToString());
                int kbitg = Convert.ToInt32(dateTimePicker3.Value.Day.ToString());
                DateTime kirabaslangic = new DateTime(kbasy, kbasa, kbasg);
                DateTime aracteslim = new DateTime(kbity, kbita, kbitg);
                TimeSpan gun = aracteslim - kirabaslangic;
                int para = Convert.ToInt32(arcfyt) * Convert.ToInt32(gun.Days);

                string kbitg2 = (kbitg.ToString().Length == 1) ? ("0" + kbitg) : (kbitg.ToString());
                string kbita2= (kbita.ToString().Length == 1) ? ("0" + kbita) : (kbita.ToString());
                string kbity2= (kbity.ToString().Length == 1) ? ("0" + kbity) : (kbity.ToString());
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "insert into kayitlar values('" + listBox4.Items[0] + "','" + listBox4.Items[1] + "','" + listBox4.Items[2] + "','" + listBox4.Items[3] + "','" + listBox4.Items[4] + "','" + listBox4.Items[5] + "','" + kbitg2+"."+kbita2+"."+kbity2 + "','" + listBox4.Items[7] + "','" + gun.Days.ToString() + "'," + para + ",'" + textBox6.Text + "')";
                komut.ExecuteNonQuery();
                komut.CommandText = "delete from kiralik where aracid='" + aracid + "' and id='" + textBox5.Text + "'";
                komut.ExecuteNonQuery();
                komut.CommandText = "update araclar set durum='bos' where id=" + aracid + "";
                komut.ExecuteNonQuery();
                baglanti.Close();
                temizle1();
                label33.Text = "Araç " + gun.Days.ToString() + " gün kirada kaldı.";
                label34.Text = para + "TL kazanıldı";
                MessageBox.Show("Araç teslim edildi");
            }
            else
            {
                MessageBox.Show("Gerekli alanları doldurunuz.");
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox8.Text==""||textBox9.Text==""||textBox10.Text==""||textBox15.Text==""||textBox11.Text==""||textBox12.Text==""||textBox13.Text==""||textBox14.Text=="")
            {
                MessageBox.Show("Şase no haricinde boş alan kalamaz.");
            }
            else
            {
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "insert into araclar(marka,model,modelyili,plaka,saseno,motor,km,renk,fiyat,durum) values('"+textBox8.Text+"','"+textBox9.Text+"','"+textBox10.Text+"','"+textBox15.Text+"','"+textBox24.Text+"','"+textBox11.Text+"','"+textBox12.Text+"','"+textBox13.Text+"','"+textBox14.Text+"','bos')";
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yeni araç başarıyla eklendi","Araç Ekleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
                aracidcekme();
                temizle1();
            }
        }

        private void comboBox10_Click(object sender, EventArgs e)
        {
            comboBox10.Items.Clear();
            comboBox10.Text = "";
            textBox3.Text = "";
            aracidcekme();
        }

        void aracidcekme()
        {
            string id;
            comboBox10.Items.Clear();
            comboBox11.Items.Clear();
            comboBox10.Text = "";
            comboBox11.Text = "";
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select id from araclar";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                id = oku["id"].ToString();
                comboBox10.Items.Add(id);
                comboBox11.Items.Add(id);
            }
            baglanti.Close();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            try
            {
                komut.Connection = baglanti;
                komut.CommandText = "update araclar set " + ozellik + "='" + textBox3.Text + "' where id=" + comboBox10.Text;
                komut.ExecuteNonQuery();
                MessageBox.Show("Güncelleme Başarılı","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Güncelleme Başarısız","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            baglanti.Close();
            comboBox10.Text = "";
            comboBox10.Items.Clear();
            comboBox9.Text = "";
            aracidcekme();
            textBox3.Text = "";
        }

        string ozellik;
        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox10.Text == "")
            {
                MessageBox.Show("Araç seçimi yapınız!");
            }
            else
            {
                ozellik = (comboBox9.SelectedItem.ToString() == "Marka") ? (ozellik = "marka") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Model") ? (ozellik = "model") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Model Yılı") ? (ozellik = "modelyili") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Plaka") ? (ozellik = "plaka") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Şase No") ? (ozellik = "saseno") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Motor") ? (ozellik = "motor") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Km") ? (ozellik = "km") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Renk") ? (ozellik = "renk") : (ozellik);
                ozellik = (comboBox9.SelectedItem.ToString() == "Fiyat") ? (ozellik = "fiyat") : (ozellik);

                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "select " + ozellik + " from araclar where id=" + comboBox10.Text;
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    textBox3.Text = oku[ozellik].ToString();
                }
                baglanti.Close();
            }
        }

        private void comboBox11_Click(object sender, EventArgs e)
        {
            aracidcekme();
        }

        string arcdurum;
        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "Select * from araclar where id="+comboBox11.Text;
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                listBox3.Items.Add("Marka : "+oku["marka"]);
                listBox3.Items.Add("Model : " + oku["model"]);
                listBox3.Items.Add("Model Yılı : " + oku["modelyili"]);
                listBox3.Items.Add("Plaka : " + oku["plaka"]);
                listBox3.Items.Add("Şase no : " + oku["saseno"]);
                listBox3.Items.Add("Motor : " + oku["motor"]);
                listBox3.Items.Add("Km : " + oku["km"]);
                listBox3.Items.Add("Renk : " + oku["renk"]);
                listBox3.Items.Add("Fiyat(TL/gun) : " + oku["fiyat"]);
                arcdurum = oku["durum"].ToString();
                listBox3.Items.Add("Marka : "+arcdurum.Replace('s','ş').Replace('b','B').Replace('d','D'));
            }
            baglanti.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (arcdurum=="dolu")
            {
                MessageBox.Show("Kirada olan araç silinemez öncelikle aracın teslim edilmesi gerekir.");
            }
            else
            {
                if (comboBox11.Text=="")
                {
                    MessageBox.Show("Araç seçimi yapınız.");
                }
                else
                {
                    try
                    {
                        baglanti.Open();
                        komut.Connection = baglanti;
                        komut.CommandText = "delete from araclar where id=" + comboBox11.Text;
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Araç silindi");
                        aracidcekme();
                        listBox3.Items.Clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Silme işlemi başarısız daha sonra tekrar deneyiniz.","Araç Silme",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        throw;
                    }
                    
                }
            }
        }

        Form3 frm3;
        private void button17_Click(object sender, EventArgs e)
        {
            if (frm3 == null || frm3.IsDisposed)
            {
                frm3 = new Form3();
                frm3.Show();
            }
            else
            {
                frm3.BringToFront();
            }
            frm3.araccek();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            kayitcek("kiralik");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            kayitcek("kayitlar");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            kayitcek("araclar");
        }

        DataTable tablo = new DataTable();
        
        void kayitcek(string tabload)
        {
            tablo.Clear();
            tablo.Columns.Clear();
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * from "+tabload,baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}