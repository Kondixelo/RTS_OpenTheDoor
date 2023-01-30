using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> roomsList;
    public List<float> rotationList;

    private GameObject roomObject;
    private int roomIndex; // random int seed of object's room index;
    private float rotation;
    private int rotationIndex; // random int seed to rotate rooms

    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame()
    {
        DestoryOldRoom();
        roomIndex = GenerateIndex(0, roomsList.Count);
        rotationIndex = GenerateIndex(0, 4);

        GenerateRoom(roomIndex, rotationIndex);

    }

    private int GenerateIndex(int min, int max) { return Random.Range(min, max); }

    private void GenerateRoom(int roomListIndex, int roomRotationIndex)
    {
        roomObject = roomsList[roomListIndex];
        rotation = rotationList[roomRotationIndex];
        Instantiate(roomObject, new Vector3(0f,2.5f,0f), Quaternion.Euler(0f, rotation, 0f));
    }

    private void DestoryOldRoom()
    {
        GameObject oldRoom = GameObject.FindGameObjectWithTag("Room");

        if (oldRoom != null)
        {
            Destroy(oldRoom);
        }
    }

}
