using UnityEngine;
using System;

public static class STTUtils
{
    public static void OnResultCallback(string result)
    {
        Debug.Log("STT Result: " + result);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameObject textObj = GameObject.Find("ResultText");
            if (textObj != null)
            {
                var txt = textObj.GetComponent<UnityEngine.UI.Text>();
                if (txt != null)
                {
                    txt.text = result;
                }
            }
        });
    }
}
