using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guessing_Game
{
    public partial class ReadInformation : Form
    {
        public string Answer { get; set; }

        public ReadInformation(string question)
        {
            InitializeComponent();

            lblQuestion.Text = question;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Answer = txtAnswer.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
