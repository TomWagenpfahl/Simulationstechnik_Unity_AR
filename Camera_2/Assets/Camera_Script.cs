using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Script : MonoBehaviour
{
    public RawImage myImage;
    //private WebCamTexture myCam, frontCam, backCam;
    private WebCamTexture myCam;

    // Gets the list of devices and prints them to the console.
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        myCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        
        
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
            /*
            // Front oder back camera
            if(devices[i].isFrontFacing)
            {
                // Front Kamera
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }else
            {
                // Back Kamera
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            */
        }
            
        //myCam = frontCam

        // Starte Kamera
        myCam.Play();
        // Anzeige auf dem Raw Image (UI)
        myImage.texture = myCam;
    }

    // Update is called once per frame
    void Update()
    {
        int orient = -myCam.videoRotationAngle;
        myImage.rectTransform.localEulerAngles = new Vector3(0,0, orient);
    }
}
