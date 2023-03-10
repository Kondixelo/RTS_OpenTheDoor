using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    public AudioClip WalkingSound;
    public AudioClip pointDestinationSound;
    private int groundMask;
    private bool gameON;

    private GameObject humanObject; //Object with human model
    private Animator humanAnimator; //Animator component in human model
    private NavMeshAgent playerAgent; //NavMeshAgent component in this game object 


    private float timePassed;
    [Range(0, 2)]
    public float soundLength;
    void Start()
    {
        SetGameStatus(false); 
        playerAgent = gameObject.GetComponent<NavMeshAgent>();
        groundMask = LayerMask.NameToLayer("Ground");
        humanObject = gameObject.transform.GetChild(0).gameObject;
        humanAnimator = humanObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (gameON)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitPoint;
                if (Physics.Raycast(ray,out hitPoint, Mathf.Infinity))
                {
                    if (hitPoint.collider.gameObject.layer == groundMask)
                    {
                        MoveTo(hitPoint.point);
                    }                
                }
            }   
        }

        if (playerAgent.velocity == Vector3.zero)
        {
            humanAnimator.SetBool("isWalking", false);
        }else
        {
            humanAnimator.SetBool("isWalking", true);
            CheckAndPlaySound();
        }
 
    }

    private void CheckAndPlaySound(){
        timePassed += Time.deltaTime;
        if(timePassed >= soundLength){
            SoundManager.PlaySound(WalkingSound, 0.5f);
            timePassed = 0;
        }
    }

    public void MoveTo(Vector3 destination) //Move player to the set destination
    {
        SoundManager.PlaySound(pointDestinationSound, 1f);
        playerAgent.SetDestination(destination);
    }

    public void SetGameStatus(bool gameOnStatus) { gameON = gameOnStatus; }

    public void ResetPlayerPosition()
    {
        gameObject.transform.GetComponent<NavMeshAgent>().Warp(Vector3.zero);
        MoveTo(Vector3.zero);
        SoundManager.PlaySound(pointDestinationSound,1f);
    }

}
