namespace _04
{
    // Задание 1
    // Создайте приложение, использующее механизм критических секций.
    // Создайте в коде приложения несколько потоков.
    // Первый поток получает в качестве аргумента путь к файлу,
    // считывает содержимое файла и подсчитывает количество предложений.
    // Второй поток ожидает, пока первый закончит свою работу и
    // модифицирует файл (производится замена всех ! на #). 

    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "file.txt";
            string startText = "My text. This text! Hello world!";

            // создание файла в главном потоке
            WriteToFile(path, startText);

            // таск для подсчета предложений в файле
            Task taskSentNum = Task.Run(() =>
            {
                Console.WriteLine("Sentences count: " + SentencesCount(path));
            });
            taskSentNum.Wait();

            // таск для замены символов в файле
            string modifiedText = "";
            Task taskModify = Task.Run(() =>
            {
                modifiedText = SymbolReplace(path);
            });
            taskModify.Wait();

            // таск для записи в файл обновленного текста
            Task taskRewrite = Task.Run(() =>
            {
                WriteToFile(path, modifiedText);
            });
            taskRewrite.Wait();
        }
        
        // запись в файл
        static void WriteToFile(string path, string text)
        {
            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using(StreamWriter sw =  new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }
        }

        // подсчет предложений
        static int SentencesCount(string path)
        {
            int count = 0;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string str = "";
                    str = sr.ReadToEnd();

                    foreach(var item in str)
                    {
                        if (item == '.' || item == '!' || item == '?')
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        // замена всех ! на #
        static string SymbolReplace(string path)
        {
            string str = "";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    str = sr.ReadToEnd();
                }
            }
            string s2 = str.Replace('!', '#');
            return s2;
        }
    }
}
