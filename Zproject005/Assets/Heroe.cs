using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Aldeano = NPC.Ally;
using Zombie = NPC.Enemy;
using UnityEngine.UI;
/// <summary>
/// este componente se encarga de asignarle camara,rigidbody (Se desactiva la gravedad y se activan todos los constrains), velocidad al azar e inicia una corrutina para los controladores del heroe al objeto que lo tenga
/// </summary>
public sealed class Heroe : MonoBehaviour
{
    GameObject disparo;
    GameObject[] disparos = new GameObject[20];
    private IEnumerator movCo;
    public float cd = 1;
    GameObject[] aldeanos, zombies;
    float distanciaAldeano, distanciaZombie;
    TextMeshProUGUI textAldeano, textZombie;
    float timer1, timer2, timer3;
    public Slider disparoDebug;
    /// <summary>
    /// Se encarga de asignar los textos, ejecutar la asgnacion de los readonly y de agregar componentes
    /// </summary>
    public void Awake()
    {
        gameObject.tag = "Heroe";
        disparoDebug = GameObject.FindGameObjectWithTag("DisparoDebug").GetComponent<Slider>(); ;
        disparo = GameObject.FindGameObjectWithTag("Disparo");
        for(int i = 0; i < 20; i++)
        {
            GameObject instancia = Instantiate(disparo);
            instancia.AddComponent<Rigidbody>().useGravity = false;
            instancia.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            disparos[i] = instancia;
            instancia.transform.position = new Vector3(1000, 0, 1000);
            instancia.SetActive(false);
            instancia.AddComponent<Disparo>();
        }
        textAldeano = GameObject.FindGameObjectWithTag("Aldeanotxt").GetComponent<TextMeshProUGUI>();
        textZombie = GameObject.FindGameObjectWithTag("Zombietxt").GetComponent<TextMeshProUGUI>();
        AsignarReadonlys AsignarVelocidad = new AsignarReadonlys();
        gameObject.AddComponent<Camera>();
        Rigidbody playerRigid = gameObject.AddComponent<Rigidbody>();
        playerRigid.useGravity = false;
        playerRigid.constraints = RigidbodyConstraints.FreezeAll;
        movCo = Acciones(AsignarVelocidad.vel);
        StartCoroutine(movCo);
    }

    /// <summary>
    /// Se encarga de detectar la distancia con aldeanos y zombies con el fin de mostrar informacion en UI de la entidad
    /// </summary>
    void Update()
    {
        aldeanos = GameObject.FindGameObjectsWithTag("Aldeano");
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        timer3 += Time.deltaTime;
        disparoDebug.maxValue = cd;
        disparoDebug.value = timer3;
        if (timer3>cd)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Disparar();
                timer3 = 0;
            }

        foreach (GameObject aldeano in aldeanos)
        {
            distanciaAldeano = Mathf.Sqrt(Mathf.Pow((aldeano.transform.position.x - transform.position.x), 2) + Mathf.Pow((aldeano.transform.position.y - transform.position.y), 2) + Mathf.Pow((aldeano.transform.position.z - transform.position.z), 2));
            if(distanciaAldeano < 5f)
            {
                timer1 = 0;
                VillagerData villagerData = aldeano.GetComponent<Aldeano.AldeanoScript>().GetData();
                textAldeano.text = "Hola soy " + villagerData.nombre + " y tengo " + villagerData.edad;
            }
        }

        foreach(GameObject zombie in zombies)
        {
            distanciaZombie = Mathf.Sqrt(Mathf.Pow((zombie.transform.position.x - transform.position.x), 2) + Mathf.Pow((zombie.transform.position.y - transform.position.y), 2) + Mathf.Pow((zombie.transform.position.z - transform.position.z), 2));
            if (distanciaZombie < 5f)
            {
                timer2 = 0;
                ZombieData zombieData = zombie.GetComponent<Zombie.ZombieScript>().GetData();
                textZombie.text = "Wraaaaarr quiero " + zombieData.gusto;
            }
        }
        if(timer1 > 5f)
        {
            textAldeano.text = "";
        }

        if (timer1 > 5f)
        {
            textZombie.text = "";
        }

    }

    public void Disparar()
    {
        disparos[0].SetActive(true);
        GameObject disparoActual = disparos[0];
        disparoActual.transform.position = transform.position;
        disparoActual.transform.rotation = transform.rotation;
        for(int i = 0; i < disparos.Length; i++)
        {
            if(i == 19)
                disparos[i] = disparoActual;
            else
                disparos[i] = disparos[i + 1];

        }
    }

    /// <summary>
    /// Esta corrutina se encarga de asignar las clases movimiento y rotacion, aparte de ejecutarlas cada frame
    /// </summary>
    /// <returns></returns>
    public IEnumerator Acciones(float vel)
    {

        Movimiento movimiento = new Movimiento();
        Rotacion rotacion = new Rotacion();

        while (true)
        {
            movimiento.Mover(this.gameObject, vel);
            rotacion.Girar(this.gameObject, vel);

            yield return new WaitForEndOfFrame();
        }
    }
}
/// <summary>
/// Aigna una velocidad random al Heroe
/// </summary>
public sealed class AsignarReadonlys
{
    public readonly int valorMinimo;
    public readonly float vel;

    public AsignarReadonlys()
    {
        vel = Random.Range(0.2f, 0.8f);
        valorMinimo = Random.Range(5, 16);
    }
}

/// <summary>
/// Permite que el objeto que lo tenga se mueva adelante y atras con W y S
/// </summary>
public sealed class Movimiento
{
    public void Mover(GameObject x, float vel)
    {
        if (Input.GetKey(KeyCode.W))
        {
            x.transform.Translate(0, 0, vel / 4);
        }
        if (Input.GetKey(KeyCode.S))
        {
            x.transform.Translate(0, 0, -vel / 4);
        }
    }
}

/// <summary>
/// Permite que el objeto que lo tenga que gire a su mirada a los lados con A y D
/// </summary>
public sealed class Rotacion
{
    public void Girar(GameObject z, float vel)
    {
        if (Input.GetKey(KeyCode.A))
        {
            z.transform.Rotate(0, -vel * 4, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            z.transform.Rotate(0, vel * 4, 0);
        }
    }
}


/// <summary>
/// Se encarga de mover y detectar las colisiones del dispàro y de igual forma desactivar el objeto despues de un tiempo o cuando colisiona
/// </summary>
public sealed class Disparo : MonoBehaviour
{
    float timer = 0;
    void Update()
    {
        transform.Translate(0, 0, .3f);
        timer += Time.deltaTime;
        if(timer > 10)
        {
            gameObject.SetActive(false);
            timer = 0;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Zombie" || other.gameObject.tag == "ZombieSprinter")
        {
            Destroy(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

}
