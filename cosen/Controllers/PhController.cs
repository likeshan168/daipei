using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cosen.Models;
namespace cosen.Controllers
{
    public class PhController : ApiController
    {
        // GET api/ph
        public IEnumerable<ZhiZaoDanInfo> Get(string zdid)
        {
            return new LogicModel().GetZDHById(zdid);
        }
        [HttpPost]
        // POST api/ph
        public IEnumerable<ZhiZaoDanInfo> GetZDHById(string dh)
        {
            return new LogicModel().GetZDHById(dh);
        }
        [HttpPost]
        public IEnumerable<GetPeiHuo_ProcResult> GetPeiHuos(string zdid, string style)
        {
            return new LogicModel().GetPeiHuos(zdid, style);
        }


    }
}
