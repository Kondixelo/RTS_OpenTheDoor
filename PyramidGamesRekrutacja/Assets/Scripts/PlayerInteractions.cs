using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerInteractions : MonoBehaviour
{
    public LayerMask objectMask;
    private bool gameON;
    private PlayerInventory playerInv;

    private PlayerMovement playerMov;

    public UIController uIController;

    private GameObject pointedObject;
    private Material[] objectMaterials;

    private float distanceFromObject;
    void Start()
    {
        playerInv = gameObject.GetComponent<PlayerInventory>();
        playerMov = gameObject.GetComponent<PlayerMovement>();

        PauseGame();
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
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()  )
                {
                    StartCoroutine(InspectObject(pointedObject.transform.position));
                }                
                foreach (Material objMaterial in objectMaterials){
                    objMaterial.color = Color.yellow;
                }
                    
            }else
            {
                if (pointedObject != null)
                {
                    distanceFromObject = Vector3.Distance(gameObject.transform.position, pointedObject.transform.position);
                    objectMaterials = pointedObject.GetComponent<Renderer>().materials;
                    foreach (Material objMaterial in objectMaterials){
                        objMaterial.color = Color.white;
                    }
                }
            }      
        }  
    }

    public IEnumerator InspectObject(Vector3 objectPosition){
        if (distanceFromObject > 3){
            playerMov.MoveTo(objectPosition);
        }
        yield return new WaitUntil( () => distanceFromObject <= 3);
        uIController.PointedObjectMessage(pointedObject);
    }


    public void StartGame() { gameON = true; }

    public void PauseGame() { gameON = false; }

}
