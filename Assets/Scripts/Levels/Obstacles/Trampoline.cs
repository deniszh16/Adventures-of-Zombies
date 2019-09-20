using UnityEngine;

public class Trampoline : ReboundObject
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Пытаемся получить компонент физики у коснувшегося объекта
        Rigidbody2D rigbody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigbody)
        {
            // Сбрасываем скорость объекта
            rigbody.velocity *= 0;
            // Создаем импульсный отскок объекта
            rigbody.AddForce(rebound * force, ForceMode2D.Impulse);

            // Если звуки не отключены, проигрываем
            if (Options.sound) audioSource.Play();

            // Перезапускаем анимацию пружины
            animator.enabled = true;
            animator.Rebind();
        }
    }
}