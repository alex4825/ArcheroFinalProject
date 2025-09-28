using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class FreeExploderView : MonoBehaviour, IView
    {
        [SerializeField] private ParticleSystem _explosionPrefab;
        [SerializeField] private ParticleSystem _explosionDecalPrefab;

        public void ShowExplodeIn(Vector3 point, float radius)
        {
            ParticleSystem explosion = Instantiate(_explosionPrefab, point, _explosionPrefab.transform.rotation, null);
            ParticleSystem explosionDecal = Instantiate(_explosionDecalPrefab, point, _explosionDecalPrefab.transform.rotation, null);

            explosion.transform.localScale *= radius / 2;
            explosionDecal.transform.localScale *= radius / 2;
        }
    }
}