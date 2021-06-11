using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDialog : IMessageCreateType
{
    public GameObject InitialMessage(MessageDisplaySetting _mdSetting)
    {
        //==step one==(產生物件，與初始化)
        //將我們先前準備的Dialog box prefab載入。
        GameObject message = _mdSetting._dialogBoxPrefab;
        //找到帶有Text Component的子物件，接著設定message的寬度，高度可以不理他。
        //因為我們有裝Content Size Fitter，他會自動調整，因此，隨便輸入一個數值就好，這邊使用0。
        message.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(_mdSetting._textBox_MaxWidth, 0);

        //==step two==(設定文型與底圖)
        //先抓取物件的Image Component，避免這串重複打，先將他存在一個容器中。
        Image imageComponent = message.GetComponent<Image>();
        //先抓取物件上面的Text Component，避免這串重複打，先將他存在一個容器中。
        Text textComponent = message.transform.GetChild(0).GetComponent<Text>();
        //設定對話框的顏色。
        imageComponent.color = _mdSetting._dialogBoxColor;
        //根據_fontSize屬性設定字級大小。
        textComponent.fontSize = _mdSetting._fontSize;
        //根據textColor屬性設定文字顏色。
        textComponent.color = _mdSetting._textColor;

        //==step three==(設定文字內容與傷害)
        //隨機產生文字，將它賦予到我們的text中。
        Message _message = _mdSetting._dialogBoxPrefab.GetComponent<Message>();
        string content = String_Generator.generateRandomString();
        textComponent.text = content;
        _message.setText(content);
        _message.setDamage(Random.Range(-5, 2));

        //==step four==
        //將物件在scene中實體化
        GameObject messageInstance = GameObject.Instantiate(message);

        return messageInstance;
    }
}
