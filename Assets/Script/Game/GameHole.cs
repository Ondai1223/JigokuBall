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
    }
    public void UpdateHoles2DPosition(float cosAngle)
    {
        for (int i = 0; i < hole2Ds.Length; i++)
        {
            hole2Ds[i].transform.localRotation = holeParents[i].transform.localRotation;
            hole2Ds[i].transform.localPosition = new Vector2(holeParents[i].transform.localPosition.x, holeParents[i].transform.localPosition.z / cosAngle);
            hole2Ds[i].transform.localScale = holeParents[i].transform.localScale * 0.5f;
        }
    }
    
}
