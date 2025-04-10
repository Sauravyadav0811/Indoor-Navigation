using UnityEngine;
using System;

public class STTCallbackHelper : MonoBehaviour
{
    public static STTCallbackHelper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnResult(string recognizedText)
    {
        Debug.Log("STTCallbackHelper: onResult: " + recognizedText);
        CallOnMainThread(() =>
        {
            STTUtils.OnResultCallback(recognizedText);
        });
    }

    public void CallOnMainThread(Action action)
    {
        if (Application.isPlaying)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(action);
        }
    }

    public void AutoDestroy(GameObject obj)
    {
        CallOnMainThread(() =>
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        });
    }
}
