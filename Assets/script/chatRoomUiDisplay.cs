using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class chatRoomUiDisplay : MonoBehaviour
{
    public Text _livesteamerScoreText; //直播主分數的TextPrefab。
    public GameObject _messageDamageTextPrefab; //傷害值的TextPrefab。
    public Transform damageInitPos; //傷害值的初始位置。

    //更新直播主的UI。
    public void updateStreamerUI(liveStreamer _liveStreamer)
    {
        _livesteamerScoreText.text = $"score: {_liveStreamer.Value}";  
    }
    //更新傷害值的UI。
    public void updateDamageUI(message message)
    {
        //產生傷害值物件(那些飄動的數字)，他身上綁著Timeline會在產生那刻開始播動畫，因此他一出現就在動。
        GameObject instantDamageText = Instantiate(_messageDamageTextPrefab, damageInitPos.position, damageInitPos.rotation, damageInitPos.transform);
        //隨機位移它的位置。
        instantDamageText.transform.position += new Vector3(UnityEngine.Random.Range(30, -30), UnityEngine.Random.Range(10, -10), 0);
        //根據message的攻擊力，設定文字(傷害值)。
        instantDamageText.transform.GetChild(0).GetComponent<Text>().text = Convert.ToString(message._damage);
    }
}
