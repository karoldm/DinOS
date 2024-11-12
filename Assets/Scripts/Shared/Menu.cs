using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public ProfileModal modal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenModal()
    {
        this.modal.gameObject.SetActive(true);
    }

    public void CloseModal()
    {
        this.modal.gameObject.SetActive(false);
        this.modal.CloseAllContents();
    }

    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Initial");
    }
}
