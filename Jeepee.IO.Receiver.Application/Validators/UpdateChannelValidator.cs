using FluentValidation;
using Jeepee.IO.Receiver.Application.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Validators
{
    public class UpdateChannelValidator : AbstractValidator<UpdateChannel>
    {
        public UpdateChannelValidator(IConfiguration configuration)
        {
            RuleFor(model => model.Channel).GreaterThan(-1);
            RuleFor(model => model.Channel).LessThan(model => configuration.GetSection("Channels").AsEnumerable().Count());
        }
    }
}
