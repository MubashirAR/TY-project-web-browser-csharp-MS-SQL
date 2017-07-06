using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public class node
    {
        private object data;
        private node next;
        public node(object data, node next)
        {
            this.data = data;
            this.next = next;

        }
        public object Data
        {
            get { return this.data; }
            set { this.data = value; }

        }
        public node Next
        {
            get { return this.next; }
            set { this.next = value; }
        }

    }
}
