using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Shapes;
using Edit.Properties;
using System.IO;

namespace Edit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isloaded = false;
        bool issaved = true;
        public bool Isloaded { get => isloaded; set => isloaded = value; }
        public static Size WindowSize = new Size();
        DispatcherTimer timer1 = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(25)
        };
        DispatcherTimer maintimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        //RichTextBox[] RtxMain = new RichTextBox[7];
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Public.global.SetGlobal();
            Public.local.SetLocal(DateTime.Now);
            Console.WriteLine(DateTime.Now.ToShortDateString());
            Console.WriteLine(Local.GetDateString(DateTime.Now));
            Console.WriteLine(Local.IsNewDirectory(DateTime.Now, true));
            Console.WriteLine(Local.GetWeekString(DateTime.Now));
            SizeChange();
            ReadFile();
            timer1.Tick += Timer1_Tick;
            maintimer.Tick += MainTimer_Tick;
            Isloaded = true;
        }
        #region 计时器
        /// <summary>
        /// 用于调整窗体时有关的计时器.
        /// </summary>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed || (Mouse.GetPosition(null).X == 0.0 && Mouse.GetPosition(null).Y == 0.0))
            {
                timer1.Stop();
                return;
            }
            Point mouselocation = Mouse.GetPosition(null);
            Point temppoint = new Point(Public.Getlocation(mouselocation.X, 0), Public.Getlocation(mouselocation.Y, 1));
            Point tempresult1 = new Point();
            Point tempresult2 = new Point();
            int i = Public.GetIndex((Shape)ElpControl, 3);
            Console.WriteLine(mouselocation.X + " " + mouselocation.Y);
            Console.WriteLine(Public.Getlocation(mouselocation.X, 0) + " " + Public.Getlocation(mouselocation.Y, 1));
            //共享的处理过程
            if (temppoint.Y < 0.1)
            {
                tempresult1.Y = 0.1; tempresult2.Y = 0.9;
            }
            else if (temppoint.Y > 0.9)
            {
                tempresult1.Y = 0.9; tempresult2.Y = 0.1;
            }
            else
            {
                tempresult1.Y = temppoint.Y; tempresult2.Y = 1 - temppoint.Y;
            }
            //
            if (i == 1)
            {
                if (temppoint.X < (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.1)
                {
                    tempresult1.X = (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.1;
                    tempresult2.X = (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.9;
                }
                else if (temppoint.X > (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.9)
                {
                    tempresult1.X = (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.9;
                    tempresult2.X = (Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat()) * 0.1;
                }
                else
                {
                    tempresult1.X = temppoint.X;
                    tempresult2.X = Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat() - temppoint.X;
                }
                Public.local.localsettings[(int)LocalName.width1].ToFloat(tempresult1.X); Public.local.localsettings[(int)LocalName.width2].ToFloat(tempresult2.X);
                Public.local.localsettings[(int)LocalName.vscroll1].ToFloat(tempresult1.Y);
            }
            else if (i == 2)
            {
                if (temppoint.X < Public.local.localsettings[(int)LocalName.width1].ToFloat() + (1 - Public.local.localsettings[(int)LocalName.width1].ToFloat()) * 0.1)
                {
                    tempresult1.X = (1 - Public.local.localsettings[(int)LocalName.width1].ToFloat()) * 0.1;
                }
                else if (temppoint.X > Public.local.localsettings[(int)LocalName.width1].ToFloat() + (1 - Public.local.localsettings[(int)LocalName.width1].ToFloat()) * 0.9)
                {
                    tempresult1.X = (1 - Public.local.localsettings[(int)LocalName.width1].ToFloat()) * 0.9;
                }
                else
                {
                    tempresult1.X = temppoint.X - Public.local.localsettings[(int)LocalName.width1].ToFloat();
                }
                Public.local.localsettings[(int)LocalName.width2].ToFloat(tempresult1.X);
                Public.local.localsettings[(int)LocalName.vscroll2].ToFloat(tempresult1.Y);
            }
            else if (i == 4)
            {
                if (temppoint.X < Public.local.localsettings[(int)LocalName.width1].ToFloat() * 0.1)
                {
                    tempresult1.X = 0.1;
                }
                else if (temppoint.X > Public.local.localsettings[(int)LocalName.width1].ToFloat() * 0.9)
                {
                    tempresult1.X = 0.9;
                }
                else
                {
                    tempresult1.X = temppoint.X / Public.local.localsettings[(int)LocalName.width1].ToFloat();
                }
                Public.local.localsettings[(int)LocalName.hscroll1].ToFloat(tempresult1.X);
                Public.local.localsettings[(int)LocalName.height1].ToFloat(tempresult1.Y);
            }
            else if (i == 5)
            {
                if (temppoint.X < Public.local.localsettings[(int)LocalName.width1].ToFloat()+ Public.local.localsettings[(int)LocalName.width2].ToFloat() * 0.1)
                {
                    tempresult1.X = 0.1;
                }
                else if (temppoint.X > Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat() * 0.9)
                {
                    tempresult1.X = 0.9;
                }
                else
                {
                    tempresult1.X = (temppoint.X - Public.local.localsettings[(int)LocalName.width1].ToFloat()) / Public.local.localsettings[(int)LocalName.width2].ToFloat();
                }
                Public.local.localsettings[(int)LocalName.hscroll2].ToFloat(tempresult1.X);
                Public.local.localsettings[(int)LocalName.height2].ToFloat(tempresult1.Y);
            }
            else if (i==6)
            {
                double td = Public.local.localsettings[(int)LocalName.width1].ToFloat() + Public.local.localsettings[(int)LocalName.width2].ToFloat();
                if (temppoint.X < td + (1-td) *0.1 )
                {
                    tempresult1.X = 0.1;
                }
                else if (temppoint.X > td + (1 -td ) * 0.9)
                {
                    tempresult1.X = 0.9;
                }
                else
                {
                    tempresult1.X = (temppoint.X - td) / (1 - td);
                }
                Public.local.localsettings[(int)LocalName.hscroll3].ToFloat(tempresult1.X);
                Public.local.localsettings[(int)LocalName.height3].ToFloat(tempresult1.Y);
            }
            SizeChange();
            Public.local.CreateAndSave();
        }
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            Public.global.CreateAndSave();
            Public.local.CreateAndSave();
        }
        #endregion
        #region 窗体缩放时及滑块的操作
        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            WindowSize = new Size(ActualWidth -SystemParameters.BorderWidth -5  , ActualHeight - SystemParameters.CaptionHeight  -10 );
            if (Isloaded)
            {
                SizeChange();
            }
        }
        private void SizeChange()
        {
            int elphalfsize = 6;
            #region 基础数据
            int[] fwidth = new int[3];
            fwidth[0] = Convert.ToInt32(WindowSize.Width * Public.local.localsettings[(int)LocalName.width1].ToFloat());
            fwidth[1] = Convert.ToInt32(WindowSize.Width * Public.local.localsettings[(int)LocalName.width2].ToFloat());
            fwidth[2] = (int)WindowSize.Width - fwidth[0] - fwidth[1];
            int[] fheight = new int[3];
            fheight[0] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.height1].ToFloat());
            fheight[1] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.height2].ToFloat());
            fheight[2] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.height3].ToFloat());
            int[] elplocation = new int[6];
            elplocation[0] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.vscroll1].ToFloat());
            elplocation[1] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.vscroll2].ToFloat());
            elplocation[2] = Convert.ToInt32((int)WindowSize.Height * Public.local.localsettings[(int)LocalName.vscroll3].ToFloat());
            elplocation[3] = Convert.ToInt32(fwidth[0] * Public.local.localsettings[(int)LocalName.hscroll1].ToFloat());
            elplocation[4] = Convert.ToInt32(fwidth[1] * Public.local.localsettings[(int)LocalName.hscroll2].ToFloat());
            elplocation[5] = Convert.ToInt32(fwidth[2] * Public.local.localsettings[(int)LocalName.hscroll3].ToFloat());
            #endregion
            #region RichTextBox大小
            #region 第一列
            Rtx1.Width = fwidth[0]; Rtx2.Width = Rtx1.Width;
            Rtx1.Height = fheight[0]; Rtx2.Height = (int)WindowSize.Height - Rtx1.Height;
            Thickness rtx1margiin = Rtx1.Margin; rtx1margiin.Top = 0; rtx1margiin.Left = 0;
            Rtx1.Margin = rtx1margiin;
            Thickness rtx2margin = Rtx2.Margin; rtx2margin.Top = Rtx1.Height; rtx2margin.Left = 0;
            Rtx2.Margin = rtx2margin;
            #endregion
            #region 第二列
            Rtx3.Width = fwidth[1]; Rtx4.Width = Rtx3.Width;
            Rtx3.Height = fheight[1]; Rtx4.Height = (int)WindowSize.Height - Rtx3.Height;
            Thickness rtx3margin = Rtx3.Margin; rtx3margin.Left = Rtx1.Width; rtx3margin.Top = 0;
            Rtx3.Margin = rtx3margin;
            Thickness rtx4margin = Rtx4.Margin; rtx4margin.Left = Rtx1.Width; rtx4margin.Top = Rtx3.Height;
            Rtx4.Margin = rtx4margin;
            #endregion
            #region 第三列
            Rtx5.Width = fwidth[2]; Rtx6.Width = Rtx5.Width;
            Rtx5.Height = fheight[2]; Rtx6.Height = (int)WindowSize.Height - Rtx5.Height;
            Thickness rtx5margin = Rtx5.Margin; rtx5margin.Left = Rtx1.Width + Rtx3.Width; rtx5margin.Top = 0;
            Rtx5.Margin = rtx5margin;
            Thickness rtx6margin = Rtx6.Margin; rtx6margin.Left = rtx5margin.Left; rtx6margin.Top = Rtx5.Height;
            Rtx6.Margin = rtx6margin;
            #endregion
            #endregion
            #region 滑动条
            Thickness elp1margin = Elp1.Margin; elp1margin.Left = fwidth[0] - elphalfsize; elp1margin.Top = elplocation[0] - elphalfsize; Elp1.Margin = elp1margin;
            Thickness elp2margin = Elp2.Margin; elp2margin.Left = fwidth[0] + fwidth[1] - elphalfsize; elp2margin.Top = elplocation[1] - elphalfsize; Elp2.Margin = elp2margin;
            Thickness elp4margin = Elp4.Margin; elp4margin.Left = elplocation[3] - elphalfsize; elp4margin.Top = fheight[0] - elphalfsize; Elp4.Margin = elp4margin;
            Thickness elp5margin = Elp5.Margin; elp5margin.Left = fwidth[0] + elplocation[4] - elphalfsize; elp5margin.Top = fheight[1] - elphalfsize; Elp5.Margin = elp5margin;
            Thickness elp6margin = Elp6.Margin; elp6margin.Left = fwidth[0] + fwidth[1] + elplocation[5] - elphalfsize; elp6margin.Top = fheight[2] - elphalfsize; Elp6.Margin = elp6margin;
            #endregion
        }
        object ElpControl;
        private void Elp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer1.Start();
            ElpControl = sender;
        }
        private void Elp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timer1.Stop();
        }

        #endregion
        #region Rtf读取与保存操作
        private void ReadFile()
        {
            ReadRtf(Local.DirectoryPath + "0.rtf", Rtx1);
            ReadRtf(Local.DirectoryPath + "1.rtf", Rtx2);
            ReadRtf(Local.DirectoryPath + "2.rtf", Rtx3);
            ReadRtf(Local.DirectoryPath + "3.rtf", Rtx4);
            ReadRtf(Local.DirectoryPath + "4.rtf", Rtx5);
            ReadRtf(Local.DirectoryPath + "5.rtf", Rtx6);
        }
        private void SaveFile(object sender)
        {
            int i = Public.GetIndex((Control)sender, 3);
            SaveRtf(Local.DirectoryPath + (i - 1) + ".rtf", (RichTextBox)sender);
        }
        void ReadRtf(string rtfpath, RichTextBox richTextBox)
        {
            TextRange t = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            FileStream file = new FileStream(rtfpath, FileMode.Open);
            t.Load(file, System.Windows.DataFormats.Rtf);
            file.Close();
            //richTextBox.SelectAll();
            //richTextBox.Selection.Load(new FileStream(rtfpath, FileMode.Open), DataFormats.Rtf);
        }
        void SaveRtf(string rtfpath, RichTextBox richTextBox)
        {
            TextRange t = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            FileStream file = new FileStream(rtfpath, FileMode.Create);
            t.Save(file, System.Windows.DataFormats.Rtf);
            file.Close();
        }
        #endregion
        private void Rtx_TextChanged(object sender, TextChangedEventArgs e)
        {
            //保存文件[全部格式]
            if (Isloaded && issaved)
            {
                issaved = false;
                SaveFile(sender);
                issaved = true;
            }
        }


    }
}
