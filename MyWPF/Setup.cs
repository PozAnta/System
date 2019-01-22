using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;



namespace MyWPF
{
    public class Setup
    {
        public static string setupId;
        public int numberOfDrives;
        public int numberOfMotors;
        public List<string> drive_setup = new List<string>();
        public List<string> motor_setup = new List<string>();
        public List<string> mc_setup = new List<string>();
        public List<string> gui_setup = new List<string>();
        public List<string> test_setup = new List<string>();

        public List<string> tets_run = new List<string>();
        public List<string> lastDateTest = new List<string>();
        public List<string> satusSetup = new List<string>();

        public Setup() { }
        public Setup(string id, string drive, string motor, int numdr, int nummot)
        {
            setupId = id;
            drive_setup.Add(drive);
            motor_setup.Add(motor);
            numberOfDrives = numdr;
            numberOfMotors = nummot;

        }

        public bool IsSelected { get; set; }
        public int IdSetup { get; set; }

        public int QntDrive { get; set; }
        public int QntMotor { get; set; }

        public List<string> getDriveName()
        {
            return (drive_setup);
        }                
        public void setDriveName(List<string> dr)
        {
            foreach(string key in dr)
            {
                drive_setup.Add(key);
            }
            
        }

        public List<string> getMotorName()
        {
            return (motor_setup);
        }        
        public void setMotorName(List<string> mt)
        {
            foreach (string key in mt)
            {
                motor_setup.Add(key);
            }

        }

        public List<string> getMcName()
        {
            return (mc_setup);
        }
        public void setMcName(List<string> mc)
        {
            foreach (string key in mc)
            {
                mc_setup.Add(key);
            }

        }
        public void setMcNameOnce(string mc)
        {
            mc_setup.Add(mc);
        }

        public List<string> getGuiName()
        {
            return (gui_setup);
        }
        public void setGuiName(List<string> gui)
        {
            foreach (string key in gui)
            {
                gui_setup.Add(key);
            }

        }

        public List<string> getTestName()
        {
            return (test_setup);
        }
        public void setTestName(List<string> test)
        {
            foreach (string key in test)
            {
               test_setup.Add(key);
            }

        }

        public void TestsRun(List<string> tst)
        {
            foreach (string key in tst)
            {
                tets_run.Add(key);
            }
        }

        public string Status { get; set; }
        public string LastDate { get; set; }
        public string FwGUI { get; set; }
        public string FwDrive { get; set; }

        public void SaveAsText()
        {
            Application application = new Application();
            Document document = application.Documents.Open("C:\\work\\Test\\MyWPF\\Run\\HomeRigidTest.docx");
            List<string> te = new List<string>();
            // Loop through all words in the document.
            int count = document.Words.Count;
            for (int i = 1; i <= count; i++)
            {
                // Write the word.
                string text = document.Words[i].Text;
                //Console.WriteLine("Word {0} = {1}", i, text);
                te.Add(text);
            }
            // Close word.
            application.Quit();
        }

    }

    public class WordDocRead
    {
        

    }
}
