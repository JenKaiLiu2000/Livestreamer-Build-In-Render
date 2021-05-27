using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveStreamer : MonoBehaviour
{
    [SerializeField]
    private int _value; //直播主的分數
    private bool isDead;

    public int Value {
        get => _value;
        set
        {
            //確保分數不會變負的。
            if (value <= 0 && isDead != true)
            {
                _value = 0;
                isDead = true;
            }
            else if(isDead != true)
            {
                _value = value;
            }
        }
    }

    public void setValue(int value)
        => this.Value = value;
}
