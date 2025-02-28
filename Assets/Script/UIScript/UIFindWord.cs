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

    private bool isWordValid; // ���������� ��� �������� ����������

    public void CheckWord()
    {
        string word = "������";
        YandexDictionaryManager.SearchWord(word, (isFound) =>
        {
            isWordValid = isFound; // ��������� ����������� �����
            OnWordChecked(); // �������� ����� ����� ��������� ����������
        });
        
    }

    private void OnWordChecked()
    {
        if (isWordValid)
        {
            Debug.Log("����� ����������!");
        }
        else
        {
            Debug.Log("����� �� ������� � �������.");
        }
    }
}



