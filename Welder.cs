using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welder : MonoBehaviour
{
    public int whatIsParts; //the layers index
    public float range = 2.5f;
    public string whatIsPart;

    void Update()
    {
        RaycastHit hit; GameObject part;
        Ray scanRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(scanRay, out hit, range, whatIsParts))
        {
            Debug.Log(hit.transform.tag + " " + hit.transform.name);
            if (hit.transform.CompareTag(whatIsPart))
            {
                part = hit.transform.gameObject;
                Debug.DrawRay(scanRay.origin, hit.transform.position);
                //implement timer
                if(part.transform.TryGetComponent<BulletWound>(out var ok))
                {
                    part.transform.root.GetComponent<InjuriesController>().removeInjury(part.name);
                }
            }
        }
    }
}
