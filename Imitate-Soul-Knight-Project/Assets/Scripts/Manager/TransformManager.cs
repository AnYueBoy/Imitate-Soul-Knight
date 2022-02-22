using UnityEngine;

public class TransformManager : MonoBehaviour, ITransfromManager {
    [SerializeField]
    private Transform goTransform;

    [SerializeField]
    private Transform effectTransform;

    public Transform GoTransfrom => goTransform;

    public Transform EffectTransfrom => effectTransform;
}