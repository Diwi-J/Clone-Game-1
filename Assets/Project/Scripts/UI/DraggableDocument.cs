using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    /// <summary>
    /// Component placed on UI document panels to allow mouse/touch dragging across the booth desk canvas,
    /// bringing active document to front on click/drag, and clamping position to screen bounds.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableDocument : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;

        [Header("Drag Settings")]
        [SerializeField] private bool clampToCanvas = true;
        [SerializeField] private float dragAlpha = 0.9f;

        private float originalAlpha = 1f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Bring active document to front of desk view
            rectTransform.SetAsLastSibling();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canvasGroup != null)
            {
                originalAlpha = canvasGroup.alpha;
                canvasGroup.alpha = dragAlpha;
                canvasGroup.blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canvas == null) return;

            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            if (clampToCanvas)
            {
                ClampToScreen();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = originalAlpha;
                canvasGroup.blocksRaycasts = true;
            }
        }

        private void ClampToScreen()
        {
            if (canvas == null) return;

            RectTransform canvasRect = canvas.transform as RectTransform;
            if (canvasRect == null) return;

            Vector3[] canvasCorners = new Vector3[4];
            canvasRect.GetWorldCorners(canvasCorners);

            Vector3[] docCorners = new Vector3[4];
            rectTransform.GetWorldCorners(docCorners);

            Vector3 pos = rectTransform.position;

            // Clamp horizontal
            if (docCorners[0].x < canvasCorners[0].x)
                pos.x += canvasCorners[0].x - docCorners[0].x;
            if (docCorners[2].x > canvasCorners[2].x)
                pos.x -= docCorners[2].x - canvasCorners[2].x;

            // Clamp vertical
            if (docCorners[0].y < canvasCorners[0].y)
                pos.y += canvasCorners[0].y - docCorners[0].y;
            if (docCorners[2].y > canvasCorners[2].y)
                pos.y -= docCorners[2].y - canvasCorners[2].y;

            rectTransform.position = pos;
        }
    }
}
