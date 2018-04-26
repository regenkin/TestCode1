/*
 * 
 ���ܣ�ͼƬ���Ųü�
 ���ߣ��е��� QQ:43784751 Home:http://www.55rc.com TEL:18980778883
 ʱ�䣺2009-12-17
 * 
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class ZoomImage
{
    public static bool SaveCutPic(string pPath, string filePath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY, int imageWidth, int imageHeight)
    {
        using (System.Drawing.Image originalImg = System.Drawing.Image.FromFile(pPath))
        {
            try
            {
                if (originalImg.Width == imageWidth && originalImg.Height == imageHeight)
                {
                    return SaveCutPic(pPath, filePath, pPartStartPointX, pPartStartPointY, pPartWidth, pPartHeight,
                            pOrigStartPointX, pOrigStartPointY);

                }

                Bitmap thumimg = MakeThumbnail(originalImg, imageWidth, imageHeight);

                Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);

                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//Ŀ��λ��
                Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//ԭͼλ�ã�Ĭ�ϴ�ԭͼ�н�ȡ��ͼƬ��С����Ŀ��ͼƬ�Ĵ�С��

                ///����ˮӡ  
                Graphics G = Graphics.FromImage(partImg);
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                G.Clear(Color.White);
                // ָ����������˫���β�ֵ����ִ��Ԥɸѡ��ȷ������������������ģʽ�ɲ���������ߵ�ת��ͼ�� 
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // ָ�������������ٶȳ��֡� 
                G.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(thumimg, destRect, origRect, GraphicsUnit.Pixel);
                //G.DrawString("Xuanye", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                partImg.Save(filePath, ImageFormat.Jpeg);

                partImg.Dispose();
                thumimg.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public static Bitmap MakeThumbnail(System.Drawing.Image fromImg, int width, int height)
    {
        Bitmap bmp = new Bitmap(width, height);
        int ow = fromImg.Width;
        int oh = fromImg.Height;

        //�½�һ������
        Graphics g = Graphics.FromImage(bmp);

        //���ø�������ֵ��
        g.InterpolationMode = InterpolationMode.High;
        //���ø�����,���ٶȳ���ƽ���̶�
        g.SmoothingMode = SmoothingMode.Default;
        //��ջ�������͸������ɫ���
        g.Clear(Color.White);

        g.DrawImage(fromImg, new Rectangle(0, 0, width, height),
            new Rectangle(0, 0, ow, oh),
            GraphicsUnit.Pixel);

        return bmp;

    }

    public static bool SaveCutPic(string pPath, string filePath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
    {
        using (System.Drawing.Image originalImg = System.Drawing.Image.FromFile(pPath))
        {
            try
            {
                Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//Ŀ��λ��
                Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//ԭͼλ�ã�Ĭ�ϴ�ԭͼ�н�ȡ��ͼƬ��С����Ŀ��ͼƬ�Ĵ�С��

                ///ע�� ����ˮӡ  
                Graphics G = Graphics.FromImage(partImg);
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                G.Clear(Color.White);
                // ָ����������˫���β�ֵ����ִ��Ԥɸѡ��ȷ������������������ģʽ�ɲ���������ߵ�ת��ͼ�� 
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // ָ�������������ٶȳ��֡� 
                G.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
                //G.DrawString("Xuanye", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                partImg.Save(filePath, ImageFormat.Jpeg);
                partImg.Dispose();
            }
            catch
            {
                return false;
            }
        }
        return true;
    }
}