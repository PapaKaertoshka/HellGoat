using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float speed;
    [SerializeField, Range(0f, 10f)] private float maxHP;
    private float hp;

    private void Start()
    {
        hp = maxHP;
    }

    public void Damage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(
                Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")
            ) * speed * Time.deltaTime;
    }
}
