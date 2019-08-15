using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
/*
 * This class will create an object for the TMP text
 * to access. It will deserialize (make more simple)
 * the broadcasting JSON file from the masterblock 
 * and turn it into an Object file. 
 * 
 * This will also convert the hex value given to a 
 * readable decimal format.
 */
public class JSONUpdater : MonoBehaviour
{
    #region constructors
    private string json_url;
    private TextMeshProUGUI response;

    private UnityWebRequest request;
    private string httpText;
    public double sensorReadable;
    private const double tooHigh = 250.0;
    private const double tooLow = 75.0;
    GameObject go;

    #endregion
    // Start is called before the first frame update
    // Use this for initialization (json_url = "etc.")
    void Start()
    {
        json_url = "http://192.168.0.2/dprop.jsn";
        response = GetComponent<TextMeshProUGUI>();
        request = UnityWebRequest.Get(json_url);
        go = GameObject.Find("BUS Cube");
        StartCoroutine(GetText(request));
    }
    void Update()
    {
        GetText(request);
    }
    #region json methods
    IEnumerator GetText(UnityWebRequest request)
    {

        // Create WebRequest that accesses .jsn file

        request.downloadHandler = new DownloadHandlerBuffer();
        // When using IEnumerator, need yield b/c it is a dynamic type
        yield return request.SendWebRequest();


        // Error implementation
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            httpText = request.downloadHandler.text;
            /*
                * [
                *  {"ProductText": etc. ...}
                *       ]
                */
            // Uses Newtonsoft.net to deserialize (make simpler) and access objects
            // Out of the dprop.jsn file system
            var jPerson = JsonConvert.DeserializeObject<List<JsonData>>(httpText);
            string jsonProcessInputs0 = jPerson[7].processInputs;
            //Convert hexadecimal string to decimal value
            int value = convertHex(jsonProcessInputs0);

            // Convert Ultrasonic Sensor to a readable value
            sensorReadable = convertBus(value);


            //Sets TMP Text to whatever is in parenthesis
            response.SetText("Ultrasonic Sensor Data: " + sensorReadable + " mm");
        }
            
            //request.downloadHandler.Dispose();
        //}
    }
    #endregion
    #region Conversion Functions
    int convertHex(string sensorData)
    {
        
        string hexValuesCombine = sensorData.Replace(" ",string.Empty);
        string newSensorData = hexValuesCombine.Substring(0, 4);
        int value = Int32.Parse(newSensorData, System.Globalization.NumberStyles.HexNumber);
        
        return value;
    }

    double convertBus(int hexValue)
    {
        double finalNum = hexValue / 10.0;
        return finalNum;
    }
    #endregion
}
// Convert Sensor data into readable values with these functions

//added to handle Linq.JObject error from compiler
#region JsonTypes
public class JsonData
{
    [JsonProperty("ProductText")]
    public string productText { get; set; }

    [JsonProperty("VendorName")]
    public string vendorName { get; set; }

    [JsonProperty("VendorText")]
    public string vendorText { get; set; }

    [JsonProperty("ProductId")]
    public string productId { get; set; }

    [JsonProperty("SerialNumber")]
    public string serialNumber { get; set; }

    [JsonProperty("HwRev")]
    public string hwRev { get; set; }

    [JsonProperty("FwRev")]
    public string fwRev { get; set; }

    [JsonProperty("ApplTag")]
    public string applTag { get; set; }

    [JsonProperty("Event")]
    public string event0 { get; set; }

    [JsonProperty("EventFlag")]
    public string eventFlag { get; set; }

    [JsonProperty("ProcessInputs")]
    public string processInputs { get; set; }

    [JsonProperty("ProcessOutputs")]
    public string processOutputs { get; set; }

    [JsonProperty("DirectParameters")]
    public string directParameters { get; set; }

    [JsonProperty("Status")]
    public string status { get; set; }

    [JsonProperty("DsContentVendorId")]
    public string dsContentVendorId { get; set; }

    [JsonProperty("DsContentDeviceId")]
    public string dsContentDeviceId { get; set; }

    [JsonProperty("DsContentChecksum")]
    public string dsContentChecksum { get; set; }

    [JsonProperty("DsContectBuffer")]
    public string dsContentBuffer { get; set; }
}
#endregion