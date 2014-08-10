using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace CccamSniffer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        public Stream charger()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://goxxip.byethost11.com/index.php");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)req.GetResponse();
            Stream streamResponse = myHttpWebResponse.GetResponseStream();
            return streamResponse;
        }

        public void Filter()
        {
            List<string> newcamd = new List<string>();
            List<string> cccam = new List<string>();
            StreamReader re = new StreamReader(charger());
            string ligne = re.ReadLine();
            while (ligne != null)
            {
                if (ligne.Contains("C:") || ligne.Contains("N:"))
                {
                    if (ligne.Contains("====") == false)
                    {
                        int len = ligne.Length;
                        string chaine = ligne.Substring(4,len - 4);
                        listBox1.Items.Add(chaine);
                    }
                }
                ligne = re.ReadLine();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DriveInfo[] tab = DriveInfo.GetDrives();
            foreach (DriveInfo d in tab)
            {
                if (d.IsReady && d.VolumeLabel=="SHARING")
                {
                    using (StreamWriter rw= new StreamWriter(d.RootDirectory + "CCcam.cfg",true))
                    {
                        rw.WriteLine(listBox1.SelectedItem.ToString());
                        MessageBox.Show("Done");
                    }
                }
                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
