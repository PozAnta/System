using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWPF
{
    class Drive
    {
        public string nameDrive;
        public string voltDrive;
        public int qntDrive;
        public int ampDrive;

        public void setDrive(string name, string volt, int qnt, int amp)
        {
            nameDrive = name;
            voltDrive = volt;
            qntDrive = qnt;
            ampDrive = amp;
        }

        public string getDrive()
        {
            return nameDrive;
        }
        public int getDriveQnt()
        {
            return qntDrive;
        }



    }
}
