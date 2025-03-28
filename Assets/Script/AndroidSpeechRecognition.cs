using UnityEngine;
using UnityEngine.Android;
using System.Collections;

public class AndroidSpeechRecognition : MonoBehaviour
{
    public VoiceNavigation voiceNavigation;  // Assign in Inspector

    void Start()
    {
        // Request microphone permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }

    public void StartListening()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject intent = new AndroidJavaObject("android.speech.RecognizerIntent"))
            {
                intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.LANGUAGE_MODEL", "android.speech.extra.LANGUAGE_MODEL_FREE_FORM");
                intent.Call<AndroidJavaObject>("putExtra", "android.speech.extra.PROMPT", "Speak now...");

                activity.Call("startActivityForResult", intent, 10);
            }
        }
    }

    // Call this from Unity's Android Plugin
    public void ReceiveSpeechResult(string recognizedText)
    {
        voiceNavigation.ProcessSpeechResult(recognizedText);
    }
}
