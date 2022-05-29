using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class EnemyDoor : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private TMP_Text text;

    private int deadCount = 0;
    private bool opened = false;

    private void FixedUpdate()
    {
        deadCount = targets.Where(x => x == null).Count();
        text.text = $"{deadCount}/{targets.Count}";

        if (!opened && deadCount == targets.Count)
        {
            opened = true;
            GetComponent<Animation>().Play();
        }
    }
}
