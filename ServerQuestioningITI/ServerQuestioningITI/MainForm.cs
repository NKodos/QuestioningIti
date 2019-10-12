using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ServerQuestioningITI
{
    public partial class MainForm : Form
    {
        private TcpClient _tcpClient = new TcpClient();
        private TcpListener _tcpListner;

        static string _jsonData = "";
        static QueryToDb _q1 = new QueryToDb();

        public MainForm()
        {
            InitializeComponent();
        }

        // Кнопка Запустить
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartBackThread();

            txtLog.Text += DateTime.Now.ToString("hh:mm:ss") + " Сервер запущен" + Environment.NewLine;
        }

        // Кнопка Остановить
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopBackThread();

            btnStop.Enabled = false;
            btnStart.Enabled = true;
            txtLog.Text += DateTime.Now.ToString("hh:mm:ss") + " Сервер остановлен" + Environment.NewLine;
        }

        // При закрытии формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopBackThread();
        }

        // Запустить фоновый поток
        private void StartBackThread()
        {
            BackgroundThreadMethodAsync();

            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        // Остановить фоновый поток
        private void StopBackThread()
        {
            _tcpListner.Stop();
            _tcpClient.Close();
        }

        async private void BackgroundThreadMethodAsync()
        {
            await Task.Run(() => BackgroundThreadMethod());
        }

        // Фоновый поток
        private void BackgroundThreadMethod()
        {
            try
            {
                _tcpListner = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000));
                _tcpListner.Start();
                while (true)
                {
                    _tcpClient = _tcpListner.AcceptTcpClient();

                    StreamReader sr = new StreamReader(_tcpClient.GetStream());
                    _jsonData = sr.ReadLine();
                    Invoke(new Action(() =>
                    {
                        txtLog.Text += DateTime.Now.ToString("hh:mm:ss") + " Получен строка в json: " + _jsonData + Environment.NewLine;
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.ScrollToCaret();
                    }));

                    _q1 = JsonConvert.DeserializeObject<QueryToDb>(_jsonData);
                    string answer = _q1.RunQuery();

                    StreamWriter sw = new StreamWriter(_tcpClient.GetStream()) {AutoFlush = true};
                    sw.WriteLine(answer);
                    Invoke(new Action(() =>
                    {
                        txtLog.Text += DateTime.Now.ToString("hh:mm:ss") + " Запрос клиента выполнен и был передан ответ" + Environment.NewLine;
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.ScrollToCaret();
                    }));

                    _tcpClient.Close();
                }
            }
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    txtLog.Text += DateTime.Now.ToString("hh:mm:ss") + " Ошибка: " + ex + Environment.NewLine;
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.ScrollToCaret();
                    StopBackThread();
                    StartBackThread();
                }));
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
