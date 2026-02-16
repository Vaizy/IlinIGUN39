using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    public Vector3 rotate;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();

        Transform transform = this.transform;

        while (true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(rotate.x, rotate.y, rotate.z), Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
