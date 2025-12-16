using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
        [SerializeField, ReadOnly]
        private PositionSaver _save;
        [SerializeField]
        private float _currentDelay;

		//todo comment: Что произойдёт, если _delay > _duration?
		//зафиксируется только исходная позиция. Вторая точка пути не будет создана, так как истечет общее время
		[SerializeField, Range(0.2f, 1.0f)]
		private float _delay = 0.5f;
		[SerializeField, Min(0.2f)]
		private float _duration = 5f;

		private void Start()
		{
			if (_duration < _delay) _duration = _delay * 5;
			//todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
			//Start запустится один раз, а Update запускается постоянно
			_save = GetComponent<PositionSaver>();
			_save.Records.Clear();
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}

            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
            //_delay это размер шага, если мы начнем его уменьшать, то дойдем до нуля секунд и Unity быдет пытаться создавать точку пути каждые 0 секунд
            //_currentDelay длительность текущего шага, когда она дойдет до нуля, будет зафиксирована точка пути, а переменная примет значение _delay
            _currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					//для последующей интерполяции
					Time = Time.time,
				});
			}
		}
	}
}