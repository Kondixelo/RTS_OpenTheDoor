using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerInteractions : MonoBehaviour
{
    public UIController uIController; //Object with UIController script
    
    public LayerMask objectMask;
    private bool gameON;

    //Scripts attached to game object
    private PlayerInventory playerInv;
    private PlayerMovement playerMov;

    private GameObject pointedObject; //Object which player has pointed
    private Material[] objectMaterials; //Material components pointed objects

    private float distanceFromObject;
    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
        playerMov = gameObject.GetComponent<PlayerMovement>();
        SetGameStatus(false);
    }
    void Update()
    {
        if (gameON)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;
            
            if (Physics.Raycast(ray,out hitPoint, 1000f, objectMask))
            {
                pointedObject = hitPoint.transform.gameObject;
                objectMaterials = pointedObject.GetComponent<Renderer>().materials;
                distanceFromObject = Vector3.Distance(gameObject.transform.position, pointedObject.transform.position);
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && (pointedObject.tag == "Chest"||pointedObject.tag == "Door" || pointedObject.tag == "Item" ) )
                {
                    StartCoroutine(InspectObject(pointedObject.transform.position));
                }
                ChangeColor(Color.cyan);               

                    
            }else
            {
                if (pointedObject != null)
                {
                    distanceFromObject = Vector3.Distance(gameObject.transform.position, pointedObject.transform.position);
                    objectMaterials = pointedObject.GetComponent<Renderer>().materials;
                    ChangeColor(Color.white);
                }
            }      
        }  
    }

    public void ChangeColor(Color colorEnd)
    {
        foreach (Material objMaterial in objectMaterials){
            if (objMaterial.color != colorEnd){
                objMaterial.DOColor(colorEnd, "_Color", 0.5f);
            }
        }
    }

    public IEnumerator InspectObject(Vector3 objectPosition) //Inspect object and wait until distance from object object will be lass than 3 to show message 
    {
        if (distanceFromObject > 3) { playerMov.MoveTo(objectPosition); }
        yield return new WaitUntil( () => distanceFromObject <= 3);
        uIController.PointedObjectMessage(pointedObject);
    }


    public void SetGameStatus(bool gameOnStatus) { gameON = gameOnStatus; }

}
