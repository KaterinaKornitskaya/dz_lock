namespace _03
{
    // Задание 2
    // Создайте приложение, использующее механизм критических секций.
    // Создайте в коде приложения несколько задач (Task).
    // Первая задача получает в качестве аргумента массив данных
    // и сортирует массив по возрастанию.
    // Вторая задача ожидает, пока первый закончит свою работу и
    // проверяет есть ли некоторое число в отсортированном массиве.
    // Число передаётся внутрь потоковой функции в качестве параметра 

    internal class Program
    {
        static int[] arr = new int[] { 1, 2, 4, 6, 17, 18, 2, 35, 9 };
        static bool flag = false;  // флаг для Таск2, если найдено искомое число - тру
        static void Main(string[] args)
        {
            // массив на старте программы
            Console.WriteLine("Array at start:");
            foreach (int i in arr)
            {
                Console.Write(i + "\t");
            }

            Job job = new Job();

            // Таск1, здесь вызываем метод сортировки,
            // и здесь же создаем Таск2, чтобы он был дочерним для Таск1,
            // и он мог дождаться выполнения Таск1
            Task task1 = Task.Run(() =>
            {
                job.SortArr(arr);
                
                // создание Таск2
                Task task2 = Task.Run(() =>
                {
                    job.SearchNumber(arr, 4, ref flag);
                });
                // Таск1 ждет завершения Таск2
                task2.Wait();
            });

            // главный поток ждет завершения Таск1
            task1.Wait();

            // массив на финише программы
            Console.WriteLine("\nArray at the end:");
            foreach (int i in arr)
            {
                Console.Write(i + "\t");
            }

            if (flag)
                Console.WriteLine("\nIs this number in array? - true");
            else
                Console.WriteLine("\nIs this number in array? - false");
        }

        // класс, где описаны методы работы с массивом
        public class Job
        {
            public int[] arr;

            public Job()
            {
                arr = new int[] { 1, 2, 4, 6, 17, 18, 2, 35, 9 };
            }

            // сортировка массива
            public void SortArr(int[] arr)
            {
                lock(arr)
                {
                    Array.Sort(arr);
                }
            }

            // поиск искомого числа в массиве
            public bool SearchNumber(int[] arr, int key, ref bool flag)
            {
                lock(arr)
                {
                    for (int i = 0; i< arr.Length; i++)
                    {
                        if (arr[i] == key)
                        {
                            flag = true;
                            return true;
                        }
                    }
                    return false;
                }
            }

        }
    }
}
