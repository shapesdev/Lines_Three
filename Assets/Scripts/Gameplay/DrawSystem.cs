using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LineRenderer m_LineRenderer;

    private Camera m_Camera;
    private Vector3 m_StartPoint = Vector3.zero;
    private bool m_CanMoveX = false;
    private float x, z;

    private void Start() {
        m_Camera = Camera.main;
        m_LineRenderer.positionCount = 0;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            var mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_StartPoint = new Vector3(mousePos.x, 1, mousePos.z);
            m_LineRenderer.positionCount++;
            m_LineRenderer.SetPosition(0, m_StartPoint);
        }
        if(Input.GetMouseButton(0)) {
            float h = Mathf.Abs(Input.GetAxis("Mouse X"));
            float v = Mathf.Abs(Input.GetAxis("Mouse Y"));
            if (h != 0 && v != 0) {
                if (h > v || x >= m_StartPoint.x + 0.05f) {
                    m_CanMoveX = true;
                }
                else if(v > h || z >= m_StartPoint.z + 0.05f) {
                    m_CanMoveX = false;
                }
            }

            m_LineRenderer.positionCount = 2;

            if(m_CanMoveX) {
                x = m_Camera.ScreenToWorldPoint(Input.mousePosition).x;
                z = m_StartPoint.z;
            }
            else {
                z = m_Camera.ScreenToWorldPoint(Input.mousePosition).z;
                x = m_StartPoint.x;
            }
            m_LineRenderer.SetPosition(1, new Vector3(x, 1, z));
        }
        if(Input.GetMouseButtonUp(0)) {
            m_LineRenderer.positionCount = 0;
            m_CanMoveX = false;
        }
    }
}
