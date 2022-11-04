using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;

public class Camera_Script : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textOut;
    [SerializeField]
    private RectTransform _scanZone;


    [SerializeField]
    public RawImage _rawImageBackground;
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
        _rawImageBackground.texture = myCam;
    }

    // Update is called once per frame
    void Update()
    {
        int orient = -myCam.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0,0, orient);
        loop_scan();
    }

    public void OneClickScan()
    {
        Scan();
    }

    private void loop_scan()
    {
        Scan();
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(myCam.GetPixels32(), myCam.width, myCam.height);
            if(result != null)
            {
                _textOut.text = result.Text;

                if(result.Text == "Hallo")
                {
                    _textOut.text = "Toooolllllll";
                }

            }
            else
            {
                _textOut.text = "Faild QR code";
            }
        }
        catch
        {
            _textOut.text = "Faild in TRY";
        }
    }
    
}