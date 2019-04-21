using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aliado = NPC.Ally;
using Enemigo = NPC.Enemy;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class Game : MonoBehaviour {

    public Text NumAld;
    public Text NumZomb;
    public Text NumZombSprintrs;


    GameObject[] Zombies;
    GameObject[] ZombiesSprinters;
    GameObject[] Aldeano;

    int NumZombies = 0;
    int NumZombiesSprinters = 0;
    int NumAldeanos = 0;

    void Start()
    {
        AsignarComponente();
        new Items();
    }

    /// <summary>
    /// Se encarga de mostrar la cantidad total de zombies y aldeanos en UI
    /// </summary>
    void Update()
    {
        Zombies = GameObject.FindGameObjectsWithTag("Zombie");
        ZombiesSprinters = GameObject.FindGameObjectsWithTag("ZombieSprinter");
        Aldeano = GameObject.FindGameObjectsWithTag("Aldeano");
        NumZombies = 0;
        NumZombiesSprinters = 0;
        NumAldeanos = 0;

        foreach (GameObject x in Zombies)
        {
            NumZombies++;
            NumZomb.text = NumZombies.ToString() + " Zombies";
        }

        foreach (GameObject x in ZombiesSprinters)
        {
            NumZombiesSprinters++;
            NumZombSprintrs.text = NumZombiesSprinters.ToString() + " ZombiesSprinters";
        }

        foreach (GameObject x in Aldeano)
        {
            NumAldeanos++;
            NumAld.text = NumAldeanos.ToString() + " Aldeanos";
        }

        if (NumZombies == 0 && NumZombiesSprinters == 0)
            SceneManager.LoadScene(0); // Se reinicia la escena, pero se podria crear una scena de victoria prediseñada con un boton de reiniciar
    }

    const int GENERACIONMAXIMA = 26;

    /// <summary>
    /// Se encarga de dar un componente al azar a los cubos que se generan
    /// </summary>
    public void AsignarComponente()
    {
        AsignarReadonlys asignarReadonlys = new AsignarReadonlys();

        GameObject heroe = GameObject.CreatePrimitive(PrimitiveType.Cube);
        heroe.AddComponent<Heroe>();

        for (int i = 0; i < Random.Range(asignarReadonlys.valorMinimo, GENERACIONMAXIMA); i++)
        {
            int valorGeneracion = Random.Range(0, 2);
            if (valorGeneracion == 0)
            {
                GameObject aldeano = GameObject.CreatePrimitive(PrimitiveType.Cube);
                aldeano.name = "Aldeano";
                aldeano.AddComponent<Aliado.AldeanoScript>();
            }
            else
            {
                GameObject zombie = GameObject.CreatePrimitive(PrimitiveType.Cube);
                zombie.name = "Zombie";
                zombie.AddComponent<Enemigo.ZombieScript>();
            }
        }
    }
}