using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ChatRoomManerger : MonoBehaviour
{
    [Header("Origin Prefab")]
    public GameObject _container;
    [Header("Reference")]
    public LiveStreamer _liveStreamer;
    public ChatRoomUiDisplay _chatRoomUiDisplay;
    public MessageDisplaySetting _mdSetting;
    [Header("Process Controler")]
    [Tooltip("產生模式")]
    public MessageCreator.WhichMode _whitchMode = MessageCreator.WhichMode.origin;
    [Tooltip("內容的型態")]
    public MessageCreator.WhichMessage _whitchMessage = MessageCreator.WhichMessage.dialogBox;
    [Tooltip("內容數限量誌")]
    public int _listMaxCount = 15; //內容數限量誌
    [Tooltip("動態滑動效果的速度")]
    public float _slideSpeed = 3.5f; //動態滑動效果的速度

    Vector3 _containerOriginPos;
    List<GameObject> _messages = new List<GameObject>();
    [SerializeField]
    List<MessageData> _messageDatas = new List<MessageData>();
    MessageCreator _messageCreator;

    void Start()
    {
        //先存取聊天室初始的位置。
        _containerOriginPos = _container.transform.position;
    }

    void Update()
    {
        //按下空白鍵，產出message，並將message丟置container(聊天室)中。
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //輸入message型態，更改creator的種類。
            _messageCreator = MessageCreator.SwitchMessageCreator(_whitchMessage,_mdSetting);
            //產生message。
            GameObject newMessage = _messageCreator.CreateMessage(_whitchMode);
            //將message加入到container(聊天室)中，"StartCoroutine"啟動線程，可以做到延遲的效果。
            StartCoroutine(WaitFrameAddToContainer(newMessage));
            StartCoroutine(WaitForSecondsUpdateChatRoomUI(newMessage));
        }
        //聊天室的動態效果更新。
        ContainerDisplayUpdate();
    }

    IEnumerator WaitForSecondsUpdateChatRoomUI(GameObject newMessage)
    {
        //存取message實體物件的message component，避免重複程式碼。
        Message _message = newMessage.GetComponent<Message>();
        //更新攻擊力UI視覺。
        _chatRoomUiDisplay.updateDamageUI(_message);
        //因為攻擊力UI視覺是動畫，為了做到Damage打到到直播主的分數才扣分的效果，因此做延遲，可根據之後的視覺特效改變。
        yield return new WaitForSeconds(0.7f);
        //message攻擊直播主。
        _message.attack(_liveStreamer);
        //處理顯示UI，直播主分數。
        _chatRoomUiDisplay.updateStreamerUI(_liveStreamer);
    }

    //將輸入的message加入到container(聊天室)中。
    IEnumerator WaitFrameAddToContainer(GameObject messageInstance)
    {
        yield return null;
        //list追蹤以及刪除message物件。
        ListTrack(messageInstance);
        //將實例化的message物件設定parent，就自動交給container(聊天室)的對齊工具調整。
        messageInstance.transform.SetParent(_container.transform);
        //調整container(聊天室)的所在高度。
        Adj_Container_Height(messageInstance);
    }

    //將輸入的message加入到container(聊天室)中。
    void AddToContainer(GameObject messageInstance)
    {
        //list追蹤以及刪除message物件。
        ListTrack(messageInstance);
        //將實例化的message物件設定parent，就自動交給container(聊天室)的對齊工具調整。
        messageInstance.transform.SetParent(_container.transform);
        //調整container(聊天室)的所在高度。
        Adj_Container_Height(messageInstance);
    }

    //處理message的追蹤及刪除。
    void ListTrack(GameObject messageInstance)
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
        Message contentMessage = messageInstance.GetComponent<Message>();
        //使用Shallow Copy，將value type複製到新struct中。
        _messageDatas.Add(new MessageData(contentMessage._userName, contentMessage._text, contentMessage._damage));
    }

    //根據輸入的message大小調整container(聊天室)的所在高度。
    void Adj_Container_Height(GameObject messageInstance)
    {
        //抓取message的高度。
        float content_hight = messageInstance.GetComponent<RectTransform>().rect.height;
        //為了做出message由下而上的效果，將container(聊天室)的高度-message的高度，這樣一來，雖然message新增，但視覺的高度不變，接著再用位移的方式將message帶出。
        _container.transform.position -= new Vector3(0, content_hight, 0);
    }

    //處理message對於直播主的傷害。
    void CommentAttackStreamer(GameObject messageInstance)
    => messageInstance.GetComponent<Message>().attack(_liveStreamer);

    //更新container聊天室的移動效果。
    void ContainerDisplayUpdate()
    {
        //如果container不在原位，就以lerp的方式讓他歸位。
        if (_container.transform.position != _containerOriginPos)
            _container.transform.position = Vector3.Lerp(_container.transform.position, _containerOriginPos, Time.deltaTime * _slideSpeed);
    }
}
