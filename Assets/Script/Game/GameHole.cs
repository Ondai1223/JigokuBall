using UnityEngine;

public class GameHole
{
    // 穴の全体を管理するスクリプト
    // 穴の2Dコライダーを管理する
    private PolygonCollider2D[] hole2Ds;
    private GameObject[] holeParents;

    public PolygonCollider2D[] Hole2Ds { get { return hole2Ds; } }
    public GameObject[] HoleParents { get { return holeParents; } }

    public GameHole(PolygonCollider2D[] hole2Ds, GameObject[] holeParents)
    {
        this.hole2Ds = hole2Ds;
        this.holeParents = holeParents;
        Debug.Log("Initialized GameHole with " + hole2Ds.Length + " holes.");
    }
    public void UpdateHoles2DPosition(float cosAngle)
    {
        for (int i = 0; i < hole2Ds.Length; i++)
        {
            Vector3 newPosition = holeParents[i].transform.localPosition;
            Vector3 newHoleLocalPos = hole2Ds[i].transform.localPosition;
            newHoleLocalPos.y = newPosition.z / 1.75f;
            Debug.Log($"cosAngle: {cosAngle}");
            hole2Ds[i].transform.localPosition = newHoleLocalPos;
        }
    }
    
}
