using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Line m_LinePrefab;

    private List<Line> m_Lines;
    private Camera m_Camera;
    private Line m_CurrentLine;

    private Vector3 m_StartPoint = Vector3.zero;
    private float x, z;
    private bool m_CanMoveX = false;

    private void Start() {
        m_Lines = new List<Line>();
        m_Camera = Camera.main;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            m_CurrentLine = Instantiate(m_LinePrefab, transform, false);
            m_CurrentLine.transform.position = Vector3.zero;
            m_Lines.Add(m_CurrentLine);

            var mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_StartPoint = new Vector3(mousePos.x, 1, mousePos.z);
            m_CurrentLine.StartDraw(m_StartPoint);
        }
        if(Input.GetMouseButton(0)) {
            UpdateMouseInput();
            m_CurrentLine.Draw(new Vector3(x, 1, z));
        }
        if(Input.GetMouseButtonUp(0)) {
            //m_Line.EndDraw();
            m_CanMoveX = false;
        }
    }

    private void UpdateMouseInput() {
        float h = Mathf.Abs(Input.GetAxis("Mouse X"));
        float v = Mathf.Abs(Input.GetAxis("Mouse Y"));
        if (h != 0 && v != 0) {
            if (h > v || x >= m_StartPoint.x + 0.05f) {
                m_CanMoveX = true;
            }
            else if (v > h || z >= m_StartPoint.z + 0.05f) {
                m_CanMoveX = false;
            }
        }
        if (m_CanMoveX) {
            x = m_Camera.ScreenToWorldPoint(Input.mousePosition).x;
            z = m_StartPoint.z;
        }
        else {
            z = m_Camera.ScreenToWorldPoint(Input.mousePosition).z;
            x = m_StartPoint.x;
        }
    }
}
