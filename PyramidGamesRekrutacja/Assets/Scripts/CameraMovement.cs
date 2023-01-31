using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    public float movSpeed;
    public float rotateSpeed;
    public float munuRotateSpeed;

    private bool gameON;

    void Start()
    {
        PauseGame();
    }

    void Update()
    {
        CameraRotationAndMovement(gameON);
    }


    public void CameraRotationAndMovement(bool gameOnMoevement)
    {
        if (gameOnMoevement)
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = inputVertical * transform.forward * Time.deltaTime * movSpeed;
            Vector3 right = inputHorizontal * transform.right * Time.deltaTime * movSpeed;

            Vector3 came = forward + right;

            transform.Translate(came, Space.World);

            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(new Vector3(0f, rotateSpeed, 0f));
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(new Vector3(0f, -rotateSpeed, 0f));
            }
        }
        else
        {
            transform.Rotate(new Vector3(0f, munuRotateSpeed, 0f));
        }
    }


    public void StartGame() { gameON = true; }

    public void PauseGame() { gameON = false; }
}
