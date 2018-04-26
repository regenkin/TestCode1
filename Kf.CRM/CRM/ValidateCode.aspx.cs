using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace KfCrm.CRM
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        private int letterWidth = 16;//��������Ŀ�ȷ�Χ             
        private int letterHeight = 21;//��������ĸ߶ȷ�Χ             
        private int letterCount = 4;//��֤��λ��             
        private char[] chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz".ToCharArray();
        private string[] fonts = { "Arial", "Georgia" };
        /// <summary>             
        /// ���������˾�Ч��             
        /// </summary>             

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;
        protected void Page_Load(object sender, EventArgs e)
        {
            //��ֹ��ҳ����--��ֹ����                     
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.CacheControl = "no-cache";
            string str_ValidateCode = GetRandomNumberString(letterCount);
            Session["CheckCode"] = str_ValidateCode;

            CreateImage(str_ValidateCode);
        }
        public void CreateImage(string checkCode)
        {
            int int_ImageWidth = checkCode.Length * letterWidth + 8;
            Random newRandom = new Random();
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            //�������������                 
            Random random = new Random();
            //��ɫ����                 
            g.Clear(Color.White);
            //��ͼƬ�ı���������                 
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //��ͼƬ��ǰ��������                
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //����������ɫ����֤���ַ�                     
            int findex;
            for (int int_index = 0; int_index < checkCode.Length; int_index++)
            {
                findex = newRandom.Next(fonts.Length - 1);
                string str_char = checkCode.Substring(int_index, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(int_index * letterWidth + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                g.DrawString(str_char, new Font(fonts[findex], 12, FontStyle.Bold), newBrush, thePos);
            }
            //��ɫ�߿�                 
            //g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth -1, (letterHeight - 1));
            //ͼƬŤ��                 
            image = TwistImage(image, true, 2, 3);                 
            //�����ɵ�ͼƬ���ؿͻ���                
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            Response.ClearContent();
            //��Ҫ���ͼ����Ϣ Ҫ�޸�HTTPͷ                  
            Response.ContentType = "image/Png";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
        /// <summary>             
        /// ��������WaveŤ��ͼƬ             
        /// </summary>             
        /// <param name="srcBmp">ͼƬ·��</param>             
        /// <param name="bXDir">���Ť����ѡ��ΪTrue</param>             
        /// <param name="nMultValue">���εķ��ȱ�����Խ��Ť���ĳ̶�Խ�ߣ�һ��Ϊ3</param>             
        /// <param name="dPhase">���ε���ʼ��λ��ȡֵ����[0-2*PI)</param>             
        /// <returns></returns>            
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);                    
            // ��λͼ�������Ϊ��ɫ                 

            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    // ȡ�õ�ǰ�����ɫ                         
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }
        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(210);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);     
        }
        //  ������������ַ���             
        public string GetRandomNumberString(int int_NumberLength)
        {
            Random random = new Random();
            string validateCode = string.Empty;
            for (int i = 0; i < int_NumberLength; i++)
                validateCode += chars[random.Next(0, chars.Length)].ToString();
            return validateCode;
        }
    }
}