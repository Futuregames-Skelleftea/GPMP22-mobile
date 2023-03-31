using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;             //prefab for the balls
    [SerializeField] Rigidbody2D pivotRigidbody;        //rigidbody 
    [SerializeField] float detachDistance;  //distance to the pivot at which the ball detaches
    [SerializeField] float maxPullDistance;  //distance to the pivot at which the ball detaches
    [SerializeField] float respawnDelay;    //delay before the new ball spawns in
    [SerializeField] float despawnVelocity = 0.2f; //the speed at which the ball will despawn
    [SerializeField] float ballMaxLifetime = 5; //time after which the ball will automatically despawn
    [SerializeField] float despawnDelayAfterStopped;    //how long it takes for the ball to despawn after it has stopped meaningfully moving 

    private Vector2 touchWorldPos;
    private Vector2 touchStartPos;
    private float timeAtShot;   //the time at which the ball was released
    private Vector2 pullVector;  //vector from the start and current position of the current touch
    private bool isDragging;    //if the ball is being dragged back
    private bool canDrag;    //if the ball is being dragged back
    private Camera mainCamera;
    private GameObject currentBall;   //the rigidbody of the current ball
    private Rigidbody2D currentBallRigidbody;   //the rigidbody of the current ball
    private SpringJoint2D currentSpringJoint;  //the spring joint for the current ball

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(RespawnBall(0)); //instantly spawns a ball
    }

    private void Update()
    {
        GetTouchPosition(); //function for handling all things related to dragging the ball and releasing it
    }

    void GetTouchPosition()
    {
        if (currentBall == null || !canDrag) { return; }    //if you cant drag or theyre exist no ball

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());   //gets the world position of the 

            if (!isDragging)
            {
                touchStartPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());   //the start pos is only set once at the start of the dragging action
            }

            pullVector = Vector2.ClampMagnitude(touchWorldPos - touchStartPos, maxPullDistance); //vector from the start position of the touch to the current position

            currentBall.transform.position = pivotRigidbody.transform.position + new Vector3(pullVector.x, pullVector.y,0);
            isDragging = true;
        }
        else if(isDragging) //if its not being pressed but just was pressed then launch the ball
        {
            currentBallRigidbody.isKinematic = false;   //unfreezes the ball
            isDragging = false;
            canDrag = false;    //can drag variable is set to true when the new spawned ball has finished spawning in
            StartCoroutine(WaitToReleaseBall());    //coroutine that waits for the ball to pass the center to release it
        }
    }

    //coroutine that waits for the ball to pass a certain threshold to release it
    IEnumerator WaitToReleaseBall()
    {
        //the x distance from the ball to the pivot at the start, used for determening if the ball starts to the left or right of the pivot
        float startDistanceToPivotX = currentBallRigidbody.transform.position.x - pivotRigidbody.transform.position.x;   
        float distanceToPivotX = currentBallRigidbody.transform.position.x - pivotRigidbody.transform.position.x;   //the x distance from the ball to the pivot
        while (currentSpringJoint)
        {
            if(startDistanceToPivotX <= 0 && distanceToPivotX >= 0)
            {
                //below code releases the ball
                currentBall.transform.Find("Trail").gameObject.SetActive(true); //enables the trail renderer, starts disables so there is no trail when dragging around the ball
                Destroy(currentSpringJoint);
                timeAtShot = Time.time;
                StartCoroutine(WaitToDespawnBall());
            }
            else if (startDistanceToPivotX > 0 && distanceToPivotX < 0)
            {
                //below code releases the ball
                currentBall.transform.Find("Trail").gameObject.SetActive(true); //enables the trail renderer, starts disables so there is no trail when dragging around the ball
                Destroy(currentSpringJoint);
                timeAtShot = Time.time;
                StartCoroutine(WaitToDespawnBall());
            }

            distanceToPivotX = currentBallRigidbody.transform.position.x - pivotRigidbody.transform.position.x; //the x distance from the ball to the pivot
            yield return new WaitForFixedUpdate();
        }
    }

    //function that waits for the ball to stop moving and then despawns it
    IEnumerator WaitToDespawnBall()
    {
        bool hasDespawned = false;
        //first waits for the ball to barely be stopped or barely moving
        while (currentBallRigidbody.velocity.magnitude > despawnVelocity && !hasDespawned)
        {
            Debug.Log("waiting");
            if(Time.time > timeAtShot + ballMaxLifetime)   //checks if the ball has existed longer than its maximum lifetime
            {
                //shrinks the ball over time
                currentBall.transform.Find("Trail").gameObject.SetActive(false); //deactivates the trail as it is no longer needed and its width property is unaffected by scale which makes things look bad if its enabled
                StartCoroutine(ChangeBallScale(0, 1.5f));
                yield return new WaitForSeconds(0.75f);
                Destroy(currentBall);
                StartCoroutine(RespawnBall(respawnDelay));  //if it still isnt moving significantly faster then despawn it
                hasDespawned = true;    //used for stopping the despawn coroutine
                break;  //breaks out of the while loop
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(despawnDelayAfterStopped);  //waits a bit more
        if(currentBallRigidbody.velocity.magnitude < despawnVelocity * 0.9f && !hasDespawned)
        {
            //shrinks the ball over time
            currentBall.transform.Find("Trail").gameObject.SetActive(false); //deactivates the trail as it is no longer needed and its width property is unaffected by scale which makes things look bad if its enabled
            StartCoroutine(ChangeBallScale(0, 1.5f));
            yield return new WaitForSeconds(0.75f);
            Destroy(currentBall);
            StartCoroutine(RespawnBall(respawnDelay));  //if it still isnt moving significantly faster then despawn it
            hasDespawned = true;    //used for stopping the despawn coroutine
        }
        else if(!hasDespawned)
        {
            Debug.Log("getting called");
            StartCoroutine(WaitToDespawnBall());    //if it has started moving again (fallen off an edge for example) then restart the wait function
        }
    }

    //lerps the current ball's scale
    IEnumerator ChangeBallScale(float changeGoal, float changeSpeed)
    {
        Vector3 startingScale = currentBall.transform.localScale;   //the starting scale
        Vector3 endScale = Vector3.one * changeGoal;   //the scale that will be reached

        for (float i = 0; i < 1; i += Time.deltaTime * changeSpeed)
        {
            currentBall.transform.localScale = Vector3.Lerp(startingScale, endScale, i);
            yield return 0;
        }
    }

    //spawns a new ball after a delay
    IEnumerator RespawnBall(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentBall = Instantiate(ballPrefab, pivotRigidbody.transform.position, Quaternion.identity);

        //sets a reference to the spring joing and sets it connected rigidbody
        currentSpringJoint = currentBall.GetComponent<SpringJoint2D>();
        currentSpringJoint.connectedBody = pivotRigidbody;

        //sets a reference to the rigidbody and freezes it
        currentBallRigidbody = currentBall.GetComponent<Rigidbody2D>();
        currentBallRigidbody.isKinematic = true;

        //sets the balls scale to zero to then scale it up over time
        currentBall.transform.localScale = Vector3.zero;
        StartCoroutine(ChangeBallScale(0.7f, 2));

        yield return new WaitForSeconds(0.5f); //set canDrag to true after a delay to wait for the ball to increase in size
        canDrag = true;
    }
}
