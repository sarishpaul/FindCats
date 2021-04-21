using System;
using System.Collections;
using System.Collections.Generic;

namespace ListCats.Model
{
    public class Owner
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }
        public IEnumerable<Pet> Pets { get; set; }

    }
}
