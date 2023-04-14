using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private int movementTouchId;
    private int aimingTouchId;

    [SerializeField] GameObject movementStickRenderer;
    [SerializeField] float stickRendererWidth;
    [SerializeField] GameObject aimingStickRenderer;

    [SerializeField] float rotationLerpValue = 0.5f;
    [SerializeField] float laserAimingLerpValue = 0.1f;
    [SerializeField] float laserShootForce = 30;
    [SerializeField] float laserLifetime = 10;

    private Vector2 movingStartPos;
    private bool isMoving;
    private Vector2 aimingStartPos;
    private bool isAiming;

    private Camera mainCamera;

    [SerializeField] float movementForce = 1;
    [SerializeField] float movementInputLenghtClamp = 3;
    [SerializeField] float aimingInputLenghtClamp = 500;
    [SerializeField] float laserCooldown = 0.25f;

    //references to things on the player
    public GameObject laserAnchor; //needs to be accesed by the health script to diable it
    [SerializeField] GameObject laserGunHead;
    [SerializeField] GameObject laserBulletPrefab;
    [SerializeField] Rigidbody playerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LaserShootingCoroutine());
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        laserAnchor.transform.position   = transform.position;   //set the laser anchors position to that of the players
        RenderJoystickIndicators();
        KeepPlayerOnScreen();
    }

    private void RenderJoystickIndicators()
    {
        if (isMoving)
        {
            //rotating the joystick indticator 
            Vector2 currentMovePosition = Touchscreen.current.touches[movementTouchId].position.ReadValue();
            Vector2 currentMovementVector = currentMovePosition - movingStartPos;
            currentMovementVector = Vector2.ClampMagnitude(currentMovementVector, movementInputLenghtClamp);
            Vector2 joyStickEndPos = movingStartPos + currentMovementVector;
            //places the anchor between the two movement stick points and moves it back a bit so it doesnt interfere with the input
            Transform anchor = movementStickRenderer.transform.parent.transform;
            anchor.position = Vector3.Lerp(movingStartPos, joyStickEndPos, 0.5f);
            if (currentMovementVector != Vector2.zero)
            {
                anchor.rotation = Quaternion.LookRotation(currentMovementVector, Vector3.forward);
            }
            movementStickRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(currentMovementVector.magnitude / 2, stickRendererWidth + 1); //1 is minimum width
        }

        if (isAiming)
        {
            //rotating the joystick indticator 
            Vector2 currentAimingPosition = Touchscreen.current.touches[aimingTouchId].position.ReadValue();
            Vector2 currentAimingVector = currentAimingPosition - aimingStartPos;
            currentAimingVector = Vector2.ClampMagnitude(currentAimingVector, aimingInputLenghtClamp);
            Vector2 joyStickEndPos = aimingStartPos + currentAimingVector;
            //places the anchor between the two movement stick points and moves it back a bit so it doesnt interfere with the input
            Transform anchor = aimingStickRenderer.transform.parent.transform;
            anchor.position = Vector3.Lerp(aimingStartPos, joyStickEndPos, 0.5f);
            if (currentAimingVector != Vector2.zero)
            {
                Quaternion laserAnchorTargetRotation = Quaternion.LookRotation(currentAimingVector, Vector3.forward);
                laserAnchor.transform.rotation = Quaternion.Lerp(laserAnchor.transform.rotation, laserAnchorTargetRotation,laserAimingLerpValue);
                anchor.rotation = Quaternion.LookRotation(currentAimingVector, Vector3.forward);    //rotates the indicator so it lines up with where the input finger is 
            }
            aimingStickRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(currentAimingVector.magnitude / 2, stickRendererWidth + 1); //1 is minimum width
        }
    }

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
            GameObject tempBullet = Instantiate(laserBulletPrefab, laserGunHead.transform.position, laserGunHead.transform.rotation * Quaternion.Euler(90,0,0));
            tempBullet.GetComponent<Rigidbody>().AddForce(-laserGunHead.transform.forward * laserShootForce, ForceMode.VelocityChange);
            Destroy(tempBullet, laserLifetime);
            yield return new WaitForSeconds(laserCooldown);
        }
    }

    public void StartMovement()
    {
        if (!gameObject.activeSelf) { return; } //was having a problem where you could get the previous joystick indicator to show when the player was disabled

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {

            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                movementTouchId = i;
                isMoving = true;
                movingStartPos = Touchscreen.current.touches[movementTouchId].startPosition.ReadValue();
                movementStickRenderer.gameObject.SetActive(true);
            }
        }
    }

    public void StopMovement()
    {
        isMoving = false;
        movementTouchId = 1000;
        movementStickRenderer.SetActive(false);
    }

    public void StartAiming()
    {
        if (!gameObject.activeSelf) { return; } //was having a problem where you could get the previous joystick indicator to show when the player was disabled

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                aimingTouchId = i;
                isAiming = true;
                aimingStartPos = Touchscreen.current.touches[aimingTouchId].startPosition.ReadValue();
                aimingStickRenderer.SetActive(true);
            }
        }
    }

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
        if(!isMoving) { return; }

        //movement forces

        Vector2 currentMovePosition = Touchscreen.current.touches[movementTouchId].position.ReadValue();
        Vector2 movementVector =  currentMovePosition - movingStartPos;
        movementVector = Vector2.ClampMagnitude(movementVector, movementInputLenghtClamp);
        playerRigidbody.AddForce(movementVector * movementForce);
        if(movementVector != Vector2.zero) 
        { 
            Quaternion targetRotation = Quaternion.LookRotation(movementVector, Vector3.back) * Quaternion.Euler(90, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,rotationLerpValue);
        }   
    }
}
