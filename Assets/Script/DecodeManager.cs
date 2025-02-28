using System;
using System.Text;
using UnityEngine;

public class CodeConverter : MonoBehaviour
{
    public string code;
    // Кодирование предложения в Base64
    public string Encode(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }

    // Декодирование Base64 в предложение
    public string Decode(string encodedText)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            Debug.LogError("Некорректный код!");
            return null;
        }
    }
}