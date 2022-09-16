using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public List<Vector3> Points = new List<Vector3>();
    public float Speed = 5f;
    int _current = 0;

    void Start()
    {
        if(Points.Count < 1)
        {
            enabled = false;
            return;
        }
        transform.position = Points[0];
        _current = 1;
    }

    void Update()
    {
        if (Points.Count < 1)
        {
            enabled = false;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, Points[_current], Time.deltaTime * Speed);
        if(Vector3.Distance(transform.position, Points[_current]) < 0.1f)
        {
            ++_current;
            if(_current >= Points.Count)
            {
                _current = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Points.Count < 1) return;
        Gizmos.DrawSphere(Points[0], 0.3f);
        for (int i = 1; i < Points.Count; ++i)
        {
            Gizmos.DrawSphere(Points[i], 0.3f);
            Gizmos.DrawLine(Points[i], Points[i - 1]);
        }
    }
}
