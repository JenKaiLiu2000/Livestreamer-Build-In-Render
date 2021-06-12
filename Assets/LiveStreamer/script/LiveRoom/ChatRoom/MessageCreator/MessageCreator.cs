using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 負責創建Message物件。
/// </summary>
public static class MessageCreator
{
    private static IMessageCreateType _createType;
    private static MessageDisplaySetting _mdSetting;
    private static MessagePaddingEditor _messagePaddingEditor;

    /// <summary>
    /// Message的型態，圖片?文字?對話框?。
    /// </summary>
    public enum WhichTypeOfMessage { image, text, dialogBox }
    /// <summary>
    /// 創建Message的流程。
    /// </summary>
    public enum WhichProcessMode { customizedPadding, origin }

    //根據輸入的Message型態，替換創建Message的技能。
    private static IMessageCreateType SwitchMessageType(WhichTypeOfMessage whichTypeOfMessage)
    {
        switch (whichTypeOfMessage)
        {
            case WhichTypeOfMessage.dialogBox:
                return _createType = new CreateDialog();
            case WhichTypeOfMessage.image:
                return _createType = new CreateImage();
            case WhichTypeOfMessage.text:
                return _createType = new CreateText();
        }
        return null;
    }

    /// <summary>
    /// 輸入創建模式返回一個Message物件，並生成在場景中。
    /// </summary>
    public static GameObject CreateMessage(MessageDisplaySetting mdSetting,WhichProcessMode processMode,WhichTypeOfMessage messageType)
    {
        //==step 1== 初始設定。
        //將此靜態類別的靜態_mdSetting屬性，替換成使用者輸入的mdSetting，供這個類別的其他method使用。
        _mdSetting = mdSetting;
        //將此靜態類別的靜態_createType屬性，透過SwitchMessageType function根據message型態替換不同的創建技能。
        _createType = SwitchMessageType(messageType);
        //==step 2== 根據輸入的流程模式，進行創建。
        switch (processMode)
        {
            //origin模式，不修改padding。
            case WhichProcessMode.origin:
                //將創建message的職責委派給IMessageCreateType的成員，因為他們擁有創建的細節，做到單一職責。
                return _createType.InitialMessage(mdSetting);
            //customized Padding模式，根據mdSetting修改padding。
            case WhichProcessMode.customizedPadding:
                //將創建message的職責委派給IMessageCreateType的成員，因為他們擁有創建的細節，做到單一職責。
                GameObject messageInstance = _createType.InitialMessage(mdSetting);
                //修改padding需要延遲，使用corotine的效果需要一個場上實體物件，因此先創建一個物件在場中待命。
                InitPaddingEditor();
                //修改padding。
                WaitFrameSetPadding(messageInstance);
                return messageInstance;
        }
        return null;
    }

    //創建一個實體物件在場中，隨時待命運作corotine。
    private static void InitPaddingEditor()
    {
        //如果靜態 MessagePaddingEditor是空的，代表還沒創建過，那就創建一個吧!
        if (_messagePaddingEditor == null)
        {
            //創建空物件在場中，並命名為 "mpEditor" 。
            GameObject paddingEditor = new GameObject("mpEditor");
            //將這個名為"mpEditor"的空物件，掛上MessagePaddingEditor的腳本，而AddComponent function本身會return那個腳本的ref，我們再去拿靜態型別接它。
            _messagePaddingEditor = paddingEditor.AddComponent<MessagePaddingEditor>();
        }

    }

    //在場中的Padding Editor幫忙運行Coroutine。
    private static void WaitFrameSetPadding(GameObject message)
    {
        _messagePaddingEditor.StartCoroutine(WaitFrame_setPadding(message));
    }

    //延遲one frame執行，開關Content Size Fitter的設定。
    private static IEnumerator WaitFrame_setPadding(GameObject message)
    {
        //==step one==
        //如果message物件有Content Size Fitter，就讓他自動調整高度。
        if (message.GetComponent<ContentSizeFitter>())
        {
            message.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            message.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        //等待一個frame，讓他初始化。遇到這行會先return出去，因此能做到等待1frame的效果。
        //Enumerator的特性就是會保存目前區塊資料，並做完這個區塊的內容，等待個frame就會繼續做。
        yield return null;
        //==step two==
        //如果message有Content Size Fitter，調整完高度要將constrained關閉，不然讀取不了height。
        if (message.GetComponent<ContentSizeFitter>())
        {
            message.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            message.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
        //調整message的padding。
        Adj_Message_Padding(message);
    }

    //根據屬性，調整message的padding。
    private static GameObject Adj_Message_Padding(GameObject messageInstance)
    {
        //抓取目前message的寬高。
        float w = messageInstance.GetComponent<RectTransform>().rect.width;
        float h = messageInstance.GetComponent<RectTransform>().rect.height;
        //再將設定的padding加上去。
        messageInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(w + _mdSetting._dialogBox_Left_Right_Padding, h + _mdSetting._dialogBox_Top_Botton_Padding);
        return messageInstance;
    }
}
