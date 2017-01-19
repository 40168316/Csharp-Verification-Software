using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM2
{
    class message
    {
        private string universities;
        private string coursetype;
        private string subjects;
        private string originalmessage;

        public string Universities
        {
            get
            {
                return universities;
            }
            set
            {
                universities = value;
            }
        }

        public string Coursetype
        {
            get
            {
                return coursetype;
            }
            set
            {
                coursetype = value;
            }
        }

        public string Subjects
        {
            get
            {
                return subjects;
            }
            set
            {
                subjects = value;
            }
        }

        public string Originalmessage
        {
            get
            {
                return originalmessage;
            }
            set
            {
                originalmessage = value;
            }
        }
    }
}
