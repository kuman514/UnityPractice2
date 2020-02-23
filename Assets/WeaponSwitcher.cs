using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    // Settings
    public float switchDelay;

    // Uneditable
    private int index = 0;
    private bool isSwitching = false;

    // References
    public GameObject[] weapon;

    // Start is called before the first frame update
    void Start()
    {
        InitWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            index++;
            if(index >= weapon.Length)
                index = 0;
            StartCoroutine(SwitchDelay(index));
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            index--;
            if(index < 0)
                index = weapon.Length - 1;
            StartCoroutine(SwitchDelay(index));
        }

        // weapon switching by alphanum
        for(int i = 49; i < 58; i++)
        {
            if(Input.GetKeyDown((KeyCode)i) && !isSwitching && weapon.Length > i - 49 && index != i - 49)
            {
                index = i - 49;
                StartCoroutine(SwitchDelay(index));
            }
        }
    }

    private void InitWeapons()
    {
        for(int i = 0; i < weapon.Length; i++)
        {
            weapon[i].SetActive(false);
        }

        weapon[0].SetActive(true);
        index = 0;
    }

    private IEnumerator SwitchDelay(int newIndex)
    {
        isSwitching = true;
        SwitchWeapons(newIndex);
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
    }

    private void SwitchWeapons(int newIndex)
    {
        for(int i = 0; i < weapon.Length; i++)
        {
            weapon[i].SetActive(false);
        }

        weapon[newIndex].SetActive(true);
    }
}
