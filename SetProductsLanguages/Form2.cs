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

namespace EntegreDeneme
{
    public partial class Form2 : Form 
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
         public  string URLsite="";
         public  string kullaniciAdi = "";
         public  string sifre = "";
         public static string myToken ="";
        private void Button1_Click(object sender, EventArgs e)
        {
             URLsite = textBox3.Text;
             kullaniciAdi = textBox1.Text;
             sifre = textBox2.Text;

            string url = URLsite + "rest1/auth/login/"+ kullaniciAdi;

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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Encoding encoding = Encoding.UTF8;
                byte[] bt = encoding.GetBytes(postData.ToString().Substring(0, postData.ToString().Length - 1));
                request.GetRequestStream().Write(bt, 0, bt.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                retVal = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
            {
                MessageBox.Show("URL adresinizi Doğru Formatta yazınız");
            }
            
            string result = retVal;
            //Can use either JSONParseObject or JSONParseDynamic here
            List<string> tkn = JSONParseObject(result);
            JSONParseDynamic(result);
           
            for(int i = 0; i < tkn.Count; i++)
            {
                textBox4.Text = tkn[i];
            }
           
             myToken = textBox4.Text;
            //MessageBox.Show(myToken);

            
           
            
            if (result.Contains("true"))
            {
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Lütfen Kullanıcı Adı, Şifre ve URL bilgilerini Kontrol Ediniz \n örnek Url: http://www.mikail.eticaret.in/ ");

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
        public List<string> JSONParseDynamic(string jsonText)
        {
            dynamic jResults = JsonConvert.DeserializeObject(jsonText);
            List<string> token = new List<string>();
            foreach (var tokenn in jResults.data)
            {
                token.Add((string)tokenn.token);
            }
            return token;
        }
        public string ParseEt(string veri)
        {
            string cikti = "";

            JObject json = JObject.Parse(veri);
            cikti = json["data"].ToString();

            return cikti;
        }
        private String deserialiseJSON2(String token)
        {
            try
            {
                var jToken = JsonConvert.DeserializeObject<Datum>(token);

                // MessageBox.Show(jToken.token);
                return jToken.userId;

            }
            catch (Exception ex)
            {
                MessageBox.Show("We had a problem " + ex.Message.ToString());
            }
            return "sallagitsin arzular gemiler zaten batık";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox4.Visible = false;

            textBox1.Text = "mikailtorunhotmail.com";
            textBox2.Text = "M8Dpq8";
            textBox3.Text = "http://mikail.eticaret.in/";
        }

        private void Label5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                MessageBox.Show("Please enter your URL firstly");
            }
            else
            {
                recoverPassword ps = new recoverPassword();
                ps.ShowDialog();
            }
        }
    }
    public class Datum2
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public string secretKey { get; set; }
        public string expirationTime { get; set; }
        public string limited { get; set; }
        public string type { get; set; }
        public string tableId { get; set; }
    }

    public class Message
    {
        public int type { get; set; }
        public string code { get; set; }
        public int index { get; set; }
        public string id { get; set; }
        public List<string> text { get; set; }
        public List<object> errorField { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public List<Datum2> data { get; set; }
        public List<Message> message { get; set; }
        public string summary { get; set; }
    }
}
