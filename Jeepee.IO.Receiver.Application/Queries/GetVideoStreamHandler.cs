using Jeepee.IO.Receiver.Application.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Queries
{
    public class GetVideoStreamHandler : IRequestHandler<GetVideoStream, Stream>
    {
        private readonly ISystem _system;

        public GetVideoStreamHandler(ISystem system)
        {
            _system = system;
        }

        public Task<Stream> Handle(GetVideoStream request, CancellationToken cancellationToken)
        {
            return Task.Run(() => new MemoryStream() as Stream);
        }
    }
}
