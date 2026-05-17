using UnityEngine;
using System.Collections;

public class FinalSceneManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        SoundManager.Instance.PlayGunShot();
    }
}