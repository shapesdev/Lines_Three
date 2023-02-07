using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Tile m_TilePrefab;

    [SerializeField, Range(5, 13)]
    [Header("Grid Settings")]
    private int m_GridSize = 5;
    [SerializeField]
    private int m_Spacing;

    private Tile[,] m_Tiles;
    private Camera m_Camera;

    void Start()
    {
        Init();
    }

    private void Init() {
        m_Camera = Camera.main;
        m_Tiles = new Tile[m_GridSize, m_GridSize];
        GenerateRhombusGrid();
        SetupCamera();
    }

    private void SetupCamera() {
        Vector3 centerPos = m_Tiles[m_GridSize / 2, m_GridSize / 2].transform.localPosition;
        float orthoSize = m_GridSize / 2 + 1;
        m_Camera.transform.localPosition = new Vector3(centerPos.x, m_Camera.transform.localPosition.y, centerPos.z);
        m_Camera.orthographicSize = orthoSize;
    }

    private void GenerateRhombusGrid() {
        int x = 0, y = 0;
        int tileCount = 1, index = m_GridSize / 2, n = 0;

        for(int i = 0; i < m_GridSize; i++) {
            int tempX = x;
            for (int j = 0; j < m_GridSize; j++) {
                if (n < tileCount && j >= index) {
                    Tile tile = SpawnTile(tempX, y, j, i);
                    if(j > 0) {
                        if(m_Tiles[j - 1, i] != null) {
                            Tile.MakeNeighborsEastWest(tile, m_Tiles[j - 1, i]);
                        }
                        if(i > 0 && m_Tiles[j, i - 1] != null) {
                            Tile.MakeNeighborsSouthNorth(tile, m_Tiles[j, i - 1]);
                        }
                    }
                    tempX += m_Spacing;
                    n++;
                }
            }
            n = 0;
            y -= m_Spacing;
            if(i < m_GridSize / 2) {
                tileCount += 2;
                x -= m_Spacing;
                index--;
            }
            else {
                tileCount -= 2;
                x += m_Spacing;
                index++;
            }
        }
    }

    private Tile SpawnTile(int x, int y, int j, int i) {
        Tile tile = m_Tiles[j, i] = Instantiate(m_TilePrefab, transform, false);
        tile.transform.localPosition = new Vector3(x, 0, y);
        tile.gameObject.name += $"{j} {i}";
        return tile;
    }
}
