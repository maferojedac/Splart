using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{

    public float BulletSpeed;

    private GameColor _color;

    public GameObject bullet;

    public float CooldownTime;
    private float _cooldown;

    void IPlayer.TakeDamage()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Vector3 pos = Input.mousePosition;
            if(pos.y > 125f && _cooldown < 0)
            {
                pos.z = 1f;
                pos = Camera.main.ScreenToWorldPoint(pos);

                Vector3 speed = (Camera.main.transform.position - pos).normalized * -BulletSpeed;

                GameObject generatedBullet = Instantiate(bullet, pos, Quaternion.identity);
                generatedBullet.GetComponent<Bullet>()._color = _color;
                generatedBullet.GetComponent<Rigidbody>().AddForce(speed, ForceMode.VelocityChange);
                _cooldown = CooldownTime;
            }
        }
        _cooldown -= Time.deltaTime;
    }

    public void SelectRed()
    {
        _color = GameColor.Red;
    }

    public void SelectYellow()
    {
        _color = GameColor.Yellow;
    }

    public void SelectBlue()
    {
        _color = GameColor.Blue;
    }

    public void SelectWhite()
    {
        _color = GameColor.White;
    }

    public void SelectBlack()
    {
        _color = GameColor.Black;
    }
}
