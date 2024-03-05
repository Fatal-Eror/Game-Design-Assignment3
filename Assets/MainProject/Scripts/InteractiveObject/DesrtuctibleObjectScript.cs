using UnityEngine;

public class DesrtuctibleObjectScript : MonoBehaviour
{
    public void BeHurted()
    {
            Destroy(gameObject);
    }
}
