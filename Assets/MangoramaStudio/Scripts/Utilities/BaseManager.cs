using UnityEngine;
using UnityEngine.UI;


public class BaseManager : MonoBehaviour
{
    public GameManager GameManager { get; set; }

    public virtual void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
    }
}