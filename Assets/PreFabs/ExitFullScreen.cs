using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitScrenn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject fullscrenn = GameObject.FindWithTag("FullImage");

        if (fullscrenn.TryGetComponent<RectTransform>(out var rectTransform))
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                rectTransform.anchoredPosition = new Vector2(0, 1500);
            }
 
        }

    }
}
