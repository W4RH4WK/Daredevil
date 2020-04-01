using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject GameObject;

    public Vector3 Count = Vector3.one;

    public Vector3 Spacing = Vector3.one;

    void Start()
    {
        for (int z = 0; z < Count.z; z++)
        {
            for (int y = 0; y < Count.y; y++)
            {
                for (int x = 0; x < Count.x; x++)
                {
                    var instance = Instantiate(GameObject, transform);
                    instance.transform.localPosition = Vector3.Scale(Spacing, new Vector3(x, y, z));
                }
            }
        }
    }
}
