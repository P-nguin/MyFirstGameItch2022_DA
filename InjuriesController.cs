using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuriesController : MonoBehaviour
{
    public int maxInjuries = 5, minInjuries = 1;
    public int amtInjuries;
    
    HashSet<string> dmgParts = new HashSet<string>();
    int[] woundMax = { 2, 8, 6, 1 }; // maxBullet = 6, maxLaceration = 8, maxRust = 2, maxBloodLoss = 1;
                                     // 0->bullet      1->laceration,     2->rust,     3->blood loss(time based)
    int[] woundCnt;

    private GameObject[] allParts, panels, internals, arms, screws, pipes;
    public List<string> injuries = new List<string>();

    void Start()
    {
        Random.InitState(new System.Random().Next());
        getAllParts();

        
        woundCnt = new int[woundMax.Length];
        for(int i = 0; i < amtInjuries; i++)
        {
            int cnt = 0;
            while (true && cnt < 10)
            {
                int type = Random.Range(0, woundMax.Length - 2); //blood loss is timebased only, -1 for that
                if (type == 0 && woundCnt[type] + 1 <= woundMax[type])
                {
                    determinePart(type, internals);
                    break;
                }
                else if(type == 1 && woundCnt[type] + 1 <= woundMax[type])
                {
                    determinePart(type, panels);
                    break;
                }
                else if(type == 2)
                {
                    determinePart(type, arms);
                    break;
                }
                cnt++;
            }
            if (cnt == 10) Debug.Log("broken");
        }
    }

    void getAllParts()
    {
        amtInjuries = Random.Range(minInjuries, maxInjuries);
        allParts = new GameObject[gameObject.transform.GetChild(0).childCount];
        for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
        {
            allParts[i] = gameObject.transform.GetChild(0).GetChild(i).gameObject;
            Debug.Log(allParts[i].transform.name);
        }

        panels = new GameObject[allParts[0].transform.childCount];
        for(int i = 0; i < allParts[0].transform.childCount; i++)
        {
            panels[i] = allParts[0].transform.GetChild(i).transform.gameObject;
        }

        internals = new GameObject[allParts[1].transform.childCount];
        for (int i = 0; i < allParts[1].transform.childCount; i++)
        {
            internals[i] = allParts[1].transform.GetChild(i).transform.gameObject;
        }

        arms = new GameObject[allParts[2].transform.childCount];
        for (int i = 0; i < allParts[2].transform.childCount; i++)
        {
            arms[i] = allParts[2].transform.GetChild(i).transform.gameObject;
        }

        pipes = new GameObject[allParts[3].transform.childCount];
        for (int i = 0; i < allParts[3].transform.childCount; i++)
        {
            pipes[i] = allParts[3].transform.gameObject;
        }

        screws = new GameObject[allParts[4].transform.childCount];
        for (int i = 0; i < allParts[4].transform.childCount; i++)
        {
            screws[i] = allParts[4].transform.GetChild(i).transform.gameObject;
        }
    }

    void determinePart(int type, GameObject[] options)
    {
        int part = Random.Range(0, options.Length - 1);
        while (dmgParts.Contains(options[part].name))
            part = Random.Range(0, options.Length - 1);

        Debug.Log("Injured Part: " + options[part].name);
        Debug.Log("Not dying here");

        dmgParts.Add(options[part].name);
        woundCnt[type]++;
    }

    public HashSet<string> getInjuries()
    {
        return dmgParts;
    }

    public void removeInjury(string obj)
    {
        dmgParts.Remove(obj);
        Camera.main.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Scanner>().updateScanner();
    }
}
