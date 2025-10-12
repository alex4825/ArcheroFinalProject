using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundLauncher : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _upgradeApplySound;
        [SerializeField] private AudioClip _upgradeCancelSound;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this);
        }

        public void PlayClickSound()
        {
            _audioSource.clip = _clickSound;
            _audioSource.Play();
        }
        
        public void PlayUpgradeApplySound()
        {
            _audioSource.clip = _upgradeApplySound;
            _audioSource.Play();
        }
        
        public void PlayUpgradeCancelSound()
        {
            _audioSource.clip = _upgradeCancelSound;
            _audioSource.Play();
        }
    }
}