using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VocalUtau.DirectUI.Forms;
using VocalUtau.Formats.Model.Database.VocalDatabase;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
            //Application.Run(new Form2());
        }
    }
}
