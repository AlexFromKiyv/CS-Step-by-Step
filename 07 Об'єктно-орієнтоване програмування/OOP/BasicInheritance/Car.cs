namespace BasicInheritance
{
    internal class Car
    {
        public readonly int MaxSpeed;
        private int _currentSpeed;
        public Car(int maxSpeed)
        {
            MaxSpeed = maxSpeed;
        }
        public Car():this(60) 
        {
        }
        public int Speed 
        { 
            get { return _currentSpeed; }
            set
            {
                if (value > MaxSpeed)
                {
                    _currentSpeed = MaxSpeed;
                }
                else
                {
                    _currentSpeed = value;
                }
            }
        }
    }
}
