using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomSpeechRecognition;

public class VoiceNavigationManager : MonoBehaviour
{
    [Header("Reference to Navigation Script")]
    public SetNavigationTarget navigationTargetScript;

    [Header("Dropdowns")]
    public TMP_Dropdown sourceDropdown;
    public TMP_Dropdown destinationDropdown;

    [Header("Voice Recognizer")]
    public AndroidSpeechRecognizer speechRecognizer;

    private bool sourceSet = false;
    private bool destinationSet = false;

    void Start()
    {
        // Delay then start the source listening flow
        StartCoroutine(StartVoiceInputFlow());
    }

    IEnumerator StartVoiceInputFlow()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("🎤 Listening for Source...");
        speechRecognizer.StartRecording("Please say the source", "en-US");

        AndroidSpeechRecognizer.OnResultCallback = (string result) =>
        {
            if (!sourceSet)
            {
                SetDropdownByVoice(result, sourceDropdown, isSource: true);
            }
            else if (!destinationSet)
            {
                SetDropdownByVoice(result, destinationDropdown, isSource: false);
            }
        };
    }

    void SetDropdownByVoice(string voiceText, TMP_Dropdown dropdown, bool isSource)
    {
        voiceText = voiceText.ToLower().Trim();

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            string option = dropdown.options[i].text.ToLower();

            if (option.Contains(voiceText) || voiceText.Contains(option))
            {
                dropdown.value = i;
                dropdown.RefreshShownValue();

                if (isSource)
                {
                    navigationTargetScript.SetSource(i);
                    sourceSet = true;

                    Debug.Log("✅ Source set via voice: " + option);
                    StartCoroutine(WaitForDestination());
                }
                else
                {
                    navigationTargetScript.SetDestination(i);
                    destinationSet = true;

                    Debug.Log("✅ Destination set via voice: " + option);

                    // Once both are set, draw path
                    navigationTargetScript.ToggleVisibility();
                }

                return;
            }
        }

        Debug.LogWarning("❌ Voice match not found in dropdown: " + voiceText);
    }

    IEnumerator WaitForDestination()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("🎤 Listening for Destination...");
        speechRecognizer.StartRecording("Please say the destination", "en-US");
    }
}
