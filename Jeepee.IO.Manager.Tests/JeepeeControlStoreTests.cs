using Jeepee.IO.Manager.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Tests
{
    [TestClass]
    [TestCategory("Jeepee Control Store")]
    public class JeepeeControlStoreTests
    {
        /* 
         InstanceAvailable -
            returns true when no users have ever been associated with instance
            returns true when user is associated with instance, but time has expired
            returns false when user is associated with instance and time is not expired
         
         GetInstanceForUser -
            returns instance id of instance a user is associated with
            returns null when user has never been associated with an instance
            returns null when user is associated with an instance but time is expired

        GetUserForInstance -
            returns user ident that is associated with instance
            returns null when instance has never been used by user
            returns null when instance has been used by user but time has expired

        KickUser -
            does nothing if user not associated with instance
            instance becomes available after user is kicked from instance
            user associated with different instance after kick and user changes control

        ClearInstance -
            does nothing if instance is clear
            instance becomes available after clear
            user that was controlling this instance is no longer associated after clear
        
        SetControl -
            user does not become associated with instance if user already associated with non expired instance
            user does not become associated with instance if instance already controlled by another user
            user becomes associated with instance otherwise

         */
    }
}
