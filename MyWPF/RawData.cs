using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWPF
{
    public class RawData
    {
        public bool IsSelected { get; set; }
        public int IdSetup { get; set; }
        public List<string> DriveSelected { get; set; }
        public List<string> MotorSelected { get; set; }
        public List<string> MCSelected { get; set; }
        public List<string> GUISelected { get; set; }

        //public List<int> QntDriveSelected { get; set; }
        //public List<int> QntMotorSelected { get; set; }

        public int Id { get; set; }
        public string Drive { get; set; }
        public string Motor { get; set; }
        public string MC { get; set; }
        public string GUI { get; set; }

        public string StatusTest { get; set; }
        public string LastDate { get; set; }
        public string FwGUI { get; set; }
        public string FwDrive { get; set; }
        public string Report { get; set; }


        public bool IsSelectedSetup { get; set; }
        public List<string> TestSetup { get; set; }

        public string ShowSetup { get; set; }


    }
}
