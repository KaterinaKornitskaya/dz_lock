namespace dz_lock
{
    // Задание 3. Реализуйте метод который  уменьшит каждое
    // значение коллекции чисел на 1.
    // Выполните метод 3 раза в разных потоках. 

    internal class Program
    {
        static int[] arr = new int[] { 23, 26, 51, 15, 18 };

        static void Main(string[] args)
        {
            Console.WriteLine("Массив на старте:");
            foreach (var item in arr)
            {
                Console.Write(item + "\t");
            }
            
            // создаем массив потоков
            Thread[] threads = new Thread[3];

            // цикл по массиву потоков
            for(int i = 0; i < threads.Length; i++)
            {
                // создаем потоки с помощью анонимного делегата
                threads[i] = new Thread(delegate ()
                {
                    // цикл по массиву
                    for(int j = 0; j < arr.Length; j++)
                    {
                        Interlocked.Decrement(ref arr[j]);
                    }
                });
                threads[i].Start();
            }

            // в цикле по потокам ставим для каждого потока точку останова,
            // чтобы они успели завершится до вывода итогового массива в консоль
            for(int i = 0; i < threads.Length;i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("\n\nМассив после операций декремента:");
            foreach (var item in arr)
            {
                Console.Write(item + "\t");
            }
        }

        
    }
}
