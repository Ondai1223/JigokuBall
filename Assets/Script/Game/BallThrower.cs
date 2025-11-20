using JigokuBall.GameCore;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private Ball ballComponent; // 投球対象の Ball コンポーネント
    [SerializeField] private AttemptManager attemptManager; // 投球状態を管理する AttemptManager
    [SerializeField] private float maxBallSpeed = 350f; // 投球速度の上限
    [SerializeField] private float autoResolveDelay = 5f; // 投球後に自動で決着させるまでの秒数
    [SerializeField] private float minSwipeDistance = 20f; // 投球とみなす最小スワイプ距離

    private GameObject ballObject; // 実際に操作するボールオブジェクト
    private Rigidbody rb; // ボールのリジッドボディ

    private float startTime; // ドラッグ開始時間
    private float endTime; // ドラッグ終了時間
    private float swipeDistance; // スワイプ距離
    private float swipeTime; // スワイプ時間
    private Vector2 startPos; // スワイプ開始位置
    private Vector2 endPos; // スワイプ終了位置

    private float ballVelocity; // 計算された速度
    private float ballSpeed; // 投球速度
    private Vector3 angle; // 投球方向

    private bool thrown; // 既に投げ終えたか
    private bool holding; // ボールを掴んでいるか
    private bool attemptActive; // AttemptManager から投球許可が出ているか

    private Vector3 newPosition; // 掴んでいる間の補間先座標

    private void Awake()
    {
        if (attemptManager == null)
        {
            attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
        }

        SetupBall();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
        CancelInvoke(nameof(HandleThrowTimeout));
    }

    private void SubscribeEvents()
    {
        if (attemptManager == null)
        {
            return;
        }

        attemptManager.OnAttemptStarted += HandleAttemptStarted;
        attemptManager.OnAttemptResolved += HandleAttemptResolved;
        attemptManager.OnSessionEnded += HandleSessionEnded;
    }

    private void UnsubscribeEvents()
    {
        if (attemptManager == null)
        {
            return;
        }

        attemptManager.OnAttemptStarted -= HandleAttemptStarted;
        attemptManager.OnAttemptResolved -= HandleAttemptResolved;
        attemptManager.OnSessionEnded -= HandleSessionEnded;
    }

    private void SetupBall()
    {
        if (ballComponent == null)
        {
            ballComponent = FindFirstObjectByType<Ball>(FindObjectsInactive.Include);
        }

        if (ballComponent == null)
        {
            Debug.LogError("BallThrower に Ball コンポーネントが設定されていません。");
            enabled = false;
            return;
        }

        ballObject = ballComponent.gameObject;
        rb = ballObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("BallThrower: 球オブジェクトに Rigidbody が見つかりません。");
            enabled = false;
            return;
        }

        ResetBall();
    }

    private void HandleAttemptStarted(AttemptInfo info)
    {
        attemptActive = true;
        ResetBall();
    }

    private void HandleAttemptResolved(AttemptResult result)
    {
        attemptActive = false;
        ballComponent.ChangeScale();
        CancelInvoke(nameof(HandleThrowTimeout));
    }

    private void HandleSessionEnded(SessionResult result)
    {
        attemptActive = false;
        CancelInvoke(nameof(HandleThrowTimeout));
    }

    /// <summary>ボールを初期状態に戻します。</summary>
    public void ResetBall()
    {
        CancelInvoke(nameof(HandleThrowTimeout));

        angle = Vector3.zero;
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        ballSpeed = 0f;
        startTime = 0f;
        endTime = 0f;
        swipeDistance = 0f;
        swipeTime = 0f;
        thrown = false;
        holding = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
        }

        if (ballObject != null)
        {
            ballObject.transform.position = transform.position;
            var collider = ballObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }
        }
    }

    private void Update()
    {
        if (!attemptActive)
        {
            return;
        }

        if (ballObject == null || rb == null)
        {
            return;
        }

        if (holding)
        {
            PickupBall();
        }

        if (thrown)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryStartHoldingBall();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            TryThrowBall();
        }
    }

    private void TryStartHoldingBall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform == ballObject.transform)
            {
                startTime = Time.time;
                startPos = Input.mousePosition;
                holding = true;
            }
        }
    }

    private void TryThrowBall()
    {
        endTime = Time.time;
        endPos = Input.mousePosition;
        swipeDistance = (endPos - startPos).magnitude;
        swipeTime = endTime - startTime;

        if (swipeTime < 1f && swipeDistance > minSwipeDistance)
        {
            CalculateSpeed();
            CalculateAngle();
            ApplyThrowForce();
            holding = false;
            thrown = true;
            ScheduleAutoResolve();
        }
        else
        {
            ResetBall();
        }
    }

    private void PickupBall()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane * 10f;
        newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        ballObject.transform.localPosition = Vector3.Lerp(ballObject.transform.localPosition, newPosition, 80f * Time.deltaTime);
    }

    private void CalculateAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50f, Camera.main.nearClipPlane + 5f));
    }

    private void CalculateSpeed()
    {
        if (swipeTime > 0f)
        {
            ballVelocity = swipeDistance / swipeTime;
        }

        ballSpeed = ballVelocity * 0.5f;
        if (ballSpeed >= maxBallSpeed)
        {
            ballSpeed = maxBallSpeed;
        }
    }

    private void ApplyThrowForce()
    {
        if (rb == null)
        {
            return;
        }

        Vector3 force = new Vector3(angle.x * ballSpeed * 1.2f, angle.y * ballSpeed * 0.5f, angle.z * ballSpeed * -0.15f);
        rb.AddForce(force);
        rb.useGravity = true;
    }

    private void ScheduleAutoResolve()
    {
        if (autoResolveDelay <= 0f)
        {
            return;
        }

        CancelInvoke(nameof(HandleThrowTimeout));
        Invoke(nameof(HandleThrowTimeout), autoResolveDelay);
    }

    private void HandleThrowTimeout()
    {
        ResetBall();
        if (attemptManager != null)
        {
            attemptManager.ResolveAttempt(AttemptResolutionCause.Timeout);
        }
    }
}
