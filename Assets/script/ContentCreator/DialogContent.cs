using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogContent : ChatRoomContent
{
    public DialogContent(chatRoomManerger manerger) : base(manerger) { }

    public override GameObject createContent()
    {
        //==step one==(產生物件，與初始化)
        //將我們先前準備的圖片prefab載入。
        GameObject product = _dialog_prefab;
        //找到帶有Text Component的子物件，接著設定文字框的寬度，高度可以不理他。
        //因為我們有裝Content Size Fitter，他會自動調整，因此，隨便輸入一個數值就好，這邊使用0。
        product.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(_manerger.textBox_MaxWidth, 0);

        //==step two==(設定文字與底圖)
        //先抓取物件上面的Image Component，避免這串重複打，先將他存在一個物件中。
        Image imageComponent = product.GetComponent<Image>();
        //先抓取物件上面的Text Component，避免這串重複打，先將他存在一個物件中。
        Text textComponent = product.transform.GetChild(0).GetComponent<Text>();
        //設定對話框的顏色。
        imageComponent.color = _manerger.DialogBoxColor;
        //隨機產生文字，將它賦予到我們的text內容。
        textComponent.text = stringGenerator.generateRandomString();
        //根據_fontSize屬性設定字級大小。
        textComponent.fontSize = _manerger._fontSize;
        //根據textColor屬性設定文字顏色。
        textComponent.color = _manerger.textColor;
        return product;
    }
}
