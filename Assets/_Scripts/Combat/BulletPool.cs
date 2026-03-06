using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Combat
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _initialPoolSize = 20;

        private Queue<Bullet> _poolQueue = new Queue<Bullet>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                Bullet bullet = Instantiate(_bulletPrefab, transform);
                bullet.gameObject.SetActive(false);
                _poolQueue.Enqueue(bullet);
            }
        }

        public Bullet GetBullet()
        {
            if (_poolQueue.Count > 0)
            {
                Bullet bullet = _poolQueue.Dequeue();
                bullet.gameObject.SetActive(true);
                return bullet;
            }

            Bullet newBullet = Instantiate(_bulletPrefab, transform);
            newBullet.gameObject.SetActive(true);
            return newBullet;
        }

        public void Return(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _poolQueue.Enqueue(bullet);
        }
    }
}