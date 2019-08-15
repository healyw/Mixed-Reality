#region Assembly
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
#endregion
#region Instructions
/*
 * Instructions
 * 
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
 * 
 * Change the URL under #MainFunctions to your
 * MasterBlock URL
 * 
 * Also, create as many conversion functions as you
 * want and put them into the 'switch' areas
 * 
 * DO NOT change any of the UnityWebRequest functions
 * in this code, unless you know what you are doing.
 */
#endregion
public class JsonUpdater : MonoBehaviour
{
    #region Variables
    public int portNumber;
    public string deserializeType;
    public string productType;
    public string urlJson;
    public string urlHtml;

    public static int sensorValue;
    public static string sensorName;

    string toDeserialize;
    string bniDeserialize;
    string htmlText;
    TextMeshPro outputText;
    TextMeshPro httpOutputText;
    UnityWebRequest requestHttp;
    UnityWebRequest request;
    GameObject go;
    private const double tooHigh = 250.0;
    private const double tooLow = 75.0;

    #endregion
    #region Running Functions
    // Start is called before the first frame update
    // Where we declare variables
    void Start()
    {
        outputText = GetComponent<TextMeshPro>();
        httpOutputText = GetComponent<TextMeshPro>();
        toDeserialize = "";
        htmlText = "";
        go = GameObject.Find("Status Dot");
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
            if (urlJson.Length > 0)
            {
                //Request from webpage
                yield return request.SendWebRequest();

                // Pick the text to deserialize or output
                toDeserialize = request.downloadHandler.text;

                // Changes the MasterBlock's values to text readable values 
                DeserializeTypeConvert(deserializeType);
            }
            // Wait until the end of the frame to update
            yield return new WaitForEndOfFrame();

            // Currently updating every frame, if not there is another issue
        }
    }
    #endregion
    #region Type Converts
    // Convert your [insert object type] JSON object
    private void DeserializeTypeConvert(string deserializeT)
    {
        string newText = "";
        switch (deserializeT)
        {
            // Which data do I want to select?
            case "ProcessInputs4":
                sensorValue = DeserializeProcessInputs(toDeserialize, portNumber, 4);
                newText = JSONProductTypeConvert(sensorValue);
                outputText.text = newText;
                break;
            case "ProcessInputs8":
                sensorValue = DeserializeProcessInputs(toDeserialize, portNumber, 8);
                newText = JSONProductTypeConvert(sensorValue);
                outputText.text = newText;
                break;
            case "ProcessInputs12":
                sensorValue = DeserializeProcessInputs(toDeserialize, portNumber, 12);
                newText = JSONProductTypeConvert(sensorValue);
                outputText.text = newText;
                break;
            case "ProcessOutputs4":
                sensorValue = DeserializeProcessOutputs(toDeserialize, portNumber, 4);
                newText = JSONProductTypeConvert(sensorValue);
                outputText.text = newText;
                break;
            case "ProcessOutputs8":
                sensorValue = DeserializeProcessOutputs(toDeserialize, portNumber, 8);
                newText = JSONProductTypeConvert(sensorValue);
                outputText.text = newText;
                break;
            case "ProductText":
                sensorName = DeserializeProductText(toDeserialize, portNumber);
                outputText.text = sensorName;
                break;
            case "ProductId":
                sensorName = DeserializeProductId(toDeserialize, portNumber);
                outputText.text = sensorName;
                break;
            case "VendorName":
                sensorName = DeserializeVendorName(toDeserialize, portNumber);
                outputText.text = sensorName;
                break;
        }
    }
    #endregion
    #region Type of Extraction

    // Choose your product type and convert
    private string JSONProductTypeConvert(int sensorVal)
    {
        string newText = "";
        switch (productType)
        {
            // Selection of which sensor to choose
            case "BTL":
                newText = convertProduct(sensorVal, 10000.0, "cm");
                break;
            case "BIP":
                newText = convertProduct(sensorVal, 1.0, "cm");
                break;
            case "BSP":
                newText = convertProduct(sensorVal, 400.0, "bar");
                break;
            case "BMP":
                newText = convertProduct(sensorVal, 400.0, "mm");
                break;
            case "BAE":
                newText = convertProduct(sensorVal, 1.0, "mA");
                break;
        }
        return newText;
    }
    #endregion
    #region Searching Json
    private Port[] FindPorts(string shallDeserialize)
    {
        Masterblock master = JsonConvert.DeserializeObject<Masterblock>(shallDeserialize);

        // Find root object
        Port[] findPorts = master.Ports;
        return findPorts;
    }
    #endregion
    #region Deserializing Partial Functions
    private int DeserializeProcessInputs(string shallDeserialize, int portNumber, int numBytes)
    {
        Port[] findPorts = FindPorts(shallDeserialize);

        //Find "processInputs"
        var jsonProcessInputs0 = findPorts[portNumber].ProcessInputs;

        //Convert Hex Value with 8 bytes
        int value = convertHexNumBytes(jsonProcessInputs0, numBytes);
        return value;
    }
    private int DeserializeProcessOutputs(string shallDeserialize, int portNumber, int numBytes)
    {
        Port[] findPorts = FindPorts(shallDeserialize);

        //Find "processOutputs"
        var jsonProcessInputs0 = findPorts[portNumber].ProcessOutputs;

        //Convert Hex Value with 8 bytes
        int value = convertHexNumBytes(jsonProcessInputs0, numBytes);
        return value;
    }
    // "ProductText: BMP IOL[...];" to "BMP IOL [...26 chars]"
    private string DeserializeProductText(string shallDeserialize, int portNumber)
    {
        Port[] findPorts = FindPorts(shallDeserialize);

        //Find "productText"
        var jsonProductText0 = findPorts[portNumber].ProductText;

        if (jsonProductText0.Length > 26)
        {
            jsonProductText0 = jsonProductText0.Substring(0, 26);
        }
        return jsonProductText0;
    }
    // "ProductId: BMP[...]" to "BMP"
    private string DeserializeProductId(string shallDeserialize, int portNumber)
    {
        Port[] findPorts = FindPorts(shallDeserialize);

        //Find "productText"
        var jsonProductText0 = findPorts[portNumber].ProductId;

        if (jsonProductText0.Length > 3)
        {
            jsonProductText0 = jsonProductText0.Substring(0, 3);
        }
        return jsonProductText0;
    }
    //"VendorName: Balluff IO-Link 00023941" etc. to "Balluff IO-Link"
    private string DeserializeVendorName(string shallDeserialize, int portNumber)
    {
        Port[] findPorts = FindPorts(shallDeserialize);

        //Find "vendorName"
        var jsonVendorName0 = findPorts[portNumber].VendorName;

        if (jsonVendorName0.Length > 26)
        {
            jsonVendorName0 = jsonVendorName0.Substring(0, 26);
        }
        return jsonVendorName0;
    }
    #endregion    
    #region Conversion Functions
    // Convert 'numBytes' bytes of data to integer
    int convertHexNumBytes(string sensorData, int numBytes)
    {
        // "00 ff" to "00ff"
        string hexValuesCombine = sensorData.Replace(" ", string.Empty);

        // take first 4 values
        string newSensorData = hexValuesCombine.Substring(0, numBytes);

        // Parse (is there data in this string? Okay, convert and output it)
        int value = Int32.Parse(newSensorData, System.Globalization.NumberStyles.HexNumber);

        // Return a number
        return value;
    }
    string convertProduct(int hexValue, double divisor, string units)
    {
        double value = hexValue / divisor;
        value = (Math.Round(value * 10)) / 10.0;
        return value + " " + units;
    }
    #endregion
}
#region Masterblock JSON Extraction
public class Masterblock
{
    public Interface Interface { get; set; }
    public Port[] Ports { get; set; }
}

public class Interface
{
    public string Version { get; set; }
    public string Description { get; set; }
}

public class Port
{
    public string VendorName { get; set; }
    public string VendorText { get; set; }
    public string ProductName { get; set; }
    public string ProductId { get; set; }
    public string ProductText { get; set; }
    public string SerialNumber { get; set; }
    public string HardwareRevision { get; set; }
    public string FirmwareRevision { get; set; }
    public string ProcessInputs { get; set; }
    public string ProcessOutputs { get; set; }
    public string DirectParameters { get; set; }
    public string DsContentBuffer { get; set; }
}
#endregion