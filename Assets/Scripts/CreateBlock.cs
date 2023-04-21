using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class CreateBlock : MonoBehaviour
{
    [SerializeField] int currentNumOfBlocks;
    [SerializeField] int resetNumOfBlocks;
    [SerializeField] TextMeshProUGUI numText;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject currentBlock;
    [SerializeField] GameObject placeButton;

    //child objects of the block
    private Transform currentBlockMiddle; //the middle section of the block that will be scaled
    private Transform currentBlockLeftButton;   //the button on one of the end
    private Transform currentBlockRightButton; //the button on one of the end
    private Transform currentBlockMiddleLeft; //left end which the button will be placed over
    private Transform currentBlockMiddleRight; //left end which the button will be placed over

    //input getting
    private int placingTouchId;
    private bool isPlacingBlock;

    private Vector2 touchWorldPos;
    private Vector2 touchWorldStartPos;
    private Camera mainCamera;

    public List<GameObject> spawnedBlocks;

    // Start is called before the first frame update
    void Start()
    {
        numText.text = "PLACE BLOCK (" + currentNumOfBlocks.ToString() + ")";
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacingBlock)
        {
            PlaceBlock();
        }
    }

    public void DestroyAllBlocks()
    {
        for (int i = 0; i < spawnedBlocks.Count; i++)
        {
            Destroy(spawnedBlocks[i]);
        }
        currentNumOfBlocks = resetNumOfBlocks;
    }

    public void StartPlacingBlock()
    {
        if(currentNumOfBlocks < 1)
        {
            return;
        }

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //figures out the id of the touch which caused the startaiming function to get called and saves that id to use as a reference when getting input later
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                placingTouchId = i;

                touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.touches[placingTouchId].position.ReadValue());

                currentBlock = Instantiate(blockPrefab, touchWorldPos, Quaternion.identity);

                spawnedBlocks.Add(currentBlock);

                currentBlockMiddle = currentBlock.transform.Find("Middle");
                currentBlockLeftButton = currentBlock.transform.Find("Left Edge Button");
                currentBlockRightButton = currentBlock.transform.Find("Right Edge Button");
                currentBlockMiddleLeft = currentBlockMiddle.Find("Left End");
                currentBlockMiddleRight = currentBlockMiddle.Find("Right End");

                isPlacingBlock = true;
                currentNumOfBlocks--;
                numText.text = "Blocks: " + currentNumOfBlocks.ToString();
            }
        }
    }

    public void StopPlacingBlock()
    {

    }

    private void PlaceBlock()
    {
        if(Touchscreen.current.touches[placingTouchId].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            placingTouchId = 999; // some value it will never be
            isPlacingBlock = false;
            return;
        }
        touchWorldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.touches[placingTouchId].position.ReadValue());

        currentBlock.transform.position = touchWorldPos;
    }


    private void AdjustBlock()
    {

        if (isPlacingBlock)
        {
            float xScale;
            currentBlock.transform.position = Vector3.Lerp(touchWorldPos, touchWorldStartPos, 0.5f);
            currentBlock.transform.rotation = Quaternion.LookRotation(touchWorldPos - touchWorldStartPos) * Quaternion.Euler(0, 90, 0);
            xScale = (touchWorldStartPos - touchWorldPos).magnitude;

            //scaling sutff
            currentBlockMiddle.localScale = new Vector3(xScale, currentBlockMiddle.localScale.y, currentBlockMiddle.localScale.z); //Changes the Y-scale
            currentBlockLeftButton.position = currentBlockMiddleLeft.position;
            currentBlockRightButton.position = currentBlockMiddleRight.position;
        }
    }
}
