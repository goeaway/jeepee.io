using Jeepee.IO.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Application.Abstractions
{
    public interface IJeepeeControlStore
    {
        /// <summary>
        /// Returns the instance id the provided user is controlling. Returns null if user is not controlling an instance.
        /// </summary>
        /// <param name="userIdent"></param>
        /// <returns></returns>
        string GetInstanceForUser(string userIdent);
        /// <summary>
        /// Returns user id the provided instance is being controlled by. Returns null if instance is not being controlled.
        /// </summary>
        /// <param name="jeepeeId"></param>
        /// <returns></returns>
        string GetUserForInstance(string jeepeeId);
        /// <summary>
        /// Removes control of an instance that the provided user was controlling. Does nothing if the user was not controlling anything.
        /// </summary>
        /// <param name="userIdent"></param>
        void KickUser(string userIdent);
        /// <summary>
        /// Removes user that was controlling the specified instance. Does nothing if the instance was not being controlled.
        /// </summary>
        /// <param name="jeepeeId"></param>
        void ClearInstance(string jeepeeId);
        /// <summary>
        /// Sets the specified user as the controller of an instance. Does nothing if the user is already controlling another instance or the instance is already being controlled by another user.
        /// </summary>
        /// <param name="userIdent"></param>
        /// <param name="jeepeeId"></param>
        void SetControl(string userIdent, string jeepeeId);
        /// <summary>
        /// Returns true if the specified instance is not being controlled by any users.
        /// </summary>
        /// <param name="jeepeeId"></param>
        /// <returns></returns>
        bool InstanceAvailable(string jeepeeId);
    }
}
