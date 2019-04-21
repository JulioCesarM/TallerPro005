using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Aldeno = NPC.Ally;
using Zombie = NPC.Enemy;
namespace NPC
{
    namespace Enemy
    {

        /// <summary>
        /// Esta clase maneja el estado del objeto entre Idle y Mov recalculando estado cada 5 Segundos
        /// </summary>
        public class ZombieScript : MonoBehaviour
        {
            float timer;
            bool infected = false;
            string gusto;
            public ZombieData zombieData;
            int edad;
            float speed;

            enum Gusto
            {
                Piernas, Dedos, Cerebro, Ojos, Lengua,
            }


            /// <summary>
            /// Se encarga de asignar los valores que se usan en la estructura, asignar tag, cambiar color y asignar componentes extras
            /// </summary>
            public void Start()
            {


                GameObject zombie = this.gameObject;
                Renderer zRender = zombie.GetComponent<Renderer>();
                Gusto enumGusto;
                enumGusto = (Gusto)Random.Range(0, 5);
                gusto = enumGusto.ToString();
                int numeroColor = Random.Range(0, 3);

                if (numeroColor == 0)
                    zRender.material.color = Color.cyan;
                else if (numeroColor == 1)
                    zRender.material.color = Color.green;
                else
                    zRender.material.color = Color.magenta;

                if(infected)
                {
                    zombie.name = "Zombie " + zombieData.nombre;
                    edad = zombieData.edad;
                    zRender.material.color = Color.red;
                }
                else
                {
                    edad = Random.Range(15, 101);
                    zombie.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                    edad = Random.Range(15, 101);
                    zombie.name = "Zombie";
                    zombie.AddComponent<Rigidbody>();
                    zombie.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    zombie.GetComponent<Rigidbody>().useGravity = false;
                }


                if (Random.Range(0, 2) == 0)
                {
                    Zombie(zombie,edad);
                }
                else
                {
                    zombie.AddComponent<ZombieSprinter>().Zombie(zombie,edad);
                }
            }

            public virtual void Zombie(GameObject zombie,int edad)
            {
                speed = 5f / edad;
                zombie.AddComponent<NPC.NpcScript>().speed = speed;
                zombie.tag = "Zombie";
            }
        


        /// <summary>
        /// Se encarga de detectar la colision y con que colisiona
        /// </summary>
        /// <param name="other">
        /// Otro objeto con el cual colisiona
        /// </param>
        void OnCollisionEnter(Collision other)
            {
                if(other.gameObject.tag == "Heroe")
                {
                    SceneManager.LoadScene(0); // recarga la escena pero se podria crear una escena de derrota con un boton de reinicio
                }

                if(other.gameObject.tag == "Aldeano")
                {
                    ZombieScript zombie = other.gameObject.AddComponent<Enemy.ZombieScript>();
                    zombie.infected = true;
                    zombie.zombieData = other.gameObject.GetComponent<Ally.AldeanoScript>().GetData();
                    Destroy(other.gameObject.GetComponent<Aldeno.AldeanoScript>());
                }
            }
            

            /// <summary>
            /// Se encarga de dar una estructura de zombie a un aldeano para transformalo
            /// </summary>
            /// <returns>
            /// Estrucutra de aldeano para conversion
            /// </returns>
            public ZombieData GetData()
            {
                ZombieData newzombieData = new ZombieData();
                newzombieData.edad = edad;
                newzombieData.gusto = gusto;
                return newzombieData;
            }

        }

        public sealed class ZombieSprinter : ZombieScript
        {

            /// <summary>
            /// Se usa para hacer que el zombie corra mas rapido y su color sea siempre negro
            /// </summary>
            /// <param name="zombie">
            /// Gameobject del zombie
            /// </param>
            /// <param name="edad">
            /// Se usa para asignar la valocidad
            /// </param>
            public override void Zombie(GameObject zombie,int edad)
            {
                zombie.GetComponent<Renderer>().material.color = Color.black;
                float speed = 5f / edad;
                zombie.AddComponent<NPC.NpcScript>().speed = speed;
                zombie.tag = "ZombieSprinter";
            }
        }

    }
}
