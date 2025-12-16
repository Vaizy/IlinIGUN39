using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
		[Serializable]
		public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

		[SerializeField, ReadOnly, Tooltip("Для заполнения этого поля нужно воспользоваться контекстным меню в инспекторе и командой “Create File”")]
		private TextAsset _json;

		[field: SerializeField, HideInInspector]
		public List<Data> Records { get; private set; }

		private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			//продолжится выполнение метода. на первой же инструкции выпадет ошибка, так как будет попытка обратится к свойству пустого объекта
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}

			JsonUtility.FromJsonOverwrite(_json.text, this);
			//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			//если массив Records пустой, то инициализирует его пустыми объектами
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
			//todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
			//предупреждает работу с пустым массивом. При инициализации переменной prev идет обращение к первому элементу массива,
			//если массив пустой или не инициализирован, то будет ошибка
			if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
			//todo comment: Почему итерация начинается не с нулевого элемента?
			//в 0 позиции мы уже находимся, нам надо обработать следующее положение кубика
			for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
			//todo comment: Что происходит в этой строке?
			//создание пустого файла Path.txt в корне проекта (папка Assets)
			var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
			//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав)
			//освобождение ресурсов. File.Create создал файл, но держит за собой. Dispose как бы закрывает файл, предоставляя возможноть им пользоваться другим
			//после проверки Unity ещё начал писать про бесконечный цикл, не совсем понял почему
			stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
				//todo comment: Для чего нужны эти проверки?
				//убедиться что файл загружен и что загружен именно нужный файл (с названием Path)
				if (asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
					//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
					return;
				}
			}
		}

		private void OnDestroy()
		{
			var text = JsonUtility.ToJson(this, true);
			//var text = JsonConvert.SerializeObject(Records);
			var path = UnityEditor.AssetDatabase.GetAssetPath(_json);
			File.WriteAllText(path, text);
			UnityEditor.EditorUtility.SetDirty(_json);
			UnityEditor.AssetDatabase.SaveAssets();
			UnityEditor.AssetDatabase.Refresh();
			
		}

    //    public class JsonVector3Converter : JsonConverter<Vector3>
    //    {
    //        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    //        {
				//var x = Convert.ToSingle(reader.ReadAsDouble());
				//var y = Convert.ToSingle(reader.ReadAsDouble());
				//var z = Convert.ToSingle(reader.ReadAsDouble());
				//return new Vector3(x, y, z);
    //        }

    //        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    //        {
				//writer.WriteValue(value);
    //        }
    //    }
#endif
	}
}