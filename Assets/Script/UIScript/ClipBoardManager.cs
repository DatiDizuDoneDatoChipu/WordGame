using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClipboardManager : MonoBehaviour
{
    private Text textToCopy; // �����, ������� ����� �����������

    [SerializeField] private TMP_Text textCode;

    [HideInInspector] public string codeText;

    public CodeConverter codeConverter;

    public void CopyToClipboard()
    {
        if (textCode == null || string.IsNullOrEmpty(textCode.text))
        {
            Debug.LogError("����� ��� ����������� �� �������� ��� ����!");
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        // ��� Android
        AndroidJavaClass clipboardClass = new AndroidJavaClass("android.content.ClipboardManager");
        AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
        AndroidJavaObject activity = GetUnityActivity();
        AndroidJavaObject clipboard = activity.Call<AndroidJavaObject>("getSystemService", contextClass.GetStatic<string>("CLIPBOARD_SERVICE"));
        clipboard.Call("setText", textCode.text);
        
#else
        // ��� ��������� � ������ ��������
        GUIUtility.systemCopyBuffer = textCode.text;
        Debug.Log("����� ����������: " + textCode.text);
#endif
    }



    private AndroidJavaObject GetUnityActivity()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }


    // ���������� ��� ������� �� ������
    public void PasteFromClipboard()
    {
        string clipboardText = GetClipboardText();
        if (!string.IsNullOrEmpty(clipboardText))
        {
            codeText = clipboardText;
            DataHolder holder = FindAnyObjectByType<DataHolder>();
            holder.code = codeText;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            codeText = "����� ����!";
        }
    }

    private string GetClipboardText()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    try
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass clipboardClass = new AndroidJavaClass("android.content.ClipboardManager");
        AndroidJavaObject clipboard = context.Call<AndroidJavaObject>("getSystemService", "clipboard");

        AndroidJavaObject clipData = clipboard.Call<AndroidJavaObject>("getPrimaryClip");
        if (clipData == null)
        {
            Debug.Log("ClipData is null");
            return null;
        }

        int itemCount = clipData.Call<int>("getItemCount");
        if (itemCount == 0)
        {
            Debug.Log("No items in ClipData");
            return null;
        }

        AndroidJavaObject item = clipData.Call<AndroidJavaObject>("getItemAt", 0);
        if (item == null)
        {
            Debug.Log("Item is null");
            return null;
        }

        string text = item.Call<string>("getText");
        Debug.Log("Clipboard text: " + text);
        return text;
    }
    catch (System.Exception e)
    {
        Debug.LogError("������: " + e.Message + "\n" + e.StackTrace);
        return null;
    }
#else
        return GUIUtility.systemCopyBuffer;
#endif
    }
}

