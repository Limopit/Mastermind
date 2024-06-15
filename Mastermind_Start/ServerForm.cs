using System;
using System.Windows.Forms;

namespace Mastermind_Start
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.context.MainForm = new ChoiseForm(comboBox1.Text);
            Close();
            Program.context.MainForm.Show();
        }
    }
}