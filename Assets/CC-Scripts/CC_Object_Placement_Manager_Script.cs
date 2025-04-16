using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Object_Placement_Manager_Script : MonoBehaviour
{
    // correct order to place beakers
   public List<GameObject> correctOrder; 
    // order player placed beakers
    private List<GameObject> placedObjects = new List<GameObject>();
    
    // reference to mixing vessel
    public GameObject vessel; 

    // temporary eference to Success and Failure JUST FOR TESTING!
    public GameObject successAsset;
    public GameObject failureAsset;
    
    private void Start()
    {
        // initialize the correct order
        correctOrder = new List<GameObject>
        {
            GameObject.FindWithTag("yellowBeaker"),
            GameObject.FindWithTag("redBeaker"),
            GameObject.FindWithTag("orangeBeaker"),
            GameObject.FindWithTag("blueBeaker"),
            GameObject.FindWithTag("greenBeaker")
        };

        // success and failure assets are hidden initially JUST FOR TESTING!
        successAsset.SetActive(false);
        failureAsset.SetActive(false);
    }

    // function called when beaker is placed in the vessel
    public void OnObjectPlaced(GameObject placedObject)
    {
        // Add the player placed beaker to the list
        placedObjects.Add(placedObject); 

        // Disable the beaker interaction once it's placed so player cant interact with them
        placedObject.GetComponent<Collider>().enabled = false;
        placedObject.GetComponent<Rigidbody>().isKinematic = true; 
        placedObject.GetComponent<Renderer>().enabled = false;  // hides the beakers

        // Check if all beakers are placed
        if (placedObjects.Count == correctOrder.Count)
        {
            CheckPlacementOrder();
        }
    }

    private void CheckPlacementOrder()
    {
        // Compare the order of player placed beakers to the correct order
        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (placedObjects[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            // next level
            Debug.Log("Correct order!");
            // Show the success asset
            successAsset.SetActive(true);
        }
        else
        {
            // game over
            Debug.Log("Incorrect order.");
            // Show the failure asset
            failureAsset.SetActive(true);
        }
    }

    private void NextLevel()
    {
    }

    private void GameOver()
    {
    }
}
