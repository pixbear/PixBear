using UnityEngine;
using UnityEngine.EventSystems;
using PB.SYSTEM;

namespace PB.CONTROLLER
{
    public class PJoy : MonoBehaviour
    {
        [SerializeField] bool dynamicPos = true;
        [SerializeField] RectTransform bg;
        [SerializeField] RectTransform handle;

        public bool IsCanControl { get; private set; } = true; 
        public Vector2 InputVector => inputVector;
        private Vector2 inputVector = Vector2.zero;
        private float handleMoveRange;
        private Vector2 handleStartPos;

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
                inputVector = Vector2.zero;
                bg.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            handleStartPos = bg.anchoredPosition;

            float bgRadius = bg.sizeDelta.x * 0.5f;
            float handleRadius = handle.sizeDelta.x * 0.5f;
            handleMoveRange = bgRadius - handleRadius;

            if (dynamicPos)
            {
                bg.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (IsCanControl == false) return;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Vector2 localPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, touch.position, null, out localPos);
                    handle.anchoredPosition = localPos;
                    inputVector = localPos.normalized;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    handle.anchoredPosition = Vector2.zero;
                    inputVector = Vector2.zero;
                }
            }
        }
    }
}
