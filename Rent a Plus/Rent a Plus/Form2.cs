using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rent_a_Plus
{
    public partial class Form2 : Form
    {

        /* Bu projeyi yazılıma başladığım ilk zamanlarda geliştirdim.Bu sebeple gereksiz kullanımlarım/hatalarım olabilir*/

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = this.Size;
            pictureBox1.Location = new Point(0,0);
        }

        public  void rsmgoster(string yol)
        {
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.BackgroundImage = Image.FromFile(yol);            
        }
    }
}
