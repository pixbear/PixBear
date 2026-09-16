using UnityEngine;
using UnityEngine.EventSystems;

// Attach this to a UI object (with a RectTransform) to make it draggable.
[RequireComponent(typeof(CanvasGroup))]
public class UIDragAndDropTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If no drop zone changes the parent, put the item back where it began.
        if (transform.parent == canvas.transform)
            transform.SetParent(originalParent, true);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}