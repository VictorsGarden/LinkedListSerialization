using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class ListRand {
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public ListRand(int Count) {
        this.Count = Count;

        if (Count > 0)
            Initialize();
        else
            throw new Exception();
    }

    public void Initialize() {
        string[] dataArray = new string[this.Count];

        for (int i = 0; i < this.Count; i++)
            dataArray[i] = "data N" + (i + 1);

        int[] randomIndexesArray = CreateRandomIndexes();
        CreateStructure(randomIndexesArray, dataArray);
    }

    public int[] CreateRandomIndexes() {
        int[] randomIndexesArray = new int[Count];
        Random randomiser = new Random();
        
        for (int i = 0; i < Count; i ++) {
            randomIndexesArray[i] = randomiser.Next(0, Count - 1);
        }

        return randomIndexesArray;
    }

    public void CreateStructure(int[] randomIndexesArray, string[] dataArray) {
        Head = new ListNode();

        if (dataArray.Length == 1) {
            Head.Prev = null;
            Head.Next = null;
            Head.index = 0;
            Head.Data = dataArray[0];
            Head.Rand = Head;
            Tail = Head;

        } else {
            ListNode current = Head;

            for (int i = 0; i < dataArray.Length; i++) {
                current.Data = dataArray[i];
                current.Next = new ListNode();
                current.index = i;

                if (i != this.Count - 1) {
                    current.Next.Prev = current;
                    current = current.Next;
                } else {
                    Tail = current;
                }
            }
        }

        ListNode currentForRandom = Head;

        for (int i = 0; i < dataArray.Length; i++) {
            currentForRandom.Rand = Get(randomIndexesArray[i]);

            if (i == dataArray.Length)
                break;

            currentForRandom = currentForRandom.Next;
        }
    }

    public ListNode Get(int elementIndex) {
        ListNode element = null;

        if (Count == 1) {
            element = Head;

        } else {
            element = Head;

            for (int i = 0; i <= elementIndex; i++) {

                if (element.index == elementIndex)
                    break;

                element = element.Next;
            }
        }

        return element;
    }

    public void Serialize(FileStream fs) {
        ListNode current = Head;
        
        do {
            if (current.Data != null) {
                char[] indexedDataCharArray = (current.ToString()).ToCharArray();
                byte[] dataByteArray = new byte[indexedDataCharArray.Length * sizeof(char)];
                Encoding.Unicode.GetEncoder().GetBytes(indexedDataCharArray, 0, indexedDataCharArray.Length, dataByteArray, 0, true);
                fs.Write(dataByteArray, 0, dataByteArray.Length);
            }

            if (current.Next != null)
                current = current.Next;

        } while (current.Next != null);

        fs.Dispose();
    }

    public void Deserialize(FileStream fs) {
        byte[] bytesData = new byte[fs.Length];
        int numBytesToRead = (int)fs.Length;
        int numBytesRead = 0;
        char[] charData = new char[fs.Length / sizeof(char)];

        while (numBytesToRead > 0) {
            int n = fs.Read(bytesData, numBytesRead, numBytesToRead);
            
            if (n == 0)
                break;

            numBytesRead += n;
            numBytesToRead -= n;
        }

        numBytesToRead = bytesData.Length;
        
        Encoding.Unicode.GetDecoder().GetChars(bytesData, 0, bytesData.Length, charData, 0);
        string stringRawData = new string(charData);
        string[] rawDataArray = stringRawData.Split('R', '.');
        rawDataArray = rawDataArray.Skip(1).ToArray();

        string randIndexes = null;
        string data = null;

        for (int i = 1; i <= rawDataArray.Length; i ++) {

            if (i % 2 != 0)
                randIndexes += rawDataArray[i - 1] + ".";
            else
                data += rawDataArray[i - 1];
        }

        string[] randomIndexesStringArray = randIndexes.Split('.');
        randomIndexesStringArray = randomIndexesStringArray.Take(randomIndexesStringArray.Count() - 1).ToArray();

        int[] randomIndexesArray = new int[randomIndexesStringArray.Length];

        for (int i = 0; i < randomIndexesStringArray.Length; i++) {
            randomIndexesArray[i] = int.Parse(randomIndexesStringArray[i]);
        }

        string[] dataArray = data.Split('|');
        dataArray = dataArray.Take(dataArray.Count() - 1).ToArray();

        CreateStructure(randomIndexesArray, dataArray);
    }
}