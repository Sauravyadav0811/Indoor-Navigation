#if UNITY_EDITOR || UNITY_ANDROID
using UnityEngine;

namespace SpeechToTextNamespace
{
    public class STTInteractionCallbackAndroid : AndroidJavaProxy
    {
        private readonly ISpeechToTextListener listener;
        private readonly STTCallbackHelper callbackHelper;

        public STTInteractionCallbackAndroid(ISpeechToTextListener listener)
            : base("com.yasirkula.unity.SpeechToTextListener")
        {
            this.listener = listener;

            GameObject go = new GameObject("STTCallbackHelper");
            callbackHelper = go.AddComponent<STTCallbackHelper>();
        }

        [UnityEngine.Scripting.Preserve]
        public void OnReadyForSpeech() =>
            callbackHelper.CallOnMainThread(listener.OnReadyForSpeech);

        [UnityEngine.Scripting.Preserve]
        public void OnBeginningOfSpeech() =>
            callbackHelper.CallOnMainThread(listener.OnBeginningOfSpeech);

        [UnityEngine.Scripting.Preserve]
        public void OnVoiceLevelChanged(float rmsdB)
        {
            float normalized = Mathf.Clamp01(0.1f * Mathf.Pow(10f, rmsdB / 10f));
            callbackHelper.CallOnMainThread(() => listener.OnVoiceLevelChanged(normalized));
        }

        [UnityEngine.Scripting.Preserve]
        public void OnPartialResultReceived(string spokenText)
        {
            if (!string.IsNullOrEmpty(spokenText))
                callbackHelper.CallOnMainThread(() => listener.OnPartialResultReceived(spokenText));
        }

        [UnityEngine.Scripting.Preserve]
        public void OnResultReceived(string spokenText, int errorCode)
        {
            if (errorCode == 7) errorCode = 6;

            callbackHelper.CallOnMainThread(() =>
            {
                try
                {
                    listener.OnResultReceived(!string.IsNullOrEmpty(spokenText) ? spokenText : null, errorCode >= 0 ? (int?)errorCode : null);
                }
                finally
                {
                    Object.DestroyImmediate(callbackHelper.gameObject);
                }
            });
        }
    }
}
#endif
