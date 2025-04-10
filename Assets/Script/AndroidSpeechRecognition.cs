using UnityEngine;
using UnityEngine.Android;
using System;

namespace CustomSpeechRecognition
{
    public class AndroidSpeechRecognizer : MonoBehaviour
    {
        public static AndroidSpeechRecognizer Instance;

        public static Action<string> OnResultCallback;

        private AndroidJavaObject activityContext;
        private AndroidJavaObject speechPlugin;

        void Awake()
        {
            if (Instance == null)
                Instance = this;

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            speechPlugin = new AndroidJavaObject("com.yourcompany.speechplugin.SpeechPlugin", activityContext);
#endif
        }

        public void StartRecording(string message, string language)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            speechPlugin.Call("startListening", message, language);
#endif
        }

        public void onSpeechResult(string recognizedText)
        {
            Debug.Log("Speech result received: " + recognizedText);
            OnResultCallback?.Invoke(recognizedText);
        }
    }
}
