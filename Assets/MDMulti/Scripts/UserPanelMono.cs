using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanelMono : MonoBehaviour
{
    public Color inactiveBarColor;
    public Color activeBarColor;

    public GameObject userImage;
    public Text text;

    private bool authenticated = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(userImage.GetComponent<Image>().mainTexture.name);

        if (!authenticated) setUnAuthDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClick()
    {
        if (!authenticated)
        {
            Debug.Log("Attempted Sign in.");
        }
    }

    void setUnAuthDisplay()
    {
        GetComponent<Image>().color = inactiveBarColor;
        text.text = "Sign in";
    }
}
