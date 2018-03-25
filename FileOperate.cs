using System;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Edit
{
    /// 摘要
    /// <summary>
    /// 全局设置的名称 Edit.Global
    /// </summary>
    public enum GlobalName
    {
        flocation = 0,
        fsize = 1,
        ismaxshow = 2,
        buttonlocation = 3,
        autocolor = 4,
        autosize = 5,
        quickedit = 6,
        importname = 7,
        colorbackground = 8,
        colornormal = 9,
        colorgroup = 10,
        colorteacher = 11,
        colormark = 12,
    }
    /// <summary>
    /// 本地设置的名称 Edit.Local
    /// </summary>
    public enum LocalName
    {
        title = 0,
        week = 1,
        date = 2,
        filepath = 3,
        autocolor = 4,
        autosize = 5,
        quickedit = 6,
        width1 = 7,
        width2 = 8,
        height1 = 9,
        height2 = 10,
        height3 = 11,
        vscroll1 = 12,
        vscroll2 = 13,
        vscroll3 = 14,
        hscroll1 = 15,
        hscroll2 = 16,
        hscroll3 = 17
    }
    /// <summary>
    /// 设置类的结构[Name,Index,Value]
    /// </summary>
    struct Fsettings : INotifyPropertyChanged
    {
        public Fsettings(string name, int index) : this()
        {
            Name = name;
            Index = index;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        string name;
        int index;
        string value;

        public string Name { get => name; set => name = value; }
        public int Index { get => index; set => index = value; }
        public string Value { get => value; set
            {
                this.value = value;
            }
        }

        public int ToInt()
        {
            return int.Parse(this.Value);
        }
        public void ToInt(int value)
        {
            this.Value = string.Concat(value);
        }
        public float ToFloat()
        {
            return float.Parse(this.Value);
        }
        public void ToFloat(double value)
        {
            Value = string.Concat(value);
        }
        public void ToFloat(float value)
        {
            this.Value = string.Concat(value);
        }
        public Color ToColor()
        {
            string[] s = Value.Split(',');
            byte[] tb = new byte[s.Length];
            for (int i = 0; i < s.GetUpperBound(0); i++)
            {
                tb[i] = byte.Parse(s[i]);
            }
            return Color.FromRgb(tb[0], tb[1], tb[2]);
        }
        public void ToColor(Color color)
        {
            string[] s = new string[3];
            s[0] = string.Concat(color.R);
            s[1] = string.Concat(color.G);
            s[2] = string.Concat(color.B);
            string ts = s[0] + "," + s[1] + "," + s[2];
            Value = ts;
        }
        public float[] ToFloats()
        {
            string[] ts = Value.Split(',');
            float[] tf = new float[ts.GetUpperBound(0)];
            for (int i = 0; i < ts.GetUpperBound(0); i++)
            {
                tf[i] = float.Parse(ts[i]);
            }
            return tf;
        }
        public void ToFloats(float value1, float value2)
        {
            string[] ts = new string[2];
            ts[0] = string.Concat(value1);
            ts[1] = string.Concat(value2);
            Value = ts[0] + "," + ts[1];

        }
        public void ToFloats(double value1, double value2)
        {
            string[] ts = new string[2];
            ts[0] = string.Concat(value1);
            ts[1] = string.Concat(value2);
            Value = ts[0] + ',' + ts[1];
        }
        public bool Equal(object obj)
        {
            if (Equals(this, obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class Global
    {
        const int globalsettingsnum = 13;
        private static readonly string globalPath = AppDomain.CurrentDomain.BaseDirectory + "Global.xml";
        public static string GlobalPath => globalPath;
        public Fsettings[] globalsettings = new Fsettings[globalsettingsnum];
        #region 内部函数
        /// <summary>
        /// 初始化
        /// </summary>
        void Start()
        {
            foreach (int i in Enum.GetValues(typeof(GlobalName)))
            {
                globalsettings[i].Index = i;
                globalsettings[i].Name = Enum.GetName(typeof(GlobalName), i);
            }
        }
        /// <summary>
        /// 默认设置.
        /// </summary>
        void Tolerate()
        {
            globalsettings[(int)GlobalName.flocation].ToFloats(0.1, 0.1);
            globalsettings[(int)GlobalName.fsize].ToFloats(0.8, 0.8);
            globalsettings[(int)GlobalName.ismaxshow].ToInt(0);
            globalsettings[(int)GlobalName.buttonlocation].ToFloats(0.0, 0.0);
            globalsettings[(int)GlobalName.autocolor].ToInt(0);
            globalsettings[(int)GlobalName.autosize].ToInt(0);
            globalsettings[(int)GlobalName.quickedit].ToInt(0);
            globalsettings[(int)GlobalName.colorbackground].ToColor(Color.FromRgb(0, 0, 0));
            globalsettings[(int)GlobalName.colornormal].ToColor(Color.FromRgb(255, 255, 255));
            globalsettings[(int)GlobalName.colorgroup].ToColor(Color.FromRgb(255, 128, 0));
            globalsettings[(int)GlobalName.colorteacher].ToColor(Color.FromRgb(0, 212, 18));
            globalsettings[(int)GlobalName.colormark].ToColor(Color.FromRgb(0, 75, 244));
        }
        /// <summary>
        /// 创建及保存xml
        /// </summary>
       public  void CreateAndSave()
        {
            // string xmlFileName = "Global.xml";
            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点.
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            //创建根节点.
            XmlNode root = xmlDoc.CreateElement("Global");
            xmlDoc.AppendChild(root);
            for (int i = 0; i <= globalsettings.GetUpperBound(0); i++)
            {
                XmlElement parent = xmlDoc.CreateElement(globalsettings[i].Name);
                parent.SetAttribute("index", string.Concat(globalsettings[i].Index));
                parent.SetAttribute("value", globalsettings[i].Value);
                root.AppendChild(parent);
            }
            //保存文件.
            xmlDoc.Save(GlobalPath);
        }
        /// <summary>
        /// 读取xml
        /// </summary>
        void Read()
        {
            //读取xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GlobalPath);
            //获取根节点
            XmlElement root = xmlDoc.DocumentElement;
            //获取根节点下的所有子节点
            XmlNodeList globalnodelist = root.ChildNodes;
            if (globalnodelist != null)
            {
                foreach (XmlNode parent in globalnodelist)
                {
                    globalsettings[int.Parse(parent.Attributes["index"].Value)].Value = parent.Attributes["value"].Value;
                }
            }
        }

        #endregion
        /// <summary>
        /// 读取xml,不存在时即创建.
        /// </summary>
        public void SetGlobal()
        {
            Start();
            if (File.Exists(GlobalPath))
            {
                Read();
            }
            else
            {
                Tolerate();
                CreateAndSave();
            }
        }
    }
    class Local
    {

        const int localsettingsnum = 18;
        private static readonly string dataPath = AppDomain.CurrentDomain.BaseDirectory + @"数据\";
        public static string DataPath => dataPath;
        static string directoryPath = "";
        public static string DirectoryPath { get => directoryPath; set => directoryPath = value; }
        static string localPath = "";
        public static string LocalPath { get => localPath; set => localPath = value; }
        static readonly string modPath = AppDomain.CurrentDomain.BaseDirectory + @"模板\";
        public static string ModPath => modPath;
        public Fsettings[] localsettings = new Fsettings[localsettingsnum];
        #region 内部函数
        /// <summary>
        /// 初始化
        /// </summary>
        void Start()
        {
            foreach (int i in Enum.GetValues(typeof(LocalName)))
            {
                localsettings[i].Index = i;
                localsettings[i].Name = Enum.GetName(typeof(LocalName), i);
            }
        }
        /// <summary>
        /// 默认设置
        /// </summary>
        void Tolerate()
        {
            localsettings[(int)LocalName.title].Value = "";
            localsettings[(int)LocalName.week].Value = string.Concat(GetWeekString(DateTime.Now));
            localsettings[(int)LocalName.date].Value = GetDateString(DateTime.Now);
            localsettings[(int)LocalName.filepath].Value = DirectoryPath;
            localsettings[(int)LocalName.autocolor].Value = Public.global.globalsettings[(int)GlobalName.autocolor].Value;
            localsettings[(int)LocalName.autosize].Value = Public.global.globalsettings[(int)GlobalName.autosize].Value;
            localsettings[(int)LocalName.quickedit].Value = Public.global.globalsettings[(int)GlobalName.quickedit].Value;
            localsettings[(int)LocalName.width1].ToFloat(0.33);
            localsettings[(int)LocalName.width2].ToFloat(0.33);
            localsettings[(int)LocalName.height1].ToFloat(0.5);
            localsettings[(int)LocalName.height2].ToFloat(0.5);
            localsettings[(int)LocalName.height3].ToFloat(0.5);
            localsettings[(int)LocalName.vscroll1].ToFloat(0.4);
            localsettings[(int)LocalName.vscroll2].ToFloat(0.4);
            localsettings[(int)LocalName.vscroll3].ToFloat(0.4);
            localsettings[(int)LocalName.hscroll1].ToFloat(0.5);
            localsettings[(int)LocalName.hscroll2].ToFloat(0.5);
            localsettings[(int)LocalName.hscroll3].ToFloat(0.5);
        }
        /// <summary>
        /// 创建及保存xml
        /// </summary>
       public void CreateAndSave()
        {
            // string xmlFileName = "Global.xml";
            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点.
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            //创建根节点.
            XmlNode root = xmlDoc.CreateElement("Local");
            xmlDoc.AppendChild(root);
            for (int i = 0; i <= localsettings.GetUpperBound(0); i++)
            {
                XmlElement parent = xmlDoc.CreateElement(localsettings[i].Name);
                parent.SetAttribute("index", string.Concat(localsettings[i].Index));
                parent.SetAttribute("value", localsettings[i].Value);
                root.AppendChild(parent);
            }
            //保存文件.
            xmlDoc.Save(LocalPath);
        }
        /// <summary>
        /// 读取xml
        /// </summary>
        void Read()
        {
            //读取xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(LocalPath);
            //获取根节点
            XmlElement root = xmlDoc.DocumentElement;
            //获取根节点下的所有子节点
            XmlNodeList globalnodelist = root.ChildNodes;
            if (globalnodelist != null)
            {
                foreach (XmlNode parent in globalnodelist)
                {
                    localsettings[int.Parse(parent.Attributes["index"].Value)].Value = parent.Attributes["value"].Value;
                }
            }
        }
        /// <summary>
        /// 创建内容
        /// </summary>
        void CreateFile()
        {
            for (int i = 0; i <= 5; i++)
            {
                string ts = i + ".rtf";
                if (!File.Exists(DirectoryPath + ts))
                {
                    File.Copy(ModPath + ts, DirectoryPath + ts);
                }
            }
        }

        #endregion
        #region 外部函数
        /// <summary>
        /// 获得对应日期的字符串格式.
        /// </summary>
        public static string GetDateString(DateTime dateTime)
        {
            string formatstring = "0";
            string[] ts = dateTime.ToShortDateString().Split('/');
            string s = "";
            s += ts[0];
            for (int i = 1; i <= ts.GetUpperBound(0); i++)
            {
                s += formatstring.Substring(0, 2 - ts[i].Length) + ts[i];
            }
            return s;
        }
        /// <summary>
        /// 判断今天是否要新建目录,并进行设置操作.
        /// </summary>
        /// <param name = "isoperate">是否要进行操作</param>
        public static bool IsNewDirectory(DateTime dateTime, bool isoperate = false)
        {
            string NowDateString = GetDateString(dateTime);
            DirectoryPath = DataPath + NowDateString + @"\";
            LocalPath = DirectoryPath + @"Local.xml";
            if (Directory.Exists(DirectoryPath))
            {
                return true;
            }
            else
            {
                if (isoperate)
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                return false;
            }
        }
        /// <summary>
        /// 获得对应日期的星期[按年算]
        /// </summary>
        public static int GetWeekString(DateTime dateTime)
        {
            CultureInfo cultureInfo = new CultureInfo("zh-CN");
            Calendar calendar = cultureInfo.Calendar;
            return calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public void SetLocal(DateTime dateTime)
        {
            Start();
            //第一步:检测是否有对应的文件;
            IsNewDirectory(dateTime);
            //第二步:检测是否有对应的local.xml文件,并操作;
            if (File.Exists(LocalPath))
            {
                Read();
            }
            else
            {
                Tolerate();
                CreateAndSave();
            }
            //第三步:检测是否有rtf文件.
            CreateFile();
        }
        #endregion
    }

}

