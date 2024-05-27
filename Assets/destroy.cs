using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Remove", 4f);
    }

    void Remove()
    {
        ObjectPooler.Instance.ReturnObject(gameObject, gameObject);
    }
}
