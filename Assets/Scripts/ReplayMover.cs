using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			//todo comment: зачем нужны эти проверки?
			//чтобы не ловить ошибки с пустыми объектами 
			if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				//нечего отрисовывать
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
			//todo comment: Что проверяет это условие (с какой целью)?
			//
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
				//todo comment: Для чего нужна эта проверка?
				//проверка верхней границы массива
				if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
			//todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
			//текущее положение кубика между предыдущей точкой и следующей точкой, где 0 ещё в начале пути, 1 - конец пути 
			var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
			//todo comment: Зачем нужна эта проверка?
			//если значение времени не получилось высчитать, установить время равное 0  
			if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            //расчет линейной интерполяции. Задается условный отрезок между двумя позициями А и Б.
            //дельтой указывается текущее положение. Если дельта, например, 3/8, то текущее положение это сдвиг по отрезку на 3/8 
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}