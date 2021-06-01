using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public string _userName;
    public string _text;
    public int _damage;

    public Message(){}

    public Message(string userName, string text, int damage)
    {
        _userName = userName;
        _text = text;
        _damage = damage;
    }

    public void setDamage(int damage)
        =>_damage = damage;

    public void setText(string text) 
        => _text = text;

    public void attack(LiveStreamer _liveStreamer)
        => _liveStreamer.Value += _damage;
}
