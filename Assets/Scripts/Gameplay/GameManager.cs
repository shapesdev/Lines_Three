using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Grid m_Grid;
    [SerializeField]
    private DrawSystem m_DrawSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_Grid.Init();
        m_DrawSystem.Init(m_Grid.GetDrawSpots());
    }
}
