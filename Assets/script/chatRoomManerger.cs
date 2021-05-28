using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class chatRoomManerger : MonoBehaviour
{
    [Header("Reference")]
    public GameObject _imagePrefab;
    public GameObject _textPrefab;
    public GameObject _dialogBoxPrefab;
    public GameObject _container;
    public liveStreamer _liveStreamer;
    public chatRoomUiDisplay _chatRoomUiDisplay;
    [Header("Process Controler")]
    [Tooltip("內容的型態")]
    public MessageCreator.WhitchMessage _whitchMessage = MessageCreator.WhitchMessage.dialogBox;
    [Tooltip("內容數限量誌")]
    public int _listMaxCount = 15; //內容數限量誌
    [Tooltip("動態滑動效果的速度")]
    public float _slideSpeed = 3.5f; //動態滑動效果的速度
    [Header("Dialog Box Setting")]
    [Tooltip("字級大小")]
    public int _fontSize = 30; //字級大小
    [Tooltip("對話框寬度")]
    public float _textBox_MaxWidth = 450; //對話框寬度
    [Tooltip("對話框上下padding距離")]
    public float _dialogBox_Top_Botton_Padding = 15; //對話框上下padding距離
    [Tooltip("對話框左右padding距離")]
    public float _dialogBox_Left_Right_Padding = 20; //對話框左右padding距離
    [Tooltip("文字顏色")]
    public Color _textColor = new Color(255, 255, 255,195); //文字顏色
    [Tooltip("對話框顏色")]
    public Color _dialogBoxColor = new Color(0, 0, 0, 65); //對話框顏色

    Vector3 _containerOriginPos;
    List<GameObject> _messages = new List<GameObject>();
    [SerializeField]
    List<messageData> _messageDatas = new List<messageData>();
    MessageCreator _messageCreator;

    void Start()
    {
        //先存取聊天室初始的位置。
        _containerOriginPos = _container.transform.position;
        //定義直播主分數
        _liveStreamer.setValue(80);
    }

    void Update()
    {
        //按下空白鍵，產出message，並將message丟置container(聊天室)中。
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //檢查目前設定的message型態。
            checkMessageType();
            //產生message。
            GameObject newMessage = _messageCreator.createMessage();
            //將message加入到container(聊天室)中，"StartCoroutine"啟動線程，可以做到延遲的效果。
            StartCoroutine(waitFrame_addContent(newMessage));
            StartCoroutine(updateChatRoomUI(newMessage));
        }
        //聊天室的動態效果更新。
        containerDisplayUpdate();
    }

    IEnumerator updateChatRoomUI(GameObject newMessage)
    {
        //存取message實體物件的message component，避免重複程式碼。
        message _message = newMessage.GetComponent<message>();
        //更新攻擊力UI視覺。
        _chatRoomUiDisplay.updateDamageUI(_message);
        //因為攻擊力UI視覺是動畫，為了做到Damage打到到直播主的分數才扣分的效果，因此做延遲，可根據之後的視覺特效改變。
        yield return new WaitForSeconds(0.7f);
        //message攻擊直播主。
        _message.attack(_liveStreamer);
        //處理顯示UI，直播主分數。
        _chatRoomUiDisplay.updateStreamerUI(_liveStreamer);
    }

    //檢查目前設定的訊息型態。
    void checkMessageType()
    {
        //使用factory method pattern，替換不同creator，將不同型態的meaage生產過程分別封裝，達到單一職責原則。
        switch (_whitchMessage)
        {
            case MessageCreator.WhitchMessage.dialogBox:
                _messageCreator = new DialogMessage(this);
                break;
            case MessageCreator.WhitchMessage.text:
                _messageCreator = new TextMessage(this);
                break;
            case MessageCreator.WhitchMessage.image:
                _messageCreator = new ImageMessage(this);
                break;
        }
    }

    //延遲1 frame執行，將輸入的message加入到container(聊天室)中，並且開關Content Size Fitter的設定。
    IEnumerator waitFrame_addContent(GameObject message)
    {
        //==step one==
        //實例化message物件在場景中，先不指定parent，因為它要先在看不到的地方初始化，不然會造成視覺的卡頓。
        GameObject messageInstance = Instantiate(message);
        //list追蹤以及刪除message物件。
        listTrack(messageInstance);
        //如果message物件有Content Size Fitter，就讓他自動調整高度。
        if (messageInstance.GetComponent<ContentSizeFitter>())
        {
            messageInstance.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            messageInstance.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        //等待一個frame，讓他初始化。遇到這行會先return出去，因此能做到等待1frame的效果。
        //Enumerator的特性就是會保存目前區塊資料，並做完這個區塊的內容，等待個frame就會繼續做。
        yield return null;

        //==step two==
        //將實例化的message物件設定parent，就自動交給container(聊天室)的對齊工具調整。
        messageInstance.transform.SetParent(_container.transform);
        //如果message有Content Size Fitter，調整完高度要將constrained關閉，不然讀取不了height。
        if (messageInstance.GetComponent<ContentSizeFitter>())
        {
            messageInstance.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            messageInstance.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
        //調整message的padding。
        adj_Message_Padding(messageInstance);
        //調整container(聊天室)的所在高度。
        adj_Container_Height(messageInstance);
    }

    //將輸入的message加入到container(聊天室)中。
    void addContent(GameObject message)
    {
        //將輸入的message，實際產生到container(聊天室)中。
        GameObject messageInstance = Instantiate(message, _container.transform);
        //list負責追蹤物件，協助我們將先前的message刪除，節省資料空間。
        listTrack(messageInstance);
        //調整container(聊天室)的所在高度。
        adj_Container_Height(messageInstance);
    }

    //處理message的追蹤及刪除。
    void listTrack(GameObject messageInstance)
    {
        //當list滿了，就刪除最早的內容。
        if (_messages.Count == _listMaxCount)
        {
            //將message刪除。
            Destroy(_messages[0]);
            //再從清單移除。
            _messages.Remove(_messages[0]);
        }
        //將最新的message存到list，方便追蹤物件。
        _messages.Add(messageInstance);
        //因為message繼承MonoBehaviour，他一定要出現在場中，但我們刪掉後無法保存。因此，這裡使用struct將message的value type保留，包含文字內容、攻擊力等參數。
        message contentMessage = messageInstance.GetComponent<message>();
        //使用Shallow Copy，將value type複製到新struct中。
        _messageDatas.Add(new messageData(contentMessage._userName, contentMessage._text, contentMessage._damage));
    }

    //根據輸入的message大小調整container(聊天室)的所在高度。
    void adj_Container_Height(GameObject messageInstance)
    {
        //抓取message的高度。
        float content_hight = messageInstance.GetComponent<RectTransform>().rect.height;
        //為了做出message由下而上的效果，將container(聊天室)的高度-message的高度，這樣一來，雖然message新增，但視覺的高度不變，接著再用位移的方式將message帶出。
        _container.transform.position -= new Vector3(0, content_hight, 0);
    }
    //根據屬性，調整message的padding。
    GameObject adj_Message_Padding(GameObject messageInstance)
    {
        //抓取目前message的寬高。
        float w = messageInstance.GetComponent<RectTransform>().rect.width;
        float h = messageInstance.GetComponent<RectTransform>().rect.height;
        //再將設定的padding加上去。
        messageInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(w + _dialogBox_Left_Right_Padding, h + _dialogBox_Top_Botton_Padding);
        return messageInstance;
    }

    //處理message對於直播主的傷害。
    void commentAttackStreamer(GameObject messageInstance)
    => messageInstance.GetComponent<message>().attack(_liveStreamer);

    //更新container聊天室的移動效果。
    void containerDisplayUpdate()
    {
        //如果container不在原位，就以lerp的方式讓他歸位。
        if (_container.transform.position != _containerOriginPos)
            _container.transform.position = Vector3.Lerp(_container.transform.position, _containerOriginPos, Time.deltaTime * _slideSpeed);
    }
}
