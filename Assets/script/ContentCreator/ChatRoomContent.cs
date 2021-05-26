using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChatRoomContent
{
    public GameObject _image_prefab;
    public GameObject _text_prefab;
    public GameObject _dialog_prefab;
    public chatRoomManerger _manerger;

    public ChatRoomContent(chatRoomManerger manerger)
    {
        _image_prefab = manerger.image_prefab;
        _text_prefab = manerger.text_prefab;
        _dialog_prefab = manerger.DialogBox_prefab;
        _manerger = manerger;
    }

    public abstract GameObject createContent();
}
