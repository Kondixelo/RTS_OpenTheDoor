using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class GameController : MonoBehaviour
{

    public GameObject uiControllerObject;
    private UIController uIController;
    public GameObject player;
    public List<GameObject> roomsList;
    public List<float> rotationList;

    private GameObject roomObject;
    private int roomIndex; // random int seed of object's room index;
    private float rotation;
    private int rotationIndex; // random int seed to rotate rooms

    public GameObject CameraMovingObject;
    private CameraMovement camMov;

    public NavMeshSurface[] surfaces;
    public bool gameOver;

    private PlayerMovement playerMov;
    private PlayerInteractions playerInters;
    private PlayerInventory playerInv;

    public GameObject insideFloor;
    public GameObject chestPreefab;

    private GameObject interactedObject;

    void Start()
    {
        CheckPlayerPrefs();
        camMov = CameraMovingObject.GetComponent<CameraMovement>();
        playerMov = player.GetComponent<PlayerMovement>();
        playerInters = player.GetComponent<PlayerInteractions>();
        playerInv = player.GetComponent<PlayerInventory>();
        uIController = uiControllerObject.GetComponent<UIController>();
        PrepareGame();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnChest();
        }
    }



    private void CheckPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("highscore")){ PlayerPrefs.SetFloat("highscore", 0); }
    }

    public void PrepareGame()
    {
        playerMov.PauseGame();
        playerInters.PauseGame();
        playerInv.SetNewEmptyInventory();
        

        DestoryOldRoom();
        SpawnRoom();

        DestroyOldChest();
        SpawnChest();

        player.transform.position = Vector3.zero;
        CameraMovingObject.transform.position = Vector3.zero;
        UpdateMesh();
    }

    private void SpawnRoom()
    {
        int roomListIndex = Random.Range(0, roomsList.Count);
        int roomRotationIndex = Random.Range(0, 4);
        roomObject = roomsList[roomListIndex];
        rotation = rotationList[roomRotationIndex];
        Instantiate(roomObject, new Vector3(0f,2.5f,0f), Quaternion.Euler(0f, rotation, 0f));
    }
    public void SpawnChest()
    {
        float floorSizeX =  insideFloor.GetComponent<MeshCollider>().bounds.size.x;
        float floorSizeZ =  insideFloor.GetComponent<MeshCollider>().bounds.size.z;

        float spawnPosX = Random.Range(2,floorSizeX/2 - 2)*(Random.Range(0,2)*2-1);
        float spawnPosZ = Random.Range(2,floorSizeZ/2 - 2)*(Random.Range(0,2)*2-1);

        float spawnRotY = Random.Range(0,360);

        Vector3 chestPos = new Vector3(spawnPosX, 0.7f,spawnPosZ);
        chestPreefab.transform.localScale = new Vector3(2f,2f,2f);

        Instantiate(chestPreefab, chestPos, Quaternion.Euler(0f,spawnRotY,0f));
    }

    private void DestoryOldRoom()
    {
        GameObject oldRoom = GameObject.FindGameObjectWithTag("Room");

        if (oldRoom != null){ Destroy(oldRoom); }
    }
        private void DestroyOldChest()
    {
        GameObject oldChest= GameObject.FindGameObjectWithTag("Chest");

        if (oldChest != null){ Destroy(oldChest); }
    }

    



    public void StartGame()
    {
        camMov.StartGame();
        playerMov.StartGame();
        playerInters.StartGame();
    }


    public void GameOver()
    {
        gameOver = true;
        camMov.PauseGame();
        playerMov.PauseGame();
        uIController.GameOver();
        playerInters.PauseGame();
    }

    public void UpdateMesh()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }

    public void SetInteractedObject(GameObject interactedObj){
        interactedObject = interactedObj;
    }

    public void AddItemtoInventory()
    {    
        string itemName = interactedObject.name;
        int itemIndex = System.Int32.Parse(itemName.Substring(itemName.Length - 1,1));
        playerInv.AddItemtoInventory(itemIndex);
        Destroy(interactedObject);
    }

    public void OpenChest(){
        interactedObject.transform.DOLocalRotate(new Vector3(-140,0,0),2f).SetEase(Ease.OutBounce);      
    }

    public void OpenDoor(){
        List<int> playerInventory = playerInv.GetInventory();
        if(playerInventory.Contains(2))
        { 
            GameObject wood = interactedObject.transform.parent.gameObject;
            GameObject doorAsset = wood.transform.parent.gameObject;
            Animator doorAnimator = doorAsset.GetComponent<Animator>();
            doorAnimator.SetBool("open",true);
            GameOver();
        }else
        {
            StartCoroutine(uIController.ItemMessage("You need a key!"));
        }
    }

}
