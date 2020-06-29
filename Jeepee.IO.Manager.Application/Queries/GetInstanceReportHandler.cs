using Jeepee.IO.Core.Models.DTOs;
using Jeepee.IO.Core.Models.Models;
using Jeepee.IO.Manager.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Application.Queries
{
    public class GetInstanceReportHandler : IRequestHandler<GetInstanceReportRequest, IEnumerable<JeepeeInstanceReportDTO>>
    {
        private readonly IJeepeeInstanceInfoStore _infoStore;
        private readonly IJeepeeControlStore _jeepeeControlStore;

        public GetInstanceReportHandler(IJeepeeInstanceInfoStore infoStore, IJeepeeControlStore jeepeeControlStore)
        {
            _infoStore = infoStore;
            _jeepeeControlStore = jeepeeControlStore;
        } 

        public async Task<IEnumerable<JeepeeInstanceReportDTO>> Handle(GetInstanceReportRequest request, CancellationToken cancellationToken)
        {
            var successfulPings = new List<JeepeeInstanceReportDTO>();
            var client = new HttpClient();

            foreach (var instance in _infoStore.GetAll())
            {
                var online = false;
                try
                {
                    var result = await client.GetAsync($"{instance.URL}/ping");
                    online = result.StatusCode == System.Net.HttpStatusCode.OK;
                }
                catch
                {
                    // log but don't throw as we can continue
                }

                successfulPings.Add(new JeepeeInstanceReportDTO
                {
                    Available = _jeepeeControlStore.InstanceAvailable(instance.Id),
                    Id = instance.Id,
                    Online = online
                });
            }

            return successfulPings;
        }
    }
}
