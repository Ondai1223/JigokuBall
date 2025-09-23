using UnityEngine;

public class Ball : MonoBehaviour
{
    public static System.Action OnHitHole; // 当たったことを知らせるイベント

    private Vector3 initialPosition;
    [SerializeField] Collider ballCollider;
    void Awake()
    {
        initialPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        ballCollider.isTrigger = false; // トリガーを無効にする
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            Debug.Log("穴に落ちた！");
            
            ballCollider.isTrigger = true;
            // ボールが落ちる目標地点（穴の中心）を設定
            //transform.position = other.transform.position;
            
            // イベントを発生させる
            if (OnHitHole != null)
            {
                OnHitHole();
            }
        }
    }
}