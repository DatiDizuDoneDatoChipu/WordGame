using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class YandexDictionaryManager : MonoBehaviour
{
    private const string API_KEY = "dict.1.1.20250218T180351Z.0103a23072d543e7.223b1332883b08d4bb866e41abfb965d7b52c318";
    private const string BASE_URL = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key=" + API_KEY + "&lang=ru-ru&text=";
    private string FILE_PATH => Path.Combine(Application.persistentDataPath, "word.json");

    public delegate void OnSearchCompleted(bool isWordFound);

    public void SearchWord(string word, OnSearchCompleted callback)
    {
        StartCoroutine(GetWordData(word.ToLower(), callback));
    }

    private IEnumerator GetWordData(string word, OnSearchCompleted callback)
    {
        string url = BASE_URL + UnityWebRequest.EscapeURL(word);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                File.WriteAllText(FILE_PATH, jsonResponse);
                bool isFound = ParseResponse(jsonResponse);
                callback?.Invoke(isFound);
            }
            else
            {
                Debug.LogError("Ошибка: " + request.error);
                callback?.Invoke(false);
            }
        }
    }

    private bool ParseResponse(string json)
    {
        try
        {
            YandexDictionaryResponse response = JsonUtility.FromJson<YandexDictionaryResponse>(json);
            return response.def.Length > 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка парсинга: {e.Message}");
            return false;
        }
    }
}