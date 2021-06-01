using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCreator : MessageCreator
{
    public ImageCreator(MessageDisplaySetting mdSetting) : base(mdSetting) { }

    protected override GameObject InitialMessage()
    {
        //將我們先前準備的image prefab載入。
        GameObject message = _image_prefab;
        //隨機設定圖片高度(製造出變化性)。
        message.GetComponent<RectTransform>().sizeDelta = new Vector2(_mdSetting._textBox_MaxWidth, Random.Range(50, 200));

        //==step two==(設定文字內容與傷害)
        Message _message = _dialog_prefab.GetComponent<Message>();
        _message.setText("");
        _message.setDamage(0);

        //==step three==
        //將物件在scene中實體化
        GameObject messageInstance = GameObject.Instantiate(message);

        return messageInstance;
    }
}
