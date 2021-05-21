using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraScript : MonoBehaviour
{
    int currentCameraID;
    WebCamTexture tex;
    public RawImage display;
    public Text onOffText;
    // Start is called before the first frame update
    void Start()
    {
        print(WebCamTexture.devices.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SwitchCam()
    {
        currentCameraID++;
        currentCameraID %= WebCamTexture.devices.Length;
        if (tex != null)
        {
            offCam();
        }
        onCam();
    }
    public void onOffCam()
    {
        if (tex != null)
        {
            offCam();
            return;
        }
        onCam();
    }
    void offCam()
    {
        display.texture = null;
        tex.Stop();
        tex = null;
        onOffText.text = "camera off";
    }
    void onCam()
    {
        WebCamDevice device = WebCamTexture.devices[currentCameraID];
        tex = new WebCamTexture(device.name);
        display.texture = tex;

        tex.Play();
        onOffText.text = "camera on";
    }
}
