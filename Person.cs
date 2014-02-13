using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Address_Book
{
    [Serializable()]
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }

    
        // Methods perform operations on the data
        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName;
        }
  }


    [Serializable()]
    public class MyList
    {
       public ArrayList array = new ArrayList(); // here will be stored the MyObject objects
    }


}
