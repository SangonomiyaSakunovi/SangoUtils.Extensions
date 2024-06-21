using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SangoUtils.Behaviours_Unity.UGUIOPs
{
    public class UIPointerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameObject? ListenerObject { get; set; }
        public object[]? Commands { get; set; } = null;

        public Vector2 ClickDownPosition { get; set; } = Vector2.zero;
        public Vector2 ClickUpPosition { get; set; } = Vector2.zero;

        public Action<GameObject?, object[]?>? OnPointerClickCallBack0 { get; set; }
        public Action<GameObject?, object[]?>? OnPointerDownCallBack0 { get; set; }
        public Action<GameObject?, object[]?>? OnPointerUpCallBack0 { get; set; }
        public Action<GameObject?, object[]?>? OnPointerDragCallBack0 { get; set; }

        public Action<PointerEventData, GameObject?, object[]?>? OnPointerClickCallBack1 { get; set; }
        public Action<PointerEventData, GameObject?, object[]?>? OnPointerDownCallBack1 { get; set; }
        public Action<PointerEventData, GameObject?, object[]?>? OnPointerUpCallBack1 { get; set; }
        public Action<PointerEventData, GameObject?, object[]?>? OnPointerDragCallBack1 { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickCallBack0?.Invoke(ListenerObject, Commands);
            OnPointerClickCallBack1?.Invoke(eventData, ListenerObject, Commands);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownCallBack0?.Invoke(ListenerObject, Commands);
            OnPointerDownCallBack1?.Invoke(eventData, ListenerObject, Commands);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpCallBack0?.Invoke(ListenerObject, Commands);
            OnPointerUpCallBack1?.Invoke(eventData, ListenerObject, Commands);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnPointerDragCallBack0?.Invoke(ListenerObject, Commands);
            OnPointerDragCallBack1?.Invoke(eventData, ListenerObject, Commands);
        }
    }
}