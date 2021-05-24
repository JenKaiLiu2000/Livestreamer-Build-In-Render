using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManerger : MonoBehaviour
{
    public int maxMessages;
    public GameObject chatPanel, textObject;
    [SerializeField]
    List<message> messageList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sendMessage("press space key");
        }
    }

    void sendMessage(string _content)
    {
        if(messageList.Count == maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        message _message = new message();
        _message.content = _content;

        GameObject newText = Instantiate(textObject,chatPanel.transform);

        _message.textObject = newText.GetComponent<Text>();
        _message.textObject.text = _message.content;

        messageList.Add(_message);
    }
}
