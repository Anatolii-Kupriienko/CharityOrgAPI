﻿using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class ErrorsController : ApiController
    {
        public ErrorsController(ICRUDService CRUDService) : base(CRUDService){}
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
