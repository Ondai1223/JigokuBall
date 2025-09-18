using UnityEngine;

public class HoleColliderBuilder : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D ground2D;
    [SerializeField] private PolygonCollider2D hole2D;
    [SerializeField] private MeshCollider generatedMeshCollider;
    private Mesh generatedMesh;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
        hole2D.transform.position = Vector3.zero;
        ground2D.transform.position = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            hole2D.transform.localScale = transform.localScale * 0.5f;
            // 親オブジェクトの移動量を計算
            Vector3 relativePosition = transform.position - initialPos;
            hole2D.transform.position = new Vector2(relativePosition.x, relativePosition.z);

            MakeHole2D();
            MakeMeshCollider();
        }

    }

    private void MakeHole2D()
    {
        Vector2[] pointPositions = hole2D.GetPath(0);

        for (int i = 0; i < pointPositions.Length; i++)
        {
            pointPositions[i] = hole2D.transform.TransformPoint(pointPositions[i]);

        }

        ground2D.pathCount = 2;
        ground2D.SetPath(1, pointPositions);

        /** 追加
        Vector2[] transformedPoints = new Vector2[pointPositions.Length];

        for (int i = 0; i < pointPositions.Length; i++)
        {
            // hole2Dのローカル座標を3DのVector3に変換
            // 地面がXZ平面にあると仮定し、Vector2のyをVector3のzに割り当てる
            Vector3 point3D = new Vector3(pointPositions[i].x, 0, pointPositions[i].y);

            // 親オブジェクトのTransformPoint()を使ってワールド座標に変換し、
            // その後ground2DのInverseTransformPoint()でground2Dのローカル座標に変換
            Vector3 transformedPoint3D = ground2D.transform.InverseTransformPoint(transform.TransformPoint(point3D));
            
            // 変換後の3D座標を2Dに戻す（xとzを使用）
            transformedPoints[i] = new Vector2(transformedPoint3D.x, transformedPoint3D.z);
        }

        ground2D.pathCount = 2;
        ground2D.SetPath(1, transformedPoints);
        */
    }

    private void MakeMeshCollider()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2D.CreateMesh(true, true);
        generatedMesh.RecalculateNormals();
        generatedMesh.RecalculateBounds();
        generatedMeshCollider.sharedMesh = generatedMesh;
       

        
    }
}