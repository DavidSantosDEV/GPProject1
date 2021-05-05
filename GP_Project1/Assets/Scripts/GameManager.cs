using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    [SerializeField]
    private List<EnemyBase> enemies= new List<EnemyBase>();
    [SerializeField]
    private List<AlertSystem> systemsAlert = new List<AlertSystem>();

    [SerializeField]
    private float timeChangeCycle = 30;

    private int chosenBiter=0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gb in obj)
        {
            EnemyBase en = gb.GetComponent<EnemyBase>();
            AlertSystem al = gb.GetComponent<AlertSystem>();
            if (al) systemsAlert.Add(al);
            if(en) enemies.Add(en);
        }//Get the enemies first

        int sort = 0;
        foreach(EnemyBase en in enemies)
        {
            en.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sort;
            sort++;
        }
        //Start the infinite loop
        InvokeRepeating(nameof(ElectNewBiter), 0, timeChangeCycle);
    }

    /*private void Update()
    {
        Debug.Log("w");
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("heee");
            FindObjectOfType<HealthSystem>().TakeDamage(50);
        }
    }*/

    public void EnemyDied(EnemyBase enemy,AlertSystem al)
    {
        enemies.Remove(enemy);
        systemsAlert.Remove(al);
    }

    private void ElectNewBiter() { 
        ClearAllBiters();
        chosenBiter = Mathf.RoundToInt( Random.Range(0, enemies.Count)); //Random

        enemies[chosenBiter].ChangeToBiter();
        Debug.Log("Chosen biter : "+ enemies[chosenBiter].gameObject);
    }

    private void ClearAllBiters()
    {
        foreach(EnemyBase enemy in enemies)
        {
            if(enemy)
            enemy.ChangeToNormal();
        }

        foreach(AlertSystem al in systemsAlert)
        {
            if (al &&al.gameObject)
            {
                al.CancelWorkings();
            }
            else
            {
                systemsAlert.Remove(al);
            }
        }
        Debug.Log("Cleared");
    }
}
