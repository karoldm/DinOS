using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public bool fullscreen = false;

    void Start()
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}