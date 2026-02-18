using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Rigidbody _body;
    private Transform _transform;

    [SerializeField]
    public Vector3 rotate;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();

        _body = GetComponent<Rigidbody>();
        _transform = _body.transform;

        while (true)
        {
            Quaternion rotation = Quaternion.Slerp(_transform.rotation, _transform.rotation * Quaternion.Euler(rotate.x, rotate.y, rotate.z), Time.deltaTime);
            _body.MoveRotation(rotation);
            yield return new WaitForFixedUpdate();
        }
    }
}
