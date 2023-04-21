using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockController : MonoBehaviour
{
    //IDs and bools
    private bool isAdjustingLeft;
    private bool isAdjustingRight;
    private bool isMovingAll;
    private int leftAdjustingTouchId;
    private int rightAdjustingTouchId;
    private int movingTouchId;

    [SerializeField] Vector2 touchWorldPos;
    private Camera mainCamera;

    [SerializeField] GameObject middleBlock;
    [SerializeField] Transform middleLeftEnd;
    [SerializeField] Transform middleRightEnd;
    [SerializeField] GameObject leftButton;   //the button on one of the end
    [SerializeField] GameObject rightButton; //the button on one of the end


    [SerializeField] float maxBlockLength;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isAdjustingLeft)
        {
            AdjustBlockLeft();
        }

        if (isAdjustingRight)
        {
            AdjustBlockRight();   //right is mega bugged and i dont know why
        }

        if (isMovingAll)
        {
            MoveBlock();  //not really needed 
        }
    }

    public void StartAdjustingLeft()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startaiming function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                leftAdjustingTouchId = i;
                isAdjustingLeft = true;
            }
        }
    }

    public void StartAdjustingRight()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startaiming function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                rightAdjustingTouchId = i;
                isAdjustingRight = true;
            }
        }
    }
    public void StartAdjustMoving()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startaiming function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                movingTouchId = i;
                isMovingAll = true;
            }
        }
    }

    private void AdjustBlockLeft()
    {
        if (Touchscreen.current.touches[leftAdjustingTouchId].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            leftAdjustingTouchId = 999; // some value it will never be
            isAdjustingLeft = false;
            touchWorldPos = Vector2.zero;
            return;
        }

        touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.touches[leftAdjustingTouchId].position.ReadValue());

        Vector2 rightEndPos = new Vector2(middleRightEnd.position.x, middleRightEnd.position.y);
        Vector2 vectorBetweenEnds = touchWorldPos - rightEndPos;

        //clamps the lenght
        if(vectorBetweenEnds.magnitude > maxBlockLength)
        {
            touchWorldPos = rightEndPos + Vector2.ClampMagnitude(vectorBetweenEnds,maxBlockLength);
        }

        transform.position = Vector3.Lerp(touchWorldPos, middleRightEnd.position, 0.5f);
        if ((vectorBetweenEnds) != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(vectorBetweenEnds) * Quaternion.Euler(0, 90, 0);
            transform.rotation = targetRotation;
            //needed for some stupid shit
            if (transform.rotation.y == -180)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            }
        }

        float xScale;
        xScale = Mathf.Abs((rightEndPos - touchWorldPos).magnitude);

        //scaling sutff
        middleBlock.transform.localScale = new Vector3(xScale, middleBlock.transform.localScale.y, middleBlock.transform.localScale.z); //Changes the Y-scale

        //needed for the moving when clicking center thingy because it got like placed behind or something
        //its dumb but it works so its not dumb
        
        if (transform.rotation.y != 0)
        {
            middleBlock.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            middleBlock.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        

        leftButton.transform.position = middleLeftEnd.position;
        leftButton.transform.rotation = Quaternion.Euler(0,0,0);
        rightButton.transform.position = middleRightEnd.position;
        rightButton.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    private void AdjustBlockRight()
    {
        if (Touchscreen.current.touches[rightAdjustingTouchId].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            rightAdjustingTouchId = 999; // some value it will never be
            isAdjustingRight = false;
            touchWorldPos = Vector2.zero;
            return;
        }

        touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.touches[rightAdjustingTouchId].position.ReadValue());

        Vector2 leftEndPos = new Vector2(middleLeftEnd.position.x, middleLeftEnd.position.y);
        Vector2 vectorBetweenEnds = touchWorldPos - leftEndPos;

        //clamps the length
        if (vectorBetweenEnds.magnitude > maxBlockLength)
        {
            touchWorldPos = leftEndPos + Vector2.ClampMagnitude(vectorBetweenEnds, maxBlockLength);
        }

        transform.position = Vector3.Lerp(touchWorldPos, middleLeftEnd.position, 0.5f);
        if ((vectorBetweenEnds) != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(vectorBetweenEnds) * Quaternion.Euler(0, -90, 0);
            transform.rotation = targetRotation;
            //needed for some stupid shit
            if(transform.rotation.y == -180)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            }
        }
        
        float xScale;
        xScale = Mathf.Abs((leftEndPos - touchWorldPos).magnitude);

        //scaling stuff
        middleBlock.transform.localScale = new Vector3(xScale, middleBlock.transform.localScale.y, middleBlock.transform.localScale.z); //Changes the Y-scale

        //needed for the moving when clicking center thingy because it got like placed behind or something
        //its dumb but it works so its not dumb
        if (transform.rotation.y != 0)
        {
            middleBlock.transform.localRotation = Quaternion.Euler(180,0,0);
        }
        else
        {
            middleBlock.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        rightButton.transform.position = middleRightEnd.position;
        rightButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        leftButton.transform.position = middleLeftEnd.position;
        leftButton.transform.rotation = Quaternion.Euler(0, 0, 0);

        
    }

    private void MoveBlock()   //temporary function for the right adjust point cus its super bugged
    {
        if (Touchscreen.current.touches[movingTouchId].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            movingTouchId = 999; // some value it will never be
            isMovingAll = false;
            return;
        }

        touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.touches[movingTouchId].position.ReadValue());

        transform.position = touchWorldPos;
        
    }
}