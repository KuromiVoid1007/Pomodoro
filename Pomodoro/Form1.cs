using System.Media;
using System.Windows.Forms;


namespace Pomodoro
{
    public partial class Form1 : Form
    {
        private int timeLeft; // Время в секундах

        public Form1()
        {
            InitializeComponent();
            UpdateTimeDisplay();


            // Настройка NotifyIcon
            notifyIcon1.Icon = Properties.Resources.favicon; // Укажите свой файл иконки
            notifyIcon1.Text = "Мое приложение"; // Подсказка при наведении на иконку
            notifyIcon1.Visible = false; // Иконка не видна по умолчанию

            // Настройка контекстного меню
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Открыть", null, ShowForm);
            contextMenu.Items.Add("Выход", null, ExitApplication);
            notifyIcon1.ContextMenuStrip = contextMenu;

            // Обработка события двойного щелчка по иконке
            notifyIcon1.DoubleClick += ShowForm;
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            InitializeCountdown(1500); // Запуск таймера на 60 секунд по нажатию кнопки
        }

        private void InitializeCountdown(int seconds)
        {
            timeLeft = seconds;
            timer1.Interval = 1000; // 1000 миллисекунд = 1 секунда
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            UpdateTimeDisplay();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--; // Уменьшаем время
                UpdateTimeDisplay();
            }
            else
            {
                PlaySound();
                timer1.Stop();
                MessageBox.Show("Время вышло!");
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
                using (SoundPlayer player = new SoundPlayer(Pomodoro.Properties.Resources.zvon)) // Замените "alert.wav" на путь к вашему файлу
                {
                    player.Play(); // Воспроизведение звука
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
            }
        }

        // Метод для показа формы
        private void ShowForm(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        // Метод для выхода из приложения
        private void ExitApplication(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        // Событие Resize для сворачивания в трей
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // Скрыть форму
                notifyIcon1.Visible = true; // Показать иконку в трее
                notifyIcon1.ShowBalloonTip(1000, "Приложение свернуто", "Нажмите, чтобы развернуть", ToolTipIcon.Info);
            }
        }

    }
}
