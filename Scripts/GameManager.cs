using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject enemiesObject;
    private List<GameObject> enemies = new List<GameObject>();

    public GameObject player;
    public PlayerMovement playerMovementScript;

    public LayerMask enemyLayerMask;
    public LayerMask playerLayerMask;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for(int i = 0; i < enemiesObject.transform.childCount; i++)
        {
            enemies.Add(enemiesObject.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }

    public IEnumerator EnemiesTurn()
    {
        yield return new WaitForFixedUpdate();
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyBehavior>().actionFinished = false;
            enemy.GetComponent<EnemyBehavior>().Action();
            yield return new WaitUntil(() => enemy.GetComponent<EnemyBehavior>().actionFinished);
            yield return new WaitForFixedUpdate();//Each enemy act on a different frame to avoid glitchs
        }
        yield return new WaitForSeconds(0.55f);//Little wait
        playerMovementScript.canMakeAnAction = true;
    }

    public void DeleteFromEnemiesList(GameObject enemyToDelete)
    {
        enemies.Remove(enemyToDelete);
    }

    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
