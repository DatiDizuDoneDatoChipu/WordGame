using UnityEngine;

public class AddingChar : MonoBehaviour
{
    public LoadManager loadManager;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadManager.CharAdd.AddListener(AddChar);
        loadManager.NoCharAdd.AddListener(AddNoChar);
    }

    private void AddChar()
    {
        Debug.Log("������ �����");
        // ������� ��������� �������
        
    }

    private void AddNoChar()
    {
        Debug.Log("������ yt �����");
        // ������� ��������� �������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
