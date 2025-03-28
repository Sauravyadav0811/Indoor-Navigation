using System;
using UnityEngine;

[Serializable]
public class Target
{
    [SerializeField] private string name;
    [SerializeField] private GameObject positionObject;

    public string Name { get => name; set => name = value; }
    public GameObject PositionObject { get => positionObject; set => positionObject = value; }

    public Vector3 Position => positionObject != null ? positionObject.transform.position : Vector3.zero;

    public Target(string name, GameObject positionObject)
    {
        this.name = name;
        this.positionObject = positionObject;
    }
}
