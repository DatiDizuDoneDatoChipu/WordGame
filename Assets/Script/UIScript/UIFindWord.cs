using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIFindWord : MonoBehaviour
{
    public GameObject buttonCheck;
    
    public TMP_InputField InputField;

    public TextMeshProUGUI textMeshProUGUI1;
    public TextMeshProUGUI textMeshProUGUI2;
    public TextMeshProUGUI textMeshProUGUI3;

    public WordDatabase wordDatabase;

    public YandexDictionaryManager YandexDictionaryManager;

    string wordInputText;

    private bool isWordValid; // Переменная для хранения результата

    public void CheckWord()
    {
        string word = "привет";
        YandexDictionaryManager.SearchWord(word, (isFound) =>
        {
            isWordValid = isFound; // Результат сохраняется здесь
            OnWordChecked(); // Вызываем метод после получения результата
        });
        
    }

    private void OnWordChecked()
    {
        if (isWordValid)
        {
            Debug.Log("Слово существует!");
        }
        else
        {
            Debug.Log("Слово не найдено в словаре.");
        }
    }
}



