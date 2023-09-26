using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TectonaDatabaseHandlerDLL;

namespace KBAPI.DataAccessLayer
{
    public class QueryHandler
    {
        //DatabaseCommon objcommon = new DatabaseCommon();
        KBCommon objcommon = new KBCommon();
        OwnYITConstant.DatabaseTypes dbtype;
        DateTime time = new DateTime();

        //GetKBID
        public string GetKBID()
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("select isnull(MAX(KBID),0)+1 from Kb_Master");
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("select isnull(MAX(KBID),0)+1 from Kb_Master");
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "GetKBID Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }

        //Get The GetKBQid
        public string GetKBQid()
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("select isnull(MAX(QID),0)+1 from Kb_Master");
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("select isnull(MAX(QID),0)+1 from Kb_Master");
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "GetKBQid Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }

        //GET KB Table 
        public string GetKBTebal()
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("select SN, KBID, QType, PQID, QID, QText, AText from Kb_Master");
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("select SN, KBID, QType, PQID, QID, QText, AText from Kb_Master");
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "GetKBTebal Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }
        public string GetPQIDKB(string PQID)
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("select SN, KBID, QType, PQID, QID, QText, AText from Kb_Master where PQID='{0}'", PQID);
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("select SN, KBID, QType, PQID, QID, QText, AText from Kb_Master where PQID='{0}'", PQID);
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "GetPQIDKB Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }

        public string getKbcategoryByQid(string Qid)
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("select SDCatL1,SDCatL2,SDCatL3 from SD_LinkageBase_Master where QID='{0}'", Qid);
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("select SDCatL1,SDCatL2,SDCatL3  from SD_LinkageBase_Master where QID='{0}'", Qid);
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "getKbcategoryByQid Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }
        public string CreateKB(string OwnerID, string AppicationID, string SrOrgID, string KBID, string KBType, string PQID, string QID, string create_kb_question, string create_kb_answer, string ChildQjson, string ActionJson)
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("insert into Kb_Master(OwnerID,ApplicationID,SROrgID,KBID,QType,PQID,QID,QText,AText,ChildQJSON,ActionJSON)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", OwnerID, AppicationID, SrOrgID, KBID, KBType, PQID, QID, create_kb_question, create_kb_answer, ChildQjson, ActionJson);
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("insert into Kb_Master(OwnerID,ApplicationID,SROrgID,KBID,QType,PQID,QID,QText,AText,ChildQJSON,ActionJSON)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", OwnerID, AppicationID, SrOrgID, KBID, KBType, PQID, QID, create_kb_question, create_kb_answer, ChildQjson, ActionJson);
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "CreateKB Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }
        public string UpdateKB()
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("");
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("");
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "UpdateKB Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }
        //
        public string CheckKB()
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("");
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("");
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "CheckKB Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }
        //public string CreateKBLinkeg(string SrOrgID, string KBID, string KBQID, string OUID, string SDcatL1, string SDcatL2, string SDcatL3)
        //{
        //    StringBuilder strQuery = new StringBuilder();
        //    try
        //    {
        //        switch (dbtype)
        //        {
        //            case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
        //                strQuery.AppendFormat("insert into SD_LinkageBase_Master(SROrgID,KBID,OUID,QID,SDCatL1,SDCatL2,SDCatL3)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", SrOrgID, KBID, KBQID, OUID, SDcatL1, SDcatL2, SDcatL3);
        //                break;
        //            case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
        //                strQuery.AppendFormat("insert into SD_LinkageBase_Master(SROrgID,KBID,OUID,QID,SDCatL1,SDCatL2,SDCatL3)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", SrOrgID, KBID, KBQID, OUID, SDcatL1, SDcatL2, SDcatL3);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objcommon.WriteLog("QueryHandler", "log", "CreateKBLinkeg MicroService", "Create KB Linkeg Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
        //    }
        //    return strQuery.ToString();
        //}
        public string CreateKBLinkeg(string SrOrgID, string KBQID, string OUID, string QID, string SDcatL1, string SDcatL2, string SDcatL3)
        {
            StringBuilder strQuery = new StringBuilder();
            try
            {
                switch (dbtype)
                {
                    case OwnYITConstant.DatabaseTypes.MSSQL_SERVER:
                        strQuery.AppendFormat("insert into SD_LinkageBase_Master(SROrgID,KBID,OUID,QID,SDCatL1,SDCatL2,SDCatL3)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", SrOrgID, KBQID, OUID, QID, SDcatL1, SDcatL2, SDcatL3);
                        break;
                    case OwnYITConstant.DatabaseTypes.MYSQL_SERVER:
                        strQuery.AppendFormat("insert into SD_LinkageBase_Master(SROrgID,KBID,OUID,QID,SDCatL1,SDCatL2,SDCatL3)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", SrOrgID, KBQID, OUID, QID, SDcatL1, SDcatL2, SDcatL3);
                        break;
                }
            }
            catch (Exception ex)
            {
                objcommon.WriteLog("QueryHandler", "log", "KB", "CreateKBLinkeg Exception : " + ex.Message.ToString() + " , Query : " + strQuery.ToString(), true);
            }
            return strQuery.ToString();
        }

    }
}
