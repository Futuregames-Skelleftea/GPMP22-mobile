using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 vectorToCameraAnchor;
    public float distanceToCameraAnchor;
    public float yDistanceToPlayer;
    public float maxSpeed;
    public float moveSpeed;
    public Vector3 offset;

    private float maxDistanceVec = 4;

    [SerializeField] float minDistance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        /*
        transform.localPosition = offset;

        anchor.transform.position = player.transform.position;

        

        anchor.transform.rotation = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity, Vector3.up);
        */
        moveTowardsPlayer();

    }

    void moveTowardsPlayer()
    {
        float underSpeed = 1;

        //gets the distance to the desired position
        vectorToCameraAnchor = (player.transform.position + offset - gameObject.transform.position);
        distanceToCameraAnchor = Mathf.Sqrt(vectorToCameraAnchor.x * vectorToCameraAnchor.x + vectorToCameraAnchor.y * vectorToCameraAnchor.y + vectorToCameraAnchor.z * vectorToCameraAnchor.z);

        //Adds extra speed if the camera is under the ball
        yDistanceToPlayer = (player.transform.position + offset - gameObject.transform.position).y;
        if (yDistanceToPlayer > 0.4f)
        {
            underSpeed += Mathf.Sqrt(yDistanceToPlayer * yDistanceToPlayer);
        }

        //moves the camera with speed increasing with distance and if the camera is under the ball 
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * moveSpeed * (distanceToCameraAnchor - minDistance) * underSpeed);

        transform.LookAt(player.transform.position);
        /*
        if(distanceToCameraAnchor < minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + vectorToCameraAnchor, Time.deltaTime * moveSpeed * (distanceToCameraAnchor - minDistance) * underSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * moveSpeed * (distanceToCameraAnchor - minDistance) * underSpeed);
        }
        */





        /*
        //keeps the camera from going under the player 
        if( transform.position.y - player.transform.position.y < 3 )
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.x + 2, transform.position.y);
        }
        */

        //transform.LookAt(player.transform.position);


    }
}