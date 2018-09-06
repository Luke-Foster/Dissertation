using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public int chronosHealth;

    public PointInTime(Vector3 _position, Quaternion _rotation, int _chronosHealth)
    {
        position = _position;
        rotation = _rotation;
        chronosHealth = _chronosHealth;
    }
}
