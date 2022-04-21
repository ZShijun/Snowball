using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Core.Data
{
    public abstract class BaseRepository
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
    }
}
