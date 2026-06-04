using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.Collections;

using Newtonsoft.Json.Linq;
using System.Net.Security; // Add this!

public class Working : MonoBehaviour
{
    public Renderer cylinder;

    public string readUrl = "https://api.thingspeak.com/channels/2850939/feeds.json?api_key=<YOUR_READ_API_KEY>&results=1";
    public string writeUrl = "https://api.thingspeak.com/update?api_key=<YOUR_WRITE_API_KEY>&field1=";

    public UIDocument uiDocument;
    private VisualElement root;

    void Start()
    {

        root = uiDocument.rootVisualElement;
        StartCoroutine(CheckStatus());

        root.Q<Button>("on").clicked += () => StartCoroutine(SendToAPI(1));
        root.Q<Button>("off").clicked += () => StartCoroutine(SendToAPI(0));

    }

    IEnumerator CheckStatus()
    {

        while (true)
        {
            Debug.Log("Polling ThingSpeak API for status updates...");
            UnityWebRequest request = UnityWebRequest.Get(readUrl);
            Debug.Log("Request  prev is " + request.result);

            yield return request.SendWebRequest();
            Debug.Log("Request is " + request.result);

            try
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    Debug.Log("API Response: " + json);

                    JObject data = JObject.Parse(json);
                    int field1Value = int.Parse((string)data["feeds"][0]["field1"]);

                    Debug.Log("Field 1 Value: " + field1Value);

                    UpdateColor(field1Value);
                }
            }
            catch (System.Exception)
            {
                Debug.Log("Catch error");
            }

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SendToAPI(int value)
    {
        string url = writeUrl + value;
        Debug.Log("Sending write request: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Write successful: " + request.downloadHandler.text);
            UpdateColor(value);
        }
        else
        {
            Debug.LogError("Write Failed: " + request.error);
        }
    }

    // int ParseValue(string json)
    // {
    //     try
    //     {
    //         var data = JsonUtility.FromJson<Response>(json);
    //         if (data.feeds.Length > 0 && int.TryParse(data.feeds[0].field1, out int result))
    //             return result;
    //     }
    //     catch
    //     {
    //         Debug.LogError("JSON Parse Error");
    //     }
    //     return -1;
    // }

    void UpdateColor(int val)
    {
        if (val == 1)
        {
            cylinder.material.color = Color.green;
            Debug.Log("Cylinder Color: GREEN");

        }
        else if (val == 0)
        {
            cylinder.material.color = Color.red;
            Debug.Log("Cylinder Color: RED");
        }
    }

    [System.Serializable]
    public class Response { public Feed[] feeds; }
    [System.Serializable]
    public class Feed { public string field1; }
}