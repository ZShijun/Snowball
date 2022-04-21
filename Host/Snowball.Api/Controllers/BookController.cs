using Microsoft.AspNetCore.Mvc;
using Snowball.Application;
using Snowball.Domain.Bookshelf.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snowball.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookAppService _bookAppService;

        public BookController(IBookAppService bookAppService)
        {
            this._bookAppService = bookAppService;
        }

        [HttpGet]
        public Task<BookDto> GetAsync(int id)
        {
            return this._bookAppService.GetAsync(id);
        }
    }
}
