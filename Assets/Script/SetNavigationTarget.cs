using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown SourceDropDown;
    [SerializeField] private TMP_Dropdown TargetDropDown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private GameObject indicatorSphere; // ✅ Reference to the indicator sphere

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 sourcePosition = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;

    private void Start()
    {
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        if (line == null)
        {
            Debug.LogError("LineRenderer component missing!");
            return;
        }

        line.enabled = false;
        line.positionCount = 0;

        PopulateDropdowns(); // Populate dropdowns on start
    }

    private void Update()
    {
        if (lineToggle && sourcePosition != Vector3.zero && targetPosition != Vector3.zero)
        {
            UpdatePath();
        }
    }

    private void PopulateDropdowns()
    {
        List<string> options = new List<string>();

        foreach (var target in navigationTargetObjects)
        {
            options.Add(target.Name);
        }

        SourceDropDown.ClearOptions();
        TargetDropDown.ClearOptions();
        SourceDropDown.AddOptions(options);
        TargetDropDown.AddOptions(options);
    }

    public void SetSource(int selectedValue)
    {
        string selectedText = SourceDropDown.options[selectedValue].text;
        Target currentSource = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));

        if (currentSource != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(currentSource.PositionObject.transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                sourcePosition = hit.position;
                Debug.Log("✅ Source set to: " + sourcePosition);

                // ✅ Move the Indicator Sphere to the selected source position
                if (indicatorSphere != null)
                {
                    indicatorSphere.transform.position = sourcePosition;
                    Debug.Log("📍 Indicator Sphere moved to source location");
                }
                else
                {
                    Debug.LogWarning("⚠ Indicator Sphere is not assigned!");
                }
            }
            else
            {
                Debug.LogWarning("⚠ Source is not on the NavMesh!");
            }
        }

        // Update the path if already visible
        if (lineToggle)
        {
            UpdatePath();
        }
    }

    public void SetDestination(int selectedValue)
    {
        string selectedText = TargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));

        if (currentTarget != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(currentTarget.PositionObject.transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
                Debug.Log("✅ Target set to: " + targetPosition);
            }
            else
            {
                Debug.LogWarning("⚠ Target is not on the NavMesh!");
            }
        }

        // Update path after setting destination
        if (lineToggle)
        {
            UpdatePath();
        }
    }

    private void UpdatePath()
    {
        if (sourcePosition == Vector3.zero || targetPosition == Vector3.zero)
        {
            Debug.LogWarning("⚠ Source or destination is not set!");
            return;
        }

        path.ClearCorners();
        bool validPath = NavMesh.CalculatePath(sourcePosition, targetPosition, NavMesh.AllAreas, path);

        if (validPath && path.corners.Length > 1)
        {
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            Debug.Log("🛤️ Path found. Corners count: " + path.corners.Length);
        }
        else
        {
            Debug.LogWarning("⚠ No valid path found!");
            line.positionCount = 0;
        }
    }

    public void ToggleVisibility()
    {
        if (line == null)
        {
            Debug.LogError("⚠ LineRenderer not found!");
            return;
        }

        lineToggle = !lineToggle;
        line.enabled = lineToggle;

        if (lineToggle)
        {
            UpdatePath();
        }
    }
}
