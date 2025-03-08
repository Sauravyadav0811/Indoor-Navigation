using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown TargetDropDown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();

    private NavMeshPath path; // Current calculated path
    private LineRenderer line; // Line renderer to display path
    private Vector3 targetPosition = Vector3.zero; // Target position
    private bool lineToggle = false;

    [SerializeField] private Transform player; // Player or start position

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
    }

    private void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)
        {
            UpdatePath();
        }
    }

    private void UpdatePath()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing!");
            return;
        }

        path.ClearCorners(); // Clear previous path
        bool validPath = NavMesh.CalculatePath(player.position, targetPosition, NavMesh.AllAreas, path);

        if (validPath && path.corners.Length > 1)
        {
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            Debug.Log("Path found. Corners count: " + path.corners.Length);
        }
        else
        {
            Debug.LogWarning("No valid path found! Ensure target is reachable.");
            line.positionCount = 0;
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPosition = Vector3.zero;
        string selectedText = TargetDropDown.options[selectedValue].text;

        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            Vector3 rawTargetPos = currentTarget.PositionObject.transform.position;

            // Ensure target is on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(rawTargetPos, out hit, 2.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
                Debug.Log("Target set to: " + targetPosition);
            }
            else
            {
                Debug.LogWarning("Target is not on the NavMesh!");
                return;
            }
        }

        if (lineToggle)
        {
            UpdatePath();
        }
    }

    public void ToggleVisibility()
    {
        if (line == null)
        {
            Debug.LogError("LineRenderer not found!");
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
