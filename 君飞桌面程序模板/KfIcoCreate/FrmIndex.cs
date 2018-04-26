using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KfIcoCreate
{
    public partial class FrmIndex : Form
    {
        public FrmIndex()
        {
            InitializeComponent();
            SetStyles();
        }

        #region 减少闪烁
        //减少闪烁
        private void SetStyles()
        {
            base.SetStyle(
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.ResizeRedraw |
              ControlStyles.DoubleBuffer, true);
            base.UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }

        #endregion

        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSel_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedlg = new OpenFileDialog();
            filedlg.Filter = "JPG|*.jpg|BMP|*.bmp|PNG|*.png|GIF|*.gif";
            if (filedlg.ShowDialog() == DialogResult.OK)
            {
                string FileName = filedlg.FileName.Trim();
                this.txtImgPath.SkinTxt.Text = FileName;
                Image img = Image.FromFile(FileName);
                this.picDisplay.Image = img;
            }
        }

        /// <summary>
        /// 生成图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdlg = new FolderBrowserDialog();
            if (fbdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string DirPath = fbdlg.SelectedPath+"\\";
                if(chk16.Checked)
                    CreateIco(this.txtImgPath.SkinTxt.Text, DirPath, ICOTYPE.ICO16);
                if (chk32.Checked)
                    CreateIco(this.txtImgPath.SkinTxt.Text, DirPath, ICOTYPE.ICO32);
                if (chk64.Checked)
                    CreateIco(this.txtImgPath.SkinTxt.Text, DirPath, ICOTYPE.ICO64);
                if (chk128.Checked)
                    CreateIco(this.txtImgPath.SkinTxt.Text, DirPath, ICOTYPE.ICO128);
                if (chk256.Checked)
                    CreateIco(this.txtImgPath.SkinTxt.Text, DirPath, ICOTYPE.ICO256);
                MessageBox.Show("生成完成.");
            }
        }

        private void CreateIco(string ImgFileName,string TargetDir,ICOTYPE IcoType)
        {
            int iSize = 16;
            string IcoName = System.IO.Path.GetFileNameWithoutExtension(ImgFileName);
            switch (IcoType)
            {
                case ICOTYPE.ICO16:
                    iSize = 16;
                    IcoName += "_16";
                    break;
                case ICOTYPE.ICO32:
                    iSize = 32;
                    IcoName += "_32";
                    break;
                case ICOTYPE.ICO64:
                    iSize = 64;
                    IcoName += "_64";
                    break;
                case ICOTYPE.ICO128:
                    iSize = 128;
                    IcoName += "_128";
                    break;
                case ICOTYPE.ICO256:
                    iSize = 256;
                    IcoName += "_256";
                    break;
                case ICOTYPE.ICO512:
                    iSize = 512;
                    IcoName += "_512";
                    break;
            }
            Size size = new Size(iSize, iSize);
            //获得原始图片文件
            using (Bitmap bm = new Bitmap(ImgFileName))
            {
                //从现有图像缩小，为了得到合适的ICO文件
                using (Bitmap iconBm = new Bitmap(bm, size))
                {
                    //如果是windows调用，直接下面一行代码就可以了 
                    //此代码不能在web程序中调用，会有安全异常抛出 
                    using (Icon icon = Icon.FromHandle(iconBm.GetHicon()))
                    {
                        string Name_ = TargetDir + IcoName + ".ico";
                        using (System.IO.Stream stream = new System.IO.FileStream(Name_, System.IO.FileMode.Create))
                        {
                            icon.Save(stream);
                        }
                    }
                }
            }
        }

        private void CreateIco1(string ImgFileName,string TargetDir,ICOTYPE IcoType)
        {
            int iSize = 16;
            string IcoName = System.IO.Path.GetFileNameWithoutExtension(ImgFileName);
            switch (IcoType)
            {
                case ICOTYPE.ICO16:
                    iSize = 16;
                    IcoName += "_16";
                    break;
                case ICOTYPE.ICO32:
                    iSize = 32;
                    IcoName += "_32";
                    break;
                case ICOTYPE.ICO64:
                    iSize = 64;
                    IcoName += "_64";
                    break;
                case ICOTYPE.ICO128:
                    iSize = 128;
                    IcoName += "_128";
                    break;
                case ICOTYPE.ICO256:
                    iSize = 256;
                    IcoName += "_256";
                    break;
                case ICOTYPE.ICO512:
                    iSize = 512;
                    IcoName += "_512";
                    break;
            }
            Size size = new Size(iSize, iSize);
            //获得原始图片文件
            using (Bitmap bm = new Bitmap(ImgFileName))
            {
                //从现有图像缩小，为了得到合适的ICO文件
                using (Bitmap iconBm = new Bitmap(bm, size))
                {
                    //如果是windows调用，直接下面一行代码就可以了 
                    //此代码不能在web程序中调用，会有安全异常抛出 
                    using (Icon icon = Icon.FromHandle(iconBm.GetHicon()))
                    {
                        string Name_ = TargetDir + IcoName + ".ico";
                        using (System.IO.Stream stream = new System.IO.FileStream(Name_, System.IO.FileMode.Create))
                        {
                            icon.Save(stream);
                        }
                    }
                }
            }
            //this.Icon = icon; 
            //Graphics g = this.CreateGraphics();      
            //g.DrawIcon(icon, this.ClientRectangle);       
            //g.DrawIconUnstretched(icon, this.ClientRectangle); 
        }

        enum ICOTYPE
        {
            ICO16,
            ICO32,
            ICO64,
            ICO128,
            ICO256
        }
    }
}
