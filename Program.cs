using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program {
    static void Main(string[] args) {
        try {
            ListRand lr = new ListRand(100);
            ListNode listNode = lr.Get(99);
            ListNode listNodeRandom = listNode.Rand;

            // value for comparison
            Console.WriteLine(listNodeRandom.index);

            lr.Serialize(new FileStream("log.txt", FileMode.OpenOrCreate, FileAccess.Write));
            lr.Deserialize(new FileStream("log.txt", FileMode.Open, FileAccess.Read));

            listNode = lr.Get(99);
            listNodeRandom = listNode.Rand;

            // we should see the same value in output after deserialization
            Console.WriteLine(listNodeRandom.index);
            Console.ReadLine();

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
