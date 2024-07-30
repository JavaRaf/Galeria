using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ImageClick : MonoBehaviour, IPointerClickHandler 
{
    public string sceneName = "Images"; // nome da proxima cena
    public void OnPointerClick(PointerEventData eventData)
    {
       SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
