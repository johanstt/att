using System;
using System.IO;
using System.Text.Json;

namespace PhotoStudio
{
    /// <summary>
    /// Класс для сохранения и загрузки данных фотостудии (JSON-сериализация).
    /// </summary>
    public class DataStorage
    {
        private readonly JsonSerializerOptions _options;

        public DataStorage()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        /// <summary>
        /// Сохранить все данные в файл.
        /// </summary>
        public void SaveAll(string filePath, PhotoStudioData data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data, _options);
                File.WriteAllText(filePath, json);
                Console.WriteLine("Данные успешно сохранены в файл.");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ошибка работы с файлом при сохранении: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Недостаточно прав для записи файла: " + ex.Message);
            }
            finally
            {
                // Здесь можно добавить логирование или другую очистку ресурсов при необходимости.
            }
        }

        /// <summary>
        /// Загрузить все данные из файла.
        /// </summary>
        public PhotoStudioData LoadAll(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл с данными не найден. Будет создан новый набор данных.");
                    return new PhotoStudioData();
                }

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<PhotoStudioData>(json, _options);
                if (data == null)
                {
                    Console.WriteLine("Не удалось прочитать данные. Создан новый пустой набор.");
                    return new PhotoStudioData();
                }

                Console.WriteLine("Данные успешно загружены.");
                return data;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Ошибка формата файла данных (JSON): " + ex.Message);
                return new PhotoStudioData();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ошибка работы с файлом при загрузке: " + ex.Message);
                return new PhotoStudioData();
            }
            finally
            {
                // При необходимости: закрытие потоков, логирование и т.п.
            }
        }
    }
}

