using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public class LinkedList
    {
        private node head;
        private int count;
        public LinkedList()
        {
            this.head = null;
            this.count = 0;
        }
        public bool Empty
        {
            get
            {
                return this.count == 0;
            }
        }
        public int Count
        {
            get
            {
                return this.count;
            }
        }
        public object add(int index, object o)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("Index:" + index);
            if (index > count)
                index = count;
            node current = this.head;
            if (this.Empty || index == 0)
            {

                this.head = new node(o, this.head);

            }
            else
            {
                for (int i = 0; i < index - 1; i++)
                    current = current.Next;
                current.Next = new node(o, current.Next);
            }
            count++;
            return 0;

        }
        public object add(object o)
        {
            return this.add(count, o);
        }
        public object Remove(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index" + index);
            if (this.Empty)
                return null;
            if (index > this.count)
                index = count - 1;
            node current = this.head;
            object result = null;
            if (index == 0)
            {
                result = current.Data;
                this.head = current.Next;

            }
            else
            {
                for (int i = 0; i < index - 1; i++)
                    current = current.Next;
                result = current.Next.Next;
            }
            count--;
            return result;
        }
        public object get(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("Index:" + index);
            if (this.Empty)
                return null;
            if (index >= count)
                index = this.count - 1;
            node current = this.head;
            for (int i = 0; i < index; i++)
                current = current.Next;
            return current.Data;
        }


    }
}

