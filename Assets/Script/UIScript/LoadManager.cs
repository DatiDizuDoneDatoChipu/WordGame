using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    public UnityEvent CharAdd;
    public UnityEvent NoCharAdd;

    public string word;

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

                foreach (string word2 in words)
                {
                    GameObject wordParent = Instantiate(prefabWord, parent.transform);
                    List<GameObject> letterList = new List<GameObject>();
                    foreach (char c in word2)
                    {
                        if (c != ' ')
                        {
                            GameObject item = Instantiate(CharImage, wordParent.transform);
                            letterList.Add(item);
                            item.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString().ToUpper();
                            item.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                        }
                        Debug.Log(c);
                    }
                    wordList.Add(letterList);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
