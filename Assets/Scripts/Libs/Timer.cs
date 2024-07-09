using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CountDownTimer
{
    [Serializable]
    public struct Timer
    {
        [SerializeField]private float target;
        private float passedTime;
        public float NormalizedTime => Mathf.Min(1f, passedTime / target);

        public Timer(float duration)
        {
            target = duration;
            passedTime = 0f;
        }

        public void UpdateTimer()
        {
            passedTime += Time.deltaTime;
        }

        public bool Done()
        {
            if (passedTime >= target)
            {
                return true;
            }

            return false;
        }

        public void ResetTimer()
        {
            passedTime = 0f;
        }
        public void ResetTimer(float newTime)
        {
            target = newTime;
            passedTime = 0f;
        }

        public float PassedTime()
        {
            return passedTime;
        }
        
        public float Remaining()
        {
            return target - passedTime;
        }      

        public float GetTarget()
        {
            return target;
        }
    }
}