using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class LocationSelector : MonoBehaviour
{
    public TMP_Dropdown sourceDropdown; // Updated to use TMP_Dropdown
    public Transform indicator;
    public Camera arCamera;
    public List<Transform> locations = new List<Transform>();

    void Start()
    {
        if (sourceDropdown == null)
        {
            Debug.LogError("Source Dropdown is not assigned in the Inspector!");
            return;
        }

        // Clear existing options
        sourceDropdown.ClearOptions();

        // Populate dropdown with location names
        List<string> locationNames = new List<string>();
        for (int i = 0; i < locations.Count; i++)
        {
            locationNames.Add(locations[i].name);
        }

        sourceDropdown.AddOptions(locationNames);
        sourceDropdown.onValueChanged.AddListener(UpdateIndicatorPosition);
    }

    void UpdateIndicatorPosition(int index)
    {
        if (index >= 0 && index < locations.Count)
        {
            if (indicator != null)
            {
                indicator.position = locations[index].position;
            }
            else
            {
                Debug.LogError("Indicator Transform is not assigned!");
            }
        }
    }
}
