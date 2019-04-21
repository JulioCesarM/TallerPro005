using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    public Items()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            item.GetComponent<Renderer>().material.color = Color.blue;
            item.transform.position = new Vector3(Random.Range(-30, 31),0, Random.Range(-30, 31));
            item.AddComponent<Boost>();
        }
    }
}

public class Boost : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Heroe")
        {
            other.gameObject.GetComponent<Heroe>().cd -= .2f;
            Destroy(this.gameObject);
        }
    }
}
