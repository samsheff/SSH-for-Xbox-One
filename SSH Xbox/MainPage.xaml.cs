using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Renci.SshNet;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SSH_Xbox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SshClient client;
        ShellStream shell;

        string host = "";
        string username = "";
        string password = "";
        bool connected = false;

        public MainPage()
        {
            this.InitializeComponent();

            this.textBox.Focus(FocusState.Programmatic);

            this.textBox.Text += "Host: ";

            //IInputStream inputStream = this.shell.AsInputStream();
            //inputStream.
        }

        private void ConnectToHost()
        {
            this.client = new SshClient(host, username, password);
            this.textBox.Text += "\nConnecting to Server...";
            this.client.Connect();
            this.textBox.Text += "\nConnected! Opening Shell Stream...";
            this.shell = this.client.CreateShellStream("cygwin", 256, 512, (uint)((Frame)Window.Current.Content).ActualWidth, (uint)((Frame)Window.Current.Content).ActualHeight, 512);

            this.connected = true;

            readLoop();
        }

        private async void readLoop()
        {
            while (true)
            {
                if (this.shell.Length > 0)
                {
                    var result = new byte[this.shell.Length];
                    await this.shell.ReadAsync(result, 0, (int)this.shell.Length);

                    string console = System.Text.Encoding.ASCII.GetString(result);

                    this.textBox.Text = console.GetLastLines(GetNumberOfVerticalLines());
                } else
                {
                    await Task.Delay(100);
                }
            }
        }

        private int GetNumberOfVerticalLines()
        {
            return (int)((((Frame)Window.Current.Content).ActualHeight - textBox.ActualHeight) / 20);
        }

        private void textBox1_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                if (connected == false)
                {
                    if (host == "")
                    {
                        var lines = GetNumberOfVerticalLines();
                        host = textBox1.Text;
                        textBox1.Text = "";
                        textBox.Text += "\nUsername: ";
                    } else if (username == "") {
                        username = textBox1.Text;
                        textBox1.Text = "";
                        textBox.Text += "\nPassword: ";
                    } else if (password == "")
                    {
                        password = textBox1.Text;
                        textBox1.Text = "";
                        ConnectToHost();
                    }
                } else
                {
                    string cmdToExec = textBox1.Text;
                    textBox.Text += "\n";
                    textBox1.Text = "";

                    this.shell.WriteLine(cmdToExec);
                }

            }
        }
    }
}
