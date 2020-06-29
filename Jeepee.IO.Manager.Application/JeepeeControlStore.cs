using Jeepee.IO.Core.Abstractions.Providers;
using Jeepee.IO.Core.Models.Models;
using Jeepee.IO.Manager.Application.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Application
{
    public class JeepeeControlStore : IJeepeeControlStore
    {
        private readonly INowProvider _nowProvider;
        private readonly long _maxTime;
        // tracks user associated with jeepee instances
        private readonly Dictionary<string, string> _userStore;
        // tracks instances and a timestamp for when the instance started being controlled (0 if not controlled)
        private readonly Dictionary<string, long> _instanceStore;

        private readonly object _syncObject;

        public JeepeeControlStore(IEnumerable<string> instanceIds, INowProvider nowProvider, TimeSpan maxTime)
        {
            _userStore = new Dictionary<string, string>();
            _instanceStore = new Dictionary<string, long>(instanceIds.Select(id => new KeyValuePair<string, long>(id, 0)));
            _nowProvider = nowProvider;
            _maxTime = maxTime.Ticks;
            _syncObject = new object();
        }

        private bool TimeExpired(long time) => _nowProvider.Now.Ticks - time > _maxTime;
        private void TouchInstance(string jeepeeId)
        {
            var instanceTime = _instanceStore[jeepeeId];
            // check timestamps for instance
            if (instanceTime > 0 && TimeExpired(instanceTime))
            {
                // reset to zero and remove _userStore record 
                _instanceStore[jeepeeId] = 0;
                var userStoreRecord = _userStore.FirstOrDefault(u => u.Value == jeepeeId);

                if (userStoreRecord.Key != default)
                {
                    _userStore.Remove(userStoreRecord.Key);
                }
            }
        }
        private bool InnerInstanceAvailable(string jeepeeId)
        {
            TouchInstance(jeepeeId);
            // return if the instance is in use
            return _instanceStore[jeepeeId] == 0;
        }

        public bool InstanceAvailable(string jeepeeId)
        {
            lock(_syncObject)
            {
                return InnerInstanceAvailable(jeepeeId);
            }
        }

        public void SetControl(string userIdent, string jeepeeId)
        {
            lock(_syncObject)
            {
                if(InnerInstanceAvailable(jeepeeId) && InnerGetInstanceForUser(userIdent) == null)
                {
                    _instanceStore[jeepeeId] = _nowProvider.Now.Ticks;
                    _userStore.TryAdd(userIdent, jeepeeId);
                }
            }
        }

        private string InnerGetInstanceForUser(string userIdent)
        {
            var userExists = _userStore.TryGetValue(userIdent, out var instance);
            if(!userExists)
            {
                return null;
            }

            TouchInstance(instance);
            // try get again
            userExists = _userStore.TryGetValue(userIdent, out instance);
            if(!userExists)
            {
                return null;
            }

            return instance;
        }

        private string InnerGetUserForInstance(string jeepeeId)
        {
            TouchInstance(jeepeeId);
            return _userStore.FirstOrDefault(u => u.Value == jeepeeId).Key;
        }

        public void KickUser(string userIdent)
        {
            lock(_syncObject) 
            {
                var instance = InnerGetInstanceForUser(userIdent);

                if(instance != null)
                {
                    _instanceStore.Remove(instance);
                    _userStore.Remove(userIdent);
                }
            }
        }

        public void ClearInstance(string jeepeeId)
        {
            lock(_syncObject)
            {
                var user = InnerGetUserForInstance(jeepeeId);

                if(user != null)
                {
                    _userStore.Remove(user);
                    _instanceStore.Remove(jeepeeId);
                }
            }
        }

        public string GetInstanceForUser(string userIdent)
        {
            lock(_syncObject)
            {
                return InnerGetInstanceForUser(userIdent);
            }
        }

        public string GetUserForInstance(string jeepeeId)
        {
            lock(_syncObject)
            {
                return InnerGetUserForInstance(jeepeeId);
            }
        }
    }
}
