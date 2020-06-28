using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSuite;
using System.Reflection;
using System.Net;

namespace AlSimits
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        public string hash = "323+888+1584+48af983pkef6842+3vr59f+edcchy879320rf3e5w6ds2";
        private void Form1_Load(object sender, EventArgs e)
        {
            notify_Icon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            guncelle();
            bildirim("AlSimits Başlatıldı",listBox1.Items.Count + " adet not şifrelidir.");
        }
        void bildirim(string baslik,string text)
        {
            this.Hide();
            notify_Icon.Visible = true;
            notify_Icon.Text = "AlSimits";
            notify_Icon.BalloonTipTitle = baslik;
            notify_Icon.BalloonTipText = text;
            notify_Icon.BalloonTipIcon = ToolTipIcon.Info;
            notify_Icon.ShowBalloonTip(500);
        }
        void notify_Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
        string sifrelendi,cozuldu;
        public string Sifrele(string sifre)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(sifre);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    sifrelendi = Convert.ToBase64String(results, 0, results.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }
        public string Coz(string SifrelenmisDeger)
        {
            byte[] data = Convert.FromBase64String(SifrelenmisDeger);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    cozuldu = UTF8Encoding.UTF8.GetString(results);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }


        private void yaz_TextChanged(object sender, EventArgs e)
        {
            if (yaz.Text == "" || yaz.Text == null)
            {

            }
            else
            {
                filename.Text = yaz.Lines[0];
            }

        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader(Application.StartupPath + "\\files\\" + listBox1.SelectedItem.ToString() + ".txt");
            Coz(str.ReadToEnd());
            yaz.Text = cozuldu;
            str.Close();
        }

        
        private void kayıtEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kayitet();
        }
        private void kayıtEtToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            kayitet();
        }
       
        private void notlarıGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guncelle();
        }

       
        private void yaz_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader(Application.StartupPath + "\\files\\" + listBox1.SelectedItem.ToString() + ".txt");
            Coz(str.ReadToEnd());
            yaz.Text = cozuldu;
            str.Close();
            tabControl1.SelectedIndex = 1;

        }
        void guncelle()
        {
            listBox1.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\files\\");
            FileInfo[] rgFiles = di.GetFiles();
            foreach (FileInfo fi in rgFiles)
            {
                string yeniad = fi.Name.Replace(".txt", "");
                listBox1.Items.Add(yeniad);

            }
            
        }

        private void btn_Guncelle_Click(object sender, EventArgs e)
        {
            guncelle();
        }

        private void btnCreateNewNote_Click(object sender, EventArgs e)
        {
            yeninoteyaz();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        void yeninoteyaz()
        {
            yaz.Text = "";
            tabControl1.SelectedIndex = 1;
        }
        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            kayitet();
           
        }

        private void notYazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yeninoteyaz();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            bildirim("Program hala çalışıyor","Bildirim merkezinden takip edebilirsiniz.");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btn_ChangeFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK & !String.IsNullOrEmpty(yaz.Text))
            {
                yaz.SelectAll();
                yaz.SelectionFont = fontDialog1.Font;
                yaz.DeselectAll();
            }
            else
            {
                //  richtextbox.SelectionFont = ?
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void kayitet()
        {
            if (yaz.Text == "" || yaz.Text == null)
            {

                MessageBox.Show("Uyarı", "Kayıt edilecek bir not bulunmamaktadır.",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            else
            {
                 filename.Text = yaz.Lines[0];
                 yaz.Enabled = false;
                 StreamWriter Yaz = new StreamWriter(Application.StartupPath + "\\files\\" + filename.Text + ".txt");
                 Sifrele(yaz.Text);
                 Yaz.WriteLine(sifrelendi);
                 Yaz.Close();
                 yaz.Enabled = true;
                 bildirim("Not Kayıt Edildi", yaz.Lines[0] + " şifrelendi ve kayıt edildi.");
                 yaz.Clear();
                 guncelle();
                 tabControl1.SelectedIndex = 0;

            }
            

        }


    }
}