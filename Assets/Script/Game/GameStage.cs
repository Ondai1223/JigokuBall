using UnityEngine;
public class GameStage : MonoBehaviour
{
    // ゲームステージ全体の管理を行うスクリプト
    // 穴と地面の全体を管理する
    [SerializeField] private MeshCollider generatedMeshCollider;
    [SerializeField] private GameObject groundObject;
    [SerializeField] private GameObject floorObject;
    [SerializeField] private PolygonCollider2D ground2D;
    [SerializeField] private PolygonCollider2D[] hole2Ds;
    [SerializeField] private GameObject[] holeParents;
    private GameGround gameGround;
    private GameHole gameHole;
    private Mesh generatedMesh;
    private float cosAngle;

    private void Start() 
    {
        gameGround = new GameGround(ground2D);
        gameHole = new GameHole(hole2Ds, holeParents);
        cosAngle = GameHelper.CalcCosAngle(floorObject);
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
}
