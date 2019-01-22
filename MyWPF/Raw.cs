using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MyWPF
{
    class Raw
    {
        private bool isSelected=true;
        private string drives;
        private string motors;
        private int qntDrives;
        private int qntMotors;
        private int idSetup;

        public Raw(bool selected, int id, string drives_sel, string motors_sel, int qnt_dr, int qnt_mt)
        {
            isSelected = selected;
            drives = drives_sel;
            motors = motors_sel;
            qntDrives = qnt_dr;
            qntMotors = qnt_mt;
            idSetup = id;
        }
        public Raw() { }

        public bool IsSelected {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }

        public int IdSetup
        {
            get
            {
                return idSetup;
            }
            set
            {
                idSetup = value;
            }
        }

        public string DriveSelected
        {
            get
            {
                return drives;
            }
            set
            {
                drives = value;
            }
        }

        public string MotorSelected
        {
            get
            {
                return motors;
            }
            set
            {
                motors = value;
            }
        }

        public int QntDriveSelected
        {
            get
            {
                return qntDrives;
            }
            set
            {
                qntDrives = value;
            }
        }

        public int QntMotorSelected
        {
            get
            {
                return qntMotors;
            }
            set
            {
                qntMotors = value;
            }
        }

        public static ObservableCollection<Raw> getRaw()
        {
            var raw = new ObservableCollection<Raw>();           
            raw.Add(new Raw() { IsSelected = true, IdSetup = 1, DriveSelected = "CDHD", MotorSelected = "PRO2", QntDriveSelected = 1, QntMotorSelected = 1});
            return raw;
        }

        

        

    }
}
