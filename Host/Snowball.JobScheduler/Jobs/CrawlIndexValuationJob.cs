using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz;
using Snowball.Core;
using Snowball.Core.Utils;
using Snowball.Domain.Stock.Entities;
using Snowball.Domain.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.JobScheduler.Jobs
{
    /// <summary>
    /// 指数估值爬取作业
    /// </summary>
    [DisallowConcurrentExecution]
    public class CrawlIndexValuationJob : IJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IIndexValuationRepository _indexValuationRepository;
        private readonly IUpdatePointRepository _updatePointRepository;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CrawlIndexValuationJob> _logger;

        public CrawlIndexValuationJob(IHttpClientFactory clientFactory,
                                      IIndexValuationRepository indexValuationRepository,
                                      IUpdatePointRepository updatePointRepository,
                                      TimeProvider timeProvider,
                                      ILogger<CrawlIndexValuationJob> logger)
        {
            this._clientFactory = clientFactory;
            this._indexValuationRepository = indexValuationRepository;
            this._updatePointRepository = updatePointRepository;
            this._timeProvider = timeProvider;
            this._logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var ourValuations = await this._indexValuationRepository.GetAllAsync();
            var theirValuations = await CrawlIndexValuationAsync();
            foreach (var ourValuation in ourValuations)
            {
                var theirValuation = theirValuations.FirstOrDefault(v => v.Code == ourValuation.Code);
                ourValuation.DividendYield = theirValuation.DividendYield;
                ourValuation.ModifyTime = this._timeProvider.Now;
                ourValuation.PB = theirValuation.PB;
                ourValuation.PBPercentile = theirValuation.PBPercentile;
                ourValuation.PE = theirValuation.PE;
                ourValuation.PEPercentile = theirValuation.PEPercentile;
                ourValuation.ROE = theirValuation.ROE;
                ourValuation.Title = theirValuation.Title;

                await this._indexValuationRepository.UpdateAsync(ourValuation);
            }

            await UpdateLastUpdateTimeAsync();
        }

        private async Task<IEnumerable<IndexValuationEntity>> CrawlIndexValuationAsync()
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
                    var entity = ToIndexValuationEntity(item);
                    if (entity == null)
                    {
                        continue;
                    }

                    list.Add(entity);
                }

                return list;
            }
        }

        private IndexValuationEntity ToIndexValuationEntity(JToken item)
        {
            if (item == null)
            {
                return null;
            }

            return new IndexValuationEntity
            {
                Code = item["index_code"].ToString(),
                DividendYield = Convert.ToDecimal(item["yeild"]),
                PB = Convert.ToDecimal(item["pb"]),
                PBPercentile = Convert.ToDecimal(item["pb_percentile"]),
                PE = Convert.ToDecimal(item["pe"]),
                PEPercentile = Convert.ToDecimal(item["pe_percentile"]),
                PreparationTime = DateUtil.GetDateTime(Convert.ToInt64(item["begin_at"]), true),
                ROE = Convert.ToDecimal(item["roe"]),
                Title = item["name"].ToString()
            };
        }

        private Task<bool> UpdateLastUpdateTimeAsync()
        {
            var entity = new UpdatePointEntity
            {
                UpdateKey = GlobalConstant.IndexValuationKey,
                UpdateTime = this._timeProvider.Now,
                Enabled = true
            };

            return this._updatePointRepository.UpdateAsync(entity);
        }
    }
}
