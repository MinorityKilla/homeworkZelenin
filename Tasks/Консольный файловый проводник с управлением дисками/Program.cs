using System;
using System.IO;
using System.Linq;

class FileExplorer
{
    private static string currentDirectory;
    private static DriveInfo[] allDrives;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        allDrives = DriveInfo.GetDrives();
        currentDirectory = Directory.GetCurrentDirectory();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== КОНСОЛЬНЫЙ ФАЙЛОВЫЙ ПРОВОДНИК ===");
            Console.WriteLine("1. Работа с текущим каталогом");
            Console.WriteLine("2. Управление дисками");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    WorkWithDirectory(currentDirectory);
                    break;
                case "2":
                    ManageDrives();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void WorkWithDirectory(string directory)
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine($"Текущий каталог: {directory}");
            Console.WriteLine("====================================");

           
            var directories = Directory.GetDirectories(directory);
            Console.WriteLine("\n[Каталоги]");
            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
            }

            
            var files = Directory.GetFiles(directory);
            Console.WriteLine("\n[Файлы]");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1 + directories.Length}. {Path.GetFileName(files[i])}");
            }

            
            Console.WriteLine("\nДействия:");
            Console.WriteLine("0. Назад");
            Console.WriteLine("n. Войти в каталог (введите номер каталога)");
            Console.WriteLine("f. Открыть файл (введите номер файла)");
            Console.WriteLine("c. Создать каталог");
            Console.WriteLine("t. Создать текстовый файл");
            Console.WriteLine("d. Удалить файл/каталог");
            Console.WriteLine("i. Информация о текущем каталоге");
            Console.WriteLine("q. Вернуться в главное меню");
            Console.Write("Выберите действие: ");

            string input = Console.ReadLine();
            if (input == "0")
            {
                
                if (Directory.GetParent(directory) != null)
                {
                    directory = Directory.GetParent(directory).FullName;
                }
                else
                {
                    back = true;
                }
            }
            else if (input == "q")
            {
                back = true;
            }
            else if (input == "c")
            {
                CreateDirectory(directory);
            }
            else if (input == "t")
            {
                CreateTextFile(directory);
            }
            else if (input == "d")
            {
                DeleteItem(directory, directories, files);
            }
            else if (input == "i")
            {
                ShowDirectoryInfo(directory);
            }
            else if (int.TryParse(input, out int selectedIndex))
            {
                if (selectedIndex > 0 && selectedIndex <= directories.Length + files.Length)
                {
                    if (selectedIndex <= directories.Length)
                    {
                       
                        directory = directories[selectedIndex - 1];
                    }
                    else
                    {
                       
                        OpenFile(files[selectedIndex - 1 - directories.Length]);
                    }
                }
                else
                {
                    Console.WriteLine("Неверный номер. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
            else if (input.StartsWith("f") && int.TryParse(input.Substring(1), out int fileIndex))
            {
                if (fileIndex > 0 && fileIndex <= files.Length)
                {
                    OpenFile(files[fileIndex - 1]);
                }
                else
                {
                    Console.WriteLine("Неверный номер файла. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    static void ManageDrives()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ ДИСКАМИ ===");
            Console.WriteLine("\nДоступные диски:");

            for (int i = 0; i < allDrives.Length; i++)
            {
                var drive = allDrives[i];
                if (drive.IsReady)
                {
                    Console.WriteLine($"{i + 1}. {drive.Name} ({drive.DriveType}) - " +
                        $"Свободно {drive.AvailableFreeSpace / (1024 * 1024 * 1024)}GB из {drive.TotalSize / (1024 * 1024 * 1024)}GB");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {drive.Name} ({drive.DriveType}) - не готов");
                }
            }

            Console.WriteLine("\nДействия:");
            Console.WriteLine("0. Назад");
            Console.WriteLine("n. Выбрать диск (введите номер)");
            Console.WriteLine("i. Подробная информация о диске");
            Console.Write("Выберите действие: ");

            string input = Console.ReadLine();
            if (input == "0")
            {
                back = true;
            }
            else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= allDrives.Length)
            {
                var selectedDrive = allDrives[selectedIndex - 1];
                if (selectedDrive.IsReady)
                {
                    WorkWithDirectory(selectedDrive.RootDirectory.FullName);
                }
                else
                {
                    Console.WriteLine("Диск не готов. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
            else if (input.StartsWith("i") && int.TryParse(input.Substring(1), out int infoIndex) &&
                     infoIndex > 0 && infoIndex <= allDrives.Length)
            {
                ShowDriveInfo(allDrives[infoIndex - 1]);
            }
            else
            {
                Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    static void ShowDriveInfo(DriveInfo drive)
    {
        Console.Clear();
        Console.WriteLine($"=== ИНФОРМАЦИЯ О ДИСКЕ {drive.Name} ===");

        if (drive.IsReady)
        {
            Console.WriteLine($"Тип: {drive.DriveType}");
            Console.WriteLine($"Файловая система: {drive.DriveFormat}");
            Console.WriteLine($"Общий размер: {drive.TotalSize / (1024 * 1024 * 1024)} GB");
            Console.WriteLine($"Свободное место: {drive.TotalFreeSpace / (1024 * 1024 * 1024)} GB");
            Console.WriteLine($"Доступное место: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} GB");
            Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
        }
        else
        {
            Console.WriteLine("Диск не готов.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowDirectoryInfo(string directory)
    {
        Console.Clear();
        Console.WriteLine($"=== ИНФОРМАЦИЯ О КАТАЛОГЕ {directory} ===");

        var dirInfo = new DirectoryInfo(directory);
        Console.WriteLine($"Дата создания: {dirInfo.CreationTime}");
        Console.WriteLine($"Дата последнего изменения: {dirInfo.LastWriteTime}");
        Console.WriteLine($"Атрибуты: {dirInfo.Attributes}");

        try
        {
            Console.WriteLine($"\nСодержит каталогов: {Directory.GetDirectories(directory).Length}");
            Console.WriteLine($"Содержит файлов: {Directory.GetFiles(directory).Length}");

            long totalSize = Directory.GetFiles(directory).Sum(file => new FileInfo(file).Length);
            Console.WriteLine($"Общий размер файлов: {totalSize / 1024} KB");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа к некоторым файлам/каталогам.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void CreateDirectory(string currentDir)
    {
        Console.Write("Введите имя нового каталога: ");
        string dirName = Console.ReadLine();

        try
        {
            string newDir = Path.Combine(currentDir, dirName);
            Directory.CreateDirectory(newDir);
            Console.WriteLine($"Каталог '{dirName}' создан. Нажмите любую клавишу для продолжения...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Нажмите любую клавишу для продолжения...");
        }
        Console.ReadKey();
    }

    static void CreateTextFile(string currentDir)
    {
        Console.Write("Введите имя нового файла (с расширением .txt): ");
        string fileName = Console.ReadLine();

        if (!fileName.EndsWith(".txt"))
        {
            fileName += ".txt";
        }

        Console.WriteLine("Введите содержимое файла (для завершения ввода введите 'end' на новой строке):");
        string content = "";
        string line;
        while ((line = Console.ReadLine()) != "end")
        {
            content += line + Environment.NewLine;
        }

        try
        {
            string newFile = Path.Combine(currentDir, fileName);
            File.WriteAllText(newFile, content);
            Console.WriteLine($"Файл '{fileName}' создан. Нажмите любую клавишу для продолжения...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Нажмите любую клавишу для продолжения...");
        }
        Console.ReadKey();
    }

    static void OpenFile(string filePath)
    {
        Console.Clear();
        Console.WriteLine($"=== СОДЕРЖИМОЕ ФАЙЛА {Path.GetFileName(filePath)} ===");

        try
        {
            if (Path.GetExtension(filePath).ToLower() == ".txt")
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("Этот тип файла не может быть просмотрен в программе.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void DeleteItem(string currentDir, string[] directories, string[] files)
    {
        Console.Write("Введите номер элемента для удаления (или 0 для отмены): ");
        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex != 0)
        {
            try
            {
                if (itemIndex > 0 && itemIndex <= directories.Length + files.Length)
                {
                    string path;
                    bool isDirectory;

                    if (itemIndex <= directories.Length)
                    {
                        path = directories[itemIndex - 1];
                        isDirectory = true;
                    }
                    else
                    {
                        path = files[itemIndex - 1 - directories.Length];
                        isDirectory = false;
                    }

                    Console.Write($"Вы уверены, что хотите удалить '{Path.GetFileName(path)}'? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        if (isDirectory)
                        {
                            Directory.Delete(path, true);
                            Console.WriteLine("Каталог удалён.");
                        }
                        else
                        {
                            File.Delete(path);
                            Console.WriteLine("Файл удалён.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Неверный номер элемента.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }

        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}