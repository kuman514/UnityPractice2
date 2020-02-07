using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // object specification
    public float maxHP;

    // uneditable
    private float hp;

    // reference
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            Destroy(obj);
        }
    }

    public void Damage(float dmg)
    {
        hp -= dmg;
    }
}
