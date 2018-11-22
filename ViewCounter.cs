using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCounter : MonoBehaviour
{
    // Public params
    public int viewCount;
    public int startingViewCount;
    public int viewsFromCollectingNote = 500;
    public int viewsFromNearbyGuardMax = 5;
    public int viewsLostFromGuardDistance = 1;
    public int viewsLostFromBeingStill = 1;
    public float guardDistanceThreshold = 5.0f;


    // Working vars
    int t;
    int vc = 0;
    Vector3 lastPosition;
    float speed = 0;
    

    void Start()
    {
        viewCount = startingViewCount;
        lastPosition = transform.position;
    }

    void Update()
    {


        // Update view counter every second instead of every frame
        t++;
        float fps = Mathf.FloorToInt(1 / Time.deltaTime);
        if ((t % fps) == 0)
        {

            // Get all instances of Guard
            GameObject[] array = GameObject.FindGameObjectsWithTag("Guard");
            
            foreach (GameObject enemy in array)  // Loop over each Guard instance
            {
                float distanceFromGuard = Vector3.Distance(gameObject.transform.position, enemy.transform.position); // Gets the distance between the guard and the player

                
                if (distanceFromGuard < guardDistanceThreshold) // If the enemy is close enough
                {
                    float viewsGained = viewsFromNearbyGuardMax * (guardDistanceThreshold / distanceFromGuard); // Increment the view counter
                    vc += Mathf.RoundToInt(viewsGained);
                }
                else
                {
                    vc -= viewsLostFromGuardDistance;
                }
            }



            //Decrement the view counter if the player is slow
            speed = (transform.position - lastPosition).magnitude; 
            lastPosition = transform.position;
            
            if (speed <= 0.5) {
                vc -= viewsLostFromBeingStill; 
            }



            // Apply change and reset
            viewCount += vc;
            vc = 0;
            t = 0;

            // View count must never be negative
            if (viewCount <= 0)
            {
                viewCount = 0;
            }
            
        }

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Note")
        {
            vc += viewsFromCollectingNote;
            Destroy(other.gameObject);
        }

    }



}
