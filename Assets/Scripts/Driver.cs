using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class Driver : Agent
{
    [SerializeField]float currentSpeed;
    [SerializeField]float steerSpeed = 200f;
    [SerializeField]float aditionalBoosterSpeed = 8f;
    [SerializeField]float usualSpeed = 12f;
    [SerializeField]TMP_Text boosterText;
    [SerializeField]EnviromentController enviromentController;
    [SerializeField]bool useClosestTargetObservation = true;

    private float stepsWithoutPack;
    bool isPackage = false;
    public override void OnEpisodeBegin() {
        currentSpeed = usualSpeed;
        stepsWithoutPack = 0;
       
        enviromentController.ResetEnvironment();

        boosterText.gameObject.SetActive(false);    

        GetComponent<ParticleSystem>().Stop();
        isPackage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        float reward = 10f;
        if(collision.CompareTag("Booster")){
            currentSpeed = aditionalBoosterSpeed + usualSpeed;
            collision.gameObject.SetActive(false);

            boosterText.gameObject.SetActive(true);
            AddReward(reward / 10f);
        }

        if (collision.CompareTag("Package"))
        {
            if(!isPackage){
                isPackage = true;
                GetComponent<ParticleSystem>().Play();
                collision.gameObject.SetActive(false);

                stepsWithoutPack = 0;

                AddReward(reward / 2f);
                Debug.Log("picked it");
            }
        }

        if(collision.CompareTag("Costumer"))
        {
            if(isPackage){
                GetComponent<ParticleSystem>().Stop();
                isPackage = false;
                collision.gameObject.SetActive(false);

                stepsWithoutPack = 0;

                AddReward(reward);
                Debug.Log("Delivered");
            } 
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        float reward = -0.1f / 5f;
        if(collision.collider.CompareTag("WorldCollision")){
            currentSpeed = usualSpeed;
            boosterText.gameObject.SetActive(false);

            AddReward(reward);
        }
        if(collision.collider.CompareTag("Tree")){
            AddReward(reward);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isPackage);
        sensor.AddObservation(currentSpeed / (aditionalBoosterSpeed + usualSpeed));
        sensor.AddObservation(transform.eulerAngles.z / 360f);  
        if (useClosestTargetObservation) // remember to change 6 space size to 4 space size in behavior parameters
        {
            Vector2 closestTarget = GetClosestPackage();

            Vector2 dirToTarget = (closestTarget - (Vector2)transform.position).normalized;
            float distToTarget = Vector2.Distance(transform.position, closestTarget);

            sensor.AddObservation(dirToTarget);  
            sensor.AddObservation(distToTarget / 108f); 
        } 
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        int actMove = actions.DiscreteActions[0];
        int actSteer = actions.DiscreteActions[1];

        float move = 0f;
        float steer = 0f;

        switch (actMove)
        {
            case 1:
                move = 1f;
                
                break;
            case 2:
                move = -1f;
                
                break;
        }

        switch (actSteer)
        {
            case 1:
                steer = -1f;
                
                break;

            case 2:
                steer = 1f;
               
                break;
        }
       


        float moveAmount = move * currentSpeed * Time.deltaTime;
        float steerAmount = steer * steerSpeed * Time.deltaTime;
        transform.Translate(0, moveAmount, 0);
        transform.Rotate(0, 0, steerAmount);

        // float penaltyForWithoutPackage = MathF.Pow(1.01f ,++stepsWithoutPack) - 1f;

        // if (penaltyForWithoutPackage >= maxPenalty) EndEpisode();
        // else AddReward(-penaltyForWithoutPackage);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        float vAxis = Input.GetAxisRaw("Vertical");
        
        if(vAxis < 0) discreteActions[0] = 2;
        else if (vAxis > 0) discreteActions[0] = 1;

        float hAxis = Input.GetAxisRaw("Horizontal");

        if(hAxis < 0) discreteActions[1] = 2;
        else if (hAxis > 0) discreteActions[1] = 1;
        
    }

    private Vector2 GetClosestPackage()
    {
        string searchTag = isPackage ? "Costumer" : "Package";

        GameObject[] targets = GameObject.FindGameObjectsWithTag(searchTag);

        GameObject closest = targets[0];

        float minDistance = Vector2.Distance(transform.localPosition, closest.transform.localPosition);

        for (int i = 1; i < targets.Length; i++)
        {
            float dist = Vector2.Distance(transform.localPosition, targets[i].transform.localPosition);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = targets[i];
            }
        }

        return closest.transform.localPosition;
    }
}
