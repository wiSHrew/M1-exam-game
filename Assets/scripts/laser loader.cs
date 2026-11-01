using UnityEngine;

public class laserloader : MonoBehaviour
{
    void Start()
    {
        for (int x = -25; x <= 25; x++)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.GetComponent<Renderer>().material.color = Color.red;
            cylinder.transform.localScale = new Vector3(0.25f, 30f, 0.25f);
            cylinder.transform.position = new Vector3(x, 15f, -200f);
        }
    }
}
