using System.Collections;
using UnityEngine;

//A glowing sphere to hide the monsters being reset.
public class CoolMysticalOrb : MonoBehaviour
{
    [SerializeField] GameObject tickleMonster;
    [SerializeField] AudioSource orbSound;

    void Start()
    {
        StartCoroutine(InstantiateMonster());
        orbSound.Play();
    }

    private IEnumerator InstantiateMonster()
    {
        GameObject monsterInstance = Instantiate(tickleMonster, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5f);
        monsterInstance.GetComponent<TickleMonsters>().SetDefaultPhysicsProperties();
        Destroy(gameObject);
    }

}
