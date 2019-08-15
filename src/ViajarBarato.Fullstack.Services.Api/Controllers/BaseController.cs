using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViajarBarato.Fullstack.Services.Api.Controllers
{
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected new IActionResult Response(bool success, string message, object result = null)
        {
            if (success)
            {
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = message
                    
                });
            }

            return BadRequest(new
            {
                success = false,
                message = message
            });
        }
    }
}
