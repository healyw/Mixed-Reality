#region Assembly
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
#endregion
#region Instructions
/*
 * By clicking play, this code takes the code from 
 * the HTML webpage and translates it into text.
 * 
 * This text is then processed into a string
 * 
 * The data types in HTML code must be set as 
 * <td>"[Sensor] Status: " := [Sensor] Data:</td>
 * or change the string selection
 * 
 * The text is analyzed and converted to a readable
 * value from the decimal given, and outputted into
 * the desired TextMeshPro game object
 */
#endregion
#region HTML Updater Class
public class HtmlUpdater : MonoBehaviour
{
    #region Variables
    public string urlHtml;
    public string productType;

    GameObject statusDot;
    string htmlText;
    TextMeshPro httpOutputText;
    UnityWebRequest requestHttp;
    #endregion
    #region Main Functions

    // Constructor
    void Start()
    {
        httpOutputText = GetComponent<TextMeshPro>();
        htmlText = "";
        statusDot = GameObject.Find("Status Dot");
        StartCoroutine(Refresh());
    }
    #endregion
    #region Refresh

    // Refresh Webpage done in other methods
    IEnumerator Refresh()
    {
        while (true)
        {
            if (urlHtml.Length > 0)
            {
                // select url to request from
                requestHttp = UnityWebRequest.Get(urlHtml);

                // Starts to get data from your url
                yield return requestHttp.SendWebRequest();

                // Takes code from a webpage
                htmlText = requestHttp.downloadHandler.text;

                // Takes code from a file
                //htmlText = File.ReadAllText("C:\\Users\\rays\\Desktop\\ProjectWebServer\\Webpage.htm");

                // Text to readable data
                HttpTextOutput(htmlText);
            }
        }
    }

    #endregion
    #region Move Data to Text

    //Output your text yayyyy
    private void HttpTextOutput(string httpText)
    {
        int from = 0;
        int to = 0;
        
        // Finds the value under the product and selects the data
        string result = HttpPickCase();

        if (result != string.Empty)
        {
            Material element0 = statusDot.GetComponent<Material>();
            element0.color = new Color(176, 45, 255);
        }
        else
        {
            Material element0 = statusDot.GetComponent<Material>();
            element0.color = new Color(176, 45, 255);
        }
        // TextMeshPro text output
        httpOutputText.text = result;
    }

    #endregion
    #region Pick Bxx Type

    // Take productType and convert, 6 types currently available
    private string HttpPickCase()
    {
        int from = 0;
        int to = 0;
        string value = "";

        // Extract [productType] from select data
        value = HttpRemoveBxx(htmlText, from, to);

        switch (productType)
        {
            case "BIP":
                value = ConvertProduct(value, 133, " mm");
                break;

            case "BTL":
                value = ConvertProduct(value, 100, " mm");
                break;

            case "BMP":
                value = ConvertProduct(value, 64, " mm");
                break;

            case "BSP":
                Double.TryParse(value, out double resultBsp);
                resultBsp /= 400.0;
                resultBsp = (Math.Round(resultBsp * 10)) / 10.0;
                value = resultBsp + " bar";
                break;

            case "BAE":
                Double.TryParse(value, out double resultBae);
                value = resultBae + " mA";
                break;

            case "BOS":
                Double.TryParse(value, out double resultAdcap);
                value = resultAdcap + " rpm";
                break;
        }
        return value;
    }
    #endregion
    #region Http to Converting Product

    // Generalized conversion based on units and physical length of sensor
    private string ConvertProduct(string value, Double sensorLength, string units)
    {
        double stringValue = 0.0;

        // Check if this value is null
        if (value.Length == String.Empty.Length)
        {
            Console.Write("Your product doesn't have a value.");
            throw new ArgumentNullException();
        }

        // Tries to find a double from a string
        Double.TryParse(value, out stringValue);

        // Divides that value by the max value for 4 bytes of data
        double valDubs = (stringValue * sensorLength) / 65535.0;

        // Rounds 1.5321534 to 1.5
        valDubs = (Math.Round(valDubs * 10)) / 10.0;
        
        return valDubs + units;
    }
    #endregion
    #region Extract BTL Data

    // Take all html code and select part you want (I want "BTL Status: :=BTL Data=: </td>")
    private string HttpRemoveBxx(string httpText, int pFrom, int pTo)
    {
        string result = "";

        // Change based on product type for less methods
        pFrom = httpText.IndexOf(productType + " Status: ") + (productType + " Status: ").Length;
        pTo = httpText.IndexOf("</td>", pFrom);
        
        // Check if data comes earlier than final data point
        if(pTo < pFrom)
        {
            Console.WriteLine("Extracted data from webpage came out blank.");
        }

        result = httpText.Substring(pFrom, pTo - pFrom);

        return result;
    }
    #endregion  
}
#endregion