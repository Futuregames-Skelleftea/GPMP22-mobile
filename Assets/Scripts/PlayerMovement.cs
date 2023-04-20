using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    //touch id of the movement and aiming inputs, used to remember which is which
    private int movementTouchId;
    private int aimingTouchId;

    //joystick variables
    [SerializeField] GameObject movementStickRenderer;  //the actual joystick
    [SerializeField] float stickRendererWidth;  //how wide the joystick should be
    [SerializeField] GameObject aimingStickRenderer;    //the actual joystick
    private Vector2 movingStartPos;
    private bool isMoving;
    private Vector2 aimingStartPos;
    private bool isAiming;

    //laser shooting variables
    [SerializeField] float rotationLerpValue = 0.5f;    //how fast the player rotates to the direction the joystick is telling it to
    [SerializeField] float laserAimingLerpValue = 0.1f; //how fast the laser aims where youre telling it to
    [SerializeField] float laserShootForce = 30;    //how much force each laser shoots out with
    [SerializeField] float laserLifetime = 10;  //how long the laser lives

    private Camera mainCamera;

    //movement variables
    [SerializeField] float movementForce = 1;
    [SerializeField] float movementInputLenghtClamp = 3;
    [SerializeField] float aimingInputLenghtClamp = 500;
    [SerializeField] float laserCooldown = 0.25f;

    //references to things on the player
    public GameObject laserAnchor; //parent of the actual laser gun thing that rotates, needs to be accesed by the health script to diable it which is why it is public
    [SerializeField] GameObject laserGunHead;   //the thing that shoots the laser
    [SerializeField] GameObject laserBulletPrefab;
    [SerializeField] Rigidbody playerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LaserShootingCoroutine());   //start shooting
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        laserAnchor.transform.position   = transform.position;   //set the laser anchors position to that of the players
        RenderJoystickIndicators();
        KeepPlayerOnScreen();
    }

    //function which renders a line between the start position of the input the end position, this functions sort of as a joystick without having it be there before inputing
    //this rendering happnes on an overlay canvas and i couldnt quite get line renderers to work with that and as such am adjusting scales instead
    private void RenderJoystickIndicators()
    {
        if (isMoving)   //if movement input
        {
            //rotating the joystick indticator 
            Vector2 currentMovePosition = Touchscreen.current.touches[movementTouchId].position.ReadValue();
            Vector2 currentMovementVector = currentMovePosition - movingStartPos;   //gets a vector between the start end current position of the input
            currentMovementVector = Vector2.ClampMagnitude(currentMovementVector, movementInputLenghtClamp);    //clamps the joystick to a maximum lenght
            Vector2 joyStickEndPos = movingStartPos + currentMovementVector;

            //places the anchor between the two movement stick points and moves it back a bit so it doesnt interfere with the input, second bit might not be necessary but doesnt hurt
            Transform anchor = movementStickRenderer.transform.parent.transform;
            anchor.position = Vector3.Lerp(movingStartPos, joyStickEndPos, 0.5f);
            if (currentMovementVector != Vector2.zero)
            {
                anchor.rotation = Quaternion.LookRotation(currentMovementVector, Vector3.forward);
            }

            //scales the joystick renderer
            //1 is minimum width
            movementStickRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(currentMovementVector.magnitude / 2, stickRendererWidth + 1); //1 is minimum width
        }

        if (isAiming)   //if aiming input
        {
            //rotating the joystick indticator 
            Vector2 currentAimingPosition = Touchscreen.current.touches[aimingTouchId].position.ReadValue();
            Vector2 currentAimingVector = currentAimingPosition - aimingStartPos;   //gets a vector between the start end current position of the input
            currentAimingVector = Vector2.ClampMagnitude(currentAimingVector, aimingInputLenghtClamp);      //clamps the joystick to a maximum lenght
            Vector2 joyStickEndPos = aimingStartPos + currentAimingVector;

            //places the anchor between the two movement stick points and moves it back a bit so it doesnt interfere with the input, second bit might not be necessary but doesnt hurt
            Transform anchor = aimingStickRenderer.transform.parent.transform;
            anchor.position = Vector3.Lerp(aimingStartPos, joyStickEndPos, 0.5f);
            if (currentAimingVector != Vector2.zero)
            {
                Quaternion laserAnchorTargetRotation = Quaternion.LookRotation(currentAimingVector, Vector3.forward);   //target rotation that will be lerped to
                laserAnchor.transform.rotation = Quaternion.Lerp(laserAnchor.transform.rotation, laserAnchorTargetRotation,laserAimingLerpValue);   //rotates the laser aiming thing aswell
                anchor.rotation = Quaternion.LookRotation(currentAimingVector, Vector3.forward);    //rotates the indicator so it lines up with where the input finger is 
            }

            //scales the joystick renderer
            //1 is minimum width
            aimingStickRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(currentAimingVector.magnitude / 2, stickRendererWidth + 1); 
        }
    }

    //function whic keeps the player on the screen
    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(transform.position);

        //horizontal
        if(viewPortPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 5f;
        }else if(viewPortPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 5f;
        }

        //vertical
        if (viewPortPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 5;
        }
        else if (viewPortPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 5f;
        }

        transform.position = newPosition;
    }

    //coroutine that continously shoots lasers
    public IEnumerator LaserShootingCoroutine()
    {
        while (laserCooldown != 0)
        {
            GameObject tempBullet = Instantiate(laserBulletPrefab, laserGunHead.transform.position, laserGunHead.transform.rotation * Quaternion.Euler(90,0,0));    //spawns a laser
            tempBullet.GetComponent<Rigidbody>().AddForce(-laserGunHead.transform.forward * laserShootForce, ForceMode.VelocityChange); //adds a velocity to the laser
            Destroy(tempBullet, laserLifetime); //destroy the laser after its set lifetime 
            yield return new WaitForSeconds(laserCooldown); //waits a bit before spawning the next laser
        }
    }

    //Function called when clicking down on the right side of the screen ie the movement input section
    public void StartMovement()
    {
        if (!gameObject.activeSelf) { return; } //was having a problem where you could get the previous joystick indicator to show when the player was disabled

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startmovment function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                movementTouchId = i;
                isMoving = true;
                movingStartPos = Touchscreen.current.touches[movementTouchId].startPosition.ReadValue();
                movementStickRenderer.gameObject.SetActive(true);
            }
        }
    }

    //function called when a finger leaves the right side of the screen
    public void StopMovement()
    {
        isMoving = false;
        movementTouchId = 1000;
        movementStickRenderer.SetActive(false);
    }

    //Function called when clicking down on the left side of the screen ie the aiming input section
    public void StartAiming()
    {
        if (!gameObject.activeSelf) { return; } //was having a problem where you could get the previous joystick indicator to show when the player was disabled

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startaiming function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                aimingTouchId = i;
                isAiming = true;
                aimingStartPos = Touchscreen.current.touches[aimingTouchId].startPosition.ReadValue();
                aimingStickRenderer.SetActive(true);
            }
        }
    }

    //function called when a finger leaves the left side of the screen
    public void StopAiming()
    {
        isAiming = false;
        aimingTouchId = 1000;
        aimingStickRenderer.SetActive(false);
    }

    private void FixedUpdate()
    {
        MovementPhysics();
    }

    private void MovementPhysics()
    {
        if(!isMoving) { return; }   //if no movement input is being recieved then do nothing

        //calculates a vector from the start and current position of the input
        Vector2 currentMovePosition = Touchscreen.current.touches[movementTouchId].position.ReadValue();
        Vector2 movementVector =  currentMovePosition - movingStartPos;
        movementVector = Vector2.ClampMagnitude(movementVector, movementInputLenghtClamp);

        //adds a force to the player in the direction of the input
        playerRigidbody.AddForce(movementVector * movementForce);

        //gradually rotates the player towards the direction of the joystick
        if(movementVector != Vector2.zero) 
        { 
            Quaternion targetRotation = Quaternion.LookRotation(movementVector, Vector3.back) * Quaternion.Euler(90, 0, 0); //the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,rotationLerpValue);
        }   
    }
}
