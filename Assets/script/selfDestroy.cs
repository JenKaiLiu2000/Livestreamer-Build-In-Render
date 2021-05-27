using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour
{
    public void destroy()
    {
        Destroy(this.gameObject);
    }
}
