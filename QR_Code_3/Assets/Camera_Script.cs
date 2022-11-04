using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;
using System;
using System.Net;
using System.IO;
using System.Threading;


// API Zeug
public enum httpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

public class RestClient
{
    public string url;
    public httpVerb methode;

    public RestClient(string url)
    {
        this.url = url;
        this.methode = httpVerb.GET;
    }

    public string makeRequest()
    {
        string strResponse = "";

        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

        request.Method = this.methode.ToString();

        using(HttpWebResponse response = (HttpWebResponse) request.GetResponse())
        {
            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("error nicht erreichbar");
            }
            using(Stream responseStream = response.GetResponseStream())
            {
                if(responseStream != null)
                {
                    using(StreamReader reader = new StreamReader(responseStream))
                    {
                        strResponse = reader.ReadToEnd();
                    }
                }
            }
        }

        return strResponse;
    }
}

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
        // RestClient resti = new RestClient("https://api.coindesk.com/v1/bpi/currentprice.json");
        // Debug.Log(resti.makeRequest());

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
                RestClient resti = new RestClient(result.Text);
                string Data = getBetween(resti.makeRequest(), "currentConditions\":{\"temp\":", "}}");
                _textOut.text = Data;

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


    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

    return "";
    }
    
}