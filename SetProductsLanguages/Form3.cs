using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetProductsLanguages
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            string mail = textBox3.Text;
            string siteUrl = textBox1.Text;
            string url = siteUrl + "rest1/product/recoverPassword";
            //   string url = "http://mikail.eticaret.in/rest1/auth/recoverPassword";

            string retVal = "";

            Dictionary<string, object> PostData = new Dictionary<string, object>();
            PostData.Add("token",GirisForm.myToken);
            PostData.Add("email", "mikail-torun@hotmail.com");

            StringBuilder postData = new StringBuilder();

            foreach (string item in PostData.Keys)
            {
                postData.Append(item + "=" + HttpUtility.UrlEncode(PostData[item].ToString()) + "&");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            Encoding encoding = Encoding.UTF8;
            byte[] bt = encoding.GetBytes(postData.ToString().Substring(0, postData.ToString().Length - 1));
            request.GetRequestStream().Write(bt, 0, bt.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            retVal = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string result = retVal;
            MessageBox.Show(result);
        }
    }
}
