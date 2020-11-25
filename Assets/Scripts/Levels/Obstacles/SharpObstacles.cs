namespace Cubra
{
    public class SharpObstacles : CollisionObjects
    {
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
                GameManager.Instance.CharacterController.DamageToCharacter(true, true);
        }
    }
}