using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Queries
{
    public class GetVideoStream : IRequest<Stream>
    {
        public GetVideoStream()
        {

        }
    }
}
