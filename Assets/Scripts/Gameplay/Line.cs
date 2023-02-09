using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer m_LineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 0;
    }

    public void StartDraw(Vector3 pos) {
        m_LineRenderer.positionCount++;
        m_LineRenderer.SetPosition(0, pos);
    }

    public void EndDraw() {
        m_LineRenderer.positionCount = 0;
    }

    public void Draw(Vector3 pos) {
        m_LineRenderer.positionCount = 2;
        m_LineRenderer.SetPosition(1, pos);
    }

    public Vector3 GetPosition(int n) {
        return m_LineRenderer.GetPosition(n);
    }
}
