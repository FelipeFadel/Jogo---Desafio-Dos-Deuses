using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
