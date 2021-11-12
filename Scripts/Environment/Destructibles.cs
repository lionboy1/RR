using UnityEngine;

public class Destructibles : MonoBehaviour
{
    [SerializeField] GameObject _destroyedCrate;
    [SerializeField] GameObject _explosion;
    [SerializeField] AudioSource _explosionAudio;
    ObjectHealth objectHealth;

    void Start()
    {
        objectHealth = GetComponent<ObjectHealth>();
    }

    public void DestroyObject()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        //_explosionAudio.Play();
        Instantiate(_destroyedCrate, transform.position, transform.rotation);
        Destroy(this.gameObject, 0.5f);
    }
}
