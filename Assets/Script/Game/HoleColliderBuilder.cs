using UnityEngine;

public class HoleColliderBuilder : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D ground2D;
    [SerializeField] private MeshCollider generatedMeshCollider;
    [SerializeField] private GameObject floorObject;
    [SerializeField] private PolygonCollider2D[] hole2Ds;
    [SerializeField] private GameObject[] holeParents;
    private Mesh generatedMesh;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
        ground2D.transform.position = floorObject.transform.position;
    }

    private void FixedUpdate()
    {

        // 親オブジェクトの移動量を計算
        //transform.position - initialPos --> transform.position
        for (int i = 0; i < hole2Ds.Length; i++)
        {
            if (holeParents[i].transform.hasChanged)
            {
                holeParents[i].transform.hasChanged = false;
                hole2Ds[i].transform.localRotation = holeParents[i].transform.localRotation;
                hole2Ds[i].transform.localPosition = new Vector2(holeParents[i].transform.localPosition.x, holeParents[i].transform.localPosition.z);
                hole2Ds[i].transform.localScale = holeParents[i].transform.localScale * 0.5f;
                MakeHoles2D();
                MakeMeshCollider();
            }
            
        }

        // Vector3 relativePosition = transform.position - initialPos;
        // hole2D.transform.position = new Vector2(relativePosition.x, relativePosition.z);




    }

    private void MakeHoles2D()
    {
        // 最初のパスは地面の輪郭
        // 穴の数 + 1 のパスを準備
        ground2D.pathCount = hole2Ds.Length + 1;

        for (int i = 0; i < hole2Ds.Length; i++)
        {
            // 各穴のパスを取得
            Vector2[] pointPositions = hole2Ds[i].GetPath(0);

            // 各パスの点をワールド座標に変換し、ground2Dのローカル座標に戻す
            for (int j = 0; j < pointPositions.Length; j++)
            {
                Vector3 worldPoint = hole2Ds[i].transform.TransformPoint(pointPositions[j]);
                pointPositions[j] = ground2D.transform.InverseTransformPoint(worldPoint);
            }

            // ground2Dの2番目以降のパスとして設定
            ground2D.SetPath(i + 1, pointPositions);
        }
    }

    private void MakeMeshCollider()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2D.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}