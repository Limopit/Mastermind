using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mastermind_Client;
using Mastermind_Coder_Client;

namespace Mastermind
{
    public partial class MysteryForm : Form
    {
        private Color color;
        private TaskCompletionSource<bool> tcs;
        private object syncObject = new object();
        public MysteryForm()
        {
            InitializeComponent();
        }
        
        private async Task chooseColor() //Обработка окрашивания кнопок
        {
            
            EventHandler eventHandler = null;
            eventHandler = (s, e) =>
            {
                lock (syncObject)
                {
                    eventHandler -= colorButton_Click;
                    tcs.SetResult(true);
                }

            };
            eventHandler += colorButton_Click;
        }
        private void colorButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            color = button.BackColor;
        }
        
        private async void pagButton_Click(object sender, EventArgs e)
        {
            PagButton button = (PagButton)sender;
            tcs = new TaskCompletionSource<bool>();
            
            await chooseColor();
            button.BackColor = color;
        }

        private void button7_Click(object sender, EventArgs e) // Составление кода
        {
            if (pagButton97.BackColor.Name == "Transparent" ||
                pagButton98.BackColor.Name == "Transparent" ||
                pagButton99.BackColor.Name == "Transparent" ||
                pagButton100.BackColor.Name == "Transparent")
            {
                MessageBox.Show("Код составлен неверно", "Внимание");
            }
            else
            {
                CoderForm form = (CoderForm)Application.OpenForms[1];
                form.SetPagButton1(pagButton100);
                form.SetPagButton2(pagButton99);
                form.SetPagButton3(pagButton98);
                form.SetPagButton4(pagButton97);
                form.Enabled = true;
                Close();
            }
        }
    }
}