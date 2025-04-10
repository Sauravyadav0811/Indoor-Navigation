using System;
using UnityEngine;



public static class MySpeechToText
{
    public static Action<string> OnResultCallback;

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject speechPlugin;
    private static AndroidJavaObject activityContext;
#endif

    static MySpeechToText()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.plugin.speech.SpeechPlugin")) // replace with your real Java class
            {
                if (pluginClass != null)
                {
                    speechPlugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                    speechPlugin.Call("setContext", activityContext);
                }
            }
        }
#endif
    }

    public static void StartRecording()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (speechPlugin != null)
        {
            speechPlugin.Call("startListening");
        }
#else
        Debug.Log("StartRecording: Only works on Android device.");
#endif
    }

    public static void StopRecording()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (speechPlugin != null)
        {
            speechPlugin.Call("stopListening");
        }
#else
        Debug.Log("StopRecording: Only works on Android device.");
#endif
    }

    // Called by Android plugin via UnitySendMessage
    public static void OnFinalResult(string recognizedText)
    {
        Debug.Log("Speech Recognized: " + recognizedText);
        OnResultCallback?.Invoke(recognizedText);
    }
}
