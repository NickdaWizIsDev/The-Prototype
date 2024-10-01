using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button restart;
    public GameObject panel;

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Trigger("unpause");
            Deactivate();
        }

        if(DataManager.Instance != null)
        {
            if (!DataManager.Instance.hasSeenIntro)
                restart.interactable = false;
            else if (DataManager.Instance.hasSeenIntro)
                restart.interactable = true;
        }        
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        Trigger("pause");
    }

    public void Deactivate()
    {
        Time.timeScale = Mathf.Lerp(0.1f, 1, 2);
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Floor 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Trigger(string trigger)
    {        
        anim.SetTrigger(trigger);
    }
}
