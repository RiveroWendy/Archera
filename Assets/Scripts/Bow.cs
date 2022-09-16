using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Transform Top;
    public Transform Bot;
    public Transform TopAnchor;
    public Transform BotAnchor;
    public Transform Drag;
    public LineRenderer Line;
    public LineRenderer DragLine;
    public Gradient DrawGradient;
    public Arrow Arrow;

    public float MaxDistance = 1f;
    public float MaxDragDistance = 5f;
    public float MaxRotationTop = 1f;
    public float MaxRotationBot = 1f;

    public float MinForce = 0f;
    public float MaxForce = 0f;

    float _currentForce = 0f;
    Vector3 _touchedPoint;
    bool _dragging = false;
    Camera _inputCamera;
    float _dragOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _inputCamera = Camera.main;
        _dragOffset = Drag.localPosition.x;
        SetForce(0f);
        Arrow.transform.position = Drag.position;
        Arrow.transform.rotation = transform.rotation;
        DragLine.enabled = false;
    }

    Vector3 _touching;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Arrow.Drag())
        {
            _dragging = true;
            _touchedPoint = _inputCamera.ScreenToWorldPoint(Input.mousePosition);
            _touchedPoint.z = 0f;
            Arrow.transform.position = Drag.position;
            Arrow.transform.rotation = transform.rotation;
            DragLine.enabled = true;
        }

        if(_dragging)
        {
            _touching = _inputCamera.ScreenToWorldPoint(Input.mousePosition);
            _touching.z = 0f;
            var vec = _touchedPoint - _touching;
            var mag = vec.magnitude;
            vec.Normalize();
            transform.right = vec;
            float drag = Mathf.Clamp(mag, 0f, MaxDragDistance);
            SetForce(drag);
            Arrow.transform.position = Drag.position;
            Arrow.transform.rotation = transform.rotation;
            DragLine.SetPosition(0, _touchedPoint);
            DragLine.SetPosition(1, _touching);
            DragLine.startColor = DragLine.endColor = DrawGradient.Evaluate(drag / MaxDragDistance);
        }

        if(_dragging && Input.GetMouseButtonUp(0))
        {
            _dragging = false;
            Arrow.Shoot(_currentForce);
            DragLine.enabled = false;
            SetForce(0f);
        }
    }

    void SetForce(float drag)
    {
        _currentForce = Mathf.Clamp((drag / MaxDragDistance) * MaxForce, MinForce, MaxForce);
        float factor = _currentForce / MaxForce;
        Top.localRotation = Quaternion.Euler(0f, 0f, MaxRotationTop * factor);
        Bot.localRotation = Quaternion.Euler(0f, 0f, MaxRotationBot * factor);
        Drag.localPosition = new Vector3(_dragOffset + MaxDistance * factor, 0f, 0f);
        Line.SetPosition(0, TopAnchor.position);
        Line.SetPosition(1, Drag.position);
        Line.SetPosition(2, BotAnchor.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(TopAnchor.position, 0.05f);
        Gizmos.DrawSphere(Drag.position, 0.05f);
        Gizmos.DrawSphere(BotAnchor.position, 0.05f);

        if (_dragging)
        {
            Gizmos.DrawSphere(_touchedPoint, 0.05f);
            Gizmos.DrawSphere(_inputCamera.ScreenToWorldPoint(Input.mousePosition), 0.05f);
            Gizmos.DrawLine(_touchedPoint, _touching);
            Gizmos.DrawLine(transform.position, Drag.position);
        }
    }
}

