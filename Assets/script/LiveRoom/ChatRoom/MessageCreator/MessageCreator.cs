using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 負責創建Message物件。
/// </summary>
public abstract class MessageCreator
{
    protected GameObject _image_prefab;
    protected GameObject _text_prefab;
    protected GameObject _dialog_prefab;
    public MessageDisplaySetting _mdSetting;

    /// <summary>
    /// Message的型態，圖片?文字?對話框?。
    /// </summary>
    public enum WhichMessage { image, text, dialogBox }
    /// <summary>
    /// 創建Message的模式。
    /// </summary>
    public enum WhichMode { customizedPadding, origin }

    public MessageCreator(MessageDisplaySetting mdSetting)
    {
        _image_prefab = mdSetting._imagePrefab;
        _text_prefab = mdSetting._textPrefab;
        _dialog_prefab = mdSetting._dialogBoxPrefab;
        this._mdSetting = mdSetting;
    }

    protected abstract GameObject InitialMessage();

    /// <summary>
    /// 輸入創建模式返回一個Message物件，並生成在場景中。
    /// </summary>
    public GameObject CreateMessage(WhichMode mode)
    {
        switch (mode)
        {
            case WhichMode.origin:
                return InitialMessage();
            case WhichMode.customizedPadding:
                GameObject messageInstance = InitialMessage();
                setPadding(messageInstance);
                return messageInstance;
        }
        return null;
    }

    void setPadding(GameObject message)
    {
        _mdSetting._messagePaddingEditor.WaitFrameSetPadding(message, _mdSetting);
    }
}
