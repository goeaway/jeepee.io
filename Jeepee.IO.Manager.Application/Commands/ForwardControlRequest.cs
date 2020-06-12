using Jeepee.IO.Core.Models.DTOs;
using MediatR;

namespace Jeepee.IO.Manager.Application.Commands
{
    public class ForwardControlRequest : IRequest
    {
        public string UserIdent { get; set; }
        public JeepeeControlDTO DTO { get; set; }

        public ForwardControlRequest(string userIdent, JeepeeControlDTO dto)
        {
            UserIdent = userIdent;
            DTO = dto;
        }
    }
}