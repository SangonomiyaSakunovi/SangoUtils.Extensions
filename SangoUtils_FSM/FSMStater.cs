﻿using System;
using System.Collections.Generic;

namespace SangoUtils.FSMs
{
    public class FSMStater<T> : FSMStaterBase where T : struct
    {
        private readonly Action<T, T>? _transCallBack;
        public T CurrentState { get; private set; }

        private readonly Dictionary<T, List<FSMStaterItem<T>>> _transToOtherStateDict;
        private readonly List<FSMStaterItem<T>> _transToOneStateList;

        private bool _isProcessingTransition = false;
        private readonly Queue<FSMTransCommandBase> _transCommandQueue;

        public FSMStater(object owner, Action<T, T>? transCallBack = null)
        {
            Owner = owner;
            _blackboard = new Dictionary<string, object>();
            _transToOtherStateDict = new Dictionary<T, List<FSMStaterItem<T>>>();
            _transToOneStateList = new List<FSMStaterItem<T>>();
            _transCommandQueue = new Queue<FSMTransCommandBase>();
            _transCallBack = transCallBack;
        }

        public object Owner { get; }

        public void AddLocalTransition(T currentState, FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
        {
            FSMStaterItem<T> item = new FSMStaterItem<T>(command, targetState, callBack);
            _transToOtherStateDict.TryGetValue(currentState, out List<FSMStaterItem<T>> itemList);
            if (itemList == null)
            {
                itemList = new List<FSMStaterItem<T>>();
                _transToOtherStateDict.Add(currentState, itemList);
            }
            itemList.Add(item);
        }

        public void AddGlobalTransition(FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
        {
            FSMStaterItem<T> item = new FSMStaterItem<T>(command, targetState, callBack);
            _transToOneStateList.Add(item);
        }

        public void InvokeInitState(T initialState)
        {
            CurrentState = initialState;
        }

        public void InvokeTransition(FSMTransCommandBase command)
        {
            if (_isProcessingTransition)
            {
                _transCommandQueue.Enqueue(command);
                return;
            }
            _isProcessingTransition = true;

            bool result = false;

            _transToOtherStateDict.TryGetValue(CurrentState, out List<FSMStaterItem<T>> itemList);
            if (itemList != null)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    result = ProcessTransition(itemList[i], command);
                    if (result)
                    {
                        break;
                    }
                }
            }

            if (!result)
            {
                for (int i = 0; i < _transToOneStateList.Count; i++)
                {
                    result = ProcessTransition(_transToOneStateList[i], command);
                    if (result)
                    {
                        break;
                    }
                }
            }

            _isProcessingTransition = false;

            if (_transCommandQueue.Count > 0)
            {
                FSMTransCommandBase item = _transCommandQueue.Dequeue();
                InvokeTransition(item);
            }
        }

        private bool ProcessTransition(FSMStaterItem<T> item, FSMTransCommandBase command)
        {
            bool result = false;
            if (item.TransCommand.Equals(command))
            {
                if (item.TransCallBack != null)
                {
                    result = item.TransCallBack(CurrentState, command, item.TargetState);
                }
                else
                {
                    result = true;
                }

                if (result)
                {
                    T previousState = CurrentState;
                    CurrentState = item.TargetState;
                    _transCallBack?.Invoke(previousState, CurrentState);
                }
            }
            return result;
        }

        public void ClearTransCommandList()
        {
            _transCommandQueue.Clear();
        }
    }
}