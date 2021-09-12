namespace StressManagement
{
    public delegate void OnStressAdd(float stress);
    public delegate void OnStressRelief(float stress);
    public delegate void OnStressMax();

    public class StressMeta
    {
        public float maxStress = 1000;
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
            this.onStressAdd(this.currentStress);
            this.Check();
        }

        public void Relieve(float stress)
        {
            this.currentStress -= stress;
            this.onStressRelief(this.currentStress);
            this.Check();
        }

        public void Reset()
        {
            this.currentStress = 0f;
        }
    }
}
