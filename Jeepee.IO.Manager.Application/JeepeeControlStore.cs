using Jeepee.IO.Core.Models.Models;
using Jeepee.IO.Manager.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Jeepee.IO.Manager.Application
{
    public class JeepeeControlStore : IJeepeeControlStore
    {
        // have a dictionary including all possible jeepees (start from config file) (this class doesn't need to ping them ever)
        // in dictionary store user controlling and time left
        // have a thread checking time left?

        // interface methods simply interact with the dictionary
        // implementation must handle concurrent changes!
        // use ConcurrentDictionary
        
        public bool InstanceAvailable(string jeepeeId)
        {
            throw new NotImplementedException();
        }

        public void RevokeControl(string userIdent)
        {
            throw new NotImplementedException();
        }

        public void SetControl(string userIdent, string jeepeeId)
        {
            throw new NotImplementedException();
        }

        public JeepeeInstanceInfo UserControl(string userIdent)
        {
            throw new NotImplementedException();
        }
    }
}
