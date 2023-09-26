using KBAPI.DataAccessLayer;
using KBAPI.Model;
using OwnYITCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TectonaDatabaseHandlerDLL;

namespace KBAPI.BusinessLogic
{
    public class CreateKDMaster
    {
        //DatabaseCommon objcommon = new DatabaseCommon();
        KBCommon objcommon=new KBCommon();
        DataTableConversion dtconversion = new DataTableConversion();
        ParameterJSON jSON_DATA = new ParameterJSON();
        QueryHandler objQuery = new QueryHandler();
        KBCommon objCom = new KBCommon();
        DataTable resultdt = new DataTable();
        string strStatus = "0"; // 0 - Fail , 1 - Successs , 2 - Already exist
        string strMessage = "";
        string resultdata = "";
        string QID = "";

        public DataTable GetKB()
        {
            try
            {
                DatabaseHandler objDBNocdesk = LocalConstant.poolKB.getConnection();
                resultdt = objDBNocdesk.getDatatable(objQuery.GetKBTebal());
                objcommon.WriteLog("KDMaster", "log", "KB", "GetKB datatable count return : " + resultdt.Rows.Count, true);
                LocalConstant.poolKB.returnConnection(objDBNocdesk);
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("KDMaster", "log", "KB", "GetKB Exception : " + ex.Message, true);
            }
            return resultdt;
        }
        public DataTable GetPQIDKB(string PQID)
        {
            try
            {
                DatabaseHandler objDBNocdesk = LocalConstant.poolKB.getConnection();
                resultdt = objDBNocdesk.getDatatable(objQuery.GetPQIDKB(PQID));
                objcommon.WriteLog("KDMaster", "log", "KB", "GetPQIDKB datatable count return : " + resultdt.Rows.Count, true);
                LocalConstant.poolKB.returnConnection(objDBNocdesk);
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("KDMaster", "log", "KB", "GetPQIDKB Exception : " + ex.Message, true);
            }
            return resultdt;
        }
        //
        public DataTable CreateKB(string userjson)
        {
            try
            {
                resultdt.Columns.Add("status", typeof(Int16));
                resultdt.Columns.Add("status_message", typeof(string));
                var row = resultdt.NewRow();

                DatabaseHandler objDBActivity = LocalConstant.poolKB.getConnection();
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKB parameter pass json : " + userjson, true);
                string strTemp = dtconversion.Base64Decode(userjson);
                IDictionary<string, string> display = dtconversion.getJSONPropertiesFromString("[" + strTemp + "]");
                if (display["action_type"] == "1")
                {
                    //if (Convert.ToInt16(objDBActivity.executeScalar(objQuery.CheckKB())) > 0)
                    //{
                    //    strStatus = "2";
                    //    strMessage = "KB already exist.";
                    //}
                    //else
                    {
                        string KBType = "";
                        KBType = KBMaterType(display);
                        string QID = "";
                        string KBID = objDBActivity.executeScalar(objQuery.GetKBID());
                        QID = objDBActivity.executeScalar(objQuery.GetKBQid());
                        //string strQuery = objQuery.CreateKB(display["create_kb_question"], KBID, display["create_kb_answer"], KBType, QID, display["PQID"]);
                        string strQuery = objQuery.CreateKB(display["OwnerID"], display["AppicationID"], display["SrOrgID"], KBID, KBType, display["PQID"], QID, display["create_kb_question"], display["create_kb_answer"], display["ChildQjson"], display["ActionJson"]);

                        if (objDBActivity.execute(strQuery) > 0)
                        {

                            strStatus = "1";
                            strMessage = " KB created successfully.";
                        }
                        else
                        {
                            strStatus = "0";
                            strMessage = "KB created failed.";
                        }

                    }
                }
                else if (display["action_type"] == "2")
                {
                    if (objDBActivity.execute(objQuery.UpdateKB()) > 0)
                    {
                        strStatus = "1";
                        strMessage = "KB updated successfully.";
                    }
                    else
                    {
                        strStatus = "0";
                        strMessage = "KB updated failed.";
                    }
                    //row["status"] = strStatus;
                    //row["status_message"] = strMessage;
                    //resultdt.Rows.Add(row);
                    //objcommon.WriteLog("KB", "log", "CreateKB MicroService", "CreateKB datatable count return : " + resultdt.Rows.Count, true);
                    //LocalConstant.poolKB.returnConnection(objDBActivity);

                }
                row["status"] = strStatus;
                row["status_message"] = strMessage;
                resultdt.Rows.Add(row);
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKB datatable count return : " + resultdt.Rows.Count, true);
                LocalConstant.poolKB.returnConnection(objDBActivity);
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKB Exception : " + ex.Message, true);
            }
            return resultdt;
        }
        public DataTable CreateKBLinkeg(string userjson)
        {
            try
            {

                resultdt.Columns.Add("status", typeof(Int16));
                resultdt.Columns.Add("status_message", typeof(string));
                var row = resultdt.NewRow();
                DatabaseHandler objDBActivity = LocalConstant.poolKB.getConnection();
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBLinkeg parameter pass json : " + userjson, true);
                string strTemp = dtconversion.Base64Decode(userjson);
                IDictionary<string, string> display = dtconversion.getJSONPropertiesFromString("[" + strTemp + "]");

                if (display["action_type"] == "1")
                {
                    //if (Convert.ToInt16(objDBActivity.executeScalar(objQuery.CheckKB())) > 0)
                    //{
                    //    strStatus = "2";
                    //    strMessage = "KB already exist.";
                    //}
                    //else
                    {
                        //string KBID = "";
                        //string strQuery = objQuery.CreateKBLinkeg(display["OUID"], display["KBQID"], display["SDcatL1"], display["SDcatL2"], display["SDcatL3"]);
                        string strQuery = objQuery.CreateKBLinkeg(display["SrOrgID"], display["KBID"], display["OUID"], display["KBQID"], display["catID"], display["subcatID"], display["itemID"]);
                        if (objDBActivity.execute(strQuery) > 0)
                        {

                            strStatus = "1";
                            strMessage = " KB Linkage created successfully.";
                        }
                        else
                        {
                            strStatus = "0";
                            strMessage = "KB Linkage created failed.";
                        }

                    }
                }
                else if (display["action_type"] == "2")
                {
                    if (objDBActivity.execute(objQuery.UpdateKB()) > 0)
                    {
                        strStatus = "1";
                        strMessage = "KB Linkage updated successfully.";
                    }
                    else
                    {
                        strStatus = "0";
                        strMessage = "KB Linkage updated failed.";
                    }
                }
                row["status"] = strStatus;
                row["status_message"] = strMessage;
                //row["data"] = resultdata;
                resultdt.Rows.Add(row);
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBLinkeg datatable count return : " + resultdt.Rows.Count, true);
                LocalConstant.poolKB.returnConnection(objDBActivity);

            }
            catch (Exception ex)
            {
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBLinkeg Exception : " + ex.Message, true);
            }
            return resultdt;
        }
        public string KBMaterType(IDictionary<string, string> display)
        {
            string KBType = "";
            if (display["PQID"] == "0" && display["create_kb_answer"].ToUpper() == "")
            {
                KBType += "2";
            }
            else if (display["PQID"] == "0" && display["create_kb_answer"].ToString() != "")
            {
                KBType += "3";
            }
            else if (display["PQID"] != "0" && display["create_kb_leaf"].ToUpper() == "FALSE" && display["create_kb_answer"].ToString() == "")
            {
                KBType += "4";
            }
            else if (display["PQID"] != "0" && display["create_kb_leaf"].ToUpper() == "FALSE" && display["create_kb_answer"].ToString() != "")
            {
                KBType += "5";
            }
            else if (display["PQID"] != "0" && display["create_kb_leaf"].ToUpper() == "TRUE" && display["create_kb_answer"].ToString() == "")
            {
                KBType += "8";
            }
            else if (display["PQID"] != "0" && display["create_kb_leaf"].ToUpper() == "TRUE" && display["create_kb_answer"].ToString() != "")
            {
                KBType += "9";
            }
            return KBType;
        }

