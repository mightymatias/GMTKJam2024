using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private bool _menuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenu.SetActive(false);
    }

    public void SwitchMenu()
    {
        _menuActive = !_menuActive;
        if (_menuActive)
        {
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
        }        
        if (!_menuActive)
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchMenu();
        }
    }
}
