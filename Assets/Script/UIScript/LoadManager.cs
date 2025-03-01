using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LoadManager : MonoBehaviour
{
    public GameObject ButtonPrefab;

    public CodeConverter codeConverter;

    public GameObject WordHolder;

    public GameObject parent;

    public GameObject prefabWord;

    public GameObject NoCharImage;
    public GameObject CharImage;

    int stringCount;

    public string word;
    public string tempString;

    public List<List<GameObject>> wordList = new List<List<GameObject>>();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DataHolder holder = FindAnyObjectByType<DataHolder>();

        word = codeConverter.Decode(holder.code); 
        Debug.Log("Слово: " + codeConverter.Decode(holder.code));

        if (holder != null)
        {
            if(word != null)
            {
                word.ToLower();
                string[] words = word.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                stringCount = 0;
                GameObject wordParent = Instantiate(prefabWord, parent.transform);
                foreach (string word2 in words)
                {
                    if (tempString == null)
                    {
                        tempString += word2;
                    }
                    else
                    {
                        tempString += ' ' + word2;
                    }
                    if (tempString.Count() <= 9)
                    {
                        Debug.Log("Сама строка: " + tempString);
                        Debug.Log("Количество букв в строке: " + tempString.Count());

                        AddInPrevWord(word2);
                    }
                    else
                    {
                        Debug.Log("Сама строка: " + tempString);
                        Debug.Log("Количество букв в строке: " + tempString.Count());
                        CreateNewWord(word2);
                    }
                }
            }
        }
    }

    public void AddInPrevWord(string word2)
    {
        if(parent.transform.GetChild(stringCount).childCount > 0)
        {
            GameObject spaceItem = Instantiate(NoCharImage, parent.transform.GetChild(stringCount));
        }
        Debug.Log("Добавил в ребёнка номер" + stringCount);
        List<GameObject> letterList = new List<GameObject>();
        foreach (char c in word2)
        {
            if (c != ' ')
            {
                GameObject item = Instantiate(CharImage, parent.transform.GetChild(stringCount));
                letterList.Add(item);
                item.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString().ToUpper();
                item.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
            }
        }
        wordList.Add(letterList);
    }

    public void CreateNewWord(string word2)
    {
        GameObject wordParent2 = Instantiate(prefabWord, parent.transform);
        Debug.Log("Добавил в нового ребёнка");
        stringCount++;
        List<GameObject> letterList = new List<GameObject>();
        foreach (char c in word2)
        {
            if (c != ' ')
            {
                GameObject item = Instantiate(CharImage, wordParent2.transform);
                letterList.Add(item);
                item.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString().ToUpper();
                item.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
            }
        }
        wordList.Add(letterList);
        tempString = word2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
