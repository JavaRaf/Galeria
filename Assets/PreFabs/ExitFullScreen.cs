using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitScrenn : MonoBehaviour, IPointerClickHandler
{
    public static RawImage texture;
    void Awake()
    {
        texture = GetComponent<RawImage>();
    }
    public static void ShowImage(Texture image)
    {
        texture.texture = image;
        texture.enabled = true;
    }
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
