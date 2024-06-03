using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    public float time;
    public bool floating = true;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", time);
    }

    void Update()
    {
        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
