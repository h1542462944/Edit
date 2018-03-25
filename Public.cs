using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Edit
{
    class Public
    {

        public static Global global = new Global();
        public static Local local = new Local();
        public static int GetIndex(Control sender, int startnumindex)
        {
            return int.Parse(sender.Name.Substring(startnumindex));
        }
        public static int GetIndex(Shape sender, int startnumindex)
        {
            return int.Parse(sender.Name.Substring(startnumindex));
        }
        /// <summary>
        /// 获取某一个控件左上角对应的相对比例坐标.
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="type">类型,0:=水平,其他:=垂直</param>
        /// <returns></returns>
        public static float Getlocation(double value, int type)
        {
            if (type ==0)
            {
                return  Convert.ToSingle( value / MainWindow.WindowSize.Width);
            }
            else
            {
                return Convert.ToSingle( value / MainWindow.WindowSize.Height);
            }
        }
    }
}
