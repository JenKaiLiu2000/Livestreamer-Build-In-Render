using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCreator : MessageCreator
{
    public ImageCreator(ChatRoomManerger manerger) : base(manerger) { }

    public override GameObject createMessage()
    {
        //將我們先前準備的image prefab載入。
        GameObject product = _image_prefab;
        //隨機設定圖片高度(製造出變化性)。
        product.GetComponent<RectTransform>().sizeDelta = new Vector2(_manerger._textBox_MaxWidth, Random.Range(50, 200));

        //==step two==(設定文字內容與傷害)
        Message _message = _dialog_prefab.GetComponent<Message>();
        _message.setText("");
        _message.setDamage(0);

        return product;
    }
}
