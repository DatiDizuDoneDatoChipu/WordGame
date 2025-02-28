[System.Serializable]
public class YandexDictionaryResponse
{
    public Def[] def;
}

[System.Serializable]
public class Def
{
    public string text;
    public Tr[] tr;
}

[System.Serializable]
public class Tr
{
    public string text;
    public string pos; // часть речи
    public Syn[] syn; // синонимы
}

[System.Serializable]
public class Syn
{
    public string text;
}