using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpAbout : MonoBehaviour
{
    void Start()
    {
        Vector3 aboutPos = new Vector3(Screen.width * 0.2f, Screen.height* 3f / 4f, 0);
        Vector3 wishPos = aboutPos + new Vector3(0, Screen.height * -0.25f, 0);
        gameObject.transform.Find("Panel").transform.Find("About").transform.position = aboutPos;
        gameObject.transform.Find("Panel").transform.Find("Wishlist").transform.position = wishPos;
    }

}

