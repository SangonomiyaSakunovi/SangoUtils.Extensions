using SangoUtils.Extensions_Unity.Service;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils.Extensions_Unity.UnityWebRequestNet
{
    internal class UnityWebResourceLoader_RawImage
    {
        private uint _packId = 1;

        private readonly Dictionary<uint, UnityWebResourceRawImagePack> _packDict = new Dictionary<uint, UnityWebResourceRawImagePack>();
        private const string _packIdLock = "ResourceRawImageLoader_Lock";

        public uint AddPack(RawImage targetRawImage, string urlPath, bool isCahce, Action<string, Texture> onLoadCallBack, Action<object[]> completeCallBack, Action<object[]> canceledCallBack, Action<object[]> erroredCallBack)
        {
            uint tempPackId = GeneratePackId();
            UnityWebResourceRawImagePack pack = new UnityWebResourceRawImagePack()
            {
                PackId = tempPackId,
                Url = urlPath,
                TargetRawImage = targetRawImage,
                ResourceType = UnityWebResourceType.RawImage,
                OnCompleteCallBack = completeCallBack,
                OnCanceledCallBack = canceledCallBack,
                OnErroredCallBack = erroredCallBack,
                OnCompleteInvokePackRemoveCallBack = RemovePackCallBack
            };
            if (isCahce)
            {
                pack.OnLoadCallBack = onLoadCallBack;
            }
            _packDict.Add(tempPackId, pack);
            UnityWebRequestService.Instance?.HttpResource(pack);
            return tempPackId;
        }

        public bool RemovePack(uint packId)
        {
            if (_packDict.TryGetValue(packId, out var value))
            {
                value.OnCanceled();
                _packDict.Remove(packId);
                return true;
            }
            return false;
        }

        protected uint GeneratePackId()
        {
            lock (_packIdLock)
            {
                while (true)
                {
                    ++_packId;
                    if (_packId == uint.MaxValue)
                    {
                        _packId = 1;
                    }
                    if (!_packDict.ContainsKey(_packId))
                    {
                        return _packId;
                    }
                }
            }
        }

        public bool RemovePackCallBack(uint packId)
        {
            if (_packDict.TryGetValue(packId, out var value))
            {
                _packDict.Remove(packId);
                return true;
            }
            return false;
        }
    }
}
