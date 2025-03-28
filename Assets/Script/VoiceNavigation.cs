using UnityEngine;
using TMPro;  // Import TextMeshPro Namespace
using System.Collections;
using System.Collections.Generic;

public class VoiceNavigation : MonoBehaviour
{
    public TMP_Dropdown sourceDropdown;  // Use TMP_Dropdown instead of Dropdown
    public TMP_Dropdown destinationDropdown;
    public GameObject toggleLineButton; // Assign the button that triggers ToggleLine
    public AndroidSpeechRecognition speechRecognizer; // Assign in Inspector

    private bool isSettingSource = true;
    private float waitTime = 10f;

    void Start()
    {
        Invoke("AskForSource", 2f);
    }

    void AskForSource()
    {
        isSettingSource = true;
        Debug.Log("🎤 Please say your source location...");
        StartCoroutine(StartSpeechRecognition());
    }

    void AskForDestination()
    {
        isSettingSource = false;
        Debug.Log("🎤 Please say your destination location...");
        StartCoroutine(StartSpeechRecognition());
    }

    IEnumerator StartSpeechRecognition()
    {
        speechRecognizer.StartListening(); // Calls the Android plugin
        yield return new WaitForSeconds(3); // Wait for speech result
    }

    public void ProcessSpeechResult(string text)
    {
        Debug.Log("Recognized: " + text);
        List<string> options = GetDropdownOptions();

        int index = options.FindIndex(option => option.ToLower().Contains(text.ToLower()));

        if (index != -1)
        {
            if (isSettingSource)
            {
                sourceDropdown.value = index;  // Select dropdown option
                Debug.Log("✅ Source set to: " + options[index]);
                Invoke("AskForDestination", waitTime);
            }
            else
            {
                destinationDropdown.value = index;  // Select dropdown option
                Debug.Log("✅ Destination set to: " + options[index]);

                Invoke("StartNavigation", 5f); // Start navigation after 5 seconds
            }
        }
        else
        {
            Debug.LogWarning("❌ Location not recognized. Please try again.");
        }
    }

    void StartNavigation()
    {
        Debug.Log("🚀 Starting navigation...");
        if (toggleLineButton != null)
        {
            toggleLineButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        }
        else
        {
            Debug.LogError("⚠️ Toggle Line Button is not assigned in the Inspector!");
        }
    }

    List<string> GetDropdownOptions()
    {
        List<string> options = new List<string>();
        foreach (var option in sourceDropdown.options)
        {
            options.Add(option.text);
        }
        return options;
    }
}
