namespace Services.Input
{
    public interface IInputService
    {
        public float HorizontalAxis { get; }
        public bool IsJumpButtonUp { get; }
    }
}