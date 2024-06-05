using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HeartDisplay : MonoBehaviour
{

    public Transform[] RotatingObjects;

    public Image[] Hearts;
    public Image[] Shields;

    public Player _player;

    [Header("GUI colors")]
    [SerializeField] private Color softRed;
    [SerializeField] private Color softGray;
    [SerializeField] private Color softYellow;

    void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        foreach (Image heart in Hearts)
        {
            heart.color = softRed;
        }
        foreach (Image shield in Shields)
        {
            shield.enabled = true;
        }
    }

    void Update()
    {
        int item = 0;
        foreach (Transform heart in RotatingObjects)
        {
            if (Mathf.RoundToInt(Time.time) % 15 == item)
            {
                heart.Rotate(0, 360f * Time.deltaTime, 0);
            }
            else
            {
                heart.rotation = Quaternion.identity;
            }
            item++;
        }
        item = 0;
        foreach (Image heart in Hearts)
        {
            if (_player._HP <= item)
                heart.color = Color.gray;
            item++;
        }
        item = 0;
        foreach (Image shield in Shields)
        {
            if (_player._Shields <= item)
                shield.enabled = false;
            item++;
        }
    }
}
 