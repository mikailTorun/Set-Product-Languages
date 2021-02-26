using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace SetProductsLanguages
{
    public partial class GirisForm : Form
    {
        public GirisForm()
        {
            InitializeComponent();
        }

        public static string URLsite ;
        public static string kullaniciAdi;
        public static string sifre ;
        public static string myToken;
        public static string Url;

        public void Button1_Click(object sender, EventArgs e)// LOGIN BUTTON
        {
            try
            {
                URLsite = textBox3.Text; 
                kullaniciAdi = textBox1.Text;
                sifre = textBox2.Text;

                Url = URLsite + "rest1/auth/login/" + kullaniciAdi;
                string retVal = "";

                Dictionary<string, object> PostData = new Dictionary<string, object>();
                PostData.Add("pass", sifre);

                StringBuilder postData = new StringBuilder();

                foreach (string item in PostData.Keys)
                {
                    postData.Append(item + "=" + HttpUtility.UrlEncode(PostData[item].ToString()) + "&");
                }
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    Encoding encoding = Encoding.UTF8;
                    byte[] bt = encoding.GetBytes(postData.ToString().Substring(0, postData.ToString().Length - 1));
                    request.GetRequestStream().Write(bt, 0, bt.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    retVal = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("URL adresinizi Doğru Formatta yazınız" + ex.Message);
                }

                string result = retVal;
                if(result.Contains("false"))
                MessageBox.Show(result);
                //Can use either JSONParseObject or JSONParseDynamic here
                List<string> tkn = JSONParseObject(result);
                // JSONParseDynamic(result);

                for (int i = 0; i < tkn.Count; i++)
                {
                    textBox4.Text = tkn[i];
                }

                myToken = textBox4.Text;

               

                if (result.Contains("true"))
                {
                    Form1 f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Lütfen Kullanıcı Adı, Şifre ve URL bilgilerini Kontrol Ediniz \n örnek Url: http://www.örnek.eticaret.in/ ");
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lütfen tüm alanları doğru bir şekilde doldurunuz\n"+ex.Message);
            }           
        }
      
       
        public List<string> JSONParseObject(string jsonText)
        {
            JObject jResults = JObject.Parse(jsonText);
            List<string> sifre = new List<string>();
            foreach (var county in jResults["data"])
            {
                sifre.Add((string)county["token"]);
            }
            return sifre;
        }
       
        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GirisForm_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            textBox4.Visible = false;
            button3.Visible = false;
            textBox1.Text = "mikailtorunhotmail.com";
            textBox2.Text = "2oFp88";
            textBox3.Text = "http://mikail.eticaret.in/";
            label5.Visible = false;
            label5.Enabled = false;
        }

        private void Label5_Click(object sender, EventArgs e)
        {
        
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}
