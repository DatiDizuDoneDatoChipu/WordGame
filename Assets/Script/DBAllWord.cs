using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class WordDatabase : MonoBehaviour
{
    public TextAsset wordsFile; // Файл со словами
    private List<string> allWords = new List<string>();

    void Start()
    {
        LoadWords();
    }

    private void LoadWords()
    {
        if (wordsFile != null)
        {
            string[] words = wordsFile.text.Split('\n');
            foreach (string word in words)
            {
                allWords.Add(word.Trim().ToLower());
            }
            Debug.Log($"Загружено слов: {allWords.Count}");
        }
    }

    public List<string> GetAllWords()
    {
        return allWords;
    }

    public List<string> FindWordWithLetters(string inputLetters)
    {
        List<string> validWords = new List<string>();
        inputLetters = inputLetters.ToLower();

        List<char> availableLetters = new List<char>();
        foreach (char letter in inputLetters)
        {
            Debug.Log(letter);
            availableLetters.Add(letter);   
        }

        foreach (string word in allWords)
        {
            foreach (char letter in availableLetters)
            {
                if(word.Contains(letter))
                {
                    Debug.Log("Added word: " + word);
                    validWords.Add(word);
                }
            }
        }

        return validWords;
    }
}