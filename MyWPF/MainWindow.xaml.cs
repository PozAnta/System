using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;






namespace MyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
        
    public partial class MainWindow : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }

        public static string database = "C:\\work\\Test\\MyWPF\\Run\\Data.db";
        public static int id = 0;
        public static int last_num_IdSetup;
        public static int numOfSetups = 0;
        public static int flag_double_using = 0;
        public static int number_of_tests = 1;
        static string path_report = "C:\\work\\Test\\MyWPF\\Run";
         

        List<string> insert_id = new List<string>();
        List<string> insert_name_drives = new List<string>();
        List<string> insert_name_motors = new List<string>();
        List<string> insert_name_mcs = new List<string>();
        List<string> insert_name_guis = new List<string>();
        //List<int> insert_qnt_drives = new List<int>();
        //List<int> insert_qnt_motors = new List<int>();
        List<string> test = new List<string>();
        List<string> testId = new List<string>();
        //List<string> selected_test = new List<string>();

        public List<string> new_list_drives = new List<string>();
        public List<string> new_list_motors = new List<string>();
        public List<string> new_list_mcs = new List<string>();
        public List<string> new_list_guis = new List<string>();

        //public List<int> new_list_qnt_drives = new List<int>();
        //public List<int> new_list_qnt_motors = new List<int>();
        public List<int> id_setups_db = new List<int>();
        public List<Test> selected_tests = new List<Test>();

        List<int> qntDrives = new List<int>();

        Test startTest = new Test();
        SQLdata sQ = new SQLdata();

        //TODO: number of raws should be dynamic
        //RawData[] raw = new RawData[16];
        RawData[] raw1 = new RawData[10];
        Test[] sel_test = new Test[number_of_tests];

        NativeMethods.OnMessage callback;
        IntPtr kmapi;
        bool propmtRecived;
        const int MSG_LEN = 1024 * 8; //was changed from old value:4 to new value:8

        public MainWindow()
        {
            InitializeComponent();
            TheList = new ObservableCollection<BoolStringClass>();

            IntPtr pDll = NativeMethods.LoadLibrary(@"KMApi.dll");
            if (pDll == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            // ---------- creates a New KMApi object ---------------
            kmapi = NativeMethods.KMNewKMAPI();
            if (kmapi == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            
            // 1: define the callback
            callback = (value, message) =>
            {
                OnCallback(value, message);
            };

            // 2: set the callback (tell KMApi.dll it should call this method)
            NativeMethods.SetCallback(kmapi, callback);

            string table = "Drive";
            string nameOfColumn = "Name";
            List<string> drives = new List<string>();
            SQLdata dataB = new SQLdata();         
            dataB.SelectDataBase(database, table, nameOfColumn, drives);
            number_of_tests = sQ.CountInt(database, "Test", "TestId");

            //Report();
            Setup set = new Setup();
            //set.SaveAsText();        
            //XmlCreate("1", step_arr, expect_arr, test_result);

            SetTableSetup();

                  
        }

        public void SetTableSetup()
        {           
            SQLdata sQ = new SQLdata();
            id_setups_db.Clear();
            //test.Add("None");
            //test.Add("All"); // Add option run all test in a list
            sQ.SelectDataBaseInt(database ,"Setups", "id", id_setups_db); // R/W data from table "Setups" column id_setup

            last_num_IdSetup = id_setups_db.Max();
            numOfSetups = CalcNumOfSetups(id_setups_db);

            Setup[] setup = new Setup[numOfSetups]; // build new objects of class Setups by number of id_setup

            List<string> column = new List<string> {"id", "driveName", "motorName", "mcName", "guiName", "Status", "dateTest", "firmGUI", "firmDrive"};
            
            List<string> id = new List<string>();
            List<string> drives = new List<string>();
            List<string> motors = new List<string>();
            List<string> mc = new List<string>();
            List<string> gui = new List<string>();

            List<string> status = new List<string>();
            List<string> date = new List<string>();
            List<string> firm_gui = new List<string>();
            List<string> firm_drive = new List<string>();

            List<string> tmp_drives = new List<string>();
            List<string> tmp_motors = new List<string>();
            List<string> tmp_mc = new List<string>();
            List<string> tmp_gui = new List<string>();

            List<string> st = new List<string>();
            List<string> dt = new List<string>();
            List<string> fwDr = new List<string>();
            List<string> fwGui = new List<string>();
                      
            //string[] tmp_status = new string[status.Count];

            foreach (string key in column)
            {
                switch (key)
                {
                    case ("id"):
                        sQ.SelectDataBase(database, "Setups", key, id);
                        break;

                    case ("driveName"):
                        sQ.SelectDataBase(database, "Setups", key, drives);
                        break;

                    case ("mcName"):
                        sQ.SelectDataBase(database, "Setups", key, mc);
                        break;

                    case ("motorName"):
                        sQ.SelectDataBase(database, "Setups", key, motors);
                        break;

                    case ("guiName"):
                        sQ.SelectDataBase(database, "Setups", key, gui);
                        break;

                    case ("Status"):
                        sQ.SelectDataBase(database, "Setups", key, status);
                        break;

                    case ("dateTest"):
                        sQ.SelectDataBase(database, "Setups", key, date);
                        break;

                    case ("firmGUI"):
                        sQ.SelectDataBase(database, "Setups", key, firm_gui);
                        break;

                    case ("firmDrive"):
                        sQ.SelectDataBase(database, "Setups", key, firm_drive);
                        break;
                }

            }
                        

            for(int count=0; count < numOfSetups; count++)
            {
                drives = sQ.Select(database, "Setups", "driveName", tmp_drives, "id", (count).ToString());
                motors = sQ.Select(database, "Setups", "motorName", tmp_motors, "id", (count).ToString());
                mc = sQ.Select(database, "Setups", "mcName", tmp_mc, "id", (count).ToString());
                gui = sQ.Select(database, "Setups", "guiName", tmp_gui, "id", (count).ToString());

                status = sQ.Select(database, "Setups", "Status", st, "id", (count).ToString());
                date = sQ.Select(database, "Setups", "dateTest", dt, "id", (count).ToString());
                firm_gui = sQ.Select(database, "Setups", "firmGUI", fwGui, "id", (count).ToString());
                firm_drive = sQ.Select(database, "Setups", "firmDrive", fwDr, "id", (count).ToString());

                qntDrives.Add(sQ.SelectQnt(database, "Setups", "driveName", "id", count.ToString()));

                setup[count] = new Setup();
                setup[count].setDriveName(drives);
                setup[count].setMotorName(motors);
                setup[count].setMcName(mc);
                setup[count].setGuiName(gui);

                setup[count].Status = status[0];
                setup[count].LastDate = date[0];
                setup[count].FwDrive = firm_drive[0];
                setup[count].FwGUI = firm_gui[0];

                tmp_drives.Clear();
                tmp_motors.Clear();
                tmp_mc.Clear();
                tmp_gui.Clear();

                st.Clear();
                dt.Clear();
                fwDr.Clear();
                fwGui.Clear();

            }

            

            SetTableSetup(setup);
        }
        
        public void SetTableSetup(Setup[] setup)
        {
            
            // TODO: need to sort mc name
            for (int i = 0; i < setup.Length; i++)
            {
                raw1[i] = new RawData();
                raw1[i].IsSelectedSetup = false;
                raw1[i].Id = i + 1;
                foreach (string key in setup[i].getDriveName())
                {
                    raw1[i].Drive += key + "\n";
                }

                foreach (string key in setup[i].getMotorName())
                {
                    raw1[i].Motor += key + "\n";
                }

                raw1[i].MC = setup[i].getMcName()[0];

                //if (setup[i].getMcName().Count < 2)
                //{
                //    raw1[i].MC = setup[i].getMcName()[0];
                //}
                //else
                //{
                //    foreach (string key in setup[i].getMcName())
                //    {
                //        raw1[i].MC += key + "\n";
                //    }
                //}


                foreach (string key in setup[i].getGuiName())
                {
                    raw1[i].GUI += key + "\n";
                }

                // For each setup i+1 (first setup is 1) find test/s from DB with the same index: "TestId"
                List<string> list_tests = new List<string>();
                
                list_tests = sQ.Select(database, "Test", "TestName", test, "TestId", (i + 1).ToString());
                list_tests.Add("All");
                list_tests.Add(" ");
                
                setup[i].setTestName(list_tests);

                raw1[i].TestSetup = setup[i].getTestName(); // Set list of tests
                test.Clear();

                raw1[i].StatusTest = setup[i].Status;
                raw1[i].LastDate = setup[i].LastDate;
                raw1[i].FwDrive = setup[i].FwDrive;
                raw1[i].FwGUI = setup[i].FwGUI;

                grid2.Items.Add(raw1[i]);

            }

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int rawindex = grid2.Items.IndexOf(grid2.SelectedItem);
            string selectedvalue = ((ComboBox)sender).SelectedValue.ToString();
            string tag = ((ComboBox)sender).Tag.ToString();
            
            int x = 0;
            //TODO: number of setups 
            if (selectedvalue == " ")
            {                                
                raw1[rawindex].IsSelectedSetup = false;
            }
            else
            {
                raw1[rawindex].IsSelectedSetup = true;
                switch (x)
                {
                    case (0): //Use case if will be use design when: Possible to select only one test or all tests
                        
                        bool flag = true;
                        for (int i = 0; i < selected_tests.Count; i++) // check if selected test 
                        {
                            if ((selected_tests[i].NameTest.Contains(selectedvalue)) && (selected_tests[i].SetupIdTest == rawindex)) // check if selected test exist into list
                            {
                                flag = false;
                                break;
                            }                           
                        }

                        if (flag)
                        {
                            selected_tests.Add(new Test());
                            selected_tests[selected_tests.Count - 1] = new Test { SetupIdTest = rawindex, NameTest = selectedvalue };
                        }
                        
                        break;

                    case (1): // Use case if will be use design when: Possible to select list of tests or all tests

                        if (selected_tests.Count == 0)
                        {
                            selected_tests.Add(new Test());
                            selected_tests[0] = new Test { SetupIdTest = rawindex, NameTest = selectedvalue };
                        }
                        else
                        {
                            bool flag1 = true;
                            for (int i = 0; i < selected_tests.Count; i++) // check if selected test 
                            {
                                if (selected_tests[i].SetupIdTest == rawindex)
                                {
                                    if (selected_tests[i].NameTest.Contains(selectedvalue)) // check if selected test exist into list
                                    {
                                        flag1 = false;
                                        break;
                                    }

                                }
                            }

                            if (flag1)
                            {
                                selected_tests.Add(new Test());
                                selected_tests[selected_tests.Count - 1] = new Test { SetupIdTest = rawindex, NameTest = selectedvalue };
                            }

                        }

                        break;

                }
                                
                //TheList.Add(new BoolStringClass { IsSelected = true, TheText = "Test selected: " + selectedvalue });
                //this.DataContext = this;
            }

            //grid2.Items.Refresh();
            grid2.UpdateLayout();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int rawindex = grid1.Items.IndexOf(grid1.SelectedItem);
            string selectedvalue = ((ComboBox)sender).SelectedValue.ToString();
            string tag = ((ComboBox)sender).Tag.ToString();

            //TODO: Need to change selection reapetable for list
            switch (tag)
            {
                case ("drive"):

                    insert_name_drives.Insert(rawindex, selectedvalue);
                    break;

                case ("motor"):

                    insert_name_motors.Add(selectedvalue);
                    break;

                case ("mc"):

                    insert_name_mcs.Add(selectedvalue);
                    break;

                case ("gui"):

                    insert_name_guis.Add(selectedvalue);
                    break;

            }

        }

        public int CalcNumOfSetups(List<int> idSetups)
        {
            int count = 0;
            int tmp_id = -1;

            for(int i=0; i<idSetups.Count; i++)
            {
                if(idSetups[i] != tmp_id)
                {
                    count++;
                    tmp_id = idSetups[i];
                }
                else
                {
                    tmp_id = idSetups[i];
                    if (i + 1 == idSetups.Count)
                    {
                        if (idSetups.Count % 2 == 0) count++;
                    }
                }
            }

            

            return (count);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void OnCallback(int FrameType, string message)
        {
            // in case of disconnection error 
            if (FrameType == NativeMethods.ERR_ON_CLOSE)
            {
                MessageBox.Show("MC was disconnected unexpectedly");
                // handle it ...
                // free old one
                NativeMethods.MCFreeKMApi(kmapi);
                // create new one
                kmapi = NativeMethods.KMNewKMAPI();
                return;
            }

            // switch by frame type enum
            switch ((NativeMethods.FrameTypeEnum)FrameType)
            {
                case NativeMethods.FrameTypeEnum.INVALID_FRAME:
                    AddToLogTextBox("Invalid Frame:" + message);
                    break;
                case NativeMethods.FrameTypeEnum.ACKNOWLEDGE_FRAME:
                    // ...
                    break;
                case NativeMethods.FrameTypeEnum.DATA_FRAME:
                    AddToLogTextBox(message);
                    break;
                case NativeMethods.FrameTypeEnum.BINARY_FRAME:
                    break;
                case NativeMethods.FrameTypeEnum.CONTROL_FRAME:
                    break;
                case NativeMethods.FrameTypeEnum.PROMPT_FRAME:
                    propmtRecived = true;
                    break;
                case NativeMethods.FrameTypeEnum.ERROR_FRAME:
                    break;

                //  asyncronius messages
                case NativeMethods.FrameTypeEnum.ASYNC_FRAME:
                case NativeMethods.FrameTypeEnum.ASYNC2_FRAME:
                case NativeMethods.FrameTypeEnum.ASYNC_OTHER:
                    AddToLogTextBox(string.Format("ASYNC: {0}\n", message));
                    break;
                case NativeMethods.FrameTypeEnum.STATE_FRAME:
                    AddToLogTextBox(string.Format("ASYNC STATE: {0}\n", message));
                    break;
                default:
                    break;
            }
        }
        /*        
        public void startTest_btn(object sender, RoutedEventArgs e)
        {
            XMLItems xml_obj; 
                        
            for (int i = 0; i < numOfSetups; i++)
            {
                if (raw1[i].IsSelectedSetup) // Check if setup selected
                {
                    
                    for(int j =0; j< selected_tests.Count; j++)
                    {
                        if (selected_tests[j].SetupIdTest == i) //Check if index of selected setup equal to selected test(s)
                        {                            
                            MessageBox.Show("Setup selected: " + (selected_tests[j].SetupIdTest + 1).ToString() + " Test(s) selected: " + selected_tests[j].NameTest);

                            switch (selected_tests[j].SetupIdTest + 1)
                            {
                                case (6): // Tests for the setup #1
                                    List<string> ListOfSerialPort = new List<string>();                                    
                                    int number_of_axis = qntDrives[selected_tests[j].SetupIdTest];
                                    sQ.Select(database, "Port", "PortName", ListOfSerialPort, "setupId", (selected_tests[j].SetupIdTest + 1).ToString()); // Serial port selection from DB
                                                                                                            
                                    AddToLogTextBox(startTest.LoadEC(kmapi)); // Load EC setup

                                    Thread.Sleep(1000);
                                    pbStatus.Value = 50; // Progress bar 50%
                                    //ThreadPool.QueueUserWorkItem(JobThread1);

                                    //TESTS
                                    switch (selected_tests[j].NameTest)
                                    {
                                        case ("Enable"):

                                            string setupId = (selected_tests[j].SetupIdTest + 1).ToString();
                                            string testName = selected_tests[j].NameTest;
                                            string[] test_result = new string[] {
                                                    "Pass",
                                                    "Pass"
                                                };
                                            string[] step_arr = new string[] {
                                                    "Enable the setup by each axis",
                                                    "Disable the system setup"

                                                };
                                            string[] expect_arr = new string[] {
                                                    "Each axis should be in Enable state",
                                                    "The system setup should be disabled"
                                                };

                                            //ThreadPool.QueueUserWorkItem(JobThread);

                                            UpdateTableSetup(i, "Process", ListOfSerialPort[0]);
                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Enable test Running");

                                            // Start running test
                                            Application.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                test_result = startTest.EnableDisableTest(kmapi, ListOfSerialPort[0], step_arr.Count(), number_of_axis);  //Result array[number of test cases]
                                            });

                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Create report");
                                                                            
                                            bool xml_file_exist = File.Exists(path_report + "\\test" + setupId + ".xml");                                            
                                            xml_obj = new XMLItems(path_report + "\\test" + setupId + ".xml", testName, setupId);                                                                                        

                                            if (xml_file_exist)
                                            {
                                                bool test_exist = xml_obj.FindTest();

                                                if (test_exist)
                                                {
                                                    xml_obj.UpdateXML(test_result);                                                    
                                                }
                                                else
                                                {
                                                    xml_obj.InsertXML(step_arr, expect_arr, test_result);
                                                }
                                            }
                                            else
                                            {
                                                xml_obj.CreateXML(step_arr, expect_arr, test_result);
                                            }
                                            
                                            Report(setupId, testName, ListOfSerialPort[0]);  //Create report

                                            AddToLogTextBox("Report Enable ...Done!");
                                            pbStatus.Value = 100;  //Progress bar 100%
                                            Thread.Sleep(1000);
                                            UpdateTableSetup(i, "PASS!!!", ListOfSerialPort[0]);

                                            break;

                                        case ("Motion"):

                                            setupId = (selected_tests[j].SetupIdTest + 1).ToString();
                                            testName = selected_tests[j].NameTest;
                                            test_result = new string[] {
                                                    "Pass",
                                                    "Pass",
                                                    "Pass"
                                                };
                                            step_arr = new string[] {
                                                    "Enable the setup by each axis",
                                                    "Perform motion of the setup",
                                                    "Disable the system setup"

                                                };
                                            expect_arr = new string[] {
                                                    "Each axis should be in Enable state",
                                                    "Each axis should movement according to configuration",
                                                    "The system setup should be disabled"
                                                };


                                            UpdateTableSetup(i, "Process", ListOfSerialPort[0]);
                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Motion test Running");

                                            // Start running test
                                            Application.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                test_result = startTest.MotionTest(kmapi, ListOfSerialPort[0], step_arr.Count(), number_of_axis);  //Result array[number of test cases]
                                            });

                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Create report");

                                            xml_file_exist = File.Exists(path_report + "\\test" + setupId + ".xml");
                                            xml_obj = new XMLItems(path_report + "\\test" + setupId + ".xml", testName, setupId);

                                            if (xml_file_exist)
                                            {
                                                bool test_exist = xml_obj.FindTest();

                                                if (test_exist)
                                                {
                                                    xml_obj.UpdateXML(test_result);
                                                }
                                                else
                                                {
                                                    xml_obj.InsertXML(step_arr, expect_arr, test_result);
                                                }
                                            }
                                            else
                                            {
                                                xml_obj.CreateXML(step_arr, expect_arr, test_result);
                                            }

                                            Report(setupId, testName, ListOfSerialPort[0]);  //Create report

                                            AddToLogTextBox("Report for Motion test ...Done!");
                                            pbStatus.Value = 100;  //Progress bar 100%
                                            Thread.Sleep(1000);
                                            UpdateTableSetup(i, "PASS!!!", ListOfSerialPort[0]);

                                            break;

                                        default:
                                            MessageBox.Show("There arent tests");
                                            break;
                                    }
                                    

                                    break;
                            }
                        }
                            
                    }
         
                        
                    
                }
            }
        }
        */

        public void startTest_btn(object sender, RoutedEventArgs e)
        {
            XMLItems xml_obj;

            for (int i = 0; i < numOfSetups; i++)
            {
                if (raw1[i].IsSelectedSetup) // Check if setup selected
                {
                    //SETUPS
                    switch (i + 1)
                    {
                        case (6): // Tests for the setup #1
                            List<string> ListOfSerialPort = new List<string>();                            

                            AddToLogTextBox(startTest.LoadEC(kmapi)); // Load EC setup

                            Thread.Sleep(1000);
                            pbStatus.Value = 50; // Progress bar 50%

                            for (int j = 0; j < selected_tests.Count; j++)
                            {
                                if (selected_tests[j].SetupIdTest == i)
                                {
                                    int number_of_axis = qntDrives[selected_tests[j].SetupIdTest];
                                    sQ.Select(database, "Port", "PortName", ListOfSerialPort, "setupId", (selected_tests[j].SetupIdTest + 1).ToString()); // Serial port selection from DB
                                    Thread.Sleep(100);
                                    MessageBox.Show("Setup selected: " + (selected_tests[j].SetupIdTest + 1).ToString() + " Test(s) selected: " + selected_tests[j].NameTest);

                                    if(selected_tests[j].NameTest == "All")
                                    {

                                    }

                                    switch (selected_tests[j].NameTest)
                                    {
                                        case ("Enable"):

                                            string setupId = (selected_tests[j].SetupIdTest + 1).ToString();
                                            string testName = selected_tests[j].NameTest;
                                            
                                            string[] step_arr = new string[] {
                                                    "Enable the setup by each axis",
                                                    "Disable the system setup"
                                                };
                                            string[] expect_arr = new string[] {
                                                    "Each axis should be in Enable state",
                                                    "The system setup should be disabled"
                                                };
                                            string[] test_result = new string[step_arr.Count()];

                                            UpdateTableSetup(i, "Process", ListOfSerialPort[0]);
                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Enable test Running");

                                            // Start test
                                            Application.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                test_result = startTest.EnableDisableTest(kmapi, ListOfSerialPort[0], step_arr.Count(), number_of_axis);  //Result array[number of test cases]
                                            });

                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Create report");

                                            bool xml_file_exist = File.Exists(path_report + "\\test" + setupId + ".xml");
                                            xml_obj = new XMLItems(path_report + "\\test" + setupId + ".xml", testName, setupId);

                                            if (xml_file_exist)
                                            {
                                                bool test_exist = xml_obj.FindTest();

                                                if (test_exist)
                                                {
                                                    xml_obj.UpdateXML(test_result);
                                                }
                                                else
                                                {
                                                    xml_obj.InsertXML(step_arr, expect_arr, test_result);
                                                }
                                            }
                                            else
                                            {
                                                xml_obj.CreateXML(step_arr, expect_arr, test_result);
                                            }

                                            Report(setupId, testName, ListOfSerialPort[0]);  //Create report

                                            AddToLogTextBox("Report Enable ...Done!");
                                            pbStatus.Value = 100;  //Progress bar 100%
                                            Thread.Sleep(1000);
                                            UpdateTableSetup(i, "PASS!!!", ListOfSerialPort[0]);

                                            break;

                                        case ("Motion"):

                                            setupId = (selected_tests[j].SetupIdTest + 1).ToString();
                                            testName = selected_tests[j].NameTest;

                                            step_arr = new string[] {
                                                    "Enable the setup by each axis",
                                                    "Perform motion of the setup",
                                                    "Disable the system setup"
                                                };
                                            expect_arr = new string[] {
                                                    "Each axis should be in Enable state",
                                                    "Each axis should movement according to configuration",
                                                    "The system setup should be disabled"
                                                };
                                            test_result = new string[step_arr.Count()];

                                            UpdateTableSetup(i, "Process", ListOfSerialPort[0]);
                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Motion test Running");

                                            // Start test
                                            Application.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                test_result = startTest.MotionTest(kmapi, ListOfSerialPort[0], step_arr.Count(), number_of_axis);  //Result array[number of test cases]
                                            });

                                            Thread.Sleep(1000);
                                            AddToLogTextBox("Create report");

                                            xml_file_exist = File.Exists(path_report + "\\test" + setupId + ".xml");
                                            xml_obj = new XMLItems(path_report + "\\test" + setupId + ".xml", testName, setupId);

                                            if (xml_file_exist)
                                            {
                                                bool test_exist = xml_obj.FindTest();

                                                if (test_exist)
                                                {
                                                    xml_obj.UpdateXML(test_result);
                                                }
                                                else
                                                {
                                                    xml_obj.InsertXML(step_arr, expect_arr, test_result);
                                                }
                                            }
                                            else
                                            {
                                                xml_obj.CreateXML(step_arr, expect_arr, test_result);
                                            }

                                            Report(setupId, testName, ListOfSerialPort[0]);  //Create report

                                            AddToLogTextBox("Report for Motion test ...Done!");
                                            pbStatus.Value = 100;  //Progress bar 100%
                                            Thread.Sleep(1000);
                                            UpdateTableSetup(i, "PASS!!!", ListOfSerialPort[0]);

                                            break;

                                        default:
                                            MessageBox.Show("There arent tests");
                                            break;
                                    }

                                }                       

                            }

                            break;
                    }

                                        

                    



                }
            }
        }

        /*
        public void startTest_btn(object sender, RoutedEventArgs e)
        {
            Globals.threads = new List<Thread>();
            for (int i=0; i<numOfSetups; i++) //search for the setups that was selected
            {                
                if (raw1[i].IsSelectedSetup) // check if setup was selected
                {
                    AddToLogTextBox(startTest.LoadEC(kmapi)); // need to write different connections for different setups

                    for (int j =0; j<selected_tests.Count; j++)
                    {
                        if(selected_tests[j].SetupIdTest == i)  //Find testId match to setupId
                        {
                            MessageBox.Show("Setup selected: " + Convert.ToString(selected_tests[j].SetupIdTest+1) + " Test selected: " + Convert.ToString(selected_tests[j].NameTest));
                              
                                switch (selected_tests[j].NameTest)
                                {
                                    case ("Enable"):
                                        AddToLogTextBox("\nSetup selected: " + Convert.ToString(selected_tests[j].SetupIdTest + 1) + " \nTest selected: " + Convert.ToString(selected_tests[j].NameTest));
                                        Thread.Sleep(1000);
                                        Thread n = new Thread(() =>
                                         {
                                             Application.Current.Dispatcher.Invoke((Action)delegate { startTest.EnableDisableTest(kmapi); });
                                         });
                                        n.Name = i.ToString();
                                        Globals.threads.Add(n);                                        
                                        break;

                                    case ("Motion"):
                                        AddToLogTextBox("\nSetup selected: " + Convert.ToString(selected_tests[j].SetupIdTest + 1) + " \nTest selected: " + Convert.ToString(selected_tests[j].NameTest));
                                        Thread.Sleep(1000);
                                        startTest.ConnectionTest(kmapi);
                                        break;

                                    default:
                                        MessageBox.Show("Test not found!");
                                        break;

                                }
                                                                
                                //startTest.DispalyTest();
                                //Test startTest = new Test();
                                //startTest.EnableDisableTest(kmapi);
                                //startTest.ConnectionTest(kmapi);
                                //UpdateTableSetup(i, "Under Process");
                                //string st = startTest.SerialTest("COM48");
                                //UpdateTableSetup(i, st);

                            
                        }
                    }

                }
            }

            foreach (Thread t in Globals.threads)
            {
                t.Start();
                Globals.Dead = false;
            }
            
            Thread IsAlive = new Thread(() => AreThreadsAlive(Globals.threads));




        }
        */

        /*
    private XElement XmlCreate(string IdSetup, string testId, string[] step_arr, string[] expect_arr,string[] test_result)
    {           
        XElement xml = new XElement("Method");
        XElement tests = new XElement("Tests");
        XElement test1 = new XElement("Test");

        if (!string.IsNullOrEmpty(IdSetup))
        {
            xml.Add(new XAttribute("SetupId", IdSetup), new XAttribute("Cmd", "Update"));
        }
        else
        {
            xml.Add(new XAttribute("SetupId", "100"), new XAttribute("Cmd", "New"));
        }

        xml.Add(tests); //Create group tests

        test1.Add(new XAttribute("TestId", testId));  //Create new test1
        tests.Add(test1); //Write test1 to root                       

        //setXML(test, step_arr, expect_arr, test_result);
        setXML(test1, step_arr, expect_arr, test_result);

        xml.Save(path_report + "\\test" + IdSetup + ".xml");

        return (xml);


    }

    private void setXML(XElement test, string[] step_arr, string[] expect_arr, string[] test_result)
    {
        int count = 0;
        foreach (string key in step_arr)  // Write steps into the test
        {
            test.Add(new XElement("Field", new XAttribute("Name", key), new XAttribute("Expect", expect_arr[count]), test_result[count]));
            count++;
        }

    }
    */

        public void JobThread(object state)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                UpdateTableSetup(1, "Process", "COM48");
            });
            

        }

        public void JobThread1(object state)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                pbStatus.Value = 50;
            });
            

        }

        public void AddToLogTextBox(string msg)
        {

            displayTxt.Text = displayTxt.Text + "\n" + msg;
            displayTxt.ScrollToEnd();
        }

        private void AreThreadsAlive(List<Thread> t)
        {
            while (!Globals.Dead)
            {
                Globals.Dead = t.All(thread => !thread.IsAlive);
            }
            startTest.IsEnabled = true;
            

        }

        private void UpdateDb(int raw, string st, string date, string fwDr, string fwGui)
        {
            SQLdata sQ = new SQLdata();
            sQ.UpdateDataBase(database, "Setups", "Status", st, raw.ToString());
            sQ.UpdateDataBase(database, "Setups", "dateTest", date, raw.ToString());
            sQ.UpdateDataBase(database, "Setups", "firmGUI", fwGui, raw.ToString());
            sQ.UpdateDataBase(database, "Setups", "firmDrive", fwDr, raw.ToString());

        }

        public void UpdateTableSetup(int raw, string status_tst, string serial_port)
        {
            string ver_value = startTest.SendSerialPort(serial_port, "ver", true);

            string start_string = "Version:";
            string end_string = "FieldBus";
            string[] spl = ver_value.Substring(ver_value.IndexOf(start_string) + start_string.Length).Split(' ');
            //MessageBox.Show(spl[1].Substring(0, spl[1].Length - spl[1].IndexOf(end_string)));

            DateTime today = DateTime.Today;
            string date_of_test = today.ToString("dd/MM/yyyy");
            string fw_drive = "Drive: " + spl[1].Substring(0, spl[1].Length - spl[1].IndexOf(end_string));
            string fw_gui = "SSt: 2.16.4.0";

            raw1[raw].StatusTest = status_tst;  // Update status of test: PASS; FAIL; Under Process
            raw1[raw].LastDate = date_of_test;  // Update date of tested
            raw1[raw].FwDrive = fw_drive;       // Update Drive versions
            raw1[raw].FwGUI = fw_gui;           // Update GUI versions
            raw1[raw].Report = path_report;     // Update path report
            
            grid2.Items.Refresh();

            if(UpdateDB.IsChecked == true)
            {
                UpdateDb(raw, status_tst, date_of_test, fw_drive, fw_gui);
            }

        }

        private void addNewSetup_button(object sender, RoutedEventArgs e)
        {
            int num_of_setups = Convert.ToInt16(text1.Text);
            if (num_of_setups == 0 || num_of_setups < 1)  // check if value of number axis is valid
            {
                num_of_setups = 1;
            }
            
            RawData [] raw_new_setup = new RawData[num_of_setups];
                        
            string nameOfColumn = "Name";
            SQLdata dataB = new SQLdata();

            dataB.SelectDataBase(database, "Drive", nameOfColumn, new_list_drives);
            dataB.SelectDataBase(database, "motor", nameOfColumn, new_list_motors);
            dataB.SelectDataBase(database, "MC", nameOfColumn, new_list_mcs);
            dataB.SelectDataBase(database, "GUI", nameOfColumn, new_list_guis);

            //for (int i = 1; i < num_of_setups + 1; i++)
            //{
            //    new_list_qnt_drives.Add(i);
            //    new_list_qnt_motors.Add(i);
            //}

            dataB.SelectDataBaseInt(database, "Setups", "id", id_setups_db);
            last_num_IdSetup = id_setups_db.Max();

            last_num_IdSetup++;
            for (int i = 0; i < num_of_setups; i++)
            {
                raw_new_setup[i] = new RawData();
                raw_new_setup[i].IsSelected = true;
                raw_new_setup[i].IdSetup = last_num_IdSetup;
                raw_new_setup[i].DriveSelected = new_list_drives;
                raw_new_setup[i].MotorSelected = new_list_motors;
                raw_new_setup[i].MCSelected = new_list_mcs;
                raw_new_setup[i].GUISelected = new_list_guis;

                grid1.Items.Add(raw_new_setup[i]);
            }
        }

        private void AddNewSetupToDB_button(object sender, RoutedEventArgs e)
        {
            SQLdata sQ = new SQLdata();
            string[] columns = { "id", "driveName", "motorName", "mcName", "guiName", "Status", "dateTest", "firmGUI", "firmDrive" };

            List<string> dataList = new List<string>();

            if(insert_name_mcs.Count == 0)
            {
                insert_name_mcs.Add("Not using MC");
            }

            for (int i = 0; i < insert_name_drives.Count; i++)
            {
                dataList.Add(last_num_IdSetup.ToString());
                dataList.Add(insert_name_drives[i]);
                dataList.Add(insert_name_motors[i]);

                if (insert_name_mcs.Count < insert_name_drives.Count) //protection for count Mc selected less then axis in setup
                {
                    dataList.Add(insert_name_mcs[0]);
                }
                else
                {
                    dataList.Add(insert_name_mcs[i]);
                }

                if (insert_name_guis.Count < insert_name_drives.Count) //protection for count GUI selected less then axis in setup
                {
                    dataList.Add(insert_name_guis[0]);
                }
                else
                {
                    dataList.Add(insert_name_guis[i]);
                }
                              
                dataList.Add("New Setup");  // for Setup column
                dataList.Add("Not tested"); // for date test column
                dataList.Add("Not tested"); // for firware GUI column
                dataList.Add("Not tested"); // for firmware Drive column

                sQ.InsertDataBase(database, "Setups", columns, dataList);
                dataList.Clear();
            }

            MessageBox.Show("Setup inserted into Database successfully!");
        }

        private void Refresh_button(object sender, RoutedEventArgs e)
        {
            SetTableSetup();
            grid2.Items.Refresh();
        }
                
        private void log_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGridTextColumn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void open_Folder_btn(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.InitialDirectory = path.Text;
            saveFile.Title = "Select a Directory";          // instead of default "Save As"
            saveFile.Filter = "Directory|*.this.directory"; // Prevents displaying files
            saveFile.FileName = "select";                   // Filename will then be "select.this.directory"

            string nameFolder = "New_Test_" + DateTime.Now.ToString("h_mm_ss");

            if (saveFile.ShowDialog() == true)
            {
                string path_folder = saveFile.FileName;

                path_folder = path_folder.Replace("\\select.this.directory", "");
                path_folder = path_folder.Replace(".this.directory", "");

                if (!Directory.Exists(path_folder + "\\" + nameFolder))
                {
                    Directory.CreateDirectory(path_folder + "\\" + nameFolder);
                }

                path.Text = path_folder + "\\" + nameFolder;
                path_report = path_folder + "\\" + nameFolder;
            }

            //string path = Directory.GetCurrentDirectory();

        }

        public void Report(string setupId, string testName, string port)
        {

            string output = "";
            string err = "";

            try
            {
                Process p = StartProcess("python.exe", "test.py", setupId, testName, port);
                //get output
                output = GetStreamOutput(p.StandardOutput);
                err = GetStreamOutput(p.StandardError);
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            if (err.Length > 0)
                MessageBox.Show(err);
        }

        private Process StartProcess(string exe, string file_script, string setupId, string testName, string port)
        {
            //arguments                                
            //object[] arguments = new object[2] { file_script, "C:\\AT\\Python\\Report.docx" };

            object[] arguments = new object[5] { file_script, path_report, port, setupId, testName};
            //the cmd command
            string cmdcommand = string.Format("{0} {1} {2} {3} {4}", arguments);

            //build the process
            //ProcessStartInfo i = new ProcessStartInfo(exe, cmd);
            ProcessStartInfo i = new ProcessStartInfo(exe, cmdcommand);
            Process p = new Process();
            p.StartInfo = i;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.Start();
            return p;
        }

        private string GetStreamOutput(StreamReader stream)
        {
            //Read output in separate task to avoid deadlocks
            var outputReadTask = Task.Run(() => stream.ReadToEnd());

            return outputReadTask.Result;
        }
    }

    
}
