using AppGetDataWebSite.Log;
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
    public partial class Form1 : Form
    {
        private string url = @"http://vietby.net/fo3/trang-";
        private string strSelectNodes = "//html/body/section/section[1]/section[2]/section[1]/table/tbody";
        private string country = string.Empty;
        private List<Obj_IndexHidden> LstIndexHidden = new List<Obj_IndexHidden>();
        Crud cr = new Crud();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //i max 433->min 237
            Stopwatch st = new Stopwatch();
            st.Start();
            int start = FC_Convert.ParseInt(txtStartPage.Text);
            int end = FC_Convert.ParseInt(txtEndPage.Text);
            for (int i = start; i < end; i++)
            {
                lblPage.Text = i.ToString();
                GetPlayer(i.ToString());
            }
            st.Stop();
            lblTime.Text = st.Elapsed.ToString();
            Logging.Write(Logging.ERROR, new StackTrace(new StackFrame(0)).ToString().Substring(5, new StackTrace(new StackFrame(0)).ToString().Length - 5),
                            lblMessage.Text+" \n" +st.Elapsed.ToString());
            Process.Start("shutdown", "/s /f /t 0");
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
                lblMessage.Text = "GetUrlImg" + oldText + ex.Message + ". \n ";
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
                result = str[str.Length - 1];
                //client.DownloadFileAsync(new Uri(href), @"G:\images\" + str[str.Length - 1]);               
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". \n ";
            }
            return result;
        }
        #endregion

        #region td2
        private string GetSeasson(HtmlAgilityPack.HtmlDocument document, int i)
        {
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[2]/p/span";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    result = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetSeasson" + oldText + ex.Message + ". \n ";
            }
            return result;
        }

        private Obj_PlayerDetail GetUrlDetail(HtmlAgilityPack.HtmlDocument document, int i)
        {
            Obj_PlayerDetail obj = new Obj_PlayerDetail();
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[2]/p/a";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    obj = GetDetailPlayer(node.Attributes["href"].Value);
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetUrlDetail" + oldText + ex.Message + ". \n ";
            }
            return obj;
        }

        private string GetNamePlayer(HtmlAgilityPack.HtmlDocument document, int i)
        {
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[2]/p/a";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    result = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetNamePlayer" + oldText + ex.Message + ". \n ";
            }
            return result;
        }

        private List<Obj_IndexPosition> GetPosition(HtmlAgilityPack.HtmlDocument document, int i)
        {
            List<Obj_IndexPosition> lst = new List<Obj_IndexPosition>();
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[2]/section/ul/li/p";
                var tblTags = document.DocumentNode.SelectNodes(str);
                for (int z = 0; z < tblTags.Count; z++)
                {
                    Obj_IndexPosition obj = new Obj_IndexPosition();
                    obj.Positions = tblTags[z].InnerText;
                    obj.Indexs = tblTags[z + 1].InnerText;
                    lst.Add(obj);
                    z++;
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetPosition" + oldText + ex.Message + ". \n ";
            }
            return lst;
        }

        #region PlayerDetail
        private string GetCountry(HtmlNodeCollection xTags)
        {
            string result = string.Empty;
            foreach (var item in xTags)
            {
                result = item.InnerText;
            }
            return result;
        }

        private List<Obj_IndexHidden> GetIndexHidden(HtmlNodeCollection xTags)
        {
            List<Obj_IndexHidden> lst = new List<Obj_IndexHidden>();
            try
            {
                if (xTags == null)
                    return lst;
                foreach (var item in xTags)
                {
                    Obj_IndexHidden obj = new Obj_IndexHidden();
                    obj.Name = item.InnerText;
                    lst.Add(obj);
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetIndexHidden" + oldText + ex.Message + ". \n ";
            }
            return lst;
        }

        private Obj_PlayerDetail GetDetailPlayer(string href)
        {
            List<string> chiSo = new List<string>();
            try
            {
                HtmlWeb getHtmlWeb = new HtmlWeb();
                var document = getHtmlWeb.Load(@"http://vietby.net/" + href);
                var tblTags = document.DocumentNode.SelectNodes("//section[@class='wrap-state']/ul/li/span");
                HtmlNodeCollection xTags = document.DocumentNode.SelectNodes("//html/body/section/section[1]/section[2]/section[1]/header/section[3]/section[1]");
                HtmlNodeCollection yTags = document.DocumentNode.SelectNodes("//html/body/section/section[1]/section[2]/section[1]/section[3]/ul/li/a");
                country = GetCountry(xTags);
                LstIndexHidden = GetIndexHidden(yTags);
                int i = 1;
                foreach (var childNode_lv3 in tblTags)
                {
                    if (i % 2 == 0)
                    {
                        chiSo.Add(childNode_lv3.InnerHtml);
                    }
                    i++;
                }
                return CreatObjPlayerDetail(chiSo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Obj_PlayerDetail CreatObjPlayerDetail(List<string> chiSo)
        {
            Obj_PlayerDetail Obj = new Obj_PlayerDetail();
            Obj.DutDiem = Int32.Parse(chiSo[0]);
            Obj.LucSut = Int32.Parse(chiSo[1]);
            Obj.SutXoay = Int32.Parse(chiSo[2]);
            Obj.SutXa = Int32.Parse(chiSo[3]);
            Obj.VoLe = Int32.Parse(chiSo[4]);
            Obj.SutPhat = Int32.Parse(chiSo[5]);
            Obj.Penalty = Int32.Parse(chiSo[6]);
            Obj.DanhDau = Int32.Parse(chiSo[7]);
            Obj.ChonViTri = Int32.Parse(chiSo[8]);
            Obj.TocDo = Int32.Parse(chiSo[9]);
            Obj.TangToc = Int32.Parse(chiSo[10]);
            Obj.KheoLeo = Int32.Parse(chiSo[11]);
            Obj.PhanUng = Int32.Parse(chiSo[12]);
            Obj.Nhay = Int32.Parse(chiSo[13]);
            Obj.TheLuc = Int32.Parse(chiSo[14]);
            Obj.SucManh = Int32.Parse(chiSo[15]);
            Obj.ThangBang = Int32.Parse(chiSo[16]);
            Obj.ChuyenNgan = Int32.Parse(chiSo[17]);
            Obj.ChuyenDai = Int32.Parse(chiSo[18]);
            Obj.TatBong = Int32.Parse(chiSo[19]);
            Obj.GiuBong = Int32.Parse(chiSo[20]);
            Obj.ReBong = Int32.Parse(chiSo[21]);
            Obj.TocDoReBong = Int32.Parse(chiSo[22]);
            Obj.CatBong = Int32.Parse(chiSo[23]);
            Obj.TamNhin = Int32.Parse(chiSo[24]);
            Obj.TranhBong = Int32.Parse(chiSo[25]);
            Obj.XoacBong = Int32.Parse(chiSo[26]);
            Obj.KemNguoi = Int32.Parse(chiSo[27]);
            Obj.QuyetDoan = Int32.Parse(chiSo[28]);
            Obj.TMDoNguoi = Int32.Parse(chiSo[29]);
            Obj.TMBatBong = Int32.Parse(chiSo[30]);
            Obj.TMPhatBong = Int32.Parse(chiSo[31]);
            Obj.TMPhanXa = Int32.Parse(chiSo[32]);
            Obj.TMChonViTri = Int32.Parse(chiSo[33]);
            return Obj;
        }
        #endregion

        #endregion

        #region td3
        private string GetHeigtWeigtBirthDay(HtmlAgilityPack.HtmlDocument document, int i)
        {
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[3]";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    result = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetHeigtWeigtBirthDay " + i + ":" + oldText + ex.Message + ". \n ";
            }
            return result;
        }
        private string GetHeight(string height)
        {
            string result = string.Empty;
            try
            {
                string[] str = height.Split(' ');
                result = str[0];
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetHeight" + oldText + ex.Message + ". \n ";
            }
            return result;
        }
        private string GetWeight(string weight)
        {
            string result = string.Empty;
            try
            {
                string[] str = weight.Split(' ');
                result = str[0];
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetWeight" + oldText + ex.Message + ". \n ";
            }
            return result;
        }
        private string GetBirthDay(string date)
        {
            string result = string.Empty;
            try
            {
                result = date.Substring(14);
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetBirthDay" + oldText + ex.Message + ". \n ";
            }
            return result;
        }
        #endregion

        #region td4
        private string GetStar(HtmlAgilityPack.HtmlDocument document, int i)
        {
            string result = string.Empty;
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[4]";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    result = node.InnerText;
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". \n ";
            }
            return result;
        }
        #endregion
        #region td5
        private List<string> GetFoot(HtmlAgilityPack.HtmlDocument document, int i)
        {
            List<string> lst = new List<string>();
            try
            {
                string str = strSelectNodes + "/tr[" + i + "]/td[5]/ul/li";
                var tblTags = document.DocumentNode.SelectNodes(str);
                foreach (var node in tblTags)
                {
                    lst.Add(node.InnerText);
                }

            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". \n ";
            }
            return lst;
        }
        private string GetLeftFoot(string foot)
        {
            return foot;
        }
        private string GetRightFoot(string foot)
        {
            return foot;
        }
        #endregion

        private string GetIDPlayer(string str)
        {
            string result = string.Empty;
            try
            {
                string[] strArr = str.Split('.');
                result = strArr[0].Substring(2);
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = "GetIDPlayer" + oldText + ex.Message + ". \n ";
            }
            return result;
        }



        private void CreateObjPlayer(HtmlAgilityPack.HtmlDocument document, int i)
        {
            if (Check(document, i))
            {
                List<string> lstFoot = GetFoot(document, i);
                string result = GetHeigtWeigtBirthDay(document, i);
                Obj_Players objs = new Obj_Players();
                objs.LstIndexPosition = GetPosition(document, i);
                objs.PlayerDetail = GetUrlDetail(document, i);

                Obj_Player obj = new Obj_Player();
                obj.Name = GetNamePlayer(document, i);
                obj.Star = FC_Convert.ParseInt(GetStar(document, i).Substring(0, 1));
                obj.Sesson = FC_Convert.ParseSesson(GetSeasson(document, i));
                obj.LeftFoot = Int32.Parse(GetLeftFoot(lstFoot[0]));
                obj.RightFoot = Int32.Parse(GetRightFoot(lstFoot[1]));
                obj.Images = GetUrlImg(document, i);
                obj.IdPlayer = GetIDPlayer(obj.Images);
                obj.Heigth = FC_Convert.ParseInt(GetHeight(result.Substring(0, 6)));
                obj.Weight = FC_Convert.ParseInt(GetWeight(result.Substring(9, 5)));
                obj.BrithDay = FC_Convert.ParseBirthDay(GetBirthDay(result));
                obj.Country = country;
                obj.Team = string.Empty;

                objs.Player = obj;
                objs.LstIndexHidden = LstIndexHidden;
                cr.InsertPlayers(objs);
                // lstPlayDetail.Add(obj);
            }

        }
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
                lblMessage.Text = oldText + ex.Message + ". \n ";
            }
            return check;
        }

        public bool CheckConnectInternet()
        {
            return Fc_Common.IsConnectedToInternet("vietby.net");
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
                        CreateObjPlayer(document, z + 1);
                        z++;
                    }
                }
            }
            catch (Exception ex)
            {
                string oldText = lblMessage.Text;
                lblMessage.Text = oldText + ex.Message + ". \n";
            }
        }
    }
}
