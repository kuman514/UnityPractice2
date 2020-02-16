using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
    // Player HP Specification
    public float HP;

    // uneditable
    private float maxHP;

    // Reference
    public Text HPText;
    public RectTransform HPBar;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;
        HPText.text = HP + " / " + maxHP;
        HPBar.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        // Test Damaging
        if(Input.GetKeyDown(KeyCode.O))
        {
            ApplyDamage(10);
        }

        // Test Healing
        if(Input.GetKeyDown(KeyCode.P))
        {
            ApplyHeal(9);
        }
    }

    public void ApplyDamage(float dmg)
    {
        HP -= dmg;
        if(HP <= 0)
        {
            HP = 0;
            // Death
            Debug.Log("Player Dead");
        }

        DrawHP();
    }

    public void ApplyHeal(float heal)
    {
        HP += heal;
        if(HP >= maxHP)
        {
            HP = maxHP;
            // Fully Healed
            Debug.Log("Player Fully Healed");
        }

        DrawHP();
    }

    public void DrawHP()
    {
        HPText.text = HP + " / " + maxHP;
        HPBar.localScale = new Vector3(HP / maxHP, 1, 1);
    }
}
