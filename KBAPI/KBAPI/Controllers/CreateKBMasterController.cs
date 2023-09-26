using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using KBAPI.BusinessLogic;
using KBAPI.DataAccessLayer;
using KBAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KBAPI.Controllers
{
    [Route("KB/[controller]/[action]")]
    [ApiController]
    public class CreateKBMasterController : ControllerBase
    {
        CreateKDMaster objKBMAster = new CreateKDMaster();
        DataTable dt = new DataTable();
        KBCommon objCom = new KBCommon();
        string jsonvalue = "";

        [HttpGet]
        //GET: KB/CreateKBMaster/GetKB
        public DataTable GetKB()
        {
            dt = objKBMAster.GetKB();
            return dt;
        }
        [HttpGet("{GetPQID}")]
        //GET: KB/CreateKBMaster/GetPQIDKB
        public DataTable GetPQIDKB(string GetPQID)
        {
            dt = objKBMAster.GetPQIDKB(GetPQID);
            return dt;
        }
        //POST: KB/CreateKBMaster/CreateKB
        [HttpPost]
        public DataTable CreateKB([FromBody] ParameterJSON objParam)
        {
            dt = objKBMAster.CreateKB(objParam.user_json);
            return dt;
        }
        //POST: KB/CreateKBMaster/CreateKBLinkeg
        [HttpPost]
        public DataTable CreateKBLinkeg([FromBody] ParameterJSON objParam)
        {
            dt = objKBMAster.CreateKBLinkeg(objParam.user_json);
            return dt;
        }
        //POST: KB/CreateKBMaster/CreateKBTicket
        [HttpPost]
        public async Task<DataTable> CreateKBTicket([FromBody] ParameterJSON objParam)
        {
            dt = await objKBMAster.CreateKBTicketAsync(objParam.user_json);
            return dt;
        }
    }
}