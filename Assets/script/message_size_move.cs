using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class message_size_move : MonoBehaviour
{
    public GameObject image_prefab,container;
    Vector3 originPos;

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
            addContent(createImage());
        }
        //聊天室的動態更新。
        containerUpdate();
    }
    //更新container聊天室的移動效果。
    void containerUpdate()
    {
        if(container.transform.position!= originPos)
            container.transform.position = Vector3.Lerp(container.transform.position, originPos,Time.deltaTime*5);
    }
    //加入contet內容鑲嵌至container聊天室中。
    void addContent(GameObject content)
    {
        //將輸入的內容，實際產生到聊天室中。
        Instantiate(content, container.transform);
        //抓取內容的高度。
        float content_hight = content.GetComponent<RectTransform>().rect.height;
        //為了做出訊息由下而上的效果，將聊天室的高度-內容的高度，這樣一來，雖然內容新增，但視覺的高度不變，接著再用位移的方式將內容帶出。
        container.transform.position -= new Vector3(0, content_hight, 0);
    }
    //content內容產生。  之後會再增加text的版本，從database抓留言出來。
    GameObject createImage()
    {
        //將我們先前準備的圖片prefab載入。
        GameObject product = image_prefab;
        //隨機設定圖片高度(製造出變化性)。
        product.GetComponent<RectTransform>().sizeDelta = new Vector2(200, Random.Range(50,200));
        return product;
    }
}
