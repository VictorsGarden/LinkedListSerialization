using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class ListNode {
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand;
    public string Data;
    public int index;

    public override string ToString() {
        return "R" + Rand.index.ToString() + "." + Data + "|";
    }
}

