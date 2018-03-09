using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HexASCII
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            #region 为combobox赋值

            //BindingSource()方法
            IList<string> list = new List<string>();
            list.Add("true");
            list.Add("false");
            comb_isComp3.DataSource = new BindingSource(list, null);
            comb_isUnsign.DataSource = new BindingSource(list, null);
            comb_isInt.DataSource = new BindingSource(list, null);

            //Clone()方法
            //string[] list = new string[2] { "true", "false" };
            //string[] list1 = (string[])(list.Clone());
            //string[] list2 = (string[])(list.Clone());
            //string[] list3 = (string[])(list.Clone());
            //comboBox1.DataSource = list1;
            //comboBox2.DataSource = list2;
            //comboBox3.DataSource = list3;

            #endregion
        }

        #region 转换按钮点击事件
        private void ValueToTxt_Click(object sender, EventArgs e)
        {
            if (txt_value.Text == "" || txt_integerPartnum.Text == "" || txt_decimalsPartnum.Text == "")
            {
                txt_ebcdicstring.Text = "Please ensure that all text boxes are filled in.";
            }
            else
            {
                txt_value.Text = txt_value.Text.Replace(" ", "");
                string Hexstring = FromValueToHex(txt_value.Text, bool.Parse(comb_isComp3.SelectedValue.ToString()), 
                    bool.Parse(comb_isUnsign.SelectedValue.ToString()), bool.Parse(comb_isInt.SelectedValue.ToString()), 
                    int.Parse(txt_integerPartnum.Text), int.Parse(txt_decimalsPartnum.Text));
                txt_ebcdicstring.Text = Hexstring;
                WriteBYByte("C:\\Barry\\GUSDE20.txt", ebcdicToISOBytes(StrToStringArray(Hexstring)));
            }
            #region 
            //MyData data = new MyData(HexToByte(textBox1.Text));
            //WriteBYTxt("C:\\Barry\\GUSDE20.txt", ASCIIEncoding.Default.GetString(HexToByte(textBox1.Text)));

            //var encode = Encoding.UTF8;
            ////var bytes = encode.GetBytes(textBox1.Text);
            //var bytes = HexToByte(textBox1.Text);
            //StringBuilder ret = new StringBuilder();
            //foreach (byte b in bytes)
            //{
            //    //{0:X2} 大写
            //    ret.AppendFormat("{0:X2}", b);
            //}
            //var hex = ret.ToString();
            //textBox2.Text = hex;


            //textBox2.Text = HignAndLowConvert(textBox1.Text);
            //WriteTOTxt("C:\\Barry\\GUSDE20.txt", textBox2.Text);

            //MyData data = new MyData(HexToByte(textBox1.Text));
            //WriteTOTxt("C:\\Barry\\GUSDE20.txt", data);


            //// 读取16进制数据
            //int ReadInt = 123;

            //// 将每个字节取出来
            //byte byte2 = (byte)(ReadInt >> 4);  //高位
            //byte byte1 = (byte)(ReadInt & 0x0f);  //低位

            //// 拼装成 "高字节在后，低字节在前"的格式
            //Console.Write(ReadInt & 0x0f);

            //// Java读取16进制数据
            //int ReadInt = 0xBB0D8196;

            //// 将每个字节取出来  
            //byte byte4 = (byte)(ReadInt & 0xff);
            //byte byte3 = (byte)((ReadInt & 0xff00) >> 8);
            //byte byte2 = (byte)((ReadInt & 0xff0000) >> 16);
            //byte byte1 = (byte)((ReadInt & 0xff000000) >> 24);

            //// 拼装成 "高字节在后，低字节在前"的格式
            //int realint = (byte1 & 0xff) << 0 | (byte2 & 0xff) << 8 | (byte3 & 0xff) << 16 | (byte4 & 0xff) << 24;
            //textBox2.Text = realint.ToString();
            #endregion
        }
        #endregion

        #region 根据value合成字符串 条件：是否压缩 是否无符号 是否整数（整数位数，小数位数） value to string
        /// <summary>
        /// 根据value合成字符串 条件：是否压缩 是否无符号 是否整数（整数位数，小数位数）
        /// </summary>
        /// <param name="inputstr">输入value</param>
        /// <param name="isComp3">是否压缩</param>
        /// <param name="isUnsign">是否无符号</param>
        /// <param name="isInt">是否整数</param>
        /// <param name="integerPartnum">整数位数</param>
        /// <param name="decimalsPartnum">小数位数</param>
        /// <returns></returns>
        public string FromValueToHex(string inputstr, bool isComp3, bool isUnsign, 
            bool isInt, int integerPartnum, int decimalsPartnum)
        {
            string Hexstring = "";
            int bytenum = integerPartnum + decimalsPartnum;

            #region 压缩型
            if (isComp3)  //压缩型
            {
                if (bytenum % 2 == 0)
                    bytenum = bytenum / 2 + 1;
                else
                    bytenum = (bytenum + 1) / 2;
                if (isUnsign)
                {
                    if(isInt)
                    {
                        #region 压缩型 无符号 整型
                        //在后面补F
                        inputstr += "F";

                        //在前面补0
                        int zerocount = bytenum * 2 - inputstr.Length;
                        if(zerocount > 0)
                        {
                            for (int i = 0; i < zerocount; i++)
                                inputstr = "0" + inputstr;
                        }
                        
                        Hexstring = inputstr;
                        #endregion
                    }
                    else
                    {
                        #region 压缩型 无符号 浮点型
                        if (decimalsPartnum != 0)
                        {
                            if (!inputstr.Contains("."))
                            {
                                //在后面补0
                                for (int i = 0; i < decimalsPartnum; i++)
                                    inputstr += "0";
                                //为小数部分后面补F
                                inputstr += "F";
                                //为整数部分前面补0
                                int zerocount = bytenum * 2 - inputstr.Length;
                                for (int i = 0; i < zerocount; i++)
                                    inputstr = "0" + inputstr;

                                //合成最终Hex字符串
                                Hexstring = inputstr;
                            }
                            else
                            {
                                //分割整数部分和小数部分
                                string[] values = inputstr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                                //存储小数部分的字符串
                                string decimalsPart = values[1];
                                //存储整数部分的字符串
                                string integerPart = values[0];

                                //为小数部分后面补F
                                decimalsPart += "F";
                                //为整数部分前面补0
                                int zerocount = bytenum * 2 - integerPart.Length - decimalsPart.Length;
                                for (int i = 0; i < zerocount; i++)
                                    integerPart = "0" + integerPart;

                                //合成最终Hex字符串
                                Hexstring = integerPart + decimalsPart;
                            }
                        }
                        else
                        {
                            //分割整数部分和小数部分
                            string[] values = inputstr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                            //存储小数部分的字符串
                            string decimalsPart = values[1];
                            //存储整数部分的字符串
                            string integerPart = values[0];

                            //为小数部分后面补F
                            decimalsPart += "F";
                            //为整数部分前面补0
                            int zerocount = bytenum * 2 - integerPart.Length - decimalsPart.Length;
                            for (int i = 0; i < zerocount; i++)
                                integerPart = "0" + integerPart;

                            //合成最终Hex字符串
                            Hexstring = integerPart + decimalsPart;
                        }
                        #endregion
                    }
                }
                else
                {
                    bool isnegative = inputstr.Contains("-");
                    inputstr = inputstr.Replace("-", "");

                    if (isInt)
                    {
                        #region 压缩型 带符号 整型
                        //正数在后面补C 负数在后面补D
                        if (!isnegative)
                            inputstr += "C";
                        else
                            inputstr += "D";

                        //在前面补0
                        int zerocount = bytenum * 2 - inputstr.Length;
                        for (int i = 0; i < zerocount; i++)
                            inputstr = "0" + inputstr;

                        //合成最终Hex字符串
                        Hexstring = inputstr;
                        #endregion
                    }
                    else
                    {
                        #region 压缩型 带符号 浮点型
                        if (decimalsPartnum != 0)
                        {
                            if (!inputstr.Contains("."))
                            {
                                //在后面补0
                                for (int i = 0; i < decimalsPartnum; i++)
                                    inputstr += "0";
                                //小数部分 正数在后面补C 负数在后面补D
                                if (!isnegative)
                                    inputstr += "C";
                                else
                                    inputstr += "D";
                                //为整数部分前面补0
                                int zerocount = bytenum * 2 - inputstr.Length;
                                for (int i = 0; i < zerocount; i++)
                                    inputstr = "0" + inputstr;

                                //合成最终Hex字符串
                                Hexstring = inputstr;
                            }
                            else
                            {
                                //分割整数部分和小数部分
                                string[] values = inputstr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                                //存储小数部分的字符串
                                string decimalsPart = values[1];
                                //存储整数部分的字符串
                                string integerPart = values[0];

                                //小数部分 正数在后面补C 负数在后面补D
                                if (!isnegative)
                                    decimalsPart += "C";
                                else
                                    decimalsPart += "D";
                                //为整数部分前面补0
                                int zerocount = bytenum * 2 - integerPart.Length - decimalsPart.Length;
                                for (int i = 0; i < zerocount; i++)
                                {
                                    integerPart = "0" + integerPart;
                                }

                                //合成最终Hex字符串
                                Hexstring = integerPart + decimalsPart;
                            }
                        }
                        else
                        {
                            //分割整数部分和小数部分
                            string[] values = inputstr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                            //存储小数部分的字符串
                            string decimalsPart = values[1];
                            //存储整数部分的字符串
                            string integerPart = values[0];

                            //小数部分 正数在后面补C 负数在后面补D
                            if (!isnegative)
                                decimalsPart += "C";
                            else
                                decimalsPart += "D";
                            //为整数部分前面补0
                            int zerocount = bytenum * 2 - integerPart.Length - decimalsPart.Length;
                            for (int i = 0; i < zerocount; i++)
                            {
                                integerPart = "0" + integerPart;
                            }

                            //合成最终Hex字符串
                            Hexstring = integerPart + decimalsPart;
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 无压缩型
            else  //无压缩型
            {
                if (isUnsign)
                {
                    if (isInt)
                    {
                        #region 无压缩型 无符号 整型
                        //在前面补0
                        int zerocount = bytenum - inputstr.Length;
                        if(zerocount > 0)
                        {
                            for (int i = 0; i < zerocount; i++)
                                inputstr = "0" + inputstr;
                        }
                        //在前面补F
                        for (int i = 0; i < bytenum; i++)
                            inputstr = "F" + inputstr;

                        Hexstring = HignAndLowConvertToStr(inputstr);
                        #endregion
                    }
                    else
                    {
                        #region 无压缩型 无符号 浮点型
                        if (inputstr.Contains("."))
                        {
                            inputstr = inputstr.Replace(".", "");
                        }
                        else
                        {
                            if (decimalsPartnum != 0)
                            {
                                //在后面补0
                                for (int i = 0; i < decimalsPartnum; i++)
                                    inputstr += "0";
                            }
                        }

                        //在前面补0
                        int zerocount = bytenum - inputstr.Length;
                        if (zerocount > 0)
                        {
                            for (int i = 0; i < zerocount; i++)
                                inputstr = "0" + inputstr;
                        }
                        //在前面补F
                        for (int i = 0; i < bytenum; i++)
                            inputstr = "F" + inputstr;

                        //合成最终Hex字符串
                        Hexstring = HignAndLowConvertToStr(inputstr);
                        #endregion
                    }
                }
                else
                {
                    bool isnegative = inputstr.Contains("-");
                    inputstr = inputstr.Replace("-", "");

                    if (isInt)
                    {
                        #region 无压缩型 带符号 整型
                        //在前面补0
                        int zerocount = bytenum - inputstr.Length;
                        for (int i = 0; i < zerocount; i++)
                            inputstr = "0" + inputstr;
                        //正数在前面补C 负数在前面补D
                        if (!isnegative)
                            inputstr = "C" + inputstr;
                        else
                            inputstr = "D" + inputstr;
                        //在前面补F
                        for (int i = 0; i < bytenum - 1; i++)
                            inputstr = "F" + inputstr;

                        Hexstring = HignAndLowConvertToStr(inputstr);
                        #endregion
                    }
                    else
                    {
                        #region 无压缩型 带符号 浮点型
                        if (inputstr.Contains("."))
                        {
                            inputstr = inputstr.Replace(".", "");
                        }
                        else
                        {
                            if (decimalsPartnum != 0)
                            {
                                //在后面补0
                                for (int i = 0; i < decimalsPartnum; i++)
                                    inputstr += "0";
                            }
                        }

                        //在前面补0
                        int zerocount = bytenum - inputstr.Length;
                        if (zerocount > 0)
                        {
                            for (int i = 0; i < zerocount; i++)
                                inputstr = "0" + inputstr;
                        }
                        //正数在前面补C 负数在前面补D
                        if (!isnegative)
                            inputstr = "C" + inputstr;
                        else
                            inputstr = "D" + inputstr;
                        //在前面补F
                        for (int i = 0; i < bytenum - 1; i++)
                            inputstr = "F" + inputstr;

                        //合成最终Hex字符串
                        Hexstring = HignAndLowConvertToStr(inputstr);                        
                        #endregion
                    }
                }
            }
            #endregion

            return Hexstring;
        }
        #endregion

        #region 无压缩型 高位低位变换位置
        /// <summary>
        /// 无压缩型 高位低位变换位置
        /// </summary>
        /// <param name="str">无压缩型字符串</param>
        /// <returns></returns>
        public static string HignAndLowConvertToStr(string str)
        {
            List<string> list = new List<string>();
            int count = str.Length / 2;
            for (int i = 0; i < count; i++)
            {
                list.Add(str[i].ToString());
                list.Add(str[i + count].ToString());
            }
            string result = "";
            foreach (string s in list)
                result += s;
            return result;
        }
        #endregion

        #region 将string类型每两位拆分   string to string[]
        /// <summary>
        /// 将string类型每两位拆分至string[]
        /// </summary>
        /// <param name="mHex">输入EBCDIC string</param>
        /// <returns></returns>
        public string[] StrToStringArray(string mHex)
        {
            mHex = mHex.Replace(" ", "");
            string[] strs = new string[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
            {
                string resultstr = mHex.Substring(i, 2);
                strs[i / 2] = resultstr;
            }
            return strs;
        }
        #endregion

        #region EBCDIC字典 ISO 8859-1 字典 之间的转换 string[] to byte[]

        #region EBCDIC字典
        public static string[] EBCDIC = new string[] {
            "00", "01", "02", "03",
            "04", "05", "06", "07",
            "08", "09", "0A", "0B",
            "0C", "0D", "0E", "0F",

            "10", "11", "12", "13",
            "14", "15", "16", "17",
            "18", "19", "1A", "1B",
            "1C", "1D", "1E", "1F",

            "20", "21", "22", "23",
            "24", "25", "26", "27",
            "28", "29", "2A", "2B",
            "2C", "2D", "2E", "2F",

            "30", "31", "32", "33",
            "34", "35", "36", "37",
            "38", "39", "3A", "3B",
            "3C", "3D", "3E", "3F",

            "40", "41", "42","43",
            "44", "45", "46", "47",
            "48", "49", "4A", "4B",
            "4C", "4D", "4E", "4F",

            "50", "51", "52", "53",
            "54", "55", "56", "57",
            "58", "59", "5A", "5B",
            "5C", "5D", "5E", "5F",

            "60", "61", "62", "63",
            "64", "65", "66", "67",
            "68", "69", "6A", "6B",
            "6C", "6D", "6E", "6F",

            "70", "71", "72", "73",
            "74", "75", "76", "77",
            "78", "79", "7A", "7B",
            "7C", "7D", "7E", "7F",

            "80", "81", "82", "83",
            "84", "85", "86", "87",
            "88", "89", "8A", "8B",
            "8C", "8D", "8E", "8F",

            "90", "91", "92", "93",
            "94", "95", "96", "97",
            "98", "99", "9A", "9B",
            "9C", "9D", "9E", "9F",

            "A0", "A1", "A2", "A3",
            "A4", "A5", "A6", "A7",
            "A8", "A9", "AA", "AB",
            "AC", "AD", "AE", "AF",

            "B0", "B1", "B2", "B3",
            "B4", "B5", "B6", "B7",
            "B8", "B9", "BA", "BB",
            "BC", "BD", "BE", "BF",

            "C0", "C1", "C2", "C3",
            "C4", "C5", "C6", "C7",
            "C8", "C9", "CA", "CB",
            "CC", "CD", "CE", "CF",

            "D0", "D1", "D2", "D3",
            "D4", "D5", "D6", "D7",
            "D8", "D9", "DA", "DB",
            "DC", "DD", "DE", "DF",

            "E0", "E1", "E2", "E3",
            "E4", "E5", "E6", "E7",
            "E8", "E9", "EA", "EB",
            "EC", "ED", "EE", "EF",

            "F0", "F1", "F2", "F3",
            "F4", "F5", "F6", "F7",
            "F8", "F9", "FA", "FB",
            "FC", "FD", "FE", "FF"
        };
        #endregion

        #region ISO 8859-1 字典
        public static string[] ISO8859 = new string[] {
            "00", "01", "02", "03",
            "9C", "09", "86", "7F",
            "97", "8D", "8E", "0B",
            "0C", "0D", "0E", "0F",

            "10", "11", "12", "13",
            "9D", "85", "08", "87",
            "18", "19", "92", "8F",
            "1C", "1D", "1E", "1F",

            "80", "81", "82", "83",
            "84", "0A", "17", "1B",
            "88", "89", "8A", "8B",
            "8C", "05", "06", "07",

            "90", "91", "16", "93",
            "94", "95", "96", "04",
            "98", "99", "9A", "9B",
            "14", "15", "9E", "1A",

            "20", "A0", "E2","E4",
            "E0", "E1", "E3", "E5",
            "E7", "F1", "A2", "2E",
            "3C", "28", "2B", "7C",

            "26", "E9", "EA", "EB",
            "E8", "ED", "EE", "EF",
            "EC", "DF", "21", "24",
            "2A", "29", "3B", "AC",

            "2D", "2F", "C2", "C4",
            "C0", "C1", "C3", "C5",
            "C7", "D1", "A6", "2C",
            "25", "5F", "3E", "3F",

            "F8", "C9", "CA", "CB",
            "C8", "CD", "CE", "CF",
            "CC", "60", "3A", "23",
            "40", "27", "3D", "22",

            "D8", "61", "62", "63",
            "64", "65", "66", "67",
            "68", "69", "AB", "BB",
            "F0", "FD", "FE", "B1",

            "B0", "6A", "6B", "6C",
            "6D", "6E", "6F", "70",
            "71", "72", "AA", "BA",
            "E6", "B8", "C6", "A4",

            "B5", "7E", "73", "74",
            "75", "76", "77", "78",
            "79", "7A", "A1", "BF",
            "D0", "DD", "DE", "AE",

            "5E", "A3", "A5", "B7",
            "A9", "A7", "B6", "BC",
            "BD", "BE", "5B", "5D",
            "AF", "A8", "B4", "D7",

            "7B", "41", "42", "43",
            "44", "45", "46", "47",
            "48", "49", "AD", "F4",
            "F6", "F3", "F3", "F5",

            "7D", "4A", "4B", "4C",
            "4D", "4E", "4F", "50",
            "51", "52", "B9", "FB",
            "FC", "F9", "FA", "FF",

            "5C", "F7", "53", "54",
            "55", "56", "57", "58",
            "59", "5A", "B2", "D4",
            "D6", "D2", "D3", "D5",

            "30", "31", "32", "33",
            "34", "35", "36", "37",
            "38", "39", "B3", "DB",
            "DC", "D9", "DA", "9F"
        };
        #endregion

        public static byte[] ebcdicToISOBytes(string[] str)
        {
            string[] a = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                string value = str[i];
                for (int k = 0; k < EBCDIC.Length; k++)
                {
                    if (value == EBCDIC[k])
                        a[i] = ISO8859[k];
                }
            }
            byte[] vBytes = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (!byte.TryParse(a[i], NumberStyles.HexNumber, null, out vBytes[i]))
                    vBytes[i] = 0;
            }
            return vBytes;
        }
        public static byte[] ISOToEbcdicBytes(string[] str)
        {
            string[] a = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                string value = str[i];
                for (int k = 0; k < ISO8859.Length; k++)
                {
                    if (value == ISO8859[k])
                        a[i] = EBCDIC[k];
                }
            }
            byte[] vBytes = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (!byte.TryParse(a[i], NumberStyles.HexNumber, null, out vBytes[i]))
                    vBytes[i] = 0;
            }
            return vBytes;
        }

        #endregion

        #region 将byte[]写入到txt文件中
        /// <summary>
        /// 将byte[]写入到txt文件中
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="bytes">参数byte[]</param>
        public void WriteBYByte(string path, byte[] bytes)
        {
            //创建一个文件流
            FileStream fs = new FileStream(path, FileMode.Create);             
            //将byte数组写入文件中
            fs.Write(bytes, 0, bytes.Length);
            //所有流类型都要关闭流，否则会出现内存泄露问题
            fs.Close();
        }
        #endregion

        #region 是否整数comboBox 切换value时改变小数位数

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comb_isInt.SelectedValue.ToString() == "true")
            {
                txt_integerPartnum.Text = "";
                txt_decimalsPartnum.Text = "0";
            }
            else
            {
                txt_integerPartnum.Text = "";
                txt_decimalsPartnum.Text = "";
            }
        }

        #endregion

        #region 

        public byte[] Hex500ToByte(string mHex) // 返回十六进制代表的字节数组 
        {
            mHex = mHex.Replace(" ", "");
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
            {
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            }
            byte[] v500Bytes = Encoding.GetEncoding(037).GetBytes(Encoding.GetEncoding(037).GetChars(vBytes));
            return v500Bytes;
        }

        #region EBCDIC ISO 8859-1字典
        //public static byte[] EBCDIC = new byte[] {
        //    (byte)0x00, (byte)0x01, (byte)0x02, (byte)0x03,
        //    (byte)0x04, (byte)0x05, (byte)0x06, (byte)0x07,
        //    (byte)0x08, (byte)0x09, (byte)0x0A, (byte)0x0B,
        //    (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F,

        //    (byte)0x10, (byte)0x11, (byte)0x12, (byte)0x13,
        //    (byte)0x14, (byte)0x15, (byte)0x16, (byte)0x17,
        //    (byte)0x18, (byte)0x19, (byte)0x1A, (byte)0x1B,
        //    (byte)0x1C, (byte)0x1D, (byte)0x1E, (byte)0x1F,

        //    (byte)0x20, (byte)0x21, (byte)0x22, (byte)0x23,
        //    (byte)0x24, (byte)0x25, (byte)0x26, (byte)0x27,
        //    (byte)0x28, (byte)0x29, (byte)0x2A, (byte)0x2B,
        //    (byte)0x2C, (byte)0x2D, (byte)0x2E, (byte)0x2F,

        //    (byte)0x30, (byte)0x31, (byte)0x32, (byte)0x33,
        //    (byte)0x34, (byte)0x35, (byte)0x36, (byte)0x37,
        //    (byte)0x38, (byte)0x39, (byte)0x3A, (byte)0x3B,
        //    (byte)0x3C, (byte)0x3D, (byte)0x3E, (byte)0x3F,

        //    (byte)0x40, (byte)0x41, (byte)0x42,(byte)0x43,
        //    (byte)0x44, (byte)0x45, (byte)0x46, (byte)0x47,
        //    (byte)0x48, (byte)0x49, (byte)0x4A, (byte)0x4B,
        //    (byte)0x4C, (byte)0x4D, (byte)0x4E, (byte)0x4F,

        //    (byte)0x50, (byte)0x51, (byte)0x52, (byte)0x53,
        //    (byte)0x54, (byte)0x55, (byte)0x56, (byte)0x57,
        //    (byte)0x58, (byte)0x59, (byte)0x5A, (byte)0x5B,
        //    (byte)0x5C, (byte)0x5D, (byte)0x5E, (byte)0x5F,

        //    (byte)0x60, (byte)0x61, (byte)0x62, (byte)0x63,
        //    (byte)0x64, (byte)0x65, (byte)0x66, (byte)0x67,
        //    (byte)0x68, (byte)0x69, (byte)0x6A, (byte)0x6B,
        //    (byte)0x6C, (byte)0x6D, (byte)0x6E, (byte)0x6F,

        //    (byte)0x70, (byte)0x71, (byte)0x72, (byte)0x73,
        //    (byte)0x74, (byte)0x75, (byte)0x76, (byte)0x77,
        //    (byte)0x78, (byte)0x79, (byte)0x7A, (byte)0x7B,
        //    (byte)0x7C, (byte)0x7D, (byte)0x7E, (byte)0x7F,

        //    (byte)0x80, (byte)0x81, (byte)0x82, (byte)0x83,
        //    (byte)0x84, (byte)0x85, (byte)0x86, (byte)0x87,
        //    (byte)0x88, (byte)0x89, (byte)0x8A, (byte)0x8B,
        //    (byte)0x8C, (byte)0x8D, (byte)0x8E, (byte)0x8F,

        //    (byte)0x90, (byte)0x91, (byte)0x92, (byte)0x93,
        //    (byte)0x94, (byte)0x95, (byte)0x96, (byte)0x97,
        //    (byte)0x98, (byte)0x99, (byte)0x9A, (byte)0x9B,
        //    (byte)0x9C, (byte)0x9D, (byte)0x9E, (byte)0x9F,

        //    (byte)0xA0, (byte)0xA1, (byte)0xA2, (byte)0xA3,
        //    (byte)0xA4, (byte)0xA5, (byte)0xA6, (byte)0xA7,
        //    (byte)0xA8, (byte)0xA9, (byte)0xAA, (byte)0xAB,
        //    (byte)0xAC, (byte)0xAD, (byte)0xAE, (byte)0xAF,

        //    (byte)0xB0, (byte)0xB1, (byte)0xB2, (byte)0xB3,
        //    (byte)0xB4, (byte)0xB5, (byte)0xB6, (byte)0xB7,
        //    (byte)0xB8, (byte)0xB9, (byte)0xBA, (byte)0xBB,
        //    (byte)0xBC, (byte)0xBD, (byte)0xBE, (byte)0xBF,

        //    (byte)0xC0, (byte)0xC1, (byte)0xC2, (byte)0xC3,
        //    (byte)0xC4, (byte)0xC5, (byte)0xC6, (byte)0xC7,
        //    (byte)0xC8, (byte)0xC9, (byte)0xCA, (byte)0xCB,
        //    (byte)0xCC, (byte)0xCD, (byte)0xCE, (byte)0xCF,

        //    (byte)0xD0, (byte)0xD1, (byte)0xD2, (byte)0xD3,
        //    (byte)0xD4, (byte)0xD5, (byte)0xD6, (byte)0xD7,
        //    (byte)0xD8, (byte)0xD9, (byte)0xDA, (byte)0xDB,
        //    (byte)0xDC, (byte)0xDD, (byte)0xDE, (byte)0xDF,

        //    (byte)0xE0, (byte)0xE1, (byte)0xE2, (byte)0xE3,
        //    (byte)0xE4, (byte)0xE5, (byte)0xE6, (byte)0xE7,
        //    (byte)0xE8, (byte)0xE9, (byte)0xEA, (byte)0xEB,
        //    (byte)0xEC, (byte)0xED, (byte)0xEE, (byte)0xEF,

        //    (byte)0xF0, (byte)0xF1, (byte)0xF2, (byte)0xF3,
        //    (byte)0xF4, (byte)0xF5, (byte)0xF6, (byte)0xF7,
        //    (byte)0xF8, (byte)0xF9, (byte)0xFA, (byte)0xFB,
        //    (byte)0xFC, (byte)0xFD, (byte)0xFE, (byte)0xFF
        //};
        //public static byte[] ISO8859 = new byte[] {
        //    (byte)0x00, (byte)0x01, (byte)0x02, (byte)0x03,
        //    (byte)0x9C, (byte)0x09, (byte)0x86, (byte)0x7F,
        //    (byte)0x97, (byte)0x8D, (byte)0x8E, (byte)0x0B,
        //    (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F,

        //    (byte)0x10, (byte)0x11, (byte)0x12, (byte)0x13,
        //    (byte)0x9D, (byte)0x85, (byte)0x08, (byte)0x87,
        //    (byte)0x18, (byte)0x19, (byte)0x92, (byte)0x8F,
        //    (byte)0x1C, (byte)0x1D, (byte)0x1E, (byte)0x1F,

        //    (byte)0x80, (byte)0x81, (byte)0x82, (byte)0x83,
        //    (byte)0x84, (byte)0x0A, (byte)0x17, (byte)0x1B,
        //    (byte)0x88, (byte)0x89, (byte)0x8A, (byte)0x8B,
        //    (byte)0x8C, (byte)0x05, (byte)0x06, (byte)0x07,

        //    (byte)0x90, (byte)0x91, (byte)0x16, (byte)0x93,
        //    (byte)0x94, (byte)0x95, (byte)0x96, (byte)0x04,
        //    (byte)0x98, (byte)0x99, (byte)0x9A, (byte)0x9B,
        //    (byte)0x14, (byte)0x15, (byte)0x9E, (byte)0x1A,

        //    (byte)0x20, (byte)0xA0, (byte)0xE2,(byte)0xE4,
        //    (byte)0xE0, (byte)0xE1, (byte)0xE3, (byte)0xE5,
        //    (byte)0xE7, (byte)0xF1, (byte)0xA2, (byte)0x2E,
        //    (byte)0x3C, (byte)0x28, (byte)0x2B, (byte)0x7C,

        //    (byte)0x26, (byte)0xE9, (byte)0xEA, (byte)0xEB,
        //    (byte)0xE8, (byte)0xED, (byte)0xEE, (byte)0xEF,
        //    (byte)0xEC, (byte)0xDF, (byte)0x21, (byte)0x24,
        //    (byte)0x2A, (byte)0x29, (byte)0x3B, (byte)0xAC,

        //    (byte)0x2D, (byte)0x2F, (byte)0xC2, (byte)0xC4,
        //    (byte)0xC0, (byte)0xC1, (byte)0xC3, (byte)0xC5,
        //    (byte)0xC7, (byte)0xD1, (byte)0xA6, (byte)0x2C,
        //    (byte)0x25, (byte)0x5F, (byte)0x3E, (byte)0x3F,

        //    (byte)0xF8, (byte)0xC9, (byte)0xCA, (byte)0xCB,
        //    (byte)0xC8, (byte)0xCD, (byte)0xCE, (byte)0xCF,
        //    (byte)0xCC, (byte)0x60, (byte)0x3A, (byte)0x23,
        //    (byte)0x40, (byte)0x27, (byte)0x3D, (byte)0x22,

        //    (byte)0xD8, (byte)0x61, (byte)0x62, (byte)0x63,
        //    (byte)0x64, (byte)0x65, (byte)0x66, (byte)0x67,
        //    (byte)0x68, (byte)0x69, (byte)0xAB, (byte)0xBB,
        //    (byte)0xF0, (byte)0xFD, (byte)0xFE, (byte)0xB1,

        //    (byte)0xB0, (byte)0x6A, (byte)0x6B, (byte)0x6C,
        //    (byte)0x6D, (byte)0x6E, (byte)0x6F, (byte)0x70,
        //    (byte)0x71, (byte)0x72, (byte)0xAA, (byte)0xBA,
        //    (byte)0xE6, (byte)0xB8, (byte)0xC6, (byte)0xA4,

        //    (byte)0xB5, (byte)0x7E, (byte)0x73, (byte)0x74,
        //    (byte)0x75, (byte)0x76, (byte)0x77, (byte)0x78,
        //    (byte)0x79, (byte)0x7A, (byte)0xA1, (byte)0xBF,
        //    (byte)0xD0, (byte)0xDD, (byte)0xDE, (byte)0xAE,

        //    (byte)0x5E, (byte)0xA3, (byte)0xA5, (byte)0xB7,
        //    (byte)0xA9, (byte)0xA7, (byte)0xB6, (byte)0xBC,
        //    (byte)0xBD, (byte)0xBE, (byte)0x5B, (byte)0x5D,
        //    (byte)0xAF, (byte)0xA8, (byte)0xB4, (byte)0xD7,

        //    (byte)0x7B, (byte)0x41, (byte)0x42, (byte)0x43,
        //    (byte)0x44, (byte)0x45, (byte)0x46, (byte)0x47,
        //    (byte)0x48, (byte)0x49, (byte)0xAD, (byte)0xF4,
        //    (byte)0xF6, (byte)0xF3, (byte)0xF3, (byte)0xF5,

        //    (byte)0x7D, (byte)0x4A, (byte)0x4B, (byte)0x4C,
        //    (byte)0x4D, (byte)0x4E, (byte)0x4F, (byte)0x50,
        //    (byte)0x51, (byte)0x52, (byte)0xB9, (byte)0xFB,
        //    (byte)0xFC, (byte)0xF9, (byte)0xFA, (byte)0xFF,

        //    (byte)0x5C, (byte)0xF7, (byte)0x53, (byte)0x54,
        //    (byte)0x55, (byte)0x56, (byte)0x57, (byte)0x58,
        //    (byte)0x59, (byte)0x5A, (byte)0xB2, (byte)0xD4,
        //    (byte)0xD6, (byte)0xD2, (byte)0xD3, (byte)0xD5,

        //    (byte)0x30, (byte)0x31, (byte)0x32, (byte)0x33,
        //    (byte)0x34, (byte)0x35, (byte)0x36, (byte)0x37,
        //    (byte)0x38, (byte)0x39, (byte)0xB3, (byte)0xDB,
        //    (byte)0xDC, (byte)0xD9, (byte)0xDA, (byte)0x9F
        //};
        #endregion

        #region 数据类型正则表达式
        public static bool IsUnsignPositiveInt(string value)  //正整数 
        {
            return Regex.IsMatch(value, @"^[0-9]*[1-9][0-9]*$");
        }
        public static bool IsUnsignNegativeInt(string value)  //负整数 
        {
            return Regex.IsMatch(value, @"^-[0-9]*[1-9][0-9]*$");
        }
        public static bool IsUnsignPositiveFloat(string value)  //正浮点数
        {
            //return Regex.IsMatch(value, @"^/d*[.]?/d*$");
            return Regex.IsMatch(value, @"^(([0-9] +\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
        }
        public static bool IsUnsignNegativeFloat(string value)  //负浮点数
        {
            //return Regex.IsMatch(value, @"^/d*[.]?/d*$");
            return Regex.IsMatch(value, @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$");
        }
        public static bool IsNumeric(string value)  //小数
        {
            //return Regex.IsMatch(value, @"^[+-]?/d*[.]?/d*$");
            return Regex.IsMatch(value, "^([0-9]{1,}[.][0-9]*)$");
        }
        public static bool IsInt(string value)
        {
            //return Regex.IsMatch(value, @"^[+-]?/d*$");
            return Regex.IsMatch(value, @"^([0-9]{1,})$");
        }
        #endregion

        public static string HignAndLowConvert(string str)
        {
            ArrayList marray = new ArrayList();
            ArrayList narray = new ArrayList();
            foreach (char s in str)
                marray.Add(s.ToString());
            string tempstr1 = "";
            foreach (string array in marray)
                tempstr1 += array;
            for (int counter = 1; counter <= marray.Count; counter += 2)
            {
                narray[counter] = marray[counter + 1];
                narray[counter + 1] = marray[counter];
            }
            foreach (string array in narray)
                tempstr1 += array;
            return tempstr1;
            //ArrayList marray = new ArrayList();
            //ArrayList narray = new ArrayList();
            //for (int counter = 1; counter <= str.Length; counter++)
            //{
            //    if (counter == 1)
            //        marray.Add(str[0]);
            //    else if (counter % 2 == 0)
            //        narray.Add(str[counter - 1]);
            //    else
            //        marray.Add(str[counter - 1]);
            //}

            //string tempstr1 = "";
            //string tempstr2 = "";
            //foreach (char array in marray)
            //    tempstr1 += array.ToString();

            //foreach (char array in narray)
            //    tempstr2 += array.ToString();
            //return tempstr1 + "\r\n" + tempstr2;
        }
        public string FromHexToDecimalism(string inputstr)  //无符号十六进制小数转换为十进制
        {
            //这个字典是为了遇到16进制到10进制转换时遇到字母准备的
            //如果是字母，那么运行字典转换成十进制数
            Dictionary<char, int> tableOf16_10 = new Dictionary<char, int>();
            tableOf16_10.Add('a', 10);
            tableOf16_10.Add('b', 11);
            tableOf16_10.Add('c', 12);
            tableOf16_10.Add('d', 13);
            tableOf16_10.Add('e', 14);
            tableOf16_10.Add('f', 15);

            string[] values = inputstr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //存储小数部分的字符串
            string decimalsPart = values[1];
            //存储整数部分的字符串
            string integerPart = values[0];
            //小数部分出来的10进制数字一定是double类型的
            double sumOfDecimalsPart = 0.0;

            char temp;
            for (int i = 0; i < decimalsPart.Length; i++)
            {
                //decimalsPart[i]是char类型，所以要加一个tostring
                //在算法设计的初级阶段，不要写出一个有多个变量的长式子，
                //要把式子的每个变量都用单独命名，然后调试的时候可以检测到是哪一个变量出了问题
                //int itemp = Convert.ToInt32(decimalsPart[i].ToString());

                temp = decimalsPart[i];

                //判断decimalsPart[i]是否是字母
                if (char.IsLetter(temp))
                {
                    if (char.IsUpper(temp))
                    {
                        temp = Convert.ToChar(Convert.ToInt32(temp) + 32);

                    }
                    //如果是字母，就用字典来转换
                    sumOfDecimalsPart += Convert.ToDouble(tableOf16_10[temp] * Math.Pow(16, -(i + 1)));
                }
                else
                {
                    //如果不是字母正常对待
                    sumOfDecimalsPart += Convert.ToDouble(Convert.ToInt32(decimalsPart[i].ToString()) * Math.Pow(16, -(i + 1)));
                }
            }

            int sumOfintegerPart = 0;
            for (int i = 0; i < integerPart.Length; i++)
            {
                //decimalsPart[i]是char类型，所以要加一个tostring
                //在算法设计的初级阶段，不要写出一个有多个变量的长式子，
                //要把式子的每个变量都用单独命名，然后调试的时候可以检测到是哪一个变量出了问题
                //int itemp = Convert.ToInt32(decimalsPart[i].ToString());
                temp = integerPart[i];
                //integerPart[i]是否是字母
                if (char.IsLetter(temp))
                {
                    if (char.IsUpper(temp))
                    {
                        temp = Convert.ToChar(Convert.ToInt32(temp) + 32);

                    }
                    //如果是字母，先转换成小写字母，再用字典来转换
                    sumOfintegerPart += Convert.ToInt32(tableOf16_10[char.ToLower(temp)] * Math.Pow(16, i));
                }
                else
                {
                    //如果不是字母正常对待
                    sumOfintegerPart += Convert.ToInt32(Convert.ToInt32(integerPart[i].ToString()) * Math.Pow(16, i));
                }
            }

            string result = sumOfintegerPart + sumOfDecimalsPart.ToString().Substring(1);
            return result;
        }
                
        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /**/
        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {

            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name=”hexString”></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public string StrToHex(string str) // 返回十六进制代表的字符串 
        {
            str = str.Replace(" ", "");
            if (str.Length <= 0) return "";
            byte[] vBytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2)
                if (!byte.TryParse(str.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        }

        public string HexToStr(string mHex) // 返回十六进制代表的字符串 
        {
            mHex = mHex.Replace(" ", "");
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        }

        public byte[] HexToByte(string mHex) // 返回十六进制代表的字节数组 
        {
            mHex = mHex.Replace(" ", "");
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return vBytes;
        }
        
        public void WriteBYTxt(string path,string txt)
        {
            //表示清空 txt
            StreamWriter sw = new StreamWriter(path);
            sw.Write("");
            sw.Close();
            //表示向txt写入文本
            sw = new StreamWriter(path);
            sw.Write(txt);
            sw.Close();

            //表示向txt写入文本
            //StreamWriter sw = new StreamWriter(path);
            //string w = "10";
            //sw.Write(w);
            //sw.Close();
            ////表示追加文本
            //StreamWriter sw = File.AppendText(path);
            //string w = "2";
            //sw.Write(w);
            //sw.Close();
        }

        /// <summary>
        /// 将16进制数据字符串，接两位两位进行10进制转，
        /// 各数据之间用分号隔开
        /// </summary>
        /// <param name="str">16进制数据字符串</param>
        /// <returns>返回两位两位进行分割的10进制结果（数之间用分号）</returns>
        private string get16from10(string str)
        {
            string myStr = string.Empty;
            string mytp1 = string.Empty;
            string mytp2 = string.Empty;

            ArrayList myAr = new ArrayList();

            //判断长度，若为奇位，则对字符串第一位进行处理
            if ((str.Length % 2) != 0)
            {
                mytp1 = str.Substring(0, 1);
                mytp1 = (Convert.ToInt32(mytp1, 16)).ToString();
            }

            //对偶数位字符串进行处理
            for (int i = str.Trim().Length - 2; i >= 0; i = i - 2)
            {
                mytp2 = str.Substring(i, 2);
                mytp2 = (Convert.ToInt32(mytp2, 16)).ToString();
                myStr = mytp2 + ";" + myStr;
            }
            myStr = mytp1 + ";" + myStr;

            //最后，对结果串中多余分号进行删除。
            if ((str.Length % 2) != 0)
            {
                myStr = myStr.Substring(0, myStr.Length - 1);
            }
            else
            {
                myStr = myStr.Substring(1, myStr.Length - 2);
            }

            return myStr;
        }

        #endregion
        
    }

    class MyData
    {
        BitArray bits;
        public MyData(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 9) throw new ArgumentException("must be an array of 8 bytes");

            //Array.Reverse(bytes);
            this.bits = new BitArray(bytes);
        }

        public int ObjectId
        {
            get { return GetValue(0, 6); }  // 6bit 对象ID 
        }

        public float ObjectLength
        {
            get { return GetValue(6, 8) * 0.2f; } // 8bit; 单位0.2 对象长度
        }
        public float VelocityY
        {
            get { return (GetValue(14, 11) - 1024) * 0.1f; } // 11bit，偏移1024; 单位0.1米/秒 速度Y坐标
        }
        public float VelocityX
        {
            get { return (GetValue(25, 11) - 1024) * 0.1f; } // 11bit，偏移1024; 单位0.1米/秒 速度X坐标
        }
        public float RangeY
        {
            get { return (GetValue(36, 14) - 8192) * 0.032f; } // 14bit，偏移8192; 单位0.032米 范围Y坐标
        }
        public float RangeX
        {
            get { return (GetValue(50, 14) - 8192) * 0.032f; } // 14bit，偏移8192; 单位0.032米 范围X坐标
        }

        private int GetValue(int startBit, int length)
        {
            int value = 0;
            for (int i = 0; i < length; i++)
            {
                value = value + value + (bits[startBit + i] ? 1 : 0);
            }
            return value;
        }
    }
}
