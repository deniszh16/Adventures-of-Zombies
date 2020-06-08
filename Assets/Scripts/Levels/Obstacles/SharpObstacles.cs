namespace Cubra
{
    public class SharpObstacles : CollisionObjects
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
                // Наносим урон персонажу с анимацией смерти и отскоком
                Main.Instance.CharacterController.DamageToCharacter(true, true);
        }
    }
}