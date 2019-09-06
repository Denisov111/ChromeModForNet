using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using System.IO;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UsefulThings
{
    public enum Anonimity
    {
        Unknow,
        None,
        Low,
        High
    }

    public class Proxy : INotifyPropertyChanged
    {


        public bool isGoodVKAccess;
        public Anonimity anonimity;

        public int Id { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; } = "1";
        public bool IsGoodVKAccess
        {
            get { return isGoodVKAccess; }
            set
            {
                isGoodVKAccess = value;
                OnPropertyChanged("IsGoodVKAccess");
                OnPropertyChanged("IsGoodVKAccessView");
            }
        }
        public string IsGoodVKAccessView
        {
            get
            {
                return (IsGoodVKAccess) ? "есть" : "нет";
            }
        }
        public bool IsAnonimous { get; set; }
        public Anonimity Anonimity
        {
            get { return anonimity; }
            set
            {
                anonimity = value;
                OnPropertyChanged("Anonimity");
            }
        }
        public static string OwnIP { get; set; }
        public static Dictionary<string, bool> Revealingheader { get; set; }
        public string Url
        {
            get { return "http://" + Ip + ":" + Port; }
        }


        public Proxy() { }

        public Proxy(int id, string ip, string port, string login, string pwd, string type, string status, string mode)
        {
            Id = id;
            Ip = ip;
            Port = port;
            Login = login;
            Pwd = pwd;
            Type = type;
            Status = status;
            Mode = mode;
        }

        public Proxy(string proxyString)
        {
            if (proxyString.Contains('@'))
            {
                string[] s = proxyString.Split('@');
                if (s.Length < 2) throw new ArgumentException("Неверный формат строки прокси-сервера: " + proxyString);

                string[] ipPort = s[0].Split(':');
                if (ipPort.Length < 2) throw new ArgumentException("Неверный формат строки прокси-сервера: " + proxyString);
                Ip = ipPort[0];
                Port = ipPort[1];

                string[] loginPass = s[1].Split(':');
                if (loginPass.Length < 2) throw new ArgumentException("Неверный формат строки прокси-сервера: " + proxyString);
                Login = loginPass[0];
                Pwd = loginPass[1];
                if (loginPass.Count() > 2)
                {
                    Type = loginPass[2];
                }
                else
                {
                    Type = "HTTP";
                }
            }
            else
            {
                string[] s = proxyString.Split(':');
                if (s.Length != 2) throw new ArgumentException("Неверный формат строки прокси-сервера: " + proxyString);
                Ip = s[0];
                Port = s[1];
                Type = "HTTP";
            }
        }


        public Proxy(XElement el)
        {
            Console.WriteLine(el);
            Ip = el.Element("ip").Value;
            Port = el.Element("port").Value;
            Login = el.Element("login").Value;
            Pwd = el.Element("pwd").Value;
            Type = el.Element("type").Value;

        }

        public static StringCollection GetProxiesCol()
        {
            StringCollection proxiesCol = new StringCollection();

            string ProxiesFile = @"proxies.xml";
            XDocument doc = new XDocument();
            doc = XDocument.Load(ProxiesFile);
            foreach (XElement el in doc.Root.Elements("proxy"))
            {
                Console.WriteLine(el.Element("id").Value.ToString());
                if (el.Element("mode").Value.ToString() == "1")
                {
                    proxiesCol.Add(el.Element("ip").Value.ToString() + ":" + el.Element("port").Value.ToString() + "@" + el.Element("login").Value.ToString() + ":" + el.Element("pwd").Value.ToString() + ":" + el.Element("type").Value.ToString());
                }
            }

            return proxiesCol;
        }

        private void RevealingheaderInit()
        {
            // Hardcoded list of bad and good headers. 
            Revealingheader = new Dictionary<string, bool>()
            {
                {"HTTP_CACHE_CONTROL", true},
                {"HTTP_CDN_SRC_IP", true},
                {"HTTP_CLIENT_IP", true},
                {"HTTP_REFERER", true},
                {"HTTP_IF_NONE_MATCH", true},
                {"HTTP_IF_MODIFIED_SINCE", true},
                {"HTTP_MAX_FORWARDS", true},
                {"HTTP_OCT_MAX_AGE", true},
                {"HTTP_PROXY_AGENT", true},
                {"HTTP_PROXY_CONNECTION", true},
                {"HTTP_VIA", true},
                {"HTTP_X_ACCEPT_ENCODING_PRONTOWIFI", true},
                {"HTTP_X_BLUECOAT_VIA", true},
                {"HTTP_X_FORWARDED_FOR", true},
                {"HTTP_X_FORWARD_FOR", true},
                {"HTTP_X_FORWARDED_HOST", true},
                {"HTTP_X_FORWARDED_SERVER", true},
                {"HTTP_X_MATO_PARAM", true},
                {"HTTP_X_NAI_ID", true},
                {"HTTP_X_PROXY_ID", true},
                {"HTTP_X_REAL_IP", true},
                {"HTTP_X_VIA", true},
                {"HTTP_XCNOOL_REMOTE_ADDR", true},
                {"HTTP_XROXY_CONNECTION", true},
                {"HTTP_XXPECT", true},

                {"HTTP_ACCEPT", false},
                {"HTTP_ACCEPT_ENCODING", false},
                {"HTTP_ACCEPT_LANGUAGE", false},
                {"HTTP_CONNECTION", false},
                {"HTTP_HOST", false},
                {"HTTP_USER_AGENT", false},
                {"REMOTE_ADDR", false},
                {"REMOTE_PORT", false},
                {"REQUEST_METHOD", false},
                {"REQUEST_TIME", false},
                {"REQUEST_TIME_FLOAT", false},
                {"REQUEST_SCHEME", false},
                {"REQUEST_URI", false},

            };
        }


        public override string ToString()
        {
            return Ip + ":" + Port + "@" + Login + ":" + Pwd;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
