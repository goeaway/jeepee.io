using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Core.Models.DTOs
{
    public class JeepeeInstanceReportDTO
    {
        public string Id { get; set; }
        public bool Available { get; set; }
        public bool Online { get; set; }
    }
}
