    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    namespace Ally
    {
        /// <summary>
        /// Esta clase se encarga de darle un nombre y edad al objeto almacenando esta informacion en un struct, luego le agrega un componente AldeanoCollision y le envia el struct creado
        /// </summary>
        public sealed class AldeanoScript : MonoBehaviour
        {  
            int edad;
            string nombre;
            float speed = 0;
            enum Nombres
            {
                Alejandro, Daniel, Julio, Danilo, Kevin, Brayan, Juan, Sebastian, Luis, Alex, Jorge, Anderson, Cristian, Camilo, Carlos, Felipe, Andres, Gustavo, Cesar, andres, lenght
            }

            public void Start()
            {
                GameObject aldeano = this.gameObject;
                aldeano.tag = "Aldeano";
                Nombres nombres;
                aldeano.AddComponent<Rigidbody>().useGravity = false;
                aldeano.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                aldeano.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                edad = Random.Range(15, 101);
                int numeroNombre = Random.Range(0, 20);
                nombres = (Nombres)numeroNombre;
                nombre = nombres.ToString();
                speed = 5f / edad;
                aldeano.AddComponent<NpcScript>().speed = speed;
            }

            public VillagerData GetData()
            {
                VillagerData newvillagerData = new VillagerData();
                newvillagerData.edad = edad;
                newvillagerData.nombre = nombre;
                return newvillagerData;
            }

        }
    }
}
