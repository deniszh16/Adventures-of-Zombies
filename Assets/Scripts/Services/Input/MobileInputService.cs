namespace Services.Input
{
    public class MobileInputService : InputService
    {
        public override float HorizontalAxis =>
            SimpleInput.GetAxis(Horizontal);
        
        public override bool IsJumpButtonUp =>
            UnityEngine.Input.GetButtonDown(Jump);
    }
}