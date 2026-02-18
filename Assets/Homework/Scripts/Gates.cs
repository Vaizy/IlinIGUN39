using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

public class Gates : MonoBehaviour
{
    [SerializeField]
    public int score;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Ball>(out Ball ball))
        {
            return;
        }
        score++;
        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
