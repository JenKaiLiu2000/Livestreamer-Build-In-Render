using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageContent : ChatRoomContent
{
    public ImageContent(chatRoomManerger manerger) : base(manerger) { }

    public override GameObject createContent()
    {
        //將我們先前準備的圖片prefab載入。
        GameObject product = _image_prefab;
        //隨機設定圖片高度(製造出變化性)。
        product.GetComponent<RectTransform>().sizeDelta = new Vector2(_manerger.textBox_MaxWidth, Random.Range(50, 200));
        return product;
    }
}
