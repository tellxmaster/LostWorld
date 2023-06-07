using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Renderer fondo;
    
    /*private int totalFruits;
    private int collectedFruits;*/

    void Start()
    {
        /*totalFruits = GameObject.FindGameObjectsWithTag("Item").Length;
        Debug.Log("Total Items: " + totalFruits);
        collectedFruits = 0;
        chest.SetActive(false); // ocultamos el cofre al inicio*/
    }

    void Update()
    {
        fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.015f, 0) * Time.deltaTime;
    }

    public void CollectFruit()
    {
        /*Debug.Log("ítems Colectados: " + collectedFruits);
        collectedFruits++;
        if (collectedFruits > totalFruits)
        {
            chest.SetActive(true); // mostramos el cofre
        }*/
    }

}
