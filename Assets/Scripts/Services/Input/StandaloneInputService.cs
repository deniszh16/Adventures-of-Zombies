namespace Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override float HorizontalAxis =>
            UnityEngine.Input.GetAxis(Horizontal);

        public override bool IsJumpButtonUp =>
            UnityEngine.Input.GetButtonDown(Jump);
    }
}