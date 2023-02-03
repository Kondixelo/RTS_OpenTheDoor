using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private bool opened;
    void Start()
    {
        opened = false;
    }

    public bool GetOpenStatus() { return opened; }

    public void SetOpenStatus(bool openStatus) { opened = openStatus;}
}
