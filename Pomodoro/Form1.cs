using System.Media;
using System.Windows.Forms;


namespace Pomodoro
{
    public partial class Form1 : Form
    {
        private int timeLeft; // ����� � ��������

        public Form1()
        {
            InitializeComponent();
            UpdateTimeDisplay();


            // ��������� NotifyIcon
            notifyIcon1.Icon = Properties.Resources.favicon; // ������� ���� ���� ������
            notifyIcon1.Text = "��� ����������"; // ��������� ��� ��������� �� ������
            notifyIcon1.Visible = false; // ������ �� ����� �� ���������

            // ��������� ������������ ����
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("�������", null, ShowForm);
            contextMenu.Items.Add("�����", null, ExitApplication);
            notifyIcon1.ContextMenuStrip = contextMenu;

            // ��������� ������� �������� ������ �� ������
            notifyIcon1.DoubleClick += ShowForm;
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            InitializeCountdown(1500); // ������ ������� �� 60 ������ �� ������� ������
        }

        private void InitializeCountdown(int seconds)
        {
            timeLeft = seconds;
            timer1.Interval = 1000; // 1000 ����������� = 1 �������
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            UpdateTimeDisplay();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--; // ��������� �����
                UpdateTimeDisplay();
            }
            else
            {
                PlaySound();
                timer1.Stop();
                MessageBox.Show("����� �����!");
            }
        }

        private void UpdateTimeDisplay()
        {
            label1.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
        }

        private void PlaySound()
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer(Pomodoro.Properties.Resources.zvon)) // �������� "alert.wav" �� ���� � ������ �����
                {
                    player.Play(); // ��������������� �����
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ��������������� �����: " + ex.Message);
            }
        }

        // ����� ��� ������ �����
        private void ShowForm(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        // ����� ��� ������ �� ����������
        private void ExitApplication(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        // ������� Resize ��� ������������ � ����
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // ������ �����
                notifyIcon1.Visible = true; // �������� ������ � ����
                notifyIcon1.ShowBalloonTip(1000, "���������� ��������", "�������, ����� ����������", ToolTipIcon.Info);
            }
        }

    }
}
