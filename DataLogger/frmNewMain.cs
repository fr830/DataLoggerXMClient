using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using DataLogger.Entities;
using System.Globalization;
using DataLogger.Data;
using System.Diagnostics;
using System.IO;
using Excel = ClosedXML.Excel;
//Microsoft.Office.Interop.Excel;
using System.Reflection;
using DataLogger.Utils;
using System.Resources;
using System.Net.Sockets;
using System.Net;
using WinformProtocol;
using Npgsql;

namespace DataLogger
{
    public partial class frmNewMain : Form
    {
        LanguageService lang = new LanguageService(typeof(frmNewMain).Assembly);
        public static string language_code = "en";

        public bool is_close_form = false;

        public static TcpListener tcpListener = null;
        public static DateTime datetime00;

        private System.Threading.Timer tmrThreadingTimerForFTP;
        private System.Threading.Timer tmrThreadingTimerForUpdateUI;

        public const int TRANSACTION_ADD_NEW = 1;
        public const int TRANSACTION_UPDATE = 2;

        // global dataValue
        int countingRequest = 0;
        public int firstTimeForIOControl = 0;

        public static measured_data objMeasuredDataGlobal = new measured_data();

        data_value obj5MinuteDataValue = new data_value();
        data_value obj60MinuteDataValue = new data_value();

        // delegate used for Invoke
        internal delegate void StringDelegate(string data);
        internal delegate void HeadingTimerDelegate(string data);
        private delegate void ProcessDataCallback(string text);
        internal delegate void SetHeadingLoginNameDelegate(string data);


        private readonly data_5minute_value_repository db5m = new data_5minute_value_repository();
        private readonly data_60minute_value_repository db60m = new data_60minute_value_repository();
        private readonly maintenance_log_repository _maintenance_logs = new maintenance_log_repository();

        public static Form1 protocol;
        #region Form event
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        public frmNewMain()
        {
            InitializeComponent();
        }
        private void frmNewMain_Load(object sender, EventArgs e)
        {      

            GlobalVar.maintenanceLog = new maintenance_log();

            frmConfiguration.protocol = new Form1(this);

            backgroundWorkerMain.RunWorkerAsync();

            initUserInterface();
            initConfig();
            tmrThreadingTimerForFTP = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerForFTP_TimerCallback), null, 1000 * 60, Timeout.Infinite);
            tmrThreadingTimerForFTP.Change(0, 1000 * 60 * 60 * 2);

            tmrThreadingTimerForUpdateUI = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerForUpdateUI_TimerCallback), null, 1000 * 60 * 5, Timeout.Infinite);
            tmrThreadingTimerForUpdateUI.Change(1000, 1000 * 60 * 5);

            

        }
        private void initConfig()
        {
            GlobalVar.stationSettings = new station_repository().get_info();
            GlobalVar.moduleSettings = new module_repository().get_all();

            label9.Text = Convert.ToString(GlobalVar.stationSettings.station_name);

            for (int i = 1; i <= GlobalVar.moduleSettings.Count(); i++)
            {
                foreach (var item in GlobalVar.moduleSettings)
                {
                    string currentvar = "var" + i.ToString();
                    string currentlabelname = "txt" + currentvar;
                    string currentlabelunit = "txt" + currentvar + "Unit";
                    string currentlabelvalue = "txt" + currentvar + "Value";
                    if (item.item_name.Equals(currentvar))
                    {
                       
                        ClearLabel(this, item.display_name, currentlabelname);
                        ClearLabel(this, item.unit, currentlabelunit);
                        try
                        {
                            ClearTextbox(this, "---", currentlabelvalue);
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                tcpListener.Stop();
            }
            catch (Exception ex)
            {
            }
            this.Close();
        }
        #endregion
        private void backgroundWorkerMain_DoWork(object sender, DoWorkEventArgs e)
        {
            station existedStationsSetting = new station_repository().get_info();
            if (existedStationsSetting == null)
            {

            }
            else
            {


            }
        }
        #region initial method
        private void initUserInterface()
        {

            switch_language();
        }
        private void switch_language()
        {
            lang.nextLanguage();
            switch (lang.CurrentLanguage.Language)
            {
                case ELanguage.English:
                    language_code = "en";
                    this.btnMonthlyReport.BackgroundImage = global::DataLogger.Properties.Resources.MonthlyReportButton;
                    break;
                case ELanguage.Vietnamese:
                    language_code = "vi";
                    this.btnMonthlyReport.BackgroundImage = global::DataLogger.Properties.Resources.MonthlyReportButton;
                    break;
                default:
                    break;
            }
            this.btnLanguage.BackgroundImage = lang.CurrentLanguage.Icon;
            // heading menu
            lang.setText(lblHeaderNationName, "main_menu_language");
            lang.setText(lblMainMenuTitle, "main_menu_title");
            settingForLoginStatus();
            // left menu buttong
            lang.setText(lblThaiNguyenStation, "thai_nguyen_station_text", EAlign.Center);
            lang.setText(lblAutomaticMonitoring, "automatic_monitoring_text", EAlign.Center);
            lang.setText(lblSurfaceWaterQuality, "surface_water_quality_text", EAlign.Center);
            // control panel
            lang.setText(this, "data_logger_system");
        }
        #endregion

        #region ComPort Process data
        private void setText(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(setText);
                this.txtData.Invoke(d, new object[] { text });
            }
            else
            {
                txtData.Text = text;
            }
        }
        private void setTextHeadingTimer(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                HeadingTimerDelegate d = new HeadingTimerDelegate(setTextHeadingTimer);
                this.lblHeadingTime.Invoke(d, new object[] { text });
            }
            else
            {
                lblHeadingTime.Text = text;
            }
        }
        private void setTextHeadingLogin(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                SetHeadingLoginNameDelegate d = new SetHeadingLoginNameDelegate(setTextHeadingLogin);
                this.lblLoginDisplayName.Invoke(d, new object[] { text });
            }
            else
            {
                lblLoginDisplayName.Text = text;
            }
        }
        public static ASCIIEncoding _encoder = new ASCIIEncoding();
        public void writeLog(string content, string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    File.Create(filename);
                }

                TextWriter twr = new StreamWriter(filename, true);
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                twr.Write(dt.ToString() + " : ");
                twr.WriteLine(content);
                twr.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: -" + ex.Message);
            }
        }
        public static string StringToByteArray(string hexstring)
        {
            return String.Join(String.Empty, hexstring
                .Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }
        #endregion
        #region threading timer
        public int indexSelection = 0;
        public int indexSelectionStation = 0;
        private void tmrThreadingTimerForUpdateUI_TimerCallback(object state)
        {
            try
            {
                data_value obj = new data_5minute_value_repository().get_latest_info();
                GlobalVar.moduleSettings = new module_repository().get_all();

                for (int i = 1; i <= GlobalVar.moduleSettings.Count(); i++)
                {
                    foreach (var item in GlobalVar.moduleSettings)
                    {
                        string currentvar = "var" + i.ToString();
                        string currentlabelvalue = "txt" + currentvar + "Value";

                        string param = currentvar;  //VD : var1
                        Type paramType = typeof(data_value);
                        PropertyInfo prop = paramType.GetProperty(param);
                        double value = (double)prop.GetValue(obj); //VD : lay display_var = data.var1

                        if (item.item_name.Equals(currentvar))
                        {
                            SetValueTextbox(this, value , currentlabelvalue);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private void tmrThreadingTimerForFTP_TimerCallback(object state)
        {
            try
            {
                setting_repository s = new setting_repository();
                int id = s.get_id_by_key("lasted_push");
                DateTime lastedPush = s.get_datetime_by_id(id);

                GlobalVar.stationSettings = new station_repository().get_info();
                string username = GlobalVar.stationSettings.ftpusername;
                string pwd = GlobalVar.stationSettings.ftppassword;
                string folder = GlobalVar.stationSettings.ftpfolder;
                string server = GlobalVar.stationSettings.ftpserver;
                int flag = GlobalVar.stationSettings.ftpflag;
                if (GlobalVar.stationSettings != null)
                {
                    if (flag == 1)
                    {
                        if (Application.OpenForms.OfType<Form1>().Count() == 1)
                        {
                        }
                        LoadFTP(server, username, pwd, folder, lastedPush);
                        Form1.control1.ClearTextBox(Form1.control1.getForm1fromControl, 1);
                    }
                    else
                    {
                        Form1.control1.ClearTextBox(Form1.control1.getForm1fromControl, 1);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region Utility
        public string HEX_Coding(string aHex)
        {
            switch (aHex)
            {
                case "A":
                    return ":";

                case "B":
                    return ";";

                case "C":
                    return "<";

                case "D":
                    return "=";

                case "E":
                    return ">";

                case "F":
                    return "?";
            }
            return aHex;
        }
        private string Checksum(byte[] ByteArray)
        {
            int num = 0;
            int num2 = ByteArray.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                num += ByteArray[i];
            }
            num = num % 0x100;
            return (this.HEX_Coding(((int)(num / 0x10)).ToString("X")) + this.HEX_Coding(((int)(num % 0x10)).ToString("X")));
        }
        public static void SetValueTextbox(System.Windows.Forms.Control control, double text, string label)
        {
            if (control is TextBox)
            {
                TextBox tb = (TextBox)control;
                if (tb.Name.StartsWith(label))
                {
                    if (tb.InvokeRequired)
                    {
                        tb.Invoke(new MethodInvoker(delegate { tb.Text = text.ToString("##0.00"); }));
                    }
                }

            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    SetValueTextbox(child, text, label);
                }
            }

        }
        public static void ClearLabel(System.Windows.Forms.Control control, string text, string label)
        {
            if (control is Label)
            {
                Label lbl = (Label)control;
                if (lbl.Name.StartsWith(label))
                    lbl.Text = text;

            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    ClearLabel(child, text, label);
                }
            }

        }
        public static void ClearTextbox(System.Windows.Forms.Control control, string text, string label)
        {
            if (control is TextBox)
            {
                TextBox tb = (TextBox)control;
                if (tb.Name.StartsWith(label))
                    tb.Text = text;

            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    ClearTextbox(child, text, label);
                }
            }

        }
        private static int getMinValueFromDatabinding(string code)
        {
            try
            {
                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();
                using (NpgsqlDBConnection db = new NpgsqlDBConnection())
                {
                    if (db.open_connection())
                    {
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            string sql_command1 = "SELECT * from " + "databinding";
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            int min_value = -1;
                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                if (Convert.ToString(row2["code"]).Equals(code))
                                {
                                    min_value = Convert.ToInt32(row2["min_value"]);
                                    break;
                                }
                            }
                            db.close_connection();
                            return min_value;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return -1;
            }
        }
        private static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            try
            {
                foreach (byte b in data)
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + "");
            }
            catch (Exception)
            {
                return "Error";
            }
            return sb.ToString().ToUpper();
        }
        private static Single ConvertHexToSingle(string hexVal)
        {
            try
            {
                int i = 0, j = 0;
                byte[] bArray = new byte[4];


                for (i = 0; i <= hexVal.Length - 1; i += 2)
                {
                    bArray[j] = Byte.Parse(hexVal[i].ToString() + hexVal[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                    j += 1;
                }
                Array.Reverse(bArray);
                Single s = BitConverter.ToSingle(bArray, 0);
                return (s);
            }
            catch (Exception ex)
            {
                throw new FormatException("The supplied hex value is either empty or in an incorrect format. Use the " +
                "following format: 00000000", ex);
            }
        }
        public static byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        private static byte[] Combine(byte[] first, int first_length, byte[] second)
        {
            byte[] ret = new byte[first_length + second.Length];
            try
            {
                Buffer.BlockCopy(first, 0, ret, 0, first_length);
                Buffer.BlockCopy(second, 0, ret, first_length, second.Length);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("0003," + ex.Message);
            }


            return ret;
        }
        public Boolean iSAllMinValue(data_value data)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command1 = "SELECT * from " + "databinding";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            int countNull = 0;
                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                string code = Convert.ToString(row2["code"]);
                                int min_value = Convert.ToInt32(row2["min_value"]);
                                switch (code)
                                {
                                    case "var1":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var1)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var1)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var2":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var2)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var2)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var3":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var3)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var3)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var4":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var4)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var4)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var5":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var5)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var5)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var6":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var6)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var6)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var7":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var7)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var7)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var8":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var8)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var8)) != -1)
                                        {
                                        }
                                        else 
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var9":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var9)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var9)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var10":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var10)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var10)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var11":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var11)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var11)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var12":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var12)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var12)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var13":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var13)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var13)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var14":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var14)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var14)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var15":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var15)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var15)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var16":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var16)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var16)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var17":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var17)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var17)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var18":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var18)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var18)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                }
                            }
                            if (countNull >= tbcode.Rows.Count)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                            db.close_connection();
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return false;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.StackTrace);
                    return false;
                }
            }
        }
        string[] a = null;
        string b = null;
        public void LoadFTP(string server, string username, string pwd, string folder, DateTime lastedPush)
        {
            //ftp ftpClient = new ftp(server, createdFile, pwd);
            try
            {
                obj5MinuteDataValue = new data_value();
                string folderPath = folder;
                string[] filePaths = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                station existedStationsSetting = new station_repository().get_info();
                data_value datavalue = obj5MinuteDataValue;
                DateTime created;
                string datetime = null;
                foreach (string textPath in filePaths)
                {
                    datavalue = new data_value();
                    DateTime createdFile = File.GetCreationTime(textPath);
                    int result = DateTime.Compare(createdFile, lastedPush);
                    if (
                        result >= 0
                        //result < 0
                        )
                    {
                        string[] lines = System.IO.File.ReadAllLines(textPath);
                        string stationID = existedStationsSetting.station_id;
                        string[] separators1 = { "_" };
                        a = lines;
                        b = textPath;
                        string[] lines0 = lines[0].Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        if (lines0[0].Substring(0,10).Equals(stationID))
                        {
                            List<string> custom_param_list = new List<string>();
                            lines = lines.Skip(1).ToArray();
                            foreach (string line in lines)
                            {
                                string[] separators2 = { "\t" };
                                string[] param = line.Split(separators2, StringSplitOptions.RemoveEmptyEntries);

                                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                                //conn.Open();
                                //using (NpgsqlCommand cmd = conn.CreateCommand())
                                //{
                                //    string sql_command1 = "SELECT * from " + "databinding";
                                //    cmd.CommandText = sql_command1;
                                //    NpgsqlDataReader dr = cmd.ExecuteReader();
                                //    DataTable tbcode = new DataTable();
                                //    byte[] databyte = new byte[1024];
                                //    tbcode.Load(dr); // Load bang chua mapping cac truong

                                //    foreach (DataRow row in tbcode.Rows)
                                //    {
                                //        string code = Convert.ToString(row["code"]);


                                //    }
                                //}
                                string formatString = "yyyyMMddHHmm";
                                string dt = param[0].Substring(0, 12);
                                datetime = dt;
                                datavalue.created = DateTime.ParseExact(dt, formatString, null);
                                datavalue.stored_date = datavalue.created.Date;
                                datavalue.stored_hour = datavalue.created.Hour;
                                datavalue.stored_minute = datavalue.created.Minute;

                                switch (param[1].ToLower())
                                {
                                    case "var1":
                                        datavalue.var1 = Convert.ToDouble(param[2]);
                                        datavalue.var1_status = 0;
                                        custom_param_list.Add("var1");
                                        break;
                                    case "var2":
                                        datavalue.var2 = Convert.ToDouble(param[2]);
                                        datavalue.var2_status = 0;
                                        break;
                                        custom_param_list.Add("var2");
                                    case "var3":
                                        datavalue.var3 = Convert.ToDouble(param[2]);
                                        datavalue.var3_status = 0;
                                        custom_param_list.Add("var3");
                                        break;
                                    case "var4":
                                        datavalue.var4 = Convert.ToDouble(param[2]);
                                        datavalue.var4_status = 0;
                                        custom_param_list.Add("var4");
                                        break;
                                    case "var5":
                                        datavalue.var5 = Convert.ToDouble(param[2]);
                                        datavalue.var5_status = 0;
                                        custom_param_list.Add("var5");
                                        break;
                                    case "var6":
                                        datavalue.var6 = Convert.ToDouble(param[2]);
                                        datavalue.var6_status= 0;
                                        custom_param_list.Add("var6");
                                        break;
                                    case "var7":
                                        datavalue.var7 = Convert.ToDouble(param[2]);
                                        datavalue.var7_status = 0;
                                        custom_param_list.Add("var7");
                                        break;
                                    case "var8":
                                        datavalue.var8 = Convert.ToDouble(param[2]);
                                        datavalue.var8_status = 0;
                                        custom_param_list.Add("var8");
                                        break;
                                    case "var9":
                                        datavalue.var9 = Convert.ToDouble(param[2]);
                                        datavalue.var9_status = 0;
                                        custom_param_list.Add("var9");
                                        break;                                        
                                    case "var10":
                                        datavalue.var10 = Convert.ToDouble(param[2]);
                                        datavalue.var10_status = 0;
                                        custom_param_list.Add("var10");
                                        break;
                                    case "var11":
                                        datavalue.var11 = Convert.ToDouble(param[2]);
                                        datavalue.var11_status = 0;
                                        custom_param_list.Add("var11");
                                        break;
                                    case "var12":
                                        datavalue.var12 = Convert.ToDouble(param[2]);
                                        datavalue.var12_status = 0;
                                        custom_param_list.Add("var12");
                                        break;
                                    case "var13":
                                        datavalue.var13 = Convert.ToDouble(param[2]);
                                        datavalue.var13_status = 0;
                                        custom_param_list.Add("var13");
                                        break;
                                    case "var14":
                                        datavalue.var14 = Convert.ToDouble(param[2]);
                                        datavalue.var14_status = 0;
                                        custom_param_list.Add("var14");
                                        break;
                                    case "var15":
                                        datavalue.var15 = Convert.ToDouble(param[2]);
                                        datavalue.var15_status = 0;
                                        custom_param_list.Add("var15");
                                        break;
                                    case "var16":
                                        datavalue.var16 = Convert.ToDouble(param[2]);
                                        datavalue.var16_status = 0;
                                        custom_param_list.Add("var16");
                                        break;
                                    case "var17":
                                        datavalue.var17 = Convert.ToDouble(param[2]);
                                        datavalue.var17_status = 0;
                                        custom_param_list.Add("var17");
                                        break;
                                    case "var18":
                                        datavalue.var18 = Convert.ToDouble(param[2]);
                                        datavalue.var18_status = 0;
                                        custom_param_list.Add("var18");
                                        break;
                                    case "var19":
                                        datavalue.var19 = Convert.ToDouble(param[2]);
                                        datavalue.var19_status = 0;
                                        custom_param_list.Add("var19");
                                        break;
                                    case "var20":
                                        datavalue.var20 = Convert.ToDouble(param[2]);
                                        datavalue.var20_status = 0;
                                        custom_param_list.Add("var20");
                                        break;
                                    case "var21":
                                        datavalue.var21 = Convert.ToDouble(param[2]);
                                        datavalue.var21_status = 0;
                                        custom_param_list.Add("var21");
                                        break;
                                    case "var22":
                                        datavalue.var22 = Convert.ToDouble(param[2]);
                                        datavalue.var22_status = 0;
                                        custom_param_list.Add("var22");
                                        break;
                                    case "var23":
                                        datavalue.var23 = Convert.ToDouble(param[2]);
                                        datavalue.var23_status = 0;
                                        custom_param_list.Add("var23");
                                        break;
                                    case "var24":
                                        datavalue.var24 = Convert.ToDouble(param[2]);
                                        datavalue.var24_status = 0;
                                        custom_param_list.Add("var24");
                                        break;
                                    case "var25":
                                        datavalue.var25 = Convert.ToDouble(param[2]);
                                        datavalue.var25_status = 0;
                                        custom_param_list.Add("var25");
                                        break;
                                    case "var26":
                                        datavalue.var26 = Convert.ToDouble(param[2]);
                                        datavalue.var26_status = 0;
                                        custom_param_list.Add("var26");
                                        break;
                                    case "var27":
                                        datavalue.var27 = Convert.ToDouble(param[2]);
                                        datavalue.var27_status = 0;
                                        custom_param_list.Add("var27");
                                        break;
                                    case "var28":
                                        datavalue.var28 = Convert.ToDouble(param[2]);
                                        datavalue.var28_status = 0;
                                        custom_param_list.Add("var28");
                                        break;
                                    case "var29":
                                        datavalue.var29 = Convert.ToDouble(param[2]);
                                        datavalue.var29_status = 0;
                                        custom_param_list.Add("var29");
                                        break;
                                    case "var30":
                                        datavalue.var30 = Convert.ToDouble(param[2]);
                                        datavalue.var30_status = 0;
                                        custom_param_list.Add("var30");
                                        break;
                                    case "var31":
                                        datavalue.var31 = Convert.ToDouble(param[2]);
                                        datavalue.var31_status = 0;
                                        custom_param_list.Add("var31");
                                        break;
                                    case "var32":
                                        datavalue.var32 = Convert.ToDouble(param[2]);
                                        datavalue.var32_status = 0;
                                        custom_param_list.Add("var32");
                                        break;
                                    case "var33":
                                        datavalue.var33 = Convert.ToDouble(param[2]);
                                        datavalue.var33_status = 0;
                                        custom_param_list.Add("var33");
                                        break;
                                    case "var34":
                                        datavalue.var34 = Convert.ToDouble(param[2]);
                                        datavalue.var34_status = 0;
                                        custom_param_list.Add("var34");
                                        break;
                                    case "var35":
                                        datavalue.var35 = Convert.ToDouble(param[2]);
                                        datavalue.var35_status = 0;
                                        custom_param_list.Add("var35");
                                        break;
                                }
                            }
                            data_5minute_value_repository s = new data_5minute_value_repository();
                            if (datetime == null)
                            {
                                //break;
                            }
                            else
                            {
                                string dtparam = datetime.Substring(0, 4) + "-" + datetime.Substring(4, 2) + "-" + datetime.Substring(6, 2);
                                string hourparam = datavalue.stored_hour.ToString();
                                string minuteparam = datavalue.stored_minute.ToString();
                                int id = s.get_id_by_dt(dtparam, hourparam, minuteparam);
                                datavalue.id = id;
                                if (id != -1)
                                {
                                    if (new data_5minute_value_repository().updateCustom(ref datavalue, custom_param_list) > 0)
                                    {
                                        // ok
                                    }
                                    else
                                    {
                                        // fail
                                    }
                                }
                                else
                                {
                                    if (new data_5minute_value_repository().add(ref datavalue) > 0)
                                    {
                                        // ok
                                    }
                                    else
                                    {
                                        // fail
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion
        #region update data
        private double Calculator(double D, module mod)
        {
            double A;
            A = (double)((double)((double)(mod.output_min - mod.output_max) / (double)(mod.input_min - mod.input_max))
                * (double)(D - mod.input_min))
                + mod.output_min + mod.off_set;
            return A;
        }
        #endregion
        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
                if (!GlobalVar.isLogin)
                {
                    MessageBox.Show(lang.getText("login_before_to_do_this"));
                    return;
                }
            }
            frmConfiguration frmConfig = new frmConfiguration(lang, this);
            frmConfig.ShowDialog();
            initConfig();
        }
        private void frmNewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                is_close_form = true;
                data_value obj = calculateImmediately5Minute();
                data_value obj60min = calculateImmediately60Minute();
                Process.GetCurrentProcess().Kill();
                //MessageBox.Show("123");
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(Environment.ExitCode);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Process.GetCurrentProcess().Kill();
                //Environment.FailFast();
                //Application.Exit();
                //throw ex;
            }
        }
        private data_value calculateImmediately5Minute()
        {
            var obj = new data_5minute_value_repository().get_latest_info();
            return obj;
        }
        private data_value calculateImmediately60Minute()
        {
            var obj = new data_5minute_value_repository().get_latest_info();
            return obj;
        }
        private void btnMPS5Minute_Click(object sender, EventArgs e)
        {
            data_value obj = calculateImmediately5Minute();
            frm5MinuteMPS frm = new frm5MinuteMPS(obj, lang);
            frm.ShowDialog();
        }

        private void btnMPS1Hour_Click(object sender, EventArgs e)
        {
            data_value obj = calculateImmediately60Minute();
            frm1HourMPS frm = new frm1HourMPS(obj, lang);
            frm.ShowDialog();
        }
        private void btnMPSHistoryData_Click(object sender, EventArgs e)
        {
            frmHistoryMPS frm = new frmHistoryMPS(lang);
            frm.ShowDialog();
        }
        private void btnAllHistory_Click(object sender, EventArgs e)
        {
            frmHistoryAll frm = new frmHistoryAll(lang);
            frm.ShowDialog();
        }
        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
                if (!GlobalVar.isLogin)
                {
                    MessageBox.Show(lang.getText("login_before_to_do_this"));
                    return;
                }
            }
            frmMaintenance objMaintenance = new frmMaintenance(lang);
            //this.Hide();
            objMaintenance.ShowDialog();
            //this.Show();
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
            }
            if (GlobalVar.isAdmin())
            {
                frmUserManagement frmUM = new frmUserManagement(lang);
                frmUM.ShowDialog();
            }
            else
            {
                MessageBox.Show(lang.getText("right_permission_error"));
            }


        }
        private void btnLoginLogout_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;

                GlobalVar.isLogin = false;
                GlobalVar.loginUser = null;
            }
            else
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.login;
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();

                if (GlobalVar.isLogin)
                {
                    this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;
                }
            }
        }
        private void settingForLoginStatus()
        {
            if (GlobalVar.isLogin)
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;
                setTextHeadingLogin("" + lang.getText("main_menu_welcome") + ", " + GlobalVar.loginUser.user_name + " !");
            }
            else
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.login;
                setTextHeadingLogin("" + lang.getText("main_menu_welcome") + ", " + lang.getText("main_menu_guest") + " !");
            }
        }
        private void btnMonthlyReport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(lang.getText("monthly_report_yesno_question"), lang.getText("confirm"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                btnMonthlyReport.Enabled = false;
                vprgMonthlyReport.Value = 0;
                vprgMonthlyReport.Visible = true;

                bgwMonthlyReport.RunWorkerAsync();

                //Console.Write("1");
            }
        }
        private void btnLanguage_Click(object sender, EventArgs e)
        {
            switch_language();
            initConfig();
        }


        #region backgroundWorkerMonthlyReport
        private void backgroundWorkerMonthlyReport_DoWork(object sender, DoWorkEventArgs e)
        {
            //string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            //string dataFolderName = "data";

            //string tempFileName = "monthly_report_template.xlsx";
            //string newFileName = "MonthlyReport_" + DateTime.Now.ToString("yyyy (MMddHHmmssfff)");

            //string tempFilePath = Path.Combine(appPath, dataFolderName, tempFileName);
            //string newFilePath = Path.Combine(appPath, dataFolderName, newFileName);

            //if (File.Exists(tempFilePath))
            //{
            //    int year = DateTime.Now.Year;
            //    double dayOfYearTotal = (new DateTime(year, 12, 31)).DayOfYear;
            //    double dayOfYear = 0;
            //    int percent = 0;

            //    IEnumerable<data_value> allData = db60m.get_all_for_monthly_report(year);

            //    if (allData != null)
            //    {
            //        Excel.XLWorkbook oExcelWorkbook = new Excel.XLWorkbook(tempFilePath);
            //        // Excel.Application oExcelApp = new Excel.Application();
            //        // Excel.Workbook oExcelWorkbook = oExcelApp.Workbooks.Open(tempFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //        const int startRow = 5;
            //        int row;

            //        List<MonthlyReportInfo> mps_ph = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_orp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_do = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_turbidity = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_ec = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_temp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> tn = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> tp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> toc = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> refrigeration_temperature = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> bottle_position = new List<MonthlyReportInfo>();

            //        for (int month = 1; month <= 12; month++)
            //        {
            //            Excel.IXLWorksheet oExcelWorksheet = oExcelWorkbook.Worksheet(month) as Excel.IXLWorksheet;
            //            // Excel.IXLWorkSheet oExcelWorksheet = oExcelWorkbook.Worksheets[month] as Excel.Worksheet;

            //            //rename the Sheet name
            //            oExcelWorksheet.Name = (new DateTime(year, month, 1)).ToString("MMM-yy");
            //            oExcelWorksheet.Cell(2, 1).Value = "'" + (new DateTime(year, month, 1)).ToString("MM.");
            //            oExcelWorksheet.Cell(2, 17).Value = (new DateTime(year, month, 1)).ToString("MMM-yy");

            //            // calculate average value
            //            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            //            {
            //                // get maintenance by date (year, month, day)
            //                string strDate = year + "-" + month + "-" + day;
            //                IEnumerable<maintenance_log> onDateMaintenanceLogs = _maintenance_logs.get_all_by_date(strDate);
            //                // prepare data for maintenance
            //                string maintenance_operator_name = "";
            //                string maintenance_start_time = "";
            //                string maintenance_end_time = "";
            //                string maintenance_equipments = "";

            //                Color maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_PERIODIC;
            //                if (onDateMaintenanceLogs != null && onDateMaintenanceLogs.Count() > 0)
            //                {
            //                    foreach (maintenance_log itemMaintenanceLog in onDateMaintenanceLogs)
            //                    {
            //                        maintenance_operator_name += itemMaintenanceLog.name + ";";
            //                        maintenance_start_time += itemMaintenanceLog.start_time.ToString("HH")
            //                                                    + "h" + itemMaintenanceLog.start_time.ToString("mm") + ";";
            //                        maintenance_end_time += itemMaintenanceLog.end_time.ToString("HH")
            //                                                    + "h" + itemMaintenanceLog.end_time.ToString("mm") + ";";
            //                        if (itemMaintenanceLog.tn == 1)
            //                        {
            //                            maintenance_equipments += "TN;";
            //                        }
            //                        if (itemMaintenanceLog.tp == 1)
            //                        {
            //                            maintenance_equipments += "TP;";
            //                        }
            //                        if (itemMaintenanceLog.toc == 1)
            //                        {
            //                            maintenance_equipments += "TOC;";
            //                        }
            //                        if (itemMaintenanceLog.mps == 1)
            //                        {
            //                            maintenance_equipments += "MPS;";
            //                        }
            //                        if (itemMaintenanceLog.pumping_system == 1)
            //                        {
            //                            maintenance_equipments += "Pumping;";
            //                        }
            //                        if (itemMaintenanceLog.auto_sampler == 1)
            //                        {
            //                            maintenance_equipments += "AutoSampler;";
            //                        }
            //                        if (itemMaintenanceLog.other == 1)
            //                        {
            //                            maintenance_equipments += itemMaintenanceLog.other_para + ";";
            //                        }
            //                        if (itemMaintenanceLog.maintenance_reason == 1)
            //                        {
            //                            maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_INCIDENT;
            //                        }
            //                    }
            //                    maintenance_operator_name = maintenance_operator_name.Substring(0, maintenance_operator_name.Length - 1);
            //                    maintenance_start_time = maintenance_start_time.Substring(0, maintenance_start_time.Length - 1);
            //                    maintenance_end_time = maintenance_end_time.Substring(0, maintenance_end_time.Length - 1);
            //                    try
            //                    {
            //                        maintenance_equipments = maintenance_equipments.Substring(0, maintenance_equipments.Length - 1);
            //                    }
            //                    catch { }
            //                }

            //                IEnumerable<data_value> dayData = allData.Where(t => t.stored_date.Month == month && t.stored_date.Day == day);
            //                mps_ph.Clear();
            //                mps_orp.Clear();
            //                mps_do.Clear();
            //                mps_turbidity.Clear();
            //                mps_ec.Clear();
            //                mps_temp.Clear();
            //                tn.Clear();
            //                tp.Clear();
            //                toc.Clear();
            //                refrigeration_temperature.Clear();
            //                bottle_position.Clear();
            //                foreach (data_value item in dayData)
            //                {
            //                    mps_ph.AddNewDataValue(item.MPS_pH_status, item.MPS_pH);
            //                    mps_orp.AddNewDataValue(item.MPS_ORP_status, item.MPS_ORP);
            //                    mps_do.AddNewDataValue(item.MPS_DO_status, item.MPS_DO);
            //                    mps_turbidity.AddNewDataValue(item.MPS_Turbidity_status, item.MPS_Turbidity);
            //                    mps_ec.AddNewDataValue(item.MPS_EC_status, item.MPS_EC);
            //                    mps_temp.AddNewDataValue(item.MPS_Temp_status, item.MPS_Temp);
            //                    tn.AddNewDataValue(item.TN_status, item.TN);
            //                    tp.AddNewDataValue(item.TP_status, item.TP);
            //                    toc.AddNewDataValue(item.TOC_status, item.TOC);
            //                    refrigeration_temperature.AddNewDataValue(0, item.refrigeration_temperature);
            //                    bottle_position.AddNewDataValue(0, item.bottle_position);
            //                }

            //                // update to excel worksheet
            //                row = startRow + day;

            //                oExcelWorksheet.Cell(row, 2).Value = mps_ph.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 3).Value = mps_orp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 4).Value = mps_do.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 5).Value = mps_turbidity.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 6).Value = mps_ec.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 7).Value = mps_temp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 8).Value = tn.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 9).Value = tp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 10).Value = toc.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 11).Value = refrigeration_temperature.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 12).Value = bottle_position.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 14).Value = maintenance_operator_name;
            //                oExcelWorksheet.Cell(row, 15).Value = maintenance_start_time;
            //                oExcelWorksheet.Cell(row, 16).Value = maintenance_end_time;
            //                oExcelWorksheet.Cell(row, 17).Value = maintenance_equipments;


            //                oExcelWorksheet.Range("b" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_ph.GetStatusColor()));
            //                oExcelWorksheet.Range("c" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_orp.GetStatusColor()));
            //                oExcelWorksheet.Range("d" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_do.GetStatusColor()));
            //                oExcelWorksheet.Range("e" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_turbidity.GetStatusColor()));
            //                oExcelWorksheet.Range("f" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_ec.GetStatusColor()));
            //                oExcelWorksheet.Range("g" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_temp.GetStatusColor()));
            //                oExcelWorksheet.Range("h" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(tn.GetStatusColor()));
            //                oExcelWorksheet.Range("i" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(tp.GetStatusColor()));
            //                oExcelWorksheet.Range("j" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(toc.GetStatusColor()));
            //                oExcelWorksheet.Range("k" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(refrigeration_temperature.GetStatusColor()));
            //                oExcelWorksheet.Range("l" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(bottle_position.GetStatusColor()));

            //                oExcelWorksheet.Range("n" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("o" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("p" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("q" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));

            //                dayOfYear = (new DateTime(year, month, day)).DayOfYear;
            //                percent = (int)(dayOfYear * 100d / dayOfYearTotal);
            //                bgwMonthlyReport.ReportProgress(percent);

            //                //Thread.Sleep(1);
            //            }
            //        }
            //        oExcelWorkbook.SaveAs(newFilePath + ".xlsx");
            //        //oExcelWorkbook.SaveAs(newFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //    }
            //}
            //FileInfo fi = new FileInfo(newFilePath + ".xlsx");
            //if (fi.Exists)
            //{
            //    System.Diagnostics.Process.Start(newFilePath + ".xlsx");
            //}
            //else
            //{
            //    //file doesn't exist
            //}
        }

        private void backgroundWorkerMonthlyReport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vprgMonthlyReport.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerMonthlyReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnMonthlyReport.Enabled = true;
            vprgMonthlyReport.Visible = false;

            if (!e.Cancelled && e.Error == null)
            {
                MessageBox.Show(lang.getText("successfully"));
            }
            else
            {

            }
        }

        #endregion backgroundWorkerMonthlyReport

        private void vprgMonthlyReport_Load(object sender, EventArgs e)
        {

        }
    }
    public class ReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
