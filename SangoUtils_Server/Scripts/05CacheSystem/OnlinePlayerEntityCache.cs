using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using System.Collections.Concurrent;

namespace SangoUtils_Server
{
    public class OnlinePlayerEntityCache : BaseCache<OnlinePlayerEntityCache>
    {
        private ConcurrentDictionary<ClientPeer, BaseObjectEntity> _playerEntityClientPeerDict = new();
        private ConcurrentDictionary<string, BaseObjectEntity> _playerEntityIDDict = new();

        public bool AddLoginPlayerEntity(ClientPeer peer, BaseObjectEntity entity)
        {
            if (!_playerEntityClientPeerDict.ContainsKey(peer))
            {
                if (!_playerEntityClientPeerDict.TryAdd(peer, entity))
                {
                    SangoLogger.Error("A player entity can`t join the PlayerEntityClientPeerDict.");
                    return false;
                }
                if (!_playerEntityIDDict.TryAdd(entity.EntityID, entity))
                {
                    SangoLogger.Error("A player entity can`t join the PlayerEntityIDDict.");
                    return false;
                }
                return true;
            }
            else
            {
                SangoLogger.Error("The client already exist in entityDict");
                return false;
            }
        }

        public BaseObjectEntity? GetPlayerEntityByClientPeer(ClientPeer peer)
        {
            if (_playerEntityClientPeerDict.TryGetValue(peer, out BaseObjectEntity? entity))
            {
                return entity;
            }
            else
            {
                SangoLogger.Error($"The peerID: [ {peer.PeerId} ] is not exist in PlayerEntityClientPeerDict.");
                return null;
            }
        }

        public BaseObjectEntity? GetPlayerEntityByEntityID(string entityID)
        {
            if (_playerEntityIDDict.TryGetValue(entityID, out BaseObjectEntity? entity))
            {
                return entity;
            }
            else
            {
                SangoLogger.Error($"The peerID: [ {entityID} ] is not exist in PlayerEntityClientPeerDict.");
                return null;
            }
        }

        public bool RemovePlayerEntity(ClientPeer peer)
        {
            if (_playerEntityClientPeerDict.TryRemove(peer, out BaseObjectEntity? entity))
            {
                if (!_playerEntityIDDict.TryRemove(entity.EntityID, out var value))
                {
                    SangoLogger.Error($"The peerID: [ {peer.PeerId} ] can`t remove from PlayerEntityIDDict.");
                    return false;
                }
                return true;
            }
            else
            {
                SangoLogger.Error($"The peerID: [ {peer.PeerId} ] can`t remove from PlayerEntityClientPeerDict.");
                return false;
            }
        }
    }
}
