using Jeepee.IO.Core.Models.DTOs;
using MediatR;
using System.Collections;
using System.Collections.Generic;

namespace Jeepee.IO.Manager.Application.Queries
{
    public class GetInstanceReportRequest : IRequest<IEnumerable<JeepeeInstanceReportDTO>>
    {

    }
}