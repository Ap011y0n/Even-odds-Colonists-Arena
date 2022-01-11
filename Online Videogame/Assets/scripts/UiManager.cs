using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private bool menuOpen = false;
    public GameObject UI_Menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.GameEnded)
        {
            if(menuOpen)
            {
                menuOpen = false;
                UI_Menu.SetActive(menuOpen);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                menuOpen = true;
                UI_Menu.SetActive(menuOpen);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
