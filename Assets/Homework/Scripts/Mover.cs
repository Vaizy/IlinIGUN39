using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody _body;

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

        _body = GetComponent<Rigidbody>();
        
        var target = _start;

        while (true)
        {
            yield return new YieldMoveBody(_body, target, _speed);
            if ((_body.transform.position - target).magnitude < _speed * Time.fixedDeltaTime)
            {
                yield return new WaitForSeconds(_delay);
                target = target == _start ? _end : _start;
            }
        }
    }

    private class YieldMoveBody : CustomYieldInstruction
    {
        public override bool keepWaiting => false;

        public YieldMoveBody(Rigidbody body, Vector3 target, float speed)
        {
            Transform transform = body.transform;
            Vector3 direction = target - transform.position;
            direction = Vector3.Normalize(direction) * speed;
            
            Vector3 position = transform.position + direction * Time.fixedDeltaTime;

            body.MovePosition(position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_start, 0.3f);
        Gizmos.DrawWireSphere(_end, 0.3f);
        Gizmos.DrawLine(_start, _end);
    }
}
