using Snowball.Domain.Stock.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Application.Dtos
{
    public class IndexValuationOutputDto
    {
        public string LastUpdateTime { get; set; }

        public IEnumerable<IndexValuationDto> Valuations { get; set; }
    }
}
