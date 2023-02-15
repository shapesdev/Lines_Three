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
    private List<DrawSpot> m_DrawSpots;
    private Camera m_Camera;
    private Line m_CurrentLine;

    private DrawSpot m_StartDrawSpot;
    private float m_LineCorrection = 0.05f;
    private bool m_CanMoveX = false;
    private bool m_CanDraw = false;

    public void Init(List<DrawSpot> points) {
        m_Lines = new List<Line>();
        m_Camera = Camera.main;
        m_DrawSpots = points;
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

        foreach (var spot in m_DrawSpots) {
            var dist = Vector3.Distance(spot.Position, mousePos);
            if (dist < shortestDistance) {
                shortestPoint = spot.Position;
                shortestDistance = dist;
                m_StartDrawSpot.Direction = spot.Direction;
            }
        }
        m_StartDrawSpot.Position = new Vector3(shortestPoint.x, 1, shortestPoint.z);
        m_CurrentLine.StartDraw(m_StartDrawSpot.Position);
    }

    private float x, z;
    private Direction dir = Direction.None;

    private void UpdateDraw() {
        float h = Mathf.Abs(Input.GetAxis("Mouse X"));
        float v = Mathf.Abs(Input.GetAxis("Mouse Y"));

        if (h != 0 && v != 0) {
            if (h > v || x >= m_StartDrawSpot.Position.x + 0.05f) {
                m_CanMoveX = true;
                if (x < m_StartDrawSpot.Position.x) dir = Direction.West;
                else if (x > m_StartDrawSpot.Position.x) dir = Direction.East;
            }
            else if (v > h || z >= m_StartDrawSpot.Position.z + 0.05f) {
                if (z < m_StartDrawSpot.Position.z) dir = Direction.South;
                else if (z > m_StartDrawSpot.Position.z) dir = Direction.North;
                m_CanMoveX = false;
            }
        }
        Draw();
    }

    private void Draw() {
        Vector3 inputPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        if (m_CanMoveX) {
            x = Mathf.Clamp(inputPos.x, m_StartDrawSpot.Position.x - m_LineWidth, m_StartDrawSpot.Position.x + m_LineWidth);
            z = m_StartDrawSpot.Position.z;
        }
        else {
            z = Mathf.Clamp(inputPos.z, m_StartDrawSpot.Position.z - m_LineWidth, m_StartDrawSpot.Position.z + m_LineWidth);
            x = m_StartDrawSpot.Position.x;
        }
        m_CurrentLine.Draw(new Vector3(x, 1, z));
    }

    private void EndDraw() {
        Vector3 endPos = m_CurrentLine.GetPosition(1);
        float result;

        if (m_CanMoveX) result = Mathf.Abs(endPos.x - m_StartDrawSpot.Position.x);
        else result = Mathf.Abs(endPos.z - m_StartDrawSpot.Position.z);

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
