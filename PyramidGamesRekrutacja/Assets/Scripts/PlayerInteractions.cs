using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerInteractions : MonoBehaviour
{
    public LayerMask objectMask;
    private bool gameON;
    private PlayerInventory playerInv;

    public UIController uIController;
    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
        PauseGame();
    }
    void Update()
    {
        if (gameON)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitPoint;
                if (Physics.Raycast(ray,out hitPoint, 1000f, objectMask))
                {
                    GameObject pointedObject = hitPoint.transform.gameObject;
                    float distanceFromObject = Vector3.Distance(gameObject.transform.position, pointedObject.transform.position);
                    if (distanceFromObject <= 3){ uIController.PointedObjectMessage(pointedObject); }
                }      
            }
        }  
    }


    public void StartGame() { gameON = true; }

    public void PauseGame() { gameON = false; }

}
