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
            Vector3 ParentLocalPos = holeParents[i].transform.localPosition;
            Vector3 newHoleLocalPos = Vector3.zero;
            newHoleLocalPos.y = ParentLocalPos.z / cosAngle / holeParents[i].transform.localScale.y;
            
            hole2Ds[i].transform.localRotation = holeParents[i].transform.localRotation;
            hole2Ds[i].transform.localPosition = newHoleLocalPos;
            
        }
    }
    
}
