using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject fullscrenn = GameObject.FindWithTag("FullImage");

        if (fullscrenn.TryGetComponent<RectTransform>(out var rectTransform))
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                rectTransform.anchoredPosition = Vector2.zero;

            }
 
        }

    }
}
