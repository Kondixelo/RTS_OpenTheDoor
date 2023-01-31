using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent player;
    public LayerMask groundMask;
    public LayerMask doorMask;
    Renderer rend;

    


    // Use this for initialization
    void Start () 
    {   
        player = gameObject.GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;
            if (Physics.Raycast(ray,out hitPoint, groundMask)){
                //targetDest.transform.position = hitPoint.point;
                player.SetDestination(hitPoint.point);
            }

            
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit doorPoint;
            if (Physics.Raycast(ray,out doorPoint, doorMask)){
                
                Debug.Log(doorPoint.collider.gameObject.name);
                //GameObject door =  doorPoint.collider.gameObject.transform.GetChild(1).gameObject;
                GameObject door = doorPoint.collider.gameObject.transform.parent.gameObject;
                Debug.Log(door.name);

                door.transform.position += Vector3.up * 3;
            }
        }


        
    }

    public void OpenDoor()
    {
        
    }
    /*
    private void OnMouseEnter() {
        

        //rend.material.color = new Color(0.2f,0f,0f,0.3f);
        rend.material.SetColor("_EmissionColor", Color.red);
    
    }

    private void OnMouseExit() {
        rend.material.color = Color.white;
    }
    
    */
    

}
