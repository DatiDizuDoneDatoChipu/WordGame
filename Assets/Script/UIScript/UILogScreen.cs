using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.SceneManagement;

public class UILogScreen : MonoBehaviour
{
    public CodeConverter converter;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField inputCodeField;

    [SerializeField] private TMP_Text codeText;
    [SerializeField] private DataHolder holder;

    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject copyPanel;

    string code;

    public void PlayButton()
    {
        Debug.Log(inputField.text);

        if (inputField != null)
        {
            code = converter.Encode(inputField.text);
            codeText.text = code;

            inputPanel.SetActive(false);
            copyPanel.SetActive(true);
        }
    }

    public void InputCodeButton()
    {
        holder.code = inputCodeField.text;
        SceneManager.LoadScene("SampleScene");
        
    }

    public void SecondButton()
    {
        Debug.Log(converter.Decode(code));
    }

    public void BackButton()
    {
        inputPanel.SetActive(true);
        copyPanel.SetActive(false);
    }
}
