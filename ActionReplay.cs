using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionReplay : MonoBehaviour
{
    public List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();
    public bool isInReplayMode;
    public float currentReplayIndex;
    public CheckCol checkCol;
    public Rigidbody rb;
    private Transform ballTransform;
    public TrailRenderer trail;
    public bool replayButtonOn = false;
    public GameObject replayButton;
    public float indexChangeRate;
    public bool slowReplay = false;
    public const string ball = "Ball";

   
    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount != 0) //if object has a trail then get component
        {
            trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        }
       
            rb = GetComponent<Rigidbody>();

        if (gameObject.name == ball)
        {
            checkCol = GetComponent<CheckCol>();

            ballTransform = GetComponent<Transform>();

            transform.position = ballTransform.position;  
        }

        indexChangeRate = 1;

            
    }

    // Update is called once per frame
    void Update()
    {

        if ((isInReplayMode) && (!replayButtonOn)) //if in replay mode show replay button
        {
            replayButton.SetActive(true);
            replayButtonOn = true;
        }

        else if ((!isInReplayMode) && (replayButtonOn)) //if not in replay mode hide replay button
        {
            replayButton.SetActive(false);
            replayButtonOn = false;
        }
        
        ReplayHandler();
    }
        


    private void FixedUpdate()
    {

        if ((isInReplayMode == false) && (checkCol.kicked == true) )
        {
            actionReplayRecords.Add(new ActionReplayRecord { position = transform.position, rotation = transform.rotation });

        }

        else if (isInReplayMode == true)
        {
       
            float nextIndex = currentReplayIndex + indexChangeRate; //set next replay frame to the current one + 1 (change rate)


            if (nextIndex < actionReplayRecords.Count)
            {
                SetTransform(nextIndex); //set ball position to the next replay index for as long as there is still another index in the array
                
            }

            else if (gameObject.name == ball)
            {
                rb.isKinematic = false;   
            }


            if ((nextIndex > 1) && (gameObject.name == ball))
            {
                trail.emitting = true;
            }
        }
    }


    private void SetTransform(float index)
    {
        currentReplayIndex = index;
        ActionReplayRecord actionReplayRecord = actionReplayRecords[(int)index]; //create an array of ball position indexes to show as a replay if player completes level

        transform.position = actionReplayRecord.position; //record current ball position to replay record
        transform.rotation = actionReplayRecord.rotation;
    }


    public void ReplayHandler()
    {
        if (checkCol.showReplay == true)
        {
            checkCol.stopShowReplay = true;

            isInReplayMode = !isInReplayMode; //toggle replay mode

            if (isInReplayMode == true)
            {

                SetTransform(0); //set ball position to start
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                rb.isKinematic = true;

            }

            else if (gameObject.name == ball)
            {
                rb.isKinematic = false;
            }
        }


        if (checkCol.resetReplay == true) //if player restarts level clear saved replay
        {
            isInReplayMode = false;
            checkCol.showReplay = false;

            actionReplayRecords.Clear();
            currentReplayIndex = 0;

            checkCol.stopShowReplay = false;
        }
    }

}



