namespace Cubra
{
    public class Coin : ObjectsToCollect
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            character.SetSound(Character.Sounds.Coin);
            character.PlayingSound.PlaySound();

            Main.Instance.Coins++;

            InstanseObject.SetActive(false);
        }
    }
}