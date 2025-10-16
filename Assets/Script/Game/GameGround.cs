using UnityEngine;

public class GameGround
{
    // 地面の管理を行うスクリプト
    private PolygonCollider2D ground2D;
    public PolygonCollider2D Ground2D { get { return ground2D; } }

    public GameGround(PolygonCollider2D ground2D)
    {
        this.ground2D = ground2D;
    }
    public void calcGroundOutline(GameObject groundObject, float cosAngle)
    {
        Vector3 scale = groundObject.transform.localScale;
        // 地面のアウトラインを設定する
        float halfX = scale.x / 2f;
        // Y軸スケールを補正し、2DコライダーのY軸サイズとして使用
        float halfY_corrected = (scale.y / 2f) / cosAngle;
        Vector2[] outlinePoints = new Vector2[]
        {
            new Vector2(-halfX, -halfY_corrected),
            new Vector2(-halfX, halfY_corrected),
            new Vector2(halfX, halfY_corrected),
            new Vector2(halfX, -halfY_corrected)
        };
        Debug.Log("Ground Outline Points: " + string.Join(", ", outlinePoints));
        ground2D.pathCount = 1;
        ground2D.SetPath(0, outlinePoints);
        if (ground2D.pathCount < 1)
        {
            ground2D.pathCount = 1;
        }
        Debug.Log("Calculated ground outline with cosAngle: " + cosAngle);

    }
    public void MakeHoles2D(PolygonCollider2D[] hole2Ds)
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
        Debug.Log("Updated ground2D with " + hole2Ds.Length + " holes.");
    }

}
