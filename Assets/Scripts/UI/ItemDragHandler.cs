using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Guardasmos la posición inicial
        transform.SetParent(transform.root); //
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // El objeto es semitransparente durante el movimiento
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Sigue el movimiento del ratón
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Habilita raycast
        canvasGroup.alpha = 1.0f; // Al soltarlo se vuelve sólido

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); // Slot donde se deja caer
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                // El slot contiene otro objeto y debemos intercambiarlos
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            //Movemos el objeto al slot donde lo dejamos caer
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // No hay slot debajo del punto en el que soltamos
            transform.SetParent(originalParent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centramos
    }

}
