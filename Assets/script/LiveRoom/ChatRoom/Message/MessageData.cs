[System.Serializable]
public class MessageData
{
    public string _userName;
    public string _text;
    public int _damage;

    public MessageData(string userName, string text, int damage)
    {
        _userName = userName;
        _text = text;
        _damage = damage;
    }
}
