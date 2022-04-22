using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// 根据ID查询书籍
        /// </summary>
        /// <remarks>
        /// 请求示例:
        /// 
        ///     1111111
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<BookDto> GetAsync(int id)
        {
            return this._bookAppService.GetAsync(id);
        }
    }
}
