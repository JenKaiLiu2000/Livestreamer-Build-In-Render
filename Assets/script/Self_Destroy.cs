using UnityEngine;

public class Self_Destroy : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
