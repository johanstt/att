using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoStudio
{
    /// <summary>
    /// Основная логика консольного приложения фотостудии: меню и управление сущностями.
    /// </summary>
    public class PhotoStudioConsoleApp
    {
        private readonly DataStorage _dataStorage = new DataStorage();
        private PhotoStudioData _data = new PhotoStudioData();

        // Для быстрого поиска по Id
        private readonly Dictionary<int, Client> _clientsById = new Dictionary<int, Client>();
        private readonly Dictionary<int, Photographer> _photographersById = new Dictionary<int, Photographer>();
        private readonly Dictionary<int, Equipment> _equipmentById = new Dictionary<int, Equipment>();
        private readonly Dictionary<int, PhotoSession> _sessionsById = new Dictionary<int, PhotoSession>();

        private const string DefaultFilePath = "data.json";

        public void Run()
        {
            RebuildDictionaries();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("=== Фотостудия: главное меню ===");
                Console.WriteLine("1. Клиенты");
                Console.WriteLine("2. Фотографы");
                Console.WriteLine("3. Оборудование");
                Console.WriteLine("4. Фотосессии");
                Console.WriteLine("5. Сохранить данные в файл");
                Console.WriteLine("6. Загрузить данные из файла");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ClientsMenu();
                        break;
                    case "2":
                        PhotographersMenu();
                        break;
                    case "3":
                        EquipmentMenu();
                        break;
                    case "4":
                        SessionsMenu();
                        break;
                    case "5":
                        SaveData();
                        break;
                    case "6":
                        LoadData();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine();
            }
        }

        #region Меню клиентов

        private void ClientsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Клиенты ===");
                Console.WriteLine("1. Добавить клиента");
                Console.WriteLine("2. Просмотреть всех клиентов");
                Console.WriteLine("3. Найти клиента по Id");
                Console.WriteLine("4. Найти клиента по имени");
                Console.WriteLine("5. Удалить клиента по Id");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddClient();
                        break;
                    case "2":
                        ListClients();
                        break;
                    case "3":
                        FindClientById();
                        break;
                    case "4":
                        FindClientByName();
                        break;
                    case "5":
                        DeleteClientById();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void AddClient()
        {
            Console.WriteLine("=== Добавление клиента ===");
            int id = ReadInt("Введите Id: ");
            if (_clientsById.ContainsKey(id))
            {
                Console.WriteLine("Клиент с таким Id уже существует.");
                return;
            }

            Console.Write("Введите имя: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите email: ");
            string email = Console.ReadLine() ?? string.Empty;

            int loyalty = ReadInt("Введите уровень лояльности (целое число): ");

            Console.Write("Введите примечание: ");
            string notes = Console.ReadLine() ?? string.Empty;

            try
            {
                var client = new Client(id, name, phone, email, loyalty, notes);
                _data.Clients.Add(client);
                _clientsById[id] = client;
                Console.WriteLine("Клиент добавлен.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка создания клиента: " + ex.Message);
            }
        }

        private void ListClients()
        {
            Console.WriteLine("=== Список клиентов ===");
            if (_data.Clients.Count == 0)
            {
                Console.WriteLine("Клиенты отсутствуют.");
                return;
            }

            foreach (var c in _data.Clients)
            {
                Console.WriteLine(c);
            }
        }

        private void FindClientById()
        {
            int id = ReadInt("Введите Id клиента: ");
            if (_clientsById.TryGetValue(id, out var client))
            {
                Console.WriteLine(client);
            }
            else
            {
                Console.WriteLine("Клиент с таким Id не найден.");
            }
        }

        private void FindClientByName()
        {
            Console.Write("Введите имя (или его часть): ");
            string namePart = Console.ReadLine() ?? string.Empty;
            var matches = _data.Clients
                .Where(c => c.Name.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("Клиенты не найдены.");
            }
            else
            {
                foreach (var c in matches)
                {
                    Console.WriteLine(c);
                }
            }
        }

        private void DeleteClientById()
        {
            int id = ReadInt("Введите Id клиента для удаления: ");
            if (_clientsById.TryGetValue(id, out var client))
            {
                bool hasSessions = _data.Sessions.Any(s => s.Client.Id == id);
                if (hasSessions)
                {
                    Console.WriteLine("Невозможно удалить клиента: существуют фотосессии с этим клиентом.");
                    return;
                }

                _data.Clients.Remove(client);
                _clientsById.Remove(id);
                Console.WriteLine("Клиент удалён.");
            }
            else
            {
                Console.WriteLine("Клиент с таким Id не найден.");
            }
        }

        #endregion

        #region Меню фотографов

        private void PhotographersMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Фотографы ===");
                Console.WriteLine("1. Добавить фотографа");
                Console.WriteLine("2. Просмотреть всех фотографов");
                Console.WriteLine("3. Найти фотографа по Id");
                Console.WriteLine("4. Найти фотографа по имени");
                Console.WriteLine("5. Удалить фотографа по Id");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddPhotographer();
                        break;
                    case "2":
                        ListPhotographers();
                        break;
                    case "3":
                        FindPhotographerById();
                        break;
                    case "4":
                        FindPhotographerByName();
                        break;
                    case "5":
                        DeletePhotographerById();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void AddPhotographer()
        {
            Console.WriteLine("=== Добавление фотографа ===");
            int id = ReadInt("Введите Id: ");
            if (_photographersById.ContainsKey(id))
            {
                Console.WriteLine("Фотограф с таким Id уже существует.");
                return;
            }

            Console.Write("Введите имя: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите email: ");
            string email = Console.ReadLine() ?? string.Empty;

            int experience = ReadInt("Введите стаж (в годах): ");

            Console.Write("Введите специализацию: ");
            string spec = Console.ReadLine() ?? string.Empty;

            decimal rate = ReadDecimal("Введите почасовую ставку: ");

            try
            {
                var photographer = new Photographer(id, name, phone, email, experience, spec, rate);
                _data.Photographers.Add(photographer);
                _photographersById[id] = photographer;
                Console.WriteLine("Фотограф добавлен.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка создания фотографа: " + ex.Message);
            }
        }

        private void ListPhotographers()
        {
            Console.WriteLine("=== Список фотографов ===");
            if (_data.Photographers.Count == 0)
            {
                Console.WriteLine("Фотографы отсутствуют.");
                return;
            }

            foreach (var p in _data.Photographers)
            {
                Console.WriteLine(p);
            }
        }

        private void FindPhotographerById()
        {
            int id = ReadInt("Введите Id фотографа: ");
            if (_photographersById.TryGetValue(id, out var photographer))
            {
                Console.WriteLine(photographer);
            }
            else
            {
                Console.WriteLine("Фотограф с таким Id не найден.");
            }
        }

        private void FindPhotographerByName()
        {
            Console.Write("Введите имя (или его часть): ");
            string namePart = Console.ReadLine() ?? string.Empty;
            var matches = _data.Photographers
                .Where(p => p.Name.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("Фотографы не найдены.");
            }
            else
            {
                foreach (var p in matches)
                {
                    Console.WriteLine(p);
                }
            }
        }

        private void DeletePhotographerById()
        {
            int id = ReadInt("Введите Id фотографа для удаления: ");
            if (_photographersById.TryGetValue(id, out var photographer))
            {
                bool hasSessions = _data.Sessions.Any(s => s.Photographer.Id == id);
                if (hasSessions)
                {
                    Console.WriteLine("Невозможно удалить фотографа: существуют фотосессии с этим фотографом.");
                    return;
                }

                _data.Photographers.Remove(photographer);
                _photographersById.Remove(id);
                Console.WriteLine("Фотограф удалён.");
            }
            else
            {
                Console.WriteLine("Фотограф с таким Id не найден.");
            }
        }

        #endregion

        #region Меню оборудования

        private void EquipmentMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Оборудование ===");
                Console.WriteLine("1. Добавить оборудование");
                Console.WriteLine("2. Просмотреть всё оборудование");
                Console.WriteLine("3. Найти оборудование по Id");
                Console.WriteLine("4. Найти оборудование по названию");
                Console.WriteLine("5. Удалить оборудование по Id");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddEquipment();
                        break;
                    case "2":
                        ListEquipment();
                        break;
                    case "3":
                        FindEquipmentById();
                        break;
                    case "4":
                        FindEquipmentByName();
                        break;
                    case "5":
                        DeleteEquipmentById();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void AddEquipment()
        {
            Console.WriteLine("=== Добавление оборудования ===");
            int id = ReadInt("Введите Id: ");
            if (_equipmentById.ContainsKey(id))
            {
                Console.WriteLine("Оборудование с таким Id уже существует.");
                return;
            }

            Console.Write("Введите название: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите тип (камера, объектив и т.п.): ");
            string type = Console.ReadLine() ?? string.Empty;

            Console.Write("Оборудование доступно? (y/n): ");
            bool isAvailable = (Console.ReadLine() ?? "").Trim().ToLower() == "y";

            decimal price = ReadDecimal("Введите цену аренды за час: ");

            try
            {
                var eq = new Equipment(id, name, type, isAvailable, price);
                _data.EquipmentList.Add(eq);
                _equipmentById[id] = eq;
                Console.WriteLine("Оборудование добавлено.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка создания оборудования: " + ex.Message);
            }
        }

        private void ListEquipment()
        {
            Console.WriteLine("=== Список оборудования ===");
            if (_data.EquipmentList.Count == 0)
            {
                Console.WriteLine("Оборудование отсутствует.");
                return;
            }

            foreach (var e in _data.EquipmentList)
            {
                Console.WriteLine(e);
            }
        }

        private void FindEquipmentById()
        {
            int id = ReadInt("Введите Id оборудования: ");
            if (_equipmentById.TryGetValue(id, out var eq))
            {
                Console.WriteLine(eq);
            }
            else
            {
                Console.WriteLine("Оборудование с таким Id не найдено.");
            }
        }

        private void FindEquipmentByName()
        {
            Console.Write("Введите название (или его часть): ");
            string namePart = Console.ReadLine() ?? string.Empty;
            var matches = _data.EquipmentList
                .Where(e => e.Name.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("Оборудование не найдено.");
            }
            else
            {
                foreach (var e in matches)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void DeleteEquipmentById()
        {
            int id = ReadInt("Введите Id оборудования для удаления: ");
            if (_equipmentById.TryGetValue(id, out var eq))
            {
                bool usedInSessions = _data.Sessions.Any(s => s.UsedEquipment.Any(u => u.Id == id));
                if (usedInSessions)
                {
                    Console.WriteLine("Невозможно удалить оборудование: оно используется в существующих фотосессиях.");
                    return;
                }

                _data.EquipmentList.Remove(eq);
                _equipmentById.Remove(id);
                Console.WriteLine("Оборудование удалено.");
            }
            else
            {
                Console.WriteLine("Оборудование с таким Id не найдено.");
            }
        }

        #endregion

        #region Меню фотосессий

        private void SessionsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Фотосессии ===");
                Console.WriteLine("1. Создать фотосессию");
                Console.WriteLine("2. Просмотреть все фотосессии");
                Console.WriteLine("3. Найти фотосессию по Id");
                Console.WriteLine("4. Удалить фотосессию по Id");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreateSession();
                        break;
                    case "2":
                        ListSessions();
                        break;
                    case "3":
                        FindSessionById();
                        break;
                    case "4":
                        DeleteSessionById();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void CreateSession()
        {
            Console.WriteLine("=== Создание фотосессии ===");
            int id = ReadInt("Введите Id фотосессии: ");
            if (_sessionsById.ContainsKey(id))
            {
                Console.WriteLine("Фотосессия с таким Id уже существует.");
                return;
            }

            if (_data.Clients.Count == 0 || _data.Photographers.Count == 0)
            {
                Console.WriteLine("Для создания фотосессии необходимо иметь хотя бы одного клиента и одного фотографа.");
                return;
            }

            Console.WriteLine("Выберите клиента:");
            ListClients();
            int clientId = ReadInt("Введите Id клиента: ");
            if (!_clientsById.TryGetValue(clientId, out var client))
            {
                Console.WriteLine("Клиент с таким Id не найден.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Выберите фотографа:");
            ListPhotographers();
            int photographerId = ReadInt("Введите Id фотографа: ");
            if (!_photographersById.TryGetValue(photographerId, out var photographer))
            {
                Console.WriteLine("Фотограф с таким Id не найден.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Выберите оборудование (через запятую Id, можно оставить пустым):");
            ListEquipment();
            Console.Write("Введите Id оборудования через запятую (или просто Enter, если не нужно): ");
            string eqInput = Console.ReadLine() ?? string.Empty;
            var usedEquipment = new List<Equipment>();

            if (!string.IsNullOrWhiteSpace(eqInput))
            {
                string[] parts = eqInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    try
                    {
                        int eqId = int.Parse(part.Trim());
                        if (_equipmentById.TryGetValue(eqId, out var eq))
                        {
                            usedEquipment.Add(eq);
                        }
                        else
                        {
                            Console.WriteLine($"Оборудование с Id={eqId} не найдено и не будет добавлено.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Некорректный Id оборудования: '{part}'. Пропуск.");
                    }
                }
            }

            DateTime date = ReadDateTime("Введите дату и время (например, 2025-11-18 15:30): ");
            int duration = ReadInt("Введите длительность в минутах: ");

            Console.Write("Введите место проведения: ");
            string location = Console.ReadLine() ?? string.Empty;

            decimal hours = duration / 60m;
            decimal photographerCost = photographer.RatePerHour * hours;
            decimal equipmentCost = usedEquipment.Sum(e => e.PricePerHour * hours);
            decimal totalPrice = photographerCost + equipmentCost;

            Console.WriteLine();
            Console.WriteLine($"Расчётная стоимость фотосессии: {totalPrice:C}");
            Console.Write("Подтвердить создание фотосессии? (y/n): ");
            string confirm = (Console.ReadLine() ?? "").Trim().ToLower();
            if (confirm != "y")
            {
                Console.WriteLine("Создание фотосессии отменено.");
                return;
            }

            try
            {
                var session = new PhotoSession(id, client, photographer, usedEquipment,
                    date, duration, location, totalPrice);

                _data.Sessions.Add(session);
                _sessionsById[id] = session;
                Console.WriteLine("Фотосессия создана.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка создания фотосессии: " + ex.Message);
            }
        }

        private void ListSessions()
        {
            Console.WriteLine("=== Список фотосессий ===");
            if (_data.Sessions.Count == 0)
            {
                Console.WriteLine("Фотосессии отсутствуют.");
                return;
            }

            foreach (var s in _data.Sessions)
            {
                Console.WriteLine(s);
            }
        }

        private void FindSessionById()
        {
            int id = ReadInt("Введите Id фотосессии: ");
            if (_sessionsById.TryGetValue(id, out var session))
            {
                Console.WriteLine(session);
            }
            else
            {
                Console.WriteLine("Фотосессия с таким Id не найдена.");
            }
        }

        private void DeleteSessionById()
        {
            int id = ReadInt("Введите Id фотосессии для удаления: ");
            if (_sessionsById.TryGetValue(id, out var session))
            {
                _data.Sessions.Remove(session);
                _sessionsById.Remove(id);
                Console.WriteLine("Фотосессия удалена.");
            }
            else
            {
                Console.WriteLine("Фотосессия с таким Id не найдена.");
            }
        }

        #endregion

        #region Сохранение и загрузка

        private void SaveData()
        {
            Console.Write($"Введите путь к файлу (Enter для значения по умолчанию '{DefaultFilePath}'): ");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = DefaultFilePath;
            }

            _dataStorage.SaveAll(path, _data);
        }

        private void LoadData()
        {
            Console.Write($"Введите путь к файлу (Enter для значения по умолчанию '{DefaultFilePath}'): ");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = DefaultFilePath;
            }

            _data = _dataStorage.LoadAll(path);
            RebuildDictionaries();
        }

        private void RebuildDictionaries()
        {
            _clientsById.Clear();
            _photographersById.Clear();
            _equipmentById.Clear();
            _sessionsById.Clear();

            foreach (var c in _data.Clients)
                _clientsById[c.Id] = c;

            foreach (var p in _data.Photographers)
                _photographersById[p.Id] = p;

            foreach (var e in _data.EquipmentList)
                _equipmentById[e.Id] = e;

            foreach (var s in _data.Sessions)
                _sessionsById[s.Id] = s;
        }

        #endregion

        #region Ввод с обработкой ошибок

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    int value = int.Parse(input);
                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некорректный формат числа. Попробуйте ещё раз.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Слишком большое или маленькое число. Попробуйте ещё раз.");
                }
            }
        }

        private decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    decimal value = decimal.Parse(input);
                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некорректный формат числа. Попробуйте ещё раз.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Слишком большое или маленькое число. Попробуйте ещё раз.");
                }
            }
        }

        private DateTime ReadDateTime(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    DateTime value = DateTime.Parse(input);
                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некорректный формат даты/времени. Попробуйте ещё раз.");
                }
            }
        }

        #endregion
    }
}


