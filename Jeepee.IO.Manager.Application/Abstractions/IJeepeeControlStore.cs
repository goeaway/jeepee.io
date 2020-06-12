using Jeepee.IO.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Manager.Application.Abstractions
{
    public interface IJeepeeControlStore
    {
        /// <summary>
        /// Returns info on the instance this user is authorised to control. If the user is not authorised to control an instance, null is returned
        /// </summary>
        /// <param name="userIdent"></param>
        /// <returns></returns>
        JeepeeInstanceInfo UserControl(string userIdent);
        void RevokeControl(string userIdent);
        void SetControl(string userIdent, string jeepeeId);
        bool InstanceAvailable(string jeepeeId);
    }
}
