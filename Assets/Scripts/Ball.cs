using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform ballDropLocation;
    Rigidbody rb;
    [SerializeField] float ballSpeed = 10f;
    [SerializeField] float swingStrength = 3f;
    float spinForce;
    [SerializeField] bool isSpin;
    [SerializeField] bool isLeftSpin;

    [SerializeField] bool isLeftSwing;
    Vector3 swingDirection;
    Vector3 ballStartingPos;

    float elapsedTime = 0f;
    bool ballThrown = false;

    [SerializeField] Transform OffSidePos;
    [SerializeField] Transform LegSidePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ballStartingPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pitch") )
        {
            ballThrown = false;
            if (isSpin)
            {
                rb.AddForce(Camera.main.transform.right * spinForce);
            }
        }
    }

    public void SetSpinForce(float a_spinForce)
    {
        spinForce = a_spinForce;
        if (isLeftSpin)
        {
            spinForce = spinForce * (-1f);
        }
    }



    public void ThrowBall(float a_speed)
    {
        if (isSpin)
        {
            rb.useGravity = true;
            Vector3 displacement = ballDropLocation.position - transform.position;
            Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);

            float gravity = Physics.gravity.magnitude; // Default is 9.81 m/s²
            float horizontalDistance = horizontalDisplacement.magnitude;
            ballSpeed = 10f;
            // Calculate flight time based on speed and distance
            float flightTime = horizontalDistance / ballSpeed;

            // Vertical velocity needed to reach the target
            float verticalVelocity = (displacement.y / flightTime) + (0.5f * gravity * flightTime);

            // Horizontal velocity direction
            Vector3 horizontalVelocity = horizontalDisplacement.normalized * ballSpeed;

            // Final launch velocity
            Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

            rb.velocity = launchVelocity;
        }
        else
        {
            rb.useGravity = true;
            rb = GetComponent<Rigidbody>();


            Vector3 startPoint = transform.position;
            Vector3 targetPoint = ballDropLocation.position;
            ballSpeed = a_speed; // Desired speed

            Vector3 displacement = targetPoint - startPoint;
            Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);
            Physics.gravity = new Vector3(0, -9.81f, 0);
            float gravity = Physics.gravity.magnitude;
            float horizontalDistance = horizontalDisplacement.magnitude;

            // Compute flight time based on horizontal distance and ball speed
            float flightTime = horizontalDistance / ballSpeed;

            // Compute vertical velocity needed to reach the target
            float verticalVelocity = (displacement.y / flightTime) + (0.5f * gravity * flightTime);

            // Compute horizontal velocity direction and magnitude
            Vector3 horizontalVelocity = horizontalDisplacement.normalized * ballSpeed;

            
            // Compute sideways direction (swinging direction)
            if (isLeftSwing)
            {
                swingDirection = -Vector3.right;
            }
            else 
            {
                swingDirection = Vector3.right;
            }
             // Use Vector3.left for the opposite curve

            // **Calculate compensation for swing force**
            // A more accurate compensation model
            Debug.Log(flightTime);

            Vector3 compensationVelocity;

            compensationVelocity = -swingDirection * (swingStrength * flightTime * 0.5f);


            // **Apply compensated velocity**
            Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity + compensationVelocity;

            //rb.velocity = launchVelocity;
            rb.AddForce(launchVelocity * rb.mass, ForceMode.Impulse);

            // Start applying swing force over time
            //StartCoroutine(ApplySwingForce(rb, flightTime, swingDirection, swingStrength));
            ballThrown = true;
        }
    }
    IEnumerator ApplySwingForce(Rigidbody rb, float flightTime, Vector3 swingDirection, float swingStrength)
    {
        float elapsedTime = 0f;

        while (elapsedTime < flightTime)
        {
            rb.AddForce(swingDirection * swingStrength, ForceMode.Force);
            elapsedTime += Time.fixedDeltaTime;
            yield return null; // Wait for next frame
        }
    }

    void FixedUpdate()
    {
        if (ballThrown)
        {
            rb.AddForce(swingDirection * swingStrength, ForceMode.Force);
            elapsedTime += Time.fixedDeltaTime;
        }
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = ballStartingPos;
        ballThrown = false;
    }

    public void OffSide()
    {
        transform.position = OffSidePos.position;
        ballStartingPos = OffSidePos.position;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        ballThrown = false;
    }
    public void LegSide()
    {
        transform.position = LegSidePos.position;
        ballStartingPos = LegSidePos.position;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        ballThrown = false;
    }
}

