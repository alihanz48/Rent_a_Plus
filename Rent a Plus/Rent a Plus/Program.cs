using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rent_a_Plus
{
    static class Program
    {
        /* Bu projeyi yazılıma başladığım ilk zamanlarda geliştirdim.Bu sebeple gereksiz kullanımlarım/hatalarım olabilir*/

        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
