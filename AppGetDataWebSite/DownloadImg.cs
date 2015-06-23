using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppGetDataWebSite
{
    public partial class DownloadImg : Form
    {
        private string url = @"http://vietby.net/fo3/trang-";
        private string strSelectNodes = "//html/body/section/section[1]/section[2]/section[1]/table/tbody";
        public DownloadImg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //i max 433
            Stopwatch st = new Stopwatch();
            st.Start();
            for (int i = 1; i < 20; i++)
            {
               
                GetPlayer(i.ToString());
            }
            st.Stop();
            lblTime.Text = st.Elapsed.ToString();
            
        }
        #region td1
        private string GetUrlImg(HtmlAgilityPack.HtmlDocument document, int i)
        {
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[1]/figure/img";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    result = GetNameImg(node.Attributes["src"].Value);
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetUrlImg" + oldText + ex.Message + ". ";
            }
            return result;
        }
        private string GetNameImg(string href)
        {
            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();
                string[] str = href.Split('/');
                client.DownloadFile(new Uri(href), @"G:\images\" + str[str.Length - 1]);
                //client.DownloadFileAsync(new Uri(href), @"G:\images\" + str[str.Length - 1]);               
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". ";
            }
            return result;
        }
        #endregion
        public bool Check(HtmlAgilityPack.HtmlDocument document, int i)
        {
            bool check = true;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td/figure";
                var tblTags = document.DocumentNode.SelectNodes(str);
                if (tblTags == null)
                    check = false;

            }
            catch (Exception ex)
            {
                check = false;
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". ";
            }
            return check;
        }
        private void Run(HtmlAgilityPack.HtmlDocument document, int i)
        {
            if (Check(document, i)) {
                GetUrlImg(document,i);
            }
        }
        private void GetPlayer(string page)
        {
            try
            {
                string newUrl = url + page;
                HtmlWeb getHtmlWeb = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument document = getHtmlWeb.Load(newUrl);
                var tblTags = document.DocumentNode.SelectNodes(strSelectNodes);
                int z = 0;
                for (int i = 0; i < tblTags.Count; i++)
                {
                    var childNodes_lv1 = tblTags[i].ChildNodes;
                    foreach (var childNode_lv1 in childNodes_lv1)
                    {                    
                        Run(document, z + 1);
                        z++;
                    }
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". ";
            }
        }
    }
}
