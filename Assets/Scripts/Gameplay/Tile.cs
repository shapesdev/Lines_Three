using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Claim
{
    None, Player1, Player2
}

public class Tile : MonoBehaviour
{
    private Tile m_North;
    private Tile m_East;
    private Tile m_West;
    private Tile m_South;

    public Claim m_ClaimedBy;

    public bool HasSouthNeighbor() => m_South != null;

    public static void MakeNeighborsSouthNorth(Tile south, Tile north) {
        north.m_South = south;
        south.m_North = north;
    }

    public static void MakeNeighborsEastWest(Tile east, Tile west) {
        east.m_West = west;
        west.m_East = east;
    }
}
