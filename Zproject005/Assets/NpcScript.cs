using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public sealed class NpcScript : MonoBehaviour
    {

        public GameObject[] aldeanos, zombies;
        public GameObject heroe, target;
        public float distanciaAldeano, distanciaHeroe, distanciaZombie, distanciaTarget,speed = 0;
        public Vector3 targetNormalized;
        public float timer;
        public Estado estado;
        int velocidadGiro;

        public enum Estado
        {
            Idle, Moving, Rotating, Pursuing, Running
        }

        /// <summary>
        /// Asigna velocidad de giro
        /// </summary>
        void Start()
        {
            velocidadGiro = Random.Range(1, 20);
        }

        /// <summary>
        /// Se encarga de la gestion de distancia para los zombies y aldeanos
        /// </summary>
        void Update()
        {
            aldeanos = GameObject.FindGameObjectsWithTag("Aldeano");
            zombies = GameObject.FindGameObjectsWithTag("Zombie");
            heroe = GameObject.FindGameObjectWithTag("Heroe");

            if (this.gameObject.tag == "Zombie" || this.gameObject.tag == "ZombieSprinter")
            {
                foreach (GameObject aldeano in aldeanos)
                {
                    distanciaAldeano = Mathf.Sqrt(Mathf.Pow((aldeano.transform.position.x - transform.position.x), 2) + Mathf.Pow((aldeano.transform.position.y - transform.position.y), 2) + Mathf.Pow((aldeano.transform.position.z - transform.position.z), 2));
                    distanciaHeroe = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));

                    if (target)
                    {
                        estado = Estado.Pursuing;
                        distanciaTarget = Mathf.Sqrt(Mathf.Pow((target.transform.position.x - transform.position.x), 2) + Mathf.Pow((target.transform.position.y - transform.position.y), 2) + Mathf.Pow((target.transform.position.z - transform.position.z), 2));
                        targetNormalized = Vector3.Normalize(target.transform.position - transform.position);
                        if (distanciaAldeano < distanciaTarget)
                        {
                            target = aldeano;
                        }
                        else if(distanciaTarget > 5f || target.tag == "Zombie")
                        {
                            target = null;
                            estado = (Estado)Random.Range(0, 3);
                            targetNormalized = Vector3.zero;
                        }
                    }
                    else if (distanciaAldeano < 5f)
                    {
                        target = aldeano;
                    }
                    else if (distanciaHeroe < 5f)
                    {
                        target = heroe;
                        targetNormalized = -Vector3.Normalize(target.transform.position - transform.position);
                    }
                }
            }
            else if (this.gameObject.tag == "Aldeano")
            {
                foreach (GameObject zombie in zombies)
                {
                    distanciaZombie = Mathf.Sqrt(Mathf.Pow((zombie.transform.position.x - transform.position.x), 2) + Mathf.Pow((zombie.transform.position.y - transform.position.y), 2) + Mathf.Pow((zombie.transform.position.z - transform.position.z), 2));

                    if (target)
                    {
                        estado = Estado.Running;
                        distanciaTarget = Mathf.Sqrt(Mathf.Pow((target.transform.position.x - transform.position.x), 2) + Mathf.Pow((target.transform.position.y - transform.position.y), 2) + Mathf.Pow((target.transform.position.z - transform.position.z), 2));
                        targetNormalized = Vector3.Normalize(transform.position - target.transform.position);
                        if (distanciaTarget > 5f)
                        {
                            target = null;
                            estado = (Estado)Random.Range(0, 3);
                            targetNormalized = Vector3.zero;
                        }
                        else if (distanciaZombie < distanciaTarget)
                        {
                            target = zombie;
                        }
                    }
                    else if (distanciaAldeano < 5f)
                    {
                        target = zombie;
                    }
                }
            }

            switch (estado)
            {
                case Estado.Idle:
                    timer += Time.deltaTime;
                    break;
                case Estado.Moving:
                    transform.Translate(0, 0, 0.025f);
                    timer += Time.deltaTime;
                    break;
                case Estado.Rotating:
                    transform.Rotate(0, velocidadGiro, 0);
                    timer += Time.deltaTime;
                    break;
                case Estado.Pursuing:
                    timer = 0;
                    gameObject.transform.position += (targetNormalized * speed);
                    if(aldeanos.Length == 0)
                    {
                        target = null;
                        estado = (Estado)Random.Range(0, 3);
                    }
                    break;
                case Estado.Running:
                    timer = 0;
                    gameObject.transform.position += (targetNormalized * speed);
                    break;
            }

            if (timer > 3)
            {
                estado = (Estado)Random.Range(0, 3);
                timer = 0;
            }
        }
    }
}



