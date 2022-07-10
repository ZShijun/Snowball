using Newtonsoft.Json.Linq;
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
        private readonly IHttpClientFactory _clientFactory;
        public IndexValuationService(IIndexValuationRepository indexValuationRepository,
                                     IUpdatePointRepository updatePointRepository,
                                     IHttpClientFactory clientFactory)
        {
            this._indexValuationRepository = indexValuationRepository;
            this._updatePointRepository = updatePointRepository;
            this._clientFactory = clientFactory;
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
            if (entity.PreparationTime.AddYears(5) > DateTime.Now)
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
            const string key = "IndexValuation";
            var entity = await this._updatePointRepository.GetAsync(key);
            return entity.UpdateTime;
        }

        private async Task CrawlingIndexValuation()
        {
            using (var client = this._clientFactory.CreateClient("danjuanfunds"))
            {
                var httpResponse = await client.GetAsync("/djapi/index_eva/dj");
                var json = await httpResponse.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(json);
                var items = jObject["data"]["items"];
                var list = new List<IndexValuationEntity>();
                foreach (var item in items)
                {
                    var entity = new IndexValuationEntity
                    {
                        Code = item["index_code"].ToString(),
                        Description = "",
                        DividendYield = Convert.ToDecimal(item["yeild"]),
                        PB = Convert.ToDecimal(item["pb"]),
                        PBPercentile = Convert.ToDecimal(item["pb_percentile"]),
                        PE = Convert.ToDecimal(item["pe"]),
                        PEPercentile = Convert.ToDecimal(item["pe_percentile"]),
                        PreparationTime = DateUtil.GetDateTime(Convert.ToInt64(item["begin_at"]), true),
                        ROE = Convert.ToDecimal(item["roe"]),
                        Title = item["name"].ToString()
                    };
                    await this._indexValuationRepository.InsertAsync(entity);
                    list.Add(entity);
                }
            }
        }
    }
}
