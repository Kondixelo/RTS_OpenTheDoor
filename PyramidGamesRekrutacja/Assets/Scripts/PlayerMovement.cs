using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent player;
    private int groundMask;
    Renderer rend;
    private bool gameON;
    void Start()
    {
        PauseGame(); 
        player = gameObject.GetComponent<NavMeshAgent>();
        groundMask = LayerMask.NameToLayer("Ground");
    }
    void Update()
    {
        if (gameON)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitPoint;
                if (Physics.Raycast(ray,out hitPoint, Mathf.Infinity)){
                    Debug.Log(hitPoint.collider.gameObject.layer);
                    Debug.Log(groundMask);

                    if (hitPoint.collider.gameObject.layer == groundMask){
                        MoveTo(hitPoint.point);
                    }                
                }
            }   
        }
 
    }

    public void MoveTo(Vector3 destination){
        player.SetDestination(destination);
    }

    public void StartGame() { gameON = true; }

    public void PauseGame() { gameON = false; }

}
