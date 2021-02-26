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
using excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;


namespace SetProductsLanguages
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
  
        static StringBuilder sb = new StringBuilder();
        static String[] spaces = { "ProductCode", "Language", "ProductName", "Details", "Document", "ShortDescription", "Additional1", "Additional2", "Additional3",
           "SeoLink","SeoTitle","SeoKeywords", "SeoDescription","WarrantyInfo","DeliveryInfo" };

        private void Button1_Click(object sender, EventArgs e)
        {

            string url = GirisForm.URLsite+"rest1/product/getProducts";

            string retVal = "";

            Dictionary<string, object> PostData = new Dictionary<string, object>();
            PostData.Add("token", GirisForm.myToken );
            
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
            
            int a ;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wrkbk = app.Workbooks.Add(System.Reflection.Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet sayfa1 = (Microsoft.Office.Interop.Excel.Worksheet)wrkbk.Sheets[1];


            Microsoft.Office.Interop.Excel.Range alan1 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1,1];
            alan1.Value2 = "ProductCode";
            Microsoft.Office.Interop.Excel.Range alan2 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 2];
            alan2.Value2 = "Language";
            Microsoft.Office.Interop.Excel.Range alan3 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 3];
            alan3.Value2 = "ProductName";
            Microsoft.Office.Interop.Excel.Range alan4 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 4];
            alan4.Value2 = "Details";
            Microsoft.Office.Interop.Excel.Range alan5 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 5];
            alan5.Value2 = "Document";
            Microsoft.Office.Interop.Excel.Range alan6 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 6];
            alan6.Value2 = "ShortDescription";
            Microsoft.Office.Interop.Excel.Range alan7 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 7];
            alan7.Value2 = "Additional1";
            Microsoft.Office.Interop.Excel.Range alan8 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 8];
            alan8.Value2 = "Additional2";
            Microsoft.Office.Interop.Excel.Range alan9 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 9];
            alan9.Value2 = "Additional3";
            Microsoft.Office.Interop.Excel.Range alan10 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 10];
            alan10.Value2 = "SeoLink";
            Microsoft.Office.Interop.Excel.Range alan11 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 11];
            alan11.Value2 = "SeoTitle";
            Microsoft.Office.Interop.Excel.Range alan12 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 12];
            alan12.Value2 = "SeoKeywords";
            Microsoft.Office.Interop.Excel.Range alan13 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 13];
            alan13.Value2 = "SeoDescription";
            Microsoft.Office.Interop.Excel.Range alan14 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 14];
            alan14.Value2 = "WarrantyInfo";
            Microsoft.Office.Interop.Excel.Range alan15 = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[1, 15];
            alan15.Value2 = "DeliveryInfo";


            for (int t = 0; t < spaces.Length; t++)
            {

                List<string> prdct = JSONParseObject(result, spaces[t]);

                progressBar2.Maximum = prdct.Count();
               
                a = 2;
                for (int i = 0; i < prdct.Count; i++)
                {
                        Microsoft.Office.Interop.Excel.Range alan = (Microsoft.Office.Interop.Excel.Range)sayfa1.Cells[a, t + 1];
                        try
                        {
                            if (prdct[i] != null)
                                alan.Value2 = prdct[i].ToString();
                            if (prdct[i] == null || prdct[i].ToString().Equals("") && t + 1 == 2)
                                alan.Value2 = "en";
                            else if (prdct[i] == null || prdct[i].ToString().Equals(""))
                                alan.Value2 = "";
                         }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    progressBar2.Value = i+1;
                        a++;                    
                }

            }

            MessageBox.Show("İşlem Başarıyla Tamamlandı!");
           
        }
       
        public List<string> JSONParseObject(string jsonText, String blockes)
        {
            JObject jResults = JObject.Parse(jsonText);

            List<string> prdct = new List<string>();
            foreach (var county in jResults["data"])
            {
                prdct.Add((string)county[blockes]);
               
            }
            return prdct;
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Visible = false;
            button5.Visible = false;
            button2.Enabled = false;
            button5.Enabled = false;
            button3.Enabled = false;
            progressBar1.Enabled = false;
            comboBox1.Text = "İçe Aktarılacak Alanı Seçiniz";
            comboBox1.Enabled = false;
            pictureBox1.Enabled = false;
           
            comboBox1.Visible = false;
            textBox1.Visible = false;
            comboBox1.Enabled = false;
            textBox1.Enabled = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
           

        }

        String kullaniciAdi = GirisForm.kullaniciAdi;
        String sifreGiris = GirisForm.sifre;
        String girisUrl = GirisForm.Url;
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                string url = GirisForm.URLsite + "rest1/product/setProductLanguage";
                string retVal = "";
                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);//   @"C:\Users\mikailtorun\Desktop\K.xlsx"
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                progressBar1.Maximum = rowCount;

                List<Dictionary<String, object>> retvall = new List<Dictionary<String, object>>();
                for (int i = 2; i <= rowCount; i++)
                {
                    var temDict = new Dictionary<String, object>();
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            Boolean f = Convert.ToBoolean(xlRange.Cells[i, j].Value2.ToString().Contains("%"));                        
                            if (f == true)
                            {
                                String r = xlRange.Cells[i, j].Value2.ToString().Replace("%", " % ");                          
                                temDict.Add(xlRange.Cells[1, j].Value2.ToString(), r);
                            }                         
                            else
                                temDict.Add(xlRange.Cells[1, j].Value2.ToString(), xlRange.Cells[i, j].Value2.ToString());
                        }                       
                    }                 
                    retvall.Add(temDict);
                    progressBar1.Value = i;                   
                }   

                String json = JsonConvert.SerializeObject(retvall, Formatting.Indented);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string pd = "token=" + GirisForm.myToken + "&data=" + json;//listenin json hali
                Encoding encoding = Encoding.UTF8;
                byte[] bt = encoding.GetBytes(pd);
                request.GetRequestStream().Write(bt, 0, bt.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                retVal = new StreamReader(response.GetResponseStream()).ReadToEnd();
                string result = retVal;
                //             MessageBox.Show(result);
                if (result.Contains("success\":true,\"")) 
                    MessageBox.Show("İşleminiz Başarıyla Gerçekleşti");
                else
                    MessageBox.Show("Bir Hata Oluştu\n Lütfen Seçtiğiniz Excel Dosyasını Kontrol Ediniz");
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad
                //release com objects to fully kill excel process from running in the background

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lütfen excel dosyasını seçtiğinizden emin olunuz!");
                MessageBox.Show(ex.Message);
            }
        }



        private string DeserializeObject(List<Dictionary<string, object>> retvall)
        {
            throw new NotImplementedException();
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
     

        private void Button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
       

        private void Button5_Click(object sender, EventArgs e)
        {
          

        }
        public static String path;
        private void Button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 path = openFileDialog1.FileName.ToString();
               
            }
            button3.Enabled = true;
            progressBar1.Enabled = true;
            comboBox1.Enabled = true;
            textBox2.Text = path;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
