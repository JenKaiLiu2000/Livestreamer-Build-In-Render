using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MessageCreator
{
    public GameObject _image_prefab;
    public GameObject _text_prefab;
    public GameObject _dialog_prefab;
    public chatRoomManerger _manerger;

    public enum WhitchMessage
    {
        image, text, dialogBox
    }

    public MessageCreator(chatRoomManerger manerger)
    {
        _image_prefab = manerger._imagePrefab;
        _text_prefab = manerger._textPrefab;
        _dialog_prefab = manerger._dialogBoxPrefab;
        _manerger = manerger;
    }

    public abstract GameObject createContent();
}
