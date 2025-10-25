using System.Collections.Generic;
using UnityEngine;
public class GameStage : MonoBehaviour
{
    // ゲームステージ全体の管理を行うスクリプト
    // 穴と地面の全体を管理する
    [SerializeField] private MeshCollider generatedMeshCollider;
    [SerializeField] private GameObject groundObject;
    [SerializeField] private PolygonCollider2D ground2D;
    private PolygonCollider2D[] hole2Ds;
    private GameObject[] holeParents;
    private GameGround gameGround;
    private GameHole gameHole;
    private Mesh generatedMesh;
    private float cosAngle;

    private void Start()
    {
        gameGround = new GameGround(ground2D);
        cosAngle = GameHelper.CalcCosAngle(this.gameObject);
        getHoleParentsFromWorld();
        get2DHolesFromWorld();
        gameHole = new GameHole(hole2Ds, holeParents);
        gameGround.calcGroundOutline(groundObject, cosAngle);
        gameGround.MakeHoles2D(gameHole.Hole2Ds);
        MakeMeshCollider();
    }
    private void MakeMeshCollider()
    {
        if (generatedMesh != null)
        {
            Mesh.Destroy(generatedMesh);
        }
        generatedMesh = gameGround.Ground2D.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }


    private void FixedUpdate()
    {
        // 親オブジェクトの移動量を計算
        gameHole.UpdateHoles2DPosition(cosAngle);
        gameGround.MakeHoles2D(gameHole.Hole2Ds);
        MakeMeshCollider();
    }

    private void getHoleParentsFromWorld()
    {
        List<GameObject> grandchildrenList = new List<GameObject>();
        Transform holeparent = transform.Find("HoleParents");
        foreach (Transform grandchildTransform in holeparent)
        {
            grandchildrenList.Add(grandchildTransform.gameObject);
        }
        holeParents = grandchildrenList.ToArray();
        Debug.Log("Number of hole parents: " + holeParents.Length);
    }

    private void get2DHolesFromWorld()
    {
        hole2Ds = new PolygonCollider2D[holeParents.Length];
        for (int i = 0; i < holeParents.Length; i++)
        {
            hole2Ds[i] = holeParents[i].transform.Find("2DHole").GetComponent<PolygonCollider2D>();
        }
        Debug.Log("Number of 2D holes: " + hole2Ds.Length);
    }

}
