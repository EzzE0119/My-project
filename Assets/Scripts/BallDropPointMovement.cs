using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropPointMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position = new Vector3(transform.position.x+horizontal * Time.deltaTime, transform.position.y, transform.position.z + vertical * Time.deltaTime);
    }
}
