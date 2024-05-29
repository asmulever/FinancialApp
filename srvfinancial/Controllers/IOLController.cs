using Microsoft.AspNetCore.Mvc;
using DllEntityLayer;
using DllBssFinancial;

namespace srvfinancial.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class IOLController : ControllerBase
    {
        private readonly ILogger<IOLController> _logger;
        private readonly IBssLayerIOL _BssLayerIOL;
        public IOLController(ILogger<IOLController> logger,IBssLayerIOL BssLayerIOL)
        {
            _logger = logger;
            _BssLayerIOL = BssLayerIOL;
        }

        //[HttpGet("/products2/{id}", Name = "Products_List")]

        [HttpGet(Name = "GetIOLTicker/{tktname:string}")]
        public ITickerBase Get(string tktname)
        {
            var tkt = _BssLayerIOL.getIol(tktname);
            return tkt;
        }

    }
