using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LoadImage : MonoBehaviour
{
    public GameObject model;
    public GameObject panel;
    public string serverImagesUrl;

    private List<Texture> images = new List<Texture>();

    [System.Serializable]
    public class ImageData
    {
        public string id;
        public string name;
        public string path;
    }

    [System.Serializable]
    public class ImageDataList
    {
        public List<ImageData> images;
    }

    void Start()
    {
        StartCoroutine(GetTextures(serverImagesUrl));
    }

    IEnumerator GetTextures(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonString = request.downloadHandler.text;

            // Parse o JSON para uma lista de objetos ImageData
            ImageData[] imageDatas = JsonUtility.FromJson<ImageDataList>("{\"images\":" + jsonString + "}").images.ToArray();

            foreach (ImageData imageData in imageDatas)
            {
                yield return StartCoroutine(DownloadImage(imageData.path));
            }

            // Após obter todas as texturas, instancia os objetos
            InstanciarObject();
        }
        else
        {
            Debug.LogError("Erro ao obter as URLs das imagens: " + request.error);
        }
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture texture = DownloadHandlerTexture.GetContent(request);
            images.Add(texture);
        }
        else
        {
            Debug.LogError("Erro ao baixar imagem: " + request.error);
        }
    }

    void InstanciarObject()
    {
        foreach (Texture texture in images)
        {
            GameObject instancia = Instantiate(model, panel.transform);
            if (instancia != null)
            {
                RawImage rawImage = instancia.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                }
            }
        }
    }
}
