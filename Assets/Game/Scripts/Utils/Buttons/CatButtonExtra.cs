using UnityEngine;

public class CatButtonExtra : ButtonExtra
{
    [SerializeField] private GameObject heartParticleEffect;
    protected override void Awake()
    {
        base.Awake();

        thisButton.onClick.AddListener(HeartEffect);
    }

    private void HeartEffect()
    {
        if (heartParticleEffect.activeSelf == false)
        {
            heartParticleEffect.SetActive(true);
        }
    }
}
