using Snowball.Domain.Bookshelf.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf.Repositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// 根据Id查询书籍信息
        /// </summary>
        /// <param name="id">书籍ID</param>
        /// <returns></returns>
        Task<BookEntity> GetAsync(int id);

        /// <summary>
        /// 根据书名模糊查询书籍数量
        /// </summary>
        /// <param name="bookName">书籍名称关键词</param>
        /// <returns></returns>
        Task<long> FuzzyCountByNameAsync(string bookName);

        /// <summary>
        /// 根据书名模糊查询书籍列表
        /// </summary>
        /// <param name="bookName">书籍名称关键词</param>
        /// <returns></returns>
        Task<IEnumerable<BookEntity>> FuzzySearchByNameAsync(string bookName);
    }
}