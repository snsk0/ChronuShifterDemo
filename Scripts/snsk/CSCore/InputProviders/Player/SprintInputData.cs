namespace InputProviders.Player
{
    public struct SprintInputData
    {
        public readonly bool isSprintInput;
        public readonly bool isHolding;


        public SprintInputData(bool isSprintInput,  bool isHolding)
        {
            this.isSprintInput = isSprintInput;
            this.isHolding = isHolding;
        }
    }
}
