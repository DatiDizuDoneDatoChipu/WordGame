using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyBoardMethods : MonoBehaviour
{
    TextMeshProUGUI ownText;

    public LoadManager LoadManager;

    public TextMeshProUGUI changeText;

    public Sprite newChar;
    public Sprite oldChar;

    public GameObject rightChar;
    public GameObject wrongChar;

    public GameObject wrongWord;


    public GameObject scrollView;

    public GameObject wordPrefab;

    public List<UnityEngine.UI.Button> keyUsed = new List<UnityEngine.UI.Button>();

    public string _dictionaryPath = Path.Combine(Application.streamingAssetsPath, "russian_nouns.txt");

    List<string> wordHistory = new List<string>();

    public YandexDictionaryManager YandexDictionaryManager;

    public GameObject historyParent;

    public bool isWordValid;
    private void Start()
    {
        ownText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // �����, ������� ����� ���������� ��� ������� �� ������
    public void OnKeyPressed(UnityEngine.UI.Button button)
    {
        // �������� ����� � ������ (��� TextMeshPro)
        TextMeshProUGUI keyText = button.GetComponentInChildren<TextMeshProUGUI>();


        if (keyText != null)
        {
            Debug.Log(keyText.text);
            changeText.text += keyText.text; // ��������� ������
            keyUsed.Add(button);
        }
        else
        {
            Debug.Log("sasai");
        }
    }

    public void DeleteLastCharacter()
    {
        if (changeText.text.Length > 0)
        {
            changeText.text = changeText.text.Substring(0, changeText.text.Length - 1);
        }
    }

    public void CheckButton()
    {
        
        if (changeText != null)
        {
            Debug.Log("����� �� ����");
            string myWord = changeText.text;
            Debug.Log("����� " + myWord);

            if (LoadManager.word.ToLower().Equals(myWord.ToLower()))
            {
                CheckLetters(myWord);
            }
            else
            {
                YandexDictionaryManager.SearchWord(myWord, (isFound) =>
                {
                    isWordValid = isFound; // ��������� ����������� �����
                    Debug.Log("� ����� �����" + isWordValid);

                    if (isWordValid)
                    {
                        Debug.Log(wordHistory.Count);
                        CheckLetters(myWord);
                    }
                    else
                    {
                        wrongWord.SetActive(true);
                        StartCoroutine(HideWrongWord());
                    }
                });
                /*
                if (isWordValid)
                {
                    Debug.Log(wordHistory.Count);
                    CheckLetters(myWord);
                }
                */
            }
            changeText.text = "";
        }
    }

    IEnumerator HideWrongWord()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("������ 3 �������!");
        wrongWord.SetActive(false);

    }

    public void CheckLetters(string letter)
    {
        AddHistory(letter);
        foreach (List<GameObject> word in LoadManager.wordList)
        {
            foreach (GameObject go in word)
            {
                GameObject child = go.GetComponent<Transform>().GetChild(0).gameObject;
                Debug.Log("���" + child.name);
                Debug.Log("�����" + child.GetComponent<TextMeshProUGUI>().text);
                if (letter.Contains(child.GetComponent<TextMeshProUGUI>().text))
                {
                    go.GetComponent<Image>().sprite = newChar;
                    foreach(Button temp in keyUsed)
                    {
                        Debug.Log("����� ������� ���������" + letter);
                        Debug.Log("����� � ������� ���������" + temp.GetComponentInChildren<TextMeshProUGUI>().text);

                        if (LoadManager.word.ToLower().Contains(temp.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()))
                        {
                            temp.GetComponent<Image>().sprite = newChar;
                        }
                        else
                        {
                            temp.GetComponent<Image>().sprite = oldChar;
                        }
                    }
                    child.SetActive(true);
                }
                else
                {
                    foreach (Button temp in keyUsed)
                    {
                        Debug.Log("����� ������� ���������" + letter);
                        Debug.Log("����� � ������� ���������" + temp.GetComponentInChildren<TextMeshProUGUI>().text);

                        if (LoadManager.word.ToLower().Contains(temp.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()))
                        {
                            temp.GetComponent<Image>().sprite = newChar;
                        }
                        else
                        {
                            temp.GetComponent<Image>().sprite = oldChar;
                        }
                    }
                }
            }
        }
    }

    public void AddHistory(string myWord)
    {
        for (int i = 0; i < historyParent.transform.childCount; i++)
        {
            Destroy(historyParent.transform.GetChild(i).gameObject);
        }
        wordHistory.Add(myWord);

        foreach (char c in myWord)
        {
            if(LoadManager.word.ToLower().Contains(c.ToString().ToLower()))
            {
                GameObject letter = GameObject.Instantiate(rightChar, historyParent.transform);
                letter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            }
            else
            {
                GameObject letter = GameObject.Instantiate(wrongChar, historyParent.transform);
                letter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            }
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("LogScreen");
    }

    public bool IsWordExists(string targetWord)
    {
        // �������� ��������� ��������
        bool result = false;

        // ��������� �������� � ���� �� ����������
        System.Threading.Tasks.Task.Run(async () =>
        {
            result = await CheckWordExistsAsync(targetWord);
        }).Wait();

        return result;
    }

    private async System.Threading.Tasks.Task<bool> CheckWordExistsAsync(string targetWord)
    {
        string path = _dictionaryPath;

        // ��� Android ��������� �������
#if UNITY_ANDROID && !UNITY_EDITOR
    path = "jar:file://" + path;
#endif

        try
        {
            using (UnityWebRequest request = UnityWebRequest.Get(path))
            {
                var asyncOp = request.SendWebRequest();
                while (!asyncOp.isDone)
                    await System.Threading.Tasks.Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string[] allWords = request.downloadHandler.text.Split('\n');
                    return allWords.Any(word =>
                        word.Trim().ToLower() == targetWord.Trim().ToLower());
                }
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"������: {e.Message}");
            return false;
        }
    }
}
