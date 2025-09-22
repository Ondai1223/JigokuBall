using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] Ball _ball;
    private GameObject Ball;

    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos;
    private Vector2 endPos;

    public float MinSwipDist = 0;
    private float BallVelocity = 0;
    private float BallSpeed = 0;
    public float MaxBallSpeed = 350;
    private Vector3 angle;

    private bool thrown, holding;
    private Vector3 newPosition;
    Rigidbody rb;

    void Start()
    {
        SetupBall();
    }

    void SetupBall()
    {
        GameObject _ball = GameObject.FindGameObjectWithTag("Player");
        Ball = _ball;
        rb = Ball.GetComponent<Rigidbody>();
        ResetBall();
    }

    private void ResetBall()
    {
        angle = Vector3.zero;
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        BallSpeed = 0;
        startTime = 0;
        endTime = 0;
        swipeDistance = 0;
        swipeTime = 0;
        thrown = holding = false;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        Ball.transform.position = transform.position;
        Ball.GetComponent<Collider>().isTrigger = false;
        _ball.Reset(); // Reset the ball's position and state
    }

    void PickupBall()
    {
        Vector3 mousePos = Input.mousePosition;
        //数値を変えるとボールを持った時の奥行きが変わる
        mousePos.z = Camera.main.nearClipPlane * 10f; 
        newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Ball.transform.localPosition = Vector3.Lerp(Ball.transform.localPosition, newPosition, 80f * Time.deltaTime);
    }

    private void Update()
    {
        if (holding) PickupBall();
        if (thrown) return;


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(ray, out _hit, 100f))
            {
                if (_hit.transform == Ball.transform)
                {
                    startTime = Time.time;
                    startPos = Input.mousePosition;
                    holding = true;
                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            swipeTime = endTime - startTime;

            if (swipeTime < 1f && swipeDistance > 20f)
            {
                //ボールを投げる
                CalcSpeed();
                CalcAngle();
                //飛び方を変更（要調節）
                rb.AddForce(new Vector3((angle.x * BallSpeed), (angle.y * BallSpeed / 3), (angle.z * BallSpeed) * -0.5f));
                rb.useGravity = true;
                holding = false;
                thrown = true;
                Debug.Log(BallSpeed);
                Invoke("ResetBall", 5f);
            }
            else ResetBall();
        }
    }

    private void CalcAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50f, (Camera.main.nearClipPlane + 5)));
    }

    //要調節
    private void CalcSpeed()
    {

        if (swipeTime > 0)
            BallVelocity = swipeDistance / swipeTime;


        BallSpeed = BallVelocity * 0.5f;
        if (BallSpeed >= MaxBallSpeed)
        {
            BallSpeed = MaxBallSpeed;
        }

    }
}

