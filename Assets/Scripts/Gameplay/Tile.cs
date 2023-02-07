using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Tile m_North;
    private Tile m_East;
    private Tile m_West;
    private Tile m_South;

    public static void MakeNeighborsSouthNorth(Tile south, Tile north) {
        north.m_South = south;
        south.m_North = north;
    }

    public static void MakeNeighborsEastWest(Tile east, Tile west) {
        east.m_West = west;
        west.m_East = east;
    }
}
