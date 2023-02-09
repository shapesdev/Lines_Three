using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Line m_LinePrefab;

    [Header("Settings"), Range(1, 5)]
    [SerializeField]
    private int m_LineWidth = 2;
    [SerializeField]
    private float m_MaxLineTolerance;

    private List<Line> m_Lines;
    private List<Vector3> m_StartingPoints;
    private Camera m_Camera;
    private Line m_CurrentLine;

    private Vector3 m_StartPoint = Vector3.zero;
    private float m_LineCorrection = 0.05f;
    private bool m_CanMoveX = false;
    private bool m_CanDraw = false;

    public void Init(List<Vector3> points) {
        m_Lines = new List<Line>();
        m_Camera = Camera.main;
        m_StartingPoints = points;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(IsWithinGrid()) {
                StartDraw();
            }
        }
        if(m_CanDraw) {
            if (Input.GetMouseButton(0)) {
                if(IsWithinGrid()) {
                    UpdateDraw();
                }
            }
            if (Input.GetMouseButtonUp(0)) {
                EndDraw();
            }
        }
    }
    
    private void StartDraw() {
        m_CanDraw = true;
        float shortestDistance = int.MaxValue;
        Vector3 shortestPoint = Vector3.zero;
        Vector3 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);

        m_CurrentLine = Instantiate(m_LinePrefab, transform, false);
        m_CurrentLine.transform.position = Vector3.zero;
        m_Lines.Add(m_CurrentLine);

        foreach (var point in m_StartingPoints) {
            var dist = Vector3.Distance(point, mousePos);
            if (dist < shortestDistance) {
                shortestPoint = point;
                shortestDistance = dist;
            }
        }
        //m_StartPoint = new Vector3(mousePos.x, 1, mousePos.z); // Uses current mouse position, previous solution
        m_StartPoint = new Vector3(shortestPoint.x, 1, shortestPoint.z);
        m_CurrentLine.StartDraw(m_StartPoint);
    }

    private float x, z;

    private void UpdateDraw() {
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
            x = Mathf.Clamp(m_Camera.ScreenToWorldPoint(Input.mousePosition).x, m_StartPoint.x - m_LineWidth, m_StartPoint.x + m_LineWidth);
            z = m_StartPoint.z;
        }
        else {
            z = Mathf.Clamp(m_Camera.ScreenToWorldPoint(Input.mousePosition).z, m_StartPoint.z - m_LineWidth, m_StartPoint.z + m_LineWidth);
            x = m_StartPoint.x;
        }
        m_CurrentLine.Draw(new Vector3(x, 1, z));
    }

    private void EndDraw() {
        Vector3 endPos = m_CurrentLine.GetPosition(1);
        float result;

        if (m_CanMoveX) result = Mathf.Abs(endPos.x - m_StartPoint.x);
        else result = Mathf.Abs(endPos.z - m_StartPoint.z);

        if (result < m_LineWidth - m_MaxLineTolerance) {
            Destroy(m_CurrentLine.gameObject);
            Debug.LogWarning("Too short"); // TODO: Add visual feedback to user
        }
        m_CanMoveX = false;
        m_CanDraw = false;
    }

    private bool IsWithinGrid() {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.collider.tag == "Tile") {
                return true;
            }
        }
        return false;
    }
}
