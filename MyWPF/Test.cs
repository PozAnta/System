using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
//using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyWPF
{
    static class NativeMethods
    {
        /*! \brief win Api Call LoadLibrary
        *   This call will load the specified library into application memory
        */
        const string SKernel = "kernel32";

        [DllImport(SKernel, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string fileName);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]

        [DllImport(SKernel, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(SKernel)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport(SKernel, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
        public static extern uint GetModuleFileName(
          [In]IntPtr hModule,
          [Out]StringBuilder lpFilename,
          [In]uint nSize);

        ///  KMAPI methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        /*! \brief All KMAPI commands
        *   commands exported from KMAPI.dll
        */

        /*! \brief  KMNewKMAPI: Creates a new KMAPI 
         * Returns: new KMApi
         */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr KMNewKMAPI();

        /*! \brief MCFreeKMApi frees the KMAPI object 
             * Parameter: the kmapi objec created by KMNewKMApi()
             */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MCFreeKMApi(IntPtr kmApiPtr);

        /*! \brief  SetCallback: Set the callback pointer.
         *  @Param: KMApi- the KMApi object created by KMNewKMAPI
             */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCallback(IntPtr pKmapi, OnMessage pCallback);

        /*! \brief  Connect: Connets a specific IP and port
         *  @Param: KMApi- the KMApi object created by KMNewKMAPI
         *  @Param: IP- a string with MCs IP
         *  @Param: Port - number with Port to connect to
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Connect(IntPtr pKmapi, string ip, int port);

        /*! \brief  ExecCommand: Execute a command and wait for response
         *  @Param: KMApi- the KMApi object created by KMNewKMAPI
         *  @Param: Command- a string with requested command
         *  @Param[out]: response a string buffer for retuened value from MC
         *  @param[in]:  size of response
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ExecCommand(IntPtr pKmapi, string command, StringBuilder Response, int responseLen);

        /*! \brief  SendCommand: Execute a command but dont wait for response
          *  @Param: KMApi- the KMApi object created by KMNewKMAPI
          *  @Param: Command- a string with requested command
         */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendCommand(IntPtr pKmapi, string command);

        /*! \brief  DownloadFile: Downloads a file from MC
         *  @Param: KMApi- the KMApi object created by KMNewKMAPI
         *  @Param:  sFilePath- destination to save
         *  @Param:  sFileName- name of file to download from MC
         *  
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int DownloadFile(IntPtr pKmapi, string sFilePath, string sFileName);

        /*! \brief  UploadFile: Upload a file to MC
         *  @Param: KMApi- the KMApi object created by KMNewKMAPI
         *  @Param:  Filename- the path and name of file to upload from MC
         */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int UploadFile(IntPtr pKmapi, string Filename);

        /*! \brief  GetMCLastError: Retrieve an error message from 
         *   @Param: KMApi- the KMApi object created by KMNewKMAPI
         *  @Param[out]: errorCode- a number represents an error code - can be a KMResponse or a socket error id
         *  @Param[out]: sErrorMessage- a buffer for error message
         *  @Param: msgLength- sErrorMessage's buffer length 
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMCLastError(IntPtr pKmapi,/* out*/ ref int errorCode,/* out*/  StringBuilder sErrorMessage,/* out*/  int msgLength);

        /*! \brief  SetTimeout: Set timeout
         *   @Param:  KMApi- the KMApi object created by KMNewKMAPI
         *  @Param:   type- type of timeout to set (0 for connection timeout or 1 for execute timeout)
         *  @Param: timeout- timeout in ms
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTimeout(IntPtr pKmapi, Int16 type /* 0=connect 1 = exec*/, Int16 timeout);

        /*! \brief  DetectDevices: Detect devices in local networks
         *   @Param:  KMApi- the KMApi object created by KMNewKMAPI
         *  @Param: timeout- time to wait for responses in ms
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int DetectDevices(IntPtr pKmapi, Int16 Timout);

        /*! \brief  GetDeviceList: Get a list of devices-
         *   @Param:  KMApi- the KMApi object created by KMNewKMAPI
         *   @return: returns a pointer of the list memoery
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetDeviceList(IntPtr pKmapi);

        /*! \brief  Authenticate: performe authentication using a password
         *  @Param:  KMApi- the KMApi object created by KMNewKMAPI
         *  @Param: password - the string with the password
        */
        [DllImport(@"KMApi.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Authenticate(IntPtr pKmapi, string password);

        /*! \brief  The callback definition
         * This method will be called on each async frame, error messages and progress status ( ack/ prompt )
         *  @Param: value- frame type or error code (such as unexpected disconnnection)
         *  @Param: message- the text describing value or async message
        */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnMessage(int value, string message);

        /*! \brief  enums and static lists
        * KMAPI response codes
        * This is an enum with all return codes from methods
        * response can be a sucess or error code
        * Allmost all functions return such a response 
        */
        public enum KMResponse
        {
            // ----------------------- success messages -----------------------------------------------
            KMRet_OK = 0,
            KMRet_CONNECTED, KMRet_ASYNC,
            // ----------------------   error messages -------------------------------------------
            KMErr_GENERAL,
            KMErr_TIMEOUT, KMErr_BAD_KMApiID, KMErr_FAIL_EXEC, KMErr_UNKNOWN_MESSAGE,
            KMErr_FAIL_CREATE_FILE, KMErr_FAIL_OPEN_FILE, KMErr_FILE_NOT_FOUND, KMErr_CANNOT_CONNECT,
            KMErr_INVALID_FORMAT, KMErr_BUFFER_TOO_SMALL, KMErr_FOLDER_NOT_FOUND,
            KMErr_TRUNCATED_FRAME, KMErr_MC_ERR, KMErr_RETRIEVE
        };

        public enum KMTimeout
        {
            KMTO_Connect,
            KMTO_Exec
        };

        /*! \brief  frame type enums for MC frames
         * You might recive them on the callback (1st parameter) 
         */
        public enum FrameTypeEnum
        {
            INVALID_FRAME = 0,
            ACKNOWLEDGE_FRAME,
            DATA_FRAME,
            BINARY_FRAME,
            CONTROL_FRAME,
            PROMPT_FRAME,
            ERROR_FRAME,
            //  asyncronius messages
            ASYNC_FRAME,
            ASYNC2_FRAME,
            ASYNC_OTHER,
            AUTHENTICATE_FRAME,
            STATE_FRAME
        };

        /// \brief  error sent to the callback whe error is closed unexpectedly 
        public static int ERR_ON_CLOSE = 100;

        /*! \brief  List of error messages 
        /* Note: You may want to chenge text into a clear error description
         */
        public static string[] KMResponsesNames = {
            "KMRet_OK",
            "KMRet_CONNECTED", "KMRet_ASYNC",
	        // ----------------------   error messages -------------------------------------------
	        "KMErr_GENERAL",
            "KMErr_TIMEOUT", "KMErr_BAD_KMApiID", "KMErr_FAIL_EXEC", "KMErr_UNKNOWN_MESSAGE",
            "KMErr_FAIL_CREATE_FILE", "KMErr_FAIL_OPEN_FILE", "KMErr_FILE_NOT_FOUND", "KMErr_CANNOT_CONNECT",
            "KMErr_INVALID_FORMAT", "KMErr_BUFFER_TOO_SMALL", "KMErr_FOLDER_NOT_FOUND",
            "KMErr_TRUNCATED_FRAME", "KMErr_MC_ERR", "KMErr_RETRIEVE"
        };
    }

    public class Test : Window
    {        
        bool propmtRecived;
        const int MSG_LEN = 1024 * 4;
        
        public string NameTest { get; set; }
        public int SetupIdTest { get; set; }

        public string SendSerialPort(string name_port, string command, bool return_data) //Serial port communication
        {
           int boud = 115200;
           
           SerialPort port = new SerialPort(name_port, boud, Parity.None, 8, StopBits.One);
            port.Handshake = 0;
            port.WriteTimeout = 500;

            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }

                port.Open();
                port.Write(command + "\r\n");
                Thread.Sleep(200);
                
                if (return_data)
                {
                    string output = port.ReadTo("->");
                    string cut_out = output.Substring(command.Length + 2, output.Length - 10); //Cut string from Start: lenght of command +\n\r End: of out +\n\r + ->
                    port.Close();
                    return (cut_out);
                }

                port.Close();
                return ("");
            }
            catch (IOException)
            {
                port.Close();
                return ("Error serial port connection");
            }
        }

        private bool ProcessResult(IntPtr kmapi, int ret)
        {
            if (ret < (int)NativeMethods.KMResponse.KMErr_GENERAL)
                return true;

            // it's an error
            int messaegLength = MSG_LEN;
            StringBuilder message = new StringBuilder(messaegLength);
            int errId = -1;

            int response = NativeMethods.GetMCLastError(kmapi, ref errId, message, messaegLength);

            if (ret == (int)NativeMethods.KMResponse.KMErr_MC_ERR)
            {
                MessageBox.Show(string.Format("Error: {0} ({1}) \n{2}\n", NativeMethods.KMResponsesNames[ret], errId, message));

            }
            else
            {
                MessageBox.Show(string.Format("Error: {0} ({1}) \n{2}\n", NativeMethods.KMResponsesNames[response], errId, message));
            }

            return false;
        }
        private string Connect(IntPtr kmapi, string IP, string port)
        {
            if (kmapi != IntPtr.Zero)
            {
                // connect to a specific IP / port 
                int ret = NativeMethods.Connect(kmapi, IP, Convert.ToInt16(port));
                if (!ProcessResult(kmapi, ret))
                {
                    return "Error\n";
                }

                return "Connected\n";
            }
            return "Error\n";
        }
        private string Disconnect(IntPtr kmapi)
        {
            if (kmapi != IntPtr.Zero)
                NativeMethods.MCFreeKMApi(kmapi);
            return ("Disconnected");
        }
        private string Execute(IntPtr kmapi, string command)
        {
            if (kmapi != IntPtr.Zero)
            {
                int buffsize = MSG_LEN;
                StringBuilder result = new StringBuilder(buffsize);
                propmtRecived = false;
                //execute a command and wait for response
                int ret = NativeMethods.ExecCommand(kmapi, command, result, buffsize);

                if (!ProcessResult(kmapi, ret))
                    return result.ToString();

                if (propmtRecived)
                    return result.ToString() + "-->";

                return result.ToString();
            }
            return "Error";
        }
        private void Send(IntPtr kmapi, string command)
        {
            if (kmapi != IntPtr.Zero)
            {
                int ret = NativeMethods.SendCommand(kmapi, command);
                ProcessResult(kmapi, ret);
            }

        }

        public string LoadEC(IntPtr kmapi)
        {                   
            Connect(kmapi, "10.155.151.60", "5001");
            Thread.Sleep(2000);
            Send(kmapi, "killtask STARTPRG.prg");
            Send(kmapi, "killtask ec_setup.prg");
            Send(kmapi, "killtask ecconfig.prg");
            Send(kmapi, "killtask ax_setup.prg");
            Send(kmapi, "reset tasks");
            Thread.Sleep(1000);
            Send(kmapi, "reset all");
            Thread.Sleep(1000);            
            Execute(kmapi, "load StartPRG.prg");
            Thread.Sleep(30000);
            return ("Loaded files EC");
        }

        private void IsMoveTestAllAxis(IntPtr kmapi, int nmb_axis)
        {
            //string ismove_value_a1 = Execute(kmapi, "?a1.ismoving").Substring(0, 1);
            //string ismove_value_a2 = Execute(kmapi, "?a2.ismoving").Substring(0, 1);

            bool move_st = true;
            bool[] move_st_arr = new bool[nmb_axis];
            string[] is_move = new string[nmb_axis];

            for (int i = 0; i < nmb_axis; i++)
            {
                is_move[i] = Execute(kmapi, "?a" + (i + 1).ToString() + ".ismoving").Substring(0, 1);
                Thread.Sleep(200);
            }

            while (move_st)
            {
                for (int k = 0; k < nmb_axis; k++)
                {
                    if (!is_move[k].Equals("0"))
                    {
                        move_st_arr[k] = false;
                        break;
                    }
                    else
                    {
                        move_st_arr[k] = true;
                    }
                }

                foreach (bool key in move_st_arr) // Check the status of motion for all axis
                {
                    if (key)
                    {
                        move_st = true;
                        break;
                    }
                    else
                    {
                        move_st = false;
                    }
                }

                for (int i = 0; i < nmb_axis; i++)
                {
                    is_move[i] = Execute(kmapi, "?a" + (i + 1).ToString() + ".ismoving").Substring(0, 1);
                    Thread.Sleep(200);
                }

                

            }

        }

        private bool IsActiveTestForGroup(IntPtr kmapi, string port, int num_axis)
        {
            bool flag = true;
            for(int axis =1; axis< num_axis+1; axis++)
            {
                SendSerialPort(port, "\\" + axis.ToString(), false);

                string active_axis_st = SendSerialPort(port, "active", true).Substring(0, 1);
                string oe_bit = Execute(kmapi, "?(ec_pdo_read(" + axis.ToString() + ", 0x6041, 0) band 2^2) shr 2").Substring(0, 1);

                if (!active_axis_st.Equals("1") || !oe_bit.Equals("1"))
                {
                    flag = false;
                    return (flag);
                }
            }
            
            return (flag);
        }

        private bool IsActiveTestForAxis(IntPtr kmapi, string port, string num_axis)
        {
            SendSerialPort(port, "\\" + num_axis, false);

            string active_axis_st = SendSerialPort(port, "active", true).Substring(0, 1);
            string oe_bit = Execute(kmapi, "?(ec_pdo_read(" + num_axis + ", 0x6041, 0) band 2^2) shr 2").Substring(0, 1);

            if (active_axis_st.Equals("1") || oe_bit.Equals("1")) return (true);
            return (false);
        }

        public string[] EnableDisableTest(IntPtr kmapi, string port, int nmb_test_cases, int number_axis)
        {

            string[] status_test_arr = new string[nmb_test_cases];

            for(int inx = 0; inx < number_axis; inx++)
            {
                Send(kmapi, "a" + Convert.ToString(inx + 1) + ".en=1");
                if (!IsActiveTestForAxis(kmapi, port, Convert.ToString(inx + 1)))
                {
                    MessageBox.Show("Axis: " + Convert.ToString(inx + 1) + " is not active");
                    status_test_arr[0] = "FAIL";
                    return (status_test_arr); //TODO: need to add check array results lenght
                }

            }
                                                          
            status_test_arr[0] = "PASS";
            Thread.Sleep(100);
            status_test_arr[1] = "PASS";
            Thread.Sleep(100);
            
            Send(kmapi, "a1.en=0");
            Send(kmapi, "a2.en=0");

            Thread.Sleep(100);
            //Disconnect(kmapi);

            //Thread.Sleep(3000);
            return (status_test_arr);
            
        }

        public string[] MotionTest(IntPtr kmapi, string port, int nmb_test_cases, int number_axis)
        {

            string[] status_test_arr = new string[nmb_test_cases];

            for (int inx = 0; inx < number_axis; inx++)
            {
                Send(kmapi, "a" + Convert.ToString(inx + 1) + ".en=1");
                if (!IsActiveTestForAxis(kmapi, port, Convert.ToString(inx + 1)))
                {
                    MessageBox.Show("Axis: " + Convert.ToString(inx + 1) + " is not active");
                    status_test_arr[0] = "FAIL";
                    return (status_test_arr); //TODO: need to add check array results lenght
                }

            }

            Send(kmapi, "sys.motion=1");
            status_test_arr[0] = "PASS";
            Thread.Sleep(100);
            status_test_arr[1] = "PASS";
            Thread.Sleep(100);

            for (int i = 0; i < 5; i++)
            {
                Send(kmapi, "jog a1 10 starttype=1");
                Send(kmapi, "jog a2 10 starttype=1");
                Thread.Sleep(1000);
                IsMoveTestAllAxis(kmapi, number_axis);

                Send(kmapi, "jog a1 -10 starttype=1");
                Send(kmapi, "jog a2 -10 starttype=1");
                Thread.Sleep(1000);
                IsMoveTestAllAxis(kmapi, number_axis);
            }

            Send(kmapi, "jog a1 10");
            Thread.Sleep(10000);

            Send(kmapi, "stop a1");
            Send(kmapi, "stop a2");
            Send(kmapi, "a1.en=0");
            Send(kmapi, "a2.en=0");

            //Thread.Sleep(3000);
            //Disconnect(kmapi);
            status_test_arr[2] = "PASS";
            return (status_test_arr);

            //AddToLog(Disconnect(kmapi));
            //MessageBox.Show(Disconnect(kmapi));

        }

        public void ConnectionTest(IntPtr kmapi)
        {
            //MainWindow win = new MainWindow();
            //win.AddToLog(Connect(kmapi, "10.155.151.55", "5001"));
            //MessageBox.Show(Disconnect(kmapi));
            Send(kmapi, "a1.en=1".ToString());
            Thread.Sleep(1000);
            MessageBox.Show("drive should be enabled");
            Thread.Sleep(1000);
            Send(kmapi, "jog a1 -1".ToString());
            Thread.Sleep(10000);
            Send(kmapi, "stop a1".ToString());
            Send(kmapi, "a1.en=0".ToString());
            Thread.Sleep(1000);
            Execute(kmapi, Disconnect(kmapi));

        }
        public string SerialTest(string port)
        {
            //SendSerialPort(port, "commode 1", false);

            //MessageBox.Show(spl);
            //System.IO.File.WriteAllText(@"C:\1\test.txt", SendSerialPort(port, "get"));
            //Thread.Sleep(3000);

            return ("PASS");
           
        }
        public void DispalyTest()
        {
            
            //string str = "this is a test";
            //SendMessage(str);
            MessageBox.Show("Done!");
        }
        public void SendMessage(string st, Window1 objWindow)
        {

            //Window1 objWindow = new Window1();
            //objWindow.Show();
            objWindow.winwin.Text += st;
        }
    }

    public class BoolStringClass
    {
        public string TheText { get; set; }
        public bool IsSelected { get; set; }
    }

}
