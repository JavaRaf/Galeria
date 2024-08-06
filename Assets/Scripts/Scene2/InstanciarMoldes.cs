using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoadImage : MonoBehaviour
{
    public GameObject prefab;    
    public GameObject panel;    
    public string serverImagesUrl = "http://localhost:3000/images";

    public string imagesEndPoint = "http://localhost:3000/uploads";

    private List<Texture> images = new List<Texture>();

    [Serializable]
    public class ImageData
    {
        public int id;
        public string filename;
        public int size;
        public string created_at;
    }

    [Serializable]
    public class ImageDataList
    {
        public string message;
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

        Debug.Log("request: " + request);

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonString = request.downloadHandler.text;

            ImageDataList imageDataList = JsonUtility.FromJson<ImageDataList>(jsonString);

            foreach (ImageData imageData in imageDataList.images)
            {
                string imageUrl = $"{imagesEndPoint}/{imageData.filename}"; 
                yield return StartCoroutine(DownloadImage(imageUrl));
            }

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
            GameObject instancia = Instantiate(prefab, panel.transform);
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
