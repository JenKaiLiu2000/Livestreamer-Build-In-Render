using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class message_size_move : MonoBehaviour
{
    [Header("Reference")]
    public GameObject image_prefab;
    public GameObject text_prefab;
    public GameObject container;
    [Header("Controler")]
    public int listMaxCount = 5;
    public float slideSpeed = 2;

    Vector3 originPos;
    List<GameObject> contentsList = new List<GameObject>();

    void Start()
    {
        //先存取聊天室初始的位置。
        originPos = container.transform.position;
    }

    void Update()
    {
        //按下空白鍵，產出內容，並將內容丟置聊天室。
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(waitFrame_addContent(createText()));
        }
        //聊天室的動態更新。
        containerUpdate();
    }
    //content內容產生。  之後會再增加text的版本，從database抓留言出來。
    GameObject createImage()
    {
        //將我們先前準備的圖片prefab載入。
        GameObject product = image_prefab;
        //隨機設定圖片高度(製造出變化性)。
        product.GetComponent<RectTransform>().sizeDelta = new Vector2(200, Random.Range(50, 200));
        return product;
    }
    //content內容產生。  之後會再增加text的版本，從database抓留言出來。
    GameObject createText()
    {
        //將我們先前準備的圖片prefab載入。
        GameObject product = text_prefab;
        int repeatNumber = Random.Range(1,20);
        text_prefab.GetComponent<Text>().text = new StringBuilder("TEXT".Length * repeatNumber).Insert(0, "TEXT", repeatNumber).ToString();
        return product;
    }
    //更新container聊天室的移動效果。
    void containerUpdate()
    {
        if (container.transform.position != originPos)
            container.transform.position = Vector3.Lerp(container.transform.position, originPos, Time.deltaTime * slideSpeed);
    }

    //加入contet內容鑲嵌至container聊天室中。
    void addContent(GameObject content)
    {
        //將輸入的內容，實際產生到聊天室中。
        GameObject contentInstance = Instantiate(content, container.transform);
        //list追蹤以及刪除物件。
        listTrack(contentInstance);
        //調整聊天室的所在高度。
        adj_Container_Height(contentInstance);
    }

    //處理content內容的追蹤及刪除。
    void listTrack(GameObject _contentInstance)
    {
        //當list滿了就刪除最早的內容。
        if (contentsList.Count == listMaxCount)
        {
            //將物件刪除。
            Destroy(contentsList[0]);
            //再從清單移除。
            contentsList.Remove(contentsList[0]);
        }
        //將內容存到list，方便追蹤物件。
        contentsList.Add(_contentInstance);
    }

    //根據輸入的內容大小調整聊天室的所在高度。
    void adj_Container_Height(GameObject _contentInstance)
    {
        if(_contentInstance.GetComponent<ContentSizeFitter>())
            _contentInstance.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        //抓取內容的高度。
        float content_hight = _contentInstance.GetComponent<RectTransform>().rect.height;
        //為了做出訊息由下而上的效果，將聊天室的高度-內容的高度，這樣一來，雖然內容新增，但視覺的高度不變，接著再用位移的方式將內容帶出。
        container.transform.position -= new Vector3(0, content_hight, 0);
    }

    IEnumerator waitFrame_addContent(GameObject _contentInstance)
    {
        //==step one==
        //實例化物件在場景中，先不指定parent，因為它要先在看不到的地方初始化，不然會讓視覺卡頓。
        GameObject Instance = Instantiate(_contentInstance);
        //list追蹤以及刪除物件。
        listTrack(Instance);
        //如果內容有Content Size Fitter，就讓他自動調整高度。
        if (Instance.GetComponent<ContentSizeFitter>())
            Instance.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        //等待一個frame，讓他初始化。
        yield return null;

        //==step two==
        //將實例化的物件設定parent，就自動交給聊天室的對齊工具調整。
        Instance.transform.SetParent(container.transform);
        //如果內容有Content Size Fitter，調整完高度要將constrained關閉，不然讀取不了height。
        if (Instance.GetComponent<ContentSizeFitter>())
            Instance.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        //調整聊天室的所在高度。
        adj_Container_Height(Instance);
    }
}
