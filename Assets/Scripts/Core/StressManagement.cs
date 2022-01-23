using System.Collections.Generic;

namespace StressManagement
{
    public delegate void OnStressAdd(float stress, float maxStress);
    public delegate void OnStressRelief(float stress, float maxStress);
    public delegate void OnStressMax();

    public enum StressLevel
    {
        LOW,
        MID,
        HIGH
    }

    public class StressMeta
    {
        public float maxStress = 600;
        public Dictionary<float, StressLevel> stressIndicationMap = new Dictionary<float, StressLevel>() {
            {0.25f, StressLevel.LOW},
            {0.75f, StressLevel.MID},
            {1f, StressLevel.HIGH}
        };
    }

    public class StressController
    {
        public float currentStress { get; private set; }
        public StressMeta Meta;
        protected OnStressAdd onStressAdd;
        protected OnStressRelief onStressRelief;
        protected OnStressMax onStressMax;

        public StressController(OnStressAdd onStressAdd, OnStressRelief onStressRelief, OnStressMax onStressMax)
        {
            this.Initialize(onStressAdd, onStressRelief, onStressMax);
        }

        public StressController(OnStressAdd onStressAdd, OnStressRelief onStressRelief, OnStressMax onStressMax, float maxStress)
        {
            this.Initialize(onStressAdd, onStressRelief, onStressMax);
            this.Meta.maxStress = maxStress;
        }

        public StressLevel GetStressLevel()
        {
            float stressPercentage = this.currentStress / this.Meta.maxStress;
            float minStress = 0f;
            StressLevel evaluatedLevel = StressLevel.LOW;
            foreach(KeyValuePair<float, StressLevel> stressLevel in this.Meta.stressIndicationMap)
            {
                if(minStress < stressPercentage && stressPercentage <= stressLevel.Key)
                {
                    minStress = stressLevel.Key;
                    evaluatedLevel = stressLevel.Value;
                }
            }
            return evaluatedLevel;
        }

        protected void Initialize(OnStressAdd onStressAdd, OnStressRelief onStressRelief, OnStressMax onStressMax)
        {
            this.currentStress = 0f;
            this.Meta = new StressMeta();
            this.onStressAdd = onStressAdd;
            this.onStressRelief = onStressRelief;
            this.onStressMax = onStressMax;
        }

        private void Check()
        {
            if(this.currentStress >= this.Meta.maxStress)
                this.onStressMax();
        }

        public void Add(float stress)
        {
            this.currentStress += stress;
            this.onStressAdd(this.currentStress, this.Meta.maxStress);
            this.Check();
        }

        public void Relieve(float stress)
        {
            this.currentStress -= stress;
            this.onStressRelief(this.currentStress, this.Meta.maxStress);
            this.Check();
        }

        public void Reset()
        {
            this.currentStress = 0f;
        }
    }
}
