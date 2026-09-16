using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace PB.CONTROLLER
{
    public class PJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] bool dynamicPos = true;
        [SerializeField] RectTransform bg;
        [SerializeField] RectTransform handle;

        public bool IsCanControl { get; private set; } = false; 
        public event Action<Vector2> OnInputChanged;

        private float handleMoveRange;
        private Vector2 startPos;

        public void EnableControl(bool enable)
        {
            IsCanControl = enable;

            if (IsCanControl)
            {
                if (!dynamicPos)
                {
                    bg.gameObject.SetActive(true);
                }
            }
            else
            {
                handle.anchoredPosition = Vector2.zero;
                OnInputChanged?.Invoke(Vector2.zero);
                bg.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            startPos = bg.anchoredPosition;

            float bgRadius = bg.sizeDelta.x * 0.5f;
            float handleRadius = handle.sizeDelta.x * 0.5f;
            handleMoveRange = bgRadius - handleRadius;

            if (dynamicPos)
            {
                bg.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsCanControl) return;

            if (dynamicPos)
            {
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    bg.parent as RectTransform, 
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPos
                );
                bg.anchoredPosition = localPos;
                bg.gameObject.SetActive(true);
            }

            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsCanControl) return;

            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, eventData.position, eventData.pressEventCamera, out pos))
            {
                pos = Vector2.ClampMagnitude(pos, handleMoveRange);
                handle.anchoredPosition = pos;
                var inputVector = pos / handleMoveRange;
                OnInputChanged?.Invoke(inputVector);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsCanControl) return;

            OnInputChanged?.Invoke(Vector2.zero);
            handle.anchoredPosition = Vector2.zero;

            if (dynamicPos)
            {
                bg.gameObject.SetActive(false);
            }
            else
            {
                bg.anchoredPosition = startPos;
            }
        }
    }
}
