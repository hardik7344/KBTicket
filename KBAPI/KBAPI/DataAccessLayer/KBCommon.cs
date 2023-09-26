using OwnYITCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace KBAPI.DataAccessLayer
{
    public class KBCommon
    {
        //public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\AssertYIT\\OwnYITHRMS\\Log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\AssertYIT\\NocdeskMicroservice\\";
        public String strOSType = "";
        public static string LINUX_WWW_PATH = "";
        DataTableConversion dtconversion = new DataTableConversion();

        public KBCommon()
        {
            strOSType = readOSType();
        }

        public string readDBConfig(string Item, string OSType, string LinuxFileName, string WindowsFileName)
        {
            string _dbSettings = "";
            if (OSType.ToUpper() == "LINUX")
            {
                _dbSettings = KBCommon.LINUX_WWW_PATH + LinuxFileName;
            }
            else
            {
                _dbSettings = systemcheck() + "\\AssertYIT\\Configuration\\" + WindowsFileName;
            }
            string retVal = "";
            DataSet ds = new DataSet("DataSet");
            try
            {
                ds.ReadXml(_dbSettings);
                retVal = ds.Tables["Config"].Rows[0][Item].ToString();
            }
            catch (Exception ex)
            {
                //WriteLog("Exception", "Exception while read DB Configuration : " + ex.Message.ToString());
            }
            finally
            {
                ds.Dispose();
            }
            return retVal;
        }
        public string readOSType()
        {
            string xmlPath = Directory.GetCurrentDirectory() + "/wwwroot/xml/OSType.xml";
            string retVal = "";
            DataSet ds = new DataSet("DataSet");
            try
            {
                ds.ReadXml(xmlPath);
                retVal = ds.Tables["Config"].Rows[0]["ostype"].ToString();
            }
            catch (Exception ex)
            {
                retVal = "Linux";
                //WriteLog("Exception", "Exception while read DB Configuration : " + ex.Message.ToString());
            }
            finally
            {
                ds.Dispose();
            }
            return retVal;
        }
        public string systemcheck()
        {
            string _systemPath = "";
            if (Is64BitSystem())
                _systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System).ToUpper().Replace("SYSTEM32", "SysWOW64");
            else
                _systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System).ToUpper().Replace("SysWOW64", "SYSTEM32");
            return _systemPath;
        }
        public bool Is64BitSystem()
        {
            return (Environment.GetEnvironmentVariable("ProgramFiles(x86)")) != null;
        }
        public void WriteLog(string FileName, string Extension, string ProductName, string Message, bool HourWise)
        {
            try
            {
                if (strOSType.ToUpper() == "LINUX")
                {
                    logPath = "/var/log/OwnYITHRMS/log";
                }
                else
                {
                    logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\AssertYIT\\NocdeskMicroservice\\";
                }
                if (HourWise)
                    FileName = FileName + "_" + System.DateTime.Now.ToString("yyyy-MM-dd HH") + "." + Extension;
                else
                    FileName = FileName + "." + Extension;
                FileName = logPath + ProductName + "\\" + FileName;
                if (!Directory.Exists(logPath + ProductName))
                    Directory.CreateDirectory(logPath + ProductName);
                FileStream fs;
                fs = new FileStream(FileName, FileMode.Append);
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + Message);
                s.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }
        }
        //public void WriteLog(string strFileName, string strMessage)
        //{
        //    try
        //    {
        //        if (strOSType.ToUpper() == "LINUX")
        //        {
        //            logPath = "/var/log/OwnYITHRMS/log";
        //        }
        //        else
        //        {
        //            //logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\AssertYIT\\OwnYITHRMS\\Log";
        //            logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\AssertYIT\\Log";

        //        }
        //        Console.WriteLine("Path : " + LINUX_WWW_PATH);
        //        if (!System.IO.Directory.Exists(logPath)) System.IO.Directory.CreateDirectory(logPath);
        //        FileStream fs;
        //        strFileName = logPath + "/" + strFileName + ".log";
        //        Console.WriteLine("File : " + strFileName);
        //        fs = new FileStream(strFileName, FileMode.Append);
        //        StreamWriter s = new StreamWriter(fs);
        //        s.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + strMessage);
        //        s.Close();
        //        fs.Close();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //public string GetOUJson()
        //{
        //    try
        //    {
        //        string OUList = "";
        //        OUList = LocalConstant.objRedis.GetString("-1");

        //        return OUList;

        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLog("Exception", "Exception while read DBobjRedis : " + ex.Message.ToString());
        //    }
        //}
        //public DataTable GetSystemList(string DataJson)
        //{
        //    DataTable dtSystem = new DataTable();
        //    dtSystem.Columns.Add("DeviceID");
        //    dtSystem.Columns.Add("OUID");
        //    dtSystem.Columns.Add("DeviceName");
        //    dtSystem.Columns.Add("IPAddress");
        //    dtSystem.Columns.Add("MACAddress");
        //    dtSystem.Columns.Add("OUName");
        //    dtSystem.Columns.Add("OSName");
        //    dtSystem.Columns.Add("LoginUser");
        //    dtSystem.Columns.Add("LastPollTime");
        //    DataTable dtData = new DataTable();
        //    dtData = dtconversion.JsonStringToDataTable(DataJson);
        //    DataView dv = new DataView(dtData);
        //    DataTable dtTemp = dv.ToTable(true, "device_id", "device_name", "ip_address", "mac_address", "ou_id");
        //    for (int i = 0; i < dtTemp.Rows.Count; i++)
        //    {
        //        DataRow dr = dtSystem.NewRow();
        //        dr["DeviceID"] = dtTemp.Rows[i]["device_id"];
        //        dr["OUID"] = dtTemp.Rows[i]["ou_id"];
        //        dr["DeviceName"] = dtTemp.Rows[i]["device_name"];
        //        dr["IPAddress"] = dtTemp.Rows[i]["ip_address"];
        //        dr["MACAddress"] = dtTemp.Rows[i]["mac_address"];
        //        dr["OUName"] = GetOUNameFromOUID(dtTemp.Rows[i]["ou_id"].ToString());
        //        DataRow[] drArr = dtData.Select("device_id=" + dtTemp.Rows[i]["device_id"]);
        //        for (int j = 0; j < drArr.Length; j++)
        //        {
        //            if (drArr[j]["type"].ToString() == "2004")
        //            {
        //                dr["LoginUser"] = drArr[j]["param_varchar00"].ToString();
        //                dr["LastPollTime"] = drArr[j]["server_time"].ToString();
        //            }
        //            else if (drArr[j]["type"].ToString() == "6011")
        //            {
        //                dr["OSName"] = drArr[j]["param_varchar00"].ToString();
        //            }
        //        }
        //        dtSystem.Rows.Add(dr);
        //    }

        //    return dtSystem;
        //}
        //public string GetOUNameFromOUID(string OUID)
        //{
        //    string OUName = "";
        //    try
        //    {
        //        string strOU = LocalConstant.objRedis.GetString(OUID);
        //        //WriteLog("Exception", "Exception while read DBobjRedis : " + strOU.ToString());
        //        Dictionary<string, string> str = dtconversion.getJSONPropertiesFromString("[" + strOU + "]");
        //        OUName = str["ou_long_name"];
        //        //WriteLog("Exception", "Exception while read DBobjRedis : " + OUName.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLog("Exception", "Exception while read DBobjRedis : " + ex.Message.ToString());


        //    }
        //    return OUName;

        //}
        //public void SetConfig()
        //{
        //    LocalConstant.RedisServer = readDBConfig("RedisServer", strOSType, LocalConstant.MESSAGING_FILE, LocalConstant.MESSAGING_FILE);
        //    LocalConstant.RedisDB = 12;
        //}
        public DataSet xmlToDataset(string strXML)
        {
            DataSet DS = new DataSet();
            try
            {
                XmlDocument xmltest = new XmlDocument();
                xmltest.LoadXml(strXML);
                XmlNodeReader xmlnr = new XmlNodeReader(xmltest);

                DS.ReadXml(xmlnr);
            }
            catch (Exception)
            {
            }
            return DS;
        }

    }
}
