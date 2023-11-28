
class Program
{
    
    static List<char[,]> mazes = new List<char[,]>();
    static int currentMazeIndex = -1;

    static void Main()
    {
        while (true)
        {
            DisplayAvailableMazes();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wybierz labirynt");
            Console.WriteLine("2. Utwórz nowy labirynt");
            Console.WriteLine("3. Edytuj labirynt");
            Console.WriteLine("4. Zapisz labirynt do pliku");
            Console.WriteLine("5. Wczytaj labirynt z pliku");
            Console.WriteLine("0. Wyjście");

            int choice = GetMenuChoice();

            switch (choice)
            {
                case 1:
                    SelectMaze();
                    break;
                case 2:
                    CreateNewMaze();
                    break;
                case 3:
                    EditMaze();
                    break;
                case 4:
                    SaveMazeToFile();
                    break;
                case 5:
                    LoadMazeFromFile();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    static void DisplayAvailableMazes()
    {
        Console.WriteLine("Dostępne labirynty:");

        for (int i = 0; i < mazes.Count; i++)
        {
            Console.WriteLine($"Labirynt {i + 1} ({mazes[i].GetLength(0)} rzędy x {mazes[i].GetLength(1)} kolumny):");
            DisplayMazeGrid(mazes[i]);
            Console.WriteLine();
        }

    }

    static void DisplayMazeGrid(char[,] maze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Console.Write("* ");
            }
            Console.WriteLine();
        }
    }


    static void SelectMaze()
        {
            Console.WriteLine("Wybierz numer labiryntu:");

            for (int i = 0; i < mazes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Labirynt {i + 1}");
            }

            int choice = GetMenuChoice();
            if (choice > 0 && choice <= mazes.Count)
            {
                currentMazeIndex = choice - 1;
                Console.WriteLine($"Wybrano labirynt {choice}.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer labiryntu.");
            }
        }


        static void CreateNewMaze()
        {
            Console.WriteLine("Podaj liczbę wierszy labiryntu:");
            int rows = GetPositiveInteger();

            Console.WriteLine("Podaj liczbę kolumn labiryntu:");
            int columns = GetPositiveInteger();

            char[,] maze = new char[rows, columns];
            mazes.Add(maze);
            currentMazeIndex = mazes.Count - 1;

            InitializeMaze(maze);

            Console.WriteLine($"Utworzono nowy labirynt o rozmiarach {rows}x{columns}.");
        }

        static void InitializeMaze(char[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    maze[i, j] = '*';
                }
            }
        }

        static void EditMaze()
        {
            if (currentMazeIndex == -1)
            {
                Console.WriteLine("Nie wybrano labiryntu. Wybierz labirynt przed edycją.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                return;
            }

            char[,] maze = mazes[currentMazeIndex];

            Console.WriteLine("Podaj współrzędne (wiersz kolumna) komórki do edycji (np. 1 2):");
            int row = GetPositiveInteger() - 1;
            int col = GetPositiveInteger() - 1;

            if (row >= 0 && row < maze.GetLength(0) && col >= 0 && col < maze.GetLength(1))
            {
                Console.WriteLine("Wybierz stan komórki:");
                Console.WriteLine("1. Pusta");
                Console.WriteLine("2. Ściana");
                Console.WriteLine("3. Ścieżka");

                int stateChoice = GetMenuChoice();

                switch (stateChoice)
                {
                    case 1:
                        maze[row, col] = ' ';
                        break;
                    case 2:
                        maze[row, col] = '#';
                        break;
                    case 3:
                        maze[row, col] = '.';
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Komórka pozostaje bez zmian.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowe współrzędne komórki.");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        static void SaveMazeToFile()
        {
            if (currentMazeIndex == -1)
            {
                Console.WriteLine("Nie wybrano labiryntu. Wybierz labirynt przed zapisem.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                return;
            }

            char[,] maze = mazes[currentMazeIndex];

            Console.WriteLine("Podaj nazwę pliku do zapisu:");
            string fileName = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"{maze.GetLength(0)} {maze.GetLength(1)}");

                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.GetLength(1); j++)
                    {
                        writer.Write(maze[i, j]);
                    }
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"Labirynt został zapisany do pliku {fileName}.");
            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        static void LoadMazeFromFile()
        {
            Console.WriteLine("Podaj nazwę pliku do wczytania:");
            string fileName = Console.ReadLine();

            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string[] size = reader.ReadLine().Split(' ');
                    int rows = int.Parse(size[0]);
                    int columns = int.Parse(size[1]);

                    char[,] maze = new char[rows, columns];

                    for (int i = 0; i < rows; i++)
                    {
                        string line = reader.ReadLine();
                        for (int j = 0; j < columns; j++)
                        {
                            maze[i, j] = line[j];
                        }
                    }

                    mazes.Add(maze);
                    currentMazeIndex = mazes.Count - 1;

                    Console.WriteLine($"Labirynt został wczytany z pliku {fileName}.");
                }
            }
            else
            {
                Console.WriteLine($"Plik {fileName} nie istnieje.");
            }
        }

        static int GetMenuChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Niprawidłowe dane. Podaj liczbę.");
            }
            return choice;
        }

        static int GetPositiveInteger()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value <= 0)
            {
                Console.WriteLine("Nieprawidłowe dane. Podaj dodatią liczbę.");
            }
            return value;
        }
    }
