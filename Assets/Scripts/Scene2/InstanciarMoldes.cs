using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class LoadImage : MonoBehaviour
{
    public GameObject prefab;    
    public GameObject panel;    
    public string serverUrl = "http://localhost:3000/images";
    public string imagesEndPoint = "http://localhost:3000/uploads";

    private ImageDataList dataList;

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
        StartCoroutine(GetConnectionString());
    }

    IEnumerator GetConnectionString()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonString = request.downloadHandler.text;
            dataList = JsonUtility.FromJson<ImageDataList>(jsonString);
            // Chama a função passUrl após o download dos dados
            StartCoroutine(PassUrl());
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    IEnumerator PassUrl()
    {
        foreach (var data in dataList.images)
        {
            string imageUrl = $"{imagesEndPoint}/{data.filename}";
            yield return StartCoroutine(DownloadImage(imageUrl, data.filename));
        }
    }

    IEnumerator DownloadImage(string imageUrl, string filename)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture texture = DownloadHandlerTexture.GetContent(request);
            // Instanciar o prefab
            GameObject newObject = Instantiate(prefab, panel.transform);
            // Definir a textura no componente RawImage do prefab
            RawImage rawImageComponent = newObject.GetComponent<RawImage>();
            TextMeshProUGUI filenameText = newObject.GetComponentInChildren<TextMeshProUGUI>();
            if (rawImageComponent != null && filenameText != null)
            {
                rawImageComponent.texture = texture;
                filenameText.text = filename;
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
