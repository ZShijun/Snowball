using Newtonsoft.Json.Linq;
using Snowball.Core.Utils;
using Snowball.Domain.Stock.Dtos;
using Snowball.Domain.Stock.Entities;
using Snowball.Domain.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Stock.Services
{
    public class IndexValuationService : IIndexValuationService
    {
        private readonly IIndexValuationRepository _indexValuationRepository;
        private readonly IHttpClientFactory _clientFactory;
        public IndexValuationService(IIndexValuationRepository indexValuationRepository, IHttpClientFactory clientFactory)
        {
            this._indexValuationRepository = indexValuationRepository;
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
                DividendYield = e.DividendYield,
                PB = e.PB,
                PBPercentile = e.PBPercentile,
                PE = e.PE,
                PEPercentile = e.PEPercentile,
                ROE = e.ROE,
                Style = e.Style,
                Title = e.Title,
                ValuationLevel = GetValuationLevel(e)
            }).OrderBy(dto => dto.ValuationLevel)
            .ThenBy(dto => dto.PEPercentile);
        }

        private int GetValuationLevel(IndexValuationEntity entity)
        {
            decimal percentage;
            if (entity.UsePB)
            {
                percentage = entity.PBPercentile * 100;
            }
            else
            {
                percentage = entity.PEPercentile * 100;
            }

            return PercentageToLevel(percentage);
        }
        
        private int PercentageToLevel(decimal percentage)
        {
            int level;
            if (percentage < 15)
            {
                level = 1;
            }
            else if (percentage < 35)
            {
                level = 2;
            }
            else if (percentage <= 65)
            {
                level = 3;
            }
            else if (percentage <= 85)
            {
                level = 4;
            }
            else
            {
                level = 5;
            }

            return level;
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
