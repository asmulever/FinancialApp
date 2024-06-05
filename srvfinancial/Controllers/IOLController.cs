using Microsoft.AspNetCore.Mvc;
using DllEntityLayer;
using DllBssFinancial;

namespace srvfinancial.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class Controller : ControllerBase
    {
        private readonly ILogger<Controller> _logger;
        private readonly IBssLayer _BssLayer;
        public Controller(ILogger<Controller> logger,IBssLayer BssLayer)
        {
            _logger = logger;
            _BssLayer = BssLayer;
        }

        //[HttpGet("/products2/{id}", Name = "Products_List")]

        [HttpGet(Name = "GetTicker/{tktname:string}")]
        public IEntityBase Get(string tktname)
        {
            var tkt = _BssLayer.get(tktname);
            return tkt;
        }

    }