        public async Task<DataTable> CreateKBTicketAsync(string userjson)
        {
            try
            {
                resultdt.Columns.Add("status", typeof(Int16));
                resultdt.Columns.Add("status_message", typeof(string));
                var row = resultdt.NewRow();
                DatabaseHandler objDBActivity = LocalConstant.poolKB.getConnection();
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBTicketAsync parameter pass json : " + userjson, true);
                string strTemp = dtconversion.Base64Decode(userjson);
                IDictionary<string, string> display = dtconversion.getJSONPropertiesFromString("[" + strTemp + "]");
                if (display["action_type"] == "1")
                {
                    //if (Convert.ToInt16(objDBActivity.executeScalar(objQuery.CheckKB())) > 0)
                    //{
                    //    strStatus = "2";
                    //    strMessage = "KB already exist.";
                    //}
                    //else
                    {
                        string category = "";
                        string Subcategory = "";
                        string Item = "";
                        string getCategory = objQuery.getKbcategoryByQid(display["user_qid"]);
                        DataTable dtcategory = objDBActivity.getDatatable(getCategory);
                        if (dtcategory.Rows.Count > 0)
                        {
                            category = dtcategory.Rows[0]["SDCatL1"].ToString();
                            Subcategory = dtcategory.Rows[0]["SDCatL2"].ToString();
                            Item = dtcategory.Rows[0]["SDCatL3"].ToString();

                        }
                        string KBticket = "{\"action_type\":1,\"ApplicationID\":11,\"TicketType\":\"Normal Ticket\",\"Problem\":\"<question>\",\"Discription\":\"<Discription>\",\"categoryid\":\"\",\"Category\":\"<category>\",\"Subcategoryid\":\"\",\"SubCategory\":\"<subcategory>\",\"Itemid\":\"\",\"Item\":\"<item>\",\"TicketTemplateID\":\"\",\"parentTicketID\":\"\",\"parentTicketType\":\"\",\"TicketPriority\":\"\",\"SRuserID\":\"\",\"CurrentSDUser\":\"\",\"CurrentSDRollID\":\"\",\"OUID\":\"\",\"DeviceID\":\"\",\"SLAID\":\"\",\"SLALevel\":\"\",\"CurrentStatus\":\"\",\"Visible\":\"\"}";
                        //string KBticket = "{\"action_type\":1,\"ApplicationID\":2,\"TicketType\":\"Normal Ticket\",\"user_ques\":\"null\",\"user_ans\":\"null\",\"Problem\":\"<question>\",\"Discription\":\"<Discription>\",\"categoryid\":\"\",\"Category\":\"<category>\",\"Subcategoryid\":\"\",\"SubCategory\":\"<subcategory>\",\"Itemid\":\"\",\"Item\":\"<item>\"}";
                        KBticket = KBticket.Replace("<question>", display["user_ques"]).Replace("<Discription>", display["user_ans"]).Replace("<category>", category).Replace("<subcategory>", Subcategory).Replace("<item>", Item);
                        string user_json = dtconversion.Base64Encode(KBticket);
                        jSON_DATA.user_json = user_json;
                        string baseUrl = LocalConstant.NocdeskTicket + "/NocdeskTicket/CreateTicket/CreateTicketdata";
                        resultdata = await CallingMethod.post_method(baseUrl, jSON_DATA);
                        strStatus = "1";
                        strMessage = "Knowledge Base Ticket created successfully.";
                        row["status"] = strStatus;
                        row["status_message"] = strMessage;
                        resultdt.Rows.Add(row);
                    }
                }
                else if (display["action_type"] == "2")
                {
                    if (objDBActivity.execute(objQuery.UpdateKB()) > 0)
                    {
                        strStatus = "1";
                        strMessage = "Knowledge Base Ticket Linkage updated successfully.";
                    }
                    else
                    {
                        strStatus = "0";
                        strMessage = "Knowledge Base Ticket Linkage updated failed.";
                    }
                }

                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBTicketAsync datatable count return : " + resultdt.Rows.Count, true);
                LocalConstant.poolKB.returnConnection(objDBActivity);

            }
            catch (Exception ex)
            {
                objcommon.WriteLog("KDMaster", "log", "KB", "CreateKBTicketAsync Exception : " + ex.Message, true);
            }
            return resultdt;
        }
    }
}
