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
 * 
 * This method will then refresh the values on the 
 * webpage and change the value in the player
 * 
 * Updated values will now be able to be read as 
 * different colors based on status
 */
public class JSONUpdater603 : MonoBehaviour
{
    #region Variables
    public static double sensorValue;
    string toDeserialize;
    string url;
    TextMesh outputText;
    UnityWebRequest request;
    GameObject go;
    private const double tooHigh = 250.0;
    private const double tooLow = 75.0;
    Color[] colors;
    #endregion

    // Start is called before the first frame update
    // Where we declare variables
    void Start()
    {
        url = "http://192.168.0.2/dprop.jsn";
        outputText = GetComponent<TextMesh>();
        toDeserialize = "";
        go = GameObject.Find("BUS Cube");
        outputText.richText = true;
        
        StartCoroutine(Refresh());
    }
    /*
     * Anything placed in here will update every frame
     * Also, refreshes the webpage
     */
    IEnumerator Refresh()
    {
        while (true)
        {
            // Now I know what url to use
            request = UnityWebRequest.Get(url);
            // Starts to get data from your url
            yield return request.SendWebRequest();
            // Pick the text to deserialize
            toDeserialize = request.downloadHandler.text;
            // Changes the MasterBlock's values to text readable values 
            sensorValue = Deserialize(toDeserialize);
            // This changes the color if the data value gets too high or too low in the sensor
            outputText.text = (sensorValue + " mm");
            // Wait until the end of the frame to update
            yield return new WaitForEndOfFrame();
            
            // Currently updating every frame, if not there is another issue
        }
    }
    private double Deserialize(string shallDeserialize)
    {
        //Convert JSON to C# object (polymorphized var replaces object)
        var jPerson = JsonConvert.DeserializeObject<List<JsonData>>(shallDeserialize);
        // 7 is your port, processInputs is the object you are accessing
        string jsonProcessInputs0 = jPerson[7].processInputs;
        //Convert hexadecimal string to decimal value
        int value = convertHex(jsonProcessInputs0);

        // Convert Ultrasonic Sensor to a readable value
        double dblVal = convertBus(value);
        return dblVal;
    }
    #region Conversion Functions
    /* 
     * These functions (process data and give something out; workers)
     * convert your values from the masterblock to millimeters
     */
    int convertHex(string sensorData)
    {
        // "00 ff" to "00ff"
        string hexValuesCombine = sensorData.Replace(" ", string.Empty);
        // take first 4 values
        string newSensorData = hexValuesCombine.Substring(0, 4);
        // Parse (is there data in this string? Okay, convert and output it)
        int value = Int32.Parse(newSensorData, System.Globalization.NumberStyles.HexNumber);
        // Return a number
        return value;
    }
    
    /* 
     * Convert hex to Ultrasonic Sensor data (user readable)
     */
    double convertBus(int hexValue)
    {
        double finalNum = hexValue / 10.0;
        return finalNum;
    }
    #endregion
    #region Little Helper Functions
    T[] InitializeArray<T>(int length) where T : new()
    {
        T[] array = new T[length];
        for (int i = 0; i < length; ++i)
        {
            array[i] = new T();
        }

        return array;
    }
    #endregion
}

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