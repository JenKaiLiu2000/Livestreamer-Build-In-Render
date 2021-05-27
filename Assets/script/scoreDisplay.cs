using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;

public class scoreDisplay : MonoBehaviour
{
    public liveStreamer _livesteamer;
    public messageData _Message;

    public Text _livesteamerScoreText;
    public GameObject _messageDamageTextPrefab;

    public void updateScoreUI(liveStreamer livesteamer)
    {
        _livesteamerScoreText.text = $"score: {livesteamer.Value}";  
    }
    public void updateDamageUI(messageData message)
    {
        messageDamageEffect(message);
    }
    void messageDamageEffect(messageData _Message)
    {
        GameObject instantDamageText = Instantiate(_messageDamageTextPrefab, transform.position ,transform.rotation,this.transform);
        float x = instantDamageText.transform.position.x;
        float y = instantDamageText.transform.position.y;
        float z = instantDamageText.transform.position.z;
        instantDamageText.transform.position = new Vector3(x + UnityEngine.Random.Range(30, -30), y + UnityEngine.Random.Range(10, -10), z);
        instantDamageText.transform.GetChild(0).GetComponent<Text>().text = Convert.ToString(_Message._damage);
    }
}
