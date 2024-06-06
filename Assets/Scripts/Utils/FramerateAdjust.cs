using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateAdjust : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 120;
    }
}
