using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Message Padding Editor擁有IEnumerator的方法，必須實體化在場景中才能運作，因此將此功能從MessageCreator分割。
/// </summary>
public class MessagePaddingEditor : MonoBehaviour
{
    public void WaitFrameSetPadding(GameObject message, MessageDisplaySetting mdSetting)
    {
        StartCoroutine(WaitFrame_setPadding(message, mdSetting));
    }

    //延遲one frame執行，開關Content Size Fitter的設定。
    IEnumerator WaitFrame_setPadding(GameObject message , MessageDisplaySetting mdSetting)
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
        Adj_Message_Padding(message, mdSetting);
    }

    //根據屬性，調整message的padding。
    GameObject Adj_Message_Padding(GameObject messageInstance, MessageDisplaySetting mdSetting)
    {
        //抓取目前message的寬高。
        float w = messageInstance.GetComponent<RectTransform>().rect.width;
        float h = messageInstance.GetComponent<RectTransform>().rect.height;
        //再將設定的padding加上去。
        messageInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(w + mdSetting._dialogBox_Left_Right_Padding, h + mdSetting._dialogBox_Top_Botton_Padding);
        return messageInstance;
    }
}
