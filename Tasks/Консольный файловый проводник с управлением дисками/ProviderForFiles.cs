using System;
using System.IO;

class FileExplorer
{
    static string currentDirectory = Directory.GetCurrentDirectory();

    static void Main()
    {
        Console.WriteLine("Консольный файловый проводник");
        Console.WriteLine($"Текущая директория: {currentDirectory}\n");

        while (true)
        {
            DisplayMenu();
            ProcessCommand();
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n=== МЕНЮ ===");
        Console.WriteLine("1. Просмотр содержимого");
        Console.WriteLine("2. Открыть файл/каталог");
        Console.WriteLine("3. Создать каталог");
        Console.WriteLine("4. Создать файл");
        Console.WriteLine("5. Удалить файл/каталог");
        Console.WriteLine("6. Сменить директорию");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    static void ProcessCommand()
    {
        switch (Console.ReadLine())
        {
            case "1":
                ShowDirectoryContents();
                break;
            case "2":
                OpenFileOrDirectory();
                break;
            case "3":
                CreateDirectory();
                break;
            case "4":
                CreateFile();
                break;
            case "5":
                DeleteItem();
                break;
            case "6":
                ChangeDirectory();
                break;
            case "0":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Неверная команда!");
                break;
        }
    }

    static void ShowDirectoryContents()
    {
        Console.WriteLine($"\n📂 Содержимое {currentDirectory}:\n");

        try
        {
            //подкаталоги
            foreach (var dir in Directory.GetDirectories(currentDirectory))
            {
                Console.WriteLine($"[DIR]  {Path.GetFileName(dir)}");
            }

            //файлы
            foreach (var file in Directory.GetFiles(currentDirectory))
            {
                Console.WriteLine($"[FILE] {Path.GetFileName(file)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void OpenFileOrDirectory()
    {
        Console.Write("\nВведите имя файла/каталога: ");
        string name = Console.ReadLine();
        string path = Path.Combine(currentDirectory, name);

        if (Directory.Exists(path))
        {
            currentDirectory = path;
            Console.WriteLine($"Перешли в каталог: {currentDirectory}");
            ShowDirectoryContents();
        }
        else if (File.Exists(path))
        {
            try
            {
                Console.WriteLine($"\nСодержимое файла {name}:\n");
                Console.WriteLine(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Файл или каталог не найден!");
        }
    }

    static void CreateDirectory()
    {
        Console.Write("\nВведите имя нового каталога: ");
        string dirName = Console.ReadLine();
        string dirPath = Path.Combine(currentDirectory, dirName);

        try
        {
            Directory.CreateDirectory(dirPath);
            Console.WriteLine($"Каталог {dirName} создан");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CreateFile()
    {
        Console.Write("\nВведите имя файла: ");
        string fileName = Console.ReadLine();
        string filePath = Path.Combine(currentDirectory, fileName);

        Console.WriteLine("Введите текст (для завершения введите пустую строку):");
        string content = "";
        string line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            content += line + Environment.NewLine;
        }

        try
        {
            File.WriteAllText(filePath, content);
            Console.WriteLine($"Файл {fileName} создан");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void DeleteItem()
    {
        Console.Write("\nВведите имя файла/каталога: ");
        string name = Console.ReadLine();
        string path = Path.Combine(currentDirectory, name);

        Console.Write($"Вы уверены, что хотите удалить {name}? (y/n): ");
        if (Console.ReadLine().ToLower() != "y") return;

        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Console.WriteLine($"Каталог {name} удален");
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"Файл {name} удален");
            }
            else
            {
                Console.WriteLine("Файл или каталог не найден!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ChangeDirectory()
    {
        Console.Write("\nВведите путь к новой директории: ");
        string newDir = Console.ReadLine();

        if (Path.IsPathRooted(newDir))
        {
            if (Directory.Exists(newDir))
                currentDirectory = newDir;
            else
                Console.WriteLine("Директория не существует!");
        }
        else if (newDir == "..")
        {
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName ?? currentDirectory;
        }
        else
        {
            string path = Path.Combine(currentDirectory, newDir);
            if (Directory.Exists(path))
                currentDirectory = path;
            else
                Console.WriteLine("Директория не существует!");
        }

        Console.WriteLine($"Текущая директория: {currentDirectory}");
    }
}
