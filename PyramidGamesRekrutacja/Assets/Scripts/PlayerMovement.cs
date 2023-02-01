using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent player;
    public LayerMask groundMask;
    Renderer rend;
    private bool gameON;
    void Start()
    {
        PauseGame(); 
        player = gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (gameON)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitPoint;
                if (Physics.Raycast(ray,out hitPoint, 1000f, groundMask)){
                    player.SetDestination(hitPoint.point);
                }
        }   
        }
 
    }

    public void StartGame() { gameON = true; }

    public void PauseGame() { gameON = false; }

}
