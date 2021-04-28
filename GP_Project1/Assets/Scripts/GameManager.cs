using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<EnemyMaster> enemies= new List<EnemyMaster>();

    [SerializeField]
    private float timeChangeCycle = 30;

    private int chosenBiter=0;

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gb in obj)
        {
            EnemyMaster en = gb.GetComponent<EnemyMaster>();
            if(en) enemies.Add(en);
        }//Get the enemies first

        //Start the infinite loop
        InvokeRepeating(nameof(ElectNewBiter), 0, timeChangeCycle);
    }


    private void ElectNewBiter() { 
        ClearAllBiters();
        chosenBiter = Mathf.RoundToInt( Random.Range(0, enemies.Count)); //Random

        enemies[chosenBiter].ChangeToBiter();
        Debug.Log("Chosen biter : "+ enemies[chosenBiter].gameObject);
    }

    private void ClearAllBiters()
    {
        foreach(EnemyMaster enemy in enemies)
        {
            enemy.ChangeToEnemy();
        }
        Debug.Log("Cleared");
    }
}
