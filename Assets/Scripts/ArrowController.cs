using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float arrowMax = 350f;
    public float arrowMin = -350f;

    float SpinForceBlue = 75f;
    float SpinForceGreen = 45f;
    float SpinForceYellow = 15f;

    float ballSpeedBlue = 13f;
    float ballSpeedGreen = 10f;
    float ballSpeedYellow = 7f;

    Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        MoveUp();
    }

    private void MoveUp()
    {
        transform.DOLocalMoveY(arrowMax, 2).OnComplete(() => { MoveDown(); });
    }
    private void MoveDown()
    {
        transform.DOLocalMoveY(arrowMin, 2).OnComplete(() => { MoveUp(); });
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.localPosition.y > 250f || transform.localPosition.y < -250f)
            {
                Debug.Log("NoBall");
            }
            if((transform.localPosition.y > 150f && transform.localPosition.y < 250f) || (transform.localPosition.y < -150f && transform.localPosition.y > -250f))
            {
                //Debug.Log("Yellow");
                ball.SetSpinForce(SpinForceYellow);
                ball.ThrowBall(ballSpeedYellow);
            }
            if ((transform.localPosition.y > 50f && transform.localPosition.y < 150) || (transform.localPosition.y < -50f && transform.localPosition.y > -150))
            {
                //Debug.Log("Green");
                ball.SetSpinForce(SpinForceGreen);
                ball.ThrowBall(ballSpeedGreen);
            }
            if (transform.localPosition.y < 50f && transform.localPosition.y > -50f)
            {
                //Debug.Log("Blue");
                ball.SetSpinForce(SpinForceBlue);
                ball.ThrowBall(ballSpeedBlue);
            }
        } 

    }
}
