using ProjectXPTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectXpto.View
{
    public partial class Detalhe : Form
    {
        public Detalhe()
        {
            InitializeComponent();
        }

        private Cliente cliente;

        public Detalhe(Cliente cliente)
        {
            InitializeComponent();

            this.cliente = cliente;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            lblNome.Text = cliente.DataNascimento.ToString("dd/MM/yyyy HH:mm:ss");
            lblNascimento.Text = cliente.DataNascimento.ToString("dd/MM/yyy");
            lblCpf.Text = cliente.Cpf;

        }

        
    }
}
