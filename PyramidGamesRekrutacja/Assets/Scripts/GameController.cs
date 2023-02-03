using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [Header("Music section")]
    [Tooltip("Music clip which plays in background in loop")] 
    public AudioClip backgroundMusic;

    [Header("Object section")]
    public GameObject uiControllerObject;
    public GameObject CameraMovingObject;
    public GameObject playerObject;

    [Tooltip("Object with spot light")]    
    public GameObject spotLight;
    [Tooltip("Object with main directional light")]   
    public GameObject sunLight; 

    [Header("Spawn section")]
    [Tooltip("List of generated rooms")]
    public List<GameObject> roomsList;

    private List<int> rotationList = new List<int>(); //List with room rotation in degrees

    private UIController uIController;

    [Tooltip("List of surfaces for NavMeshSurface")]
    public NavMeshSurface[] surfaces;
    [Tooltip("Plane where chest spawns")]
    public GameObject spawnPlane;
    [Tooltip("Chest to spawn")]
    public GameObject chestPreefab;

    private GameObject roomObject;
    private int roomIndex; //Rrandom int seed of object's room index;
    private float rotation;
    private int rotationIndex; //Random int seed to rotate rooms

    //Scripts attached to camera moving object, not directly to camera
    private CameraMovement camMov;
    
    // Scripts attached to player object
    private PlayerMovement playerMov;
    private PlayerInteractions playerInters;
    private PlayerInventory playerInv;

    private GameObject interactedObject; //Object which player has interactions
    private ObjectInteraction objectInteraction; //Script attached to interacted object

    void Start()
    {   
        CheckPlayerPrefs();
        rotationList.Add(0);
        rotationList.Add(90);
        rotationList.Add(180);
        rotationList.Add(270);
        camMov = CameraMovingObject.GetComponent<CameraMovement>();
        playerMov = playerObject.GetComponent<PlayerMovement>();
        playerInters = playerObject.GetComponent<PlayerInteractions>();
        playerInv = playerObject.GetComponent<PlayerInventory>();
        uIController = uiControllerObject.GetComponent<UIController>();
        PrepareGame();
        SoundManager.PlayMusic(backgroundMusic);
    }

    private void CheckPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("highscore")){ PlayerPrefs.SetFloat("highscore", 0); }
    }

    public void PrepareGame() //Set room, objects, player start position etc. generally prepare scene
    {
        playerMov.SetGameStatus(false);
        playerInters.SetGameStatus(false);
        playerInv.SetNewEmptyInventory();
        
        spotLight.SetActive(true);
        sunLight.SetActive(false);

        DestoryOldRoom();
        SpawnRoom();
        DestroyOldChests();
        SpawnChest();

        CameraMovingObject.transform.position = Vector3.zero;
        UpdateMesh();
        playerMov.ResetPlayerPosition();
    }

    private void SpawnRoom() //spawn random room
    {
        int roomListIndex = Random.Range(0, roomsList.Count);
        int roomRotationIndex = Random.Range(0, 4);
        roomObject = roomsList[roomListIndex];
        rotation = rotationList[roomRotationIndex];
        Instantiate(roomObject, new Vector3(0f,2.5f,0f), Quaternion.Euler(0f, rotation, 0f));
    }
    public void SpawnChest() //Spawn chest in random spot in Spawn Plane
    {
        float floorSizeX =  spawnPlane.GetComponent<MeshCollider>().bounds.size.x;
        float floorSizeZ =  spawnPlane.GetComponent<MeshCollider>().bounds.size.z;

        float spawnPosX = Random.Range(2,floorSizeX/2 - 2)*(Random.Range(0,2)*2-1);
        float spawnPosZ = Random.Range(2,floorSizeZ/2 - 2)*(Random.Range(0,2)*2-1);

        float spawnRotY = Random.Range(0,360);

        Vector3 chestPos = new Vector3(spawnPosX, 0.7f,spawnPosZ);
        chestPreefab.transform.localScale = new Vector3(2f,2f,2f);

        Instantiate(chestPreefab, chestPos, Quaternion.Euler(0f,spawnRotY,0f));
    }

    private void DestoryOldRoom() //Destroy old room
    {
        GameObject oldRoom = GameObject.FindGameObjectWithTag("Room");
        if (oldRoom != null){ Destroy(oldRoom);}
    }
    private void DestroyOldChests() //Destroy old chests
    {
        GameObject[] oldChests = GameObject.FindGameObjectsWithTag("Chest");
        if (oldChests != null)
        { 
            foreach(GameObject old in oldChests)
            {
                Destroy(old);
            }
        }
    }

    public void StartGame() //Start game, activate camera movement, player movement, player interactions, lights
    {
        camMov.SetGameStatus(true);
        playerMov.SetGameStatus(true);
        playerInters.SetGameStatus(true);

        spotLight.SetActive(false);
        sunLight.SetActive(true);
    }


    public void GameOver() //Stop game, activate camera movement, player movement, player interactions, activate game over menu
    {
        camMov.SetGameStatus(false);
        playerMov.SetGameStatus(false);
        uIController.GameOver();
        playerInters.SetGameStatus(false);
    }

    public void UpdateMesh() //Update NavMesh (after generate new room and spawn chest)
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();
        }    
    }

    public void SetInteractedObject(GameObject interactedObj){ //Set object which player has interaction with
        interactedObject = interactedObj;
    }

    public void AddItemtoInventory() //Add item(object) which player has interaction with to inventory
    {    
        string itemName = interactedObject.name;
        int itemIndex = System.Int32.Parse(itemName.Substring(itemName.Length - 1,1));
        playerInv.AddItemtoInventory(itemIndex);
        Destroy(interactedObject);
    }

    public void OpenChest() //Open chest(object) which player has interaction with
    {    
        objectInteraction = interactedObject.GetComponent<ObjectInteraction>();
        objectInteraction.SetOpenStatus(true);
        interactedObject.transform.DOLocalRotate(new Vector3(-140,0,0),2f).SetEase(Ease.OutBounce);      
    }

    public void OpenDoor(){ //Open door(object) which player has interaction with
        List<int> playerInventory = playerInv.GetInventory();
        if(playerInventory.Contains(2))
        { 
            GameObject wood = interactedObject.transform.parent.gameObject;
            GameObject doorAsset = wood.transform.parent.gameObject;
            Animator doorAnimator = doorAsset.GetComponent<Animator>();
            doorAnimator.SetBool("open",true);
            objectInteraction = interactedObject.GetComponent<ObjectInteraction>();
            objectInteraction.SetOpenStatus(true);
            GameOver();
        }else
        {
            uIController.ItemMessage("You need a key!", true);
        }
    }

}
