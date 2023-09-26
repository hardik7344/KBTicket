using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KBAPI.BusinessLogic
{
    public class TecRedisTCP
    {
        public class ResponseException : Exception
        {
            public ResponseException(string code) : base("Response error")
            {
                Code = code;
            }
            public string Code { get; private set; }
        }
        Socket socket;
        BufferedStream bstream;
        public string Host { get; private set; }
        public int Port { get; private set; }
        public int SendTimeout { get; set; }

        byte[] end_data = new byte[] { (byte)'\r', (byte)'\n' };

        int db;
        public int Db
        {
            get
            {
                return db;
            }
            set
            {
                db = value;
                SendExpectSuccess("SELECT", db);
            }
        }
        void SendExpectSuccess(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            ExpectSuccess();
        }
        public TecRedisTCP(string host, int port)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            Host = host;
            Port = port;
            SendTimeout = -1;
        }

        public TecRedisTCP(string host) : this(host, 6379)
        {
        }

        public TecRedisTCP() : this("localhost", 6379)
        {
        }

        public void Init(string ServerIPAddress, int ServerPort, int DatabaseNumber)
        {
            Host = ServerIPAddress;
            Port = ServerPort;
            db = DatabaseNumber;
        }
        public void Set(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            Set(key, Encoding.UTF8.GetBytes(value));
        }
        public void Set(string key, byte[] value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length > 1073741824)
                throw new ArgumentException("value exceeds 1G", "value");

            if (!SendDataCommand(value, "SET", key))
                throw new Exception("Unable to connect");
            ExpectSuccess();
        }
        bool SendDataCommand(byte[] data, string cmd, params object[] args)
        {
            string resp = "*" + (1 + args.Length + 1).ToString() + "\r\n";
            resp += "$" + cmd.Length + "\r\n" + cmd + "\r\n";
            foreach (object arg in args)
            {
                string argStr = arg.ToString();
                int argStrLength = Encoding.UTF8.GetByteCount(argStr);
                resp += "$" + argStrLength + "\r\n" + argStr + "\r\n";
            }
            resp += "$" + data.Length + "\r\n";

            return SendDataRESP(data, resp);
        }
        bool SendDataRESP(byte[] data, string resp)
        {
            if (socket == null)
                Connect();
            if (socket == null)
                return false;

            byte[] r = Encoding.UTF8.GetBytes(resp);
            try
            {
                //Log("C", resp);
                socket.Send(r);
                if (data != null)
                {
                    socket.Send(data);
                    socket.Send(end_data);
                }
            }
            catch (SocketException)
            {
                // timeout;
                socket.Close();
                socket = null;

                return false;
            }
            return true;
        }
        public byte[] Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectData("GET", key);
        }

        public string GetString(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return Encoding.UTF8.GetString(Get(key));
        }
        byte[] SendExpectData(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            return ReadData();
        }
        bool SendCommand(string cmd, params object[] args)
        {
            if (socket == null)
                Connect();
            if (socket == null)
                return false;

            string resp = "*" + (1 + args.Length).ToString() + "\r\n";
            resp += "$" + cmd.Length + "\r\n" + cmd + "\r\n";
            foreach (object arg in args)
            {
                string argStr = arg.ToString();
                int argStrLength = Encoding.UTF8.GetByteCount(argStr);
                resp += "$" + argStrLength + "\r\n" + argStr + "\r\n";
            }

            byte[] r = Encoding.UTF8.GetBytes(resp);
            try
            {
                socket.Send(r);
            }
            catch (SocketException)
            {
                // timeout;
                socket.Close();
                socket = null;

                return false;
            }
            return true;
        }
        byte[] ReadData()
        {
            string s = ReadLine();
            if (s.Length == 0)
                throw new ResponseException("Zero length respose");

            char c = s[0];
            if (c == '-')
                throw new ResponseException(s.StartsWith("-ERR ") ? s.Substring(5) : s.Substring(1));

            if (c == '$')
            {
                if (s == "$-1")
                    return null;
                int n;

                if (Int32.TryParse(s.Substring(1), out n))
                {
                    byte[] retbuf = new byte[n];

                    int bytesRead = 0;
                    do
                    {
                        int read = bstream.Read(retbuf, bytesRead, n - bytesRead);
                        if (read < 1)
                            throw new ResponseException("Invalid termination mid stream");
                        bytesRead += read;
                    }
                    while (bytesRead < n);
                    if (bstream.ReadByte() != '\r' || bstream.ReadByte() != '\n')
                        throw new ResponseException("Invalid termination");
                    return retbuf;
                }
                throw new ResponseException("Invalid length");
            }
            throw new ResponseException("Unexpected reply: " + s);
        }
        string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            int c;

            while ((c = bstream.ReadByte()) != -1)
            {
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                sb.Append((char)c);
            }
            return sb.ToString();
        }
        void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            socket.SendTimeout = SendTimeout;
            socket.Connect(Host, Port);
            if (!socket.Connected)
            {
                socket.Close();
                socket = null;
                return;
            }
            bstream = new BufferedStream(new NetworkStream(socket), 16 * 1024);

            //if (Password != null)
            //    SendExpectSuccess("AUTH", Password);
        }
        void ExpectSuccess()
        {
            int c = bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();
            //Log("S", (char)c + s);
            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR ") ? s.Substring(4) : s);
        }
        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("DEL", key) == 1;
        }
        int SendExpectInt(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            int c = bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();
            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR ") ? s.Substring(4) : s);
            if (c == ':')
            {
                int i;
                if (int.TryParse(s, out i))
                    return i;
            }
            throw new ResponseException("Unknown reply on integer request: " + c + s);
        }
    }
}
