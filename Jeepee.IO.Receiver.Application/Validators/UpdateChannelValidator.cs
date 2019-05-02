using FluentValidation;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Validators
{
    public class UpdateChannelValidator : AbstractValidator<UpdateChannel>
    {
        public readonly ISystem _system;

        public UpdateChannelValidator(ISystem system)
        {
            _system = system;

            RuleFor(model => model.Channel).GreaterThan(-1);
            RuleFor(model => model.Channel).LessThan(model => _system.Channels.Count);
        }
    }
}
