using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class LocationSelector : MonoBehaviour
{
    public TMP_Dropdown sourceDropdown; // Updated to use TMP_Dropdown
    public TMP_Dropdown destinationDropdown;
    public Transform indicator;
    public Camera arCamera;
    public List<Transform> locations = new List<Transform>();

    void Start()
    {
        if (sourceDropdown == null || destinationDropdown == null)
        {
            Debug.LogError("Source or Destination Dropdown is not assigned in the Inspector!");
            return;
        }

        // Clear existing options
        sourceDropdown.ClearOptions();
        destinationDropdown.ClearOptions();

        // Populate dropdown with location names
        List<string> locationNames = new List<string>();
        foreach (Transform loc in locations)
        {
            locationNames.Add(loc.name);
        }

        sourceDropdown.AddOptions(locationNames);
        destinationDropdown.AddOptions(locationNames);

        sourceDropdown.onValueChanged.AddListener(UpdateIndicatorPosition);
    }

    void UpdateIndicatorPosition(int index)
    {
        if (index >= 0 && index < locations.Count && indicator != null)
        {
            indicator.position = locations[index].position;
        }
        else
        {
            Debug.LogError("Indicator Transform is not assigned or index is invalid!");
        }
    }

    public void SetSourceFromVoice(string recognizedText)
    {
        int index = GetLocationIndexFromText(recognizedText);
        if (index != -1)
        {
            sourceDropdown.value = index;
            indicator.position = locations[index].position;
            Debug.Log("✅ Source set to: " + locations[index].name);
        }
        else
        {
            Debug.LogWarning("❌ Could not recognize source location: " + recognizedText);
        }
    }

    public void SetTargetFromVoice(string recognizedText)
    {
        int index = GetLocationIndexFromText(recognizedText);
        if (index != -1)
        {
            destinationDropdown.value = index;
            Debug.Log("✅ Destination set to: " + locations[index].name);
        }
        else
        {
            Debug.LogWarning("❌ Could not recognize destination location: " + recognizedText);
        }
    }

    private int GetLocationIndexFromText(string text)
    {
        text = text.ToLower().Trim();
        for (int i = 0; i < locations.Count; i++)
        {
            if (locations[i].name.ToLower().Contains(text))
            {
                return i;
            }
        }
        return -1; // Not found
    }
}