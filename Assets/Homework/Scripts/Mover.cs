using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private Vector3 _start;
    [SerializeField]
    private Vector3 _end;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _delay;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Transform transform;
        

        while (true)
        {
            transform = rigidbody.transform;
            Vector3 direction = _start - transform.position;
            direction = Vector3.Normalize(direction) * _speed;

            Vector3 position = transform.position + direction * Time.deltaTime;

            rigidbody.MovePosition(position);

            yield return new WaitForFixedUpdate();
        }
    }
}
