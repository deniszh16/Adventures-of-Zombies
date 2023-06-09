namespace Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Jump = "Jump";
        
        public abstract float HorizontalAxis { get; }
        public abstract bool IsJumpButtonUp { get; }
    }
}