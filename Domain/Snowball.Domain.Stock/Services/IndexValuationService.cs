using Newtonsoft.Json.Linq;
using Snowball.Core;
using Snowball.Core.Utils;
using Snowball.Domain.Stock.Dtos;
using Snowball.Domain.Stock.Entities;
using Snowball.Domain.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snowball.Domain.Stock.Services
{
    public class IndexValuationService : IIndexValuationService
    {
        private readonly IIndexValuationRepository _indexValuationRepository;
        private readonly IUpdatePointRepository _updatePointRepository;
        private readonly TimeProvider _timeProvider;

        public IndexValuationService(IIndexValuationRepository indexValuationRepository,
                                     IUpdatePointRepository updatePointRepository,
                                     TimeProvider timeProvider)
        {
            this._indexValuationRepository = indexValuationRepository;
            this._updatePointRepository = updatePointRepository;
            this._timeProvider = timeProvider;
        }

        public async Task<IEnumerable<IndexValuationDto>> GetAllAsync()
        {
            var entities = await this._indexValuationRepository.GetAllAsync();
            if (entities == null
                || !entities.Any())
            {
                return Enumerable.Empty<IndexValuationDto>();
            }

            return entities.Select(e => new IndexValuationDto
            {
                Code = e.Code,
                DividendYield = FormatDecimal(e.DividendYield * 100),
                PB = FormatDecimal(e.PB),
                PBPercentile = FormatDecimal(e.PBPercentile * 100),
                PE = FormatDecimal(e.PE),
                PEPercentile = FormatDecimal(e.PEPercentile * 100),
                ROE = FormatDecimal(e.ROE * 100),
                Style = e.Style,
                Title = e.Title,
                Percentile = GetPercentile(e)
            })
            .OrderBy(dto => dto.Percentile)
            .ToList();
        }

        private string FormatDecimal(decimal d)
        {
            return d.ToString("0.00");
        }

        private decimal GetPercentile(IndexValuationEntity entity)
        {
            if (entity.PreparationTime.AddYears(5) > this._timeProvider.Now)
            {// 指数编制时间小于5年，不参与估值预测
                return 101;
            }

            decimal percentage;
            if (entity.UsePB)
            {
                percentage = entity.PBPercentile * 100;
            }
            else
            {
                percentage = entity.PEPercentile * 100;
            }

            return percentage;
        }

        public async Task<DateTime> GetLastUpdateTimeAsync()
        {
            var entity = await this._updatePointRepository.GetAsync(GlobalConstant.IndexValuationKey);
            return entity.UpdateTime;
        }
    }
}
