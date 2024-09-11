class Program
{
    static object consoleLock = new object();
    static bool exitRequested = false;

    static void Main(string[] args)
    {
        List<string> maleNames = new List<string>
        {
            "Alexander",
            "Michael",
            "John",
            "William",
            "James",
            "Robert",
            "Richard",
            "Charles",
            "Thomas",
            "Benjamin",
            "Ivan",
            "Sergey",
            "Andrey",
            "Vladimir",
            "Nikolay",
            "Atticus",
            "Jasper",
            "Kai",
            "Sage",
            "Rowan"
        };

        List<char> searchList = new List<char>();
        Thread searchThread = new Thread(() => SearchAndPrintResults(maleNames, searchList));
        searchThread.Start();

        while (true)
        {
            Console.Write("Enter your search query: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                exitRequested = true;
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (searchList.Count > 0)
                {
                    searchList.RemoveAt(searchList.Count - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                searchList.Add(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }
        Console.Clear();
        searchThread.Join(); 
    }

    static void SearchAndPrintResults(List<string> strings, List<char> search)
    {
        while (!exitRequested)
        {
            string searchString = new string(search.ToArray());
            List<string> result = SearchStrings(strings, searchString);

            lock (consoleLock)
            {
                Console.SetCursorPosition(2, 0);
                Console.WriteLine("Результат поиска: ");
                foreach (string s in result)
                {
                    Console.WriteLine(s);
                }
            }

            Thread.Sleep(100);
        }
    }

    static List<string> SearchStrings(List<string> strings, string search)
    {
        return strings.Where(s => s.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }
}