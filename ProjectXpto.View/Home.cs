using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectXPTO.Controller.Services;
using ProjectXPTO.Model;

namespace ProjectXpto.View
{
    public partial class Home : Form
    {
        private ClienteService service;

        private List<Cliente> clientes;

        private Cliente cliente;

        private bool inserir;
        private bool alterar;


        public Home()
        {

            InitializeComponent();
            service = new ClienteService();

            inserir = false;
            alterar = false;

            StatusCampoBotao();
            StatusCampoTexto(false);

            InicializarListView();
            CarregarListView();

        }
        private void LimparCampoTexto()
        {
            txtNome.Clear();
            txtCpf.Clear();
            txtNascimento.Clear();
            txtEmail.Clear();
        }
        private void StatusCampoTexto(bool flag)
        {
            txtNome.Enabled = flag;
            txtCpf.Enabled = flag;
            txtNascimento.Enabled = flag;
            txtEmail.Enabled = flag;
        }
        private void StatusCampoBotao()
        {
            btnNovo.Enabled = !inserir && !alterar;
            btnAlterar.Enabled = !inserir && !alterar;
            btnExibir.Enabled = !inserir && !alterar;
            btnExcluir.Enabled = !inserir && !alterar;
            btnSalvar.Enabled = !inserir || !alterar;
            btnCancelar.Enabled = !inserir || !alterar;
        }
        private void acaocomum()
        {
            alterar = false;
            inserir = false;

            StatusCampoBotao();
            StatusCampoTexto(false);
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            inserir = true;

            StatusCampoBotao();
            StatusCampoTexto(true);
            LimparCampoTexto();

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            alterar = true;

            StatusCampoBotao();
            StatusCampoTexto(true);

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (inserir)
            {
                service.Inserir(new Cliente
                {
                    DataNascimento = DateTime.ParseExact(txtNascimento.Text, "dd/WW/yyyy", CultureInfo.InvariantCulture),
                    Cpf = txtCpf.Text,
                    Nome = txtNome.Text,
                    Email =txtEmail.Text
                });
            }
            else if (alterar)
            {
                cliente.Nome = txtNome.Text;
                cliente.Cpf = txtCpf.Text;
                cliente.DataNascimento = DateTime.ParseExact(txtNascimento.Text, "dd/WW/yyyy", CultureInfo.InvariantCulture);
                cliente.Email = txtEmail.Text;

                service.Alterar(cliente);
            }
            acaocomum();

            CarregarListView();
        }

        private void btnExibir_Click(object sender, EventArgs e)
        {
            acaocomum();

            var detalhe = new Detalhe(cliente);
            detalhe.ShowDialog();

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            service.Deletar(cliente.Id);

            acaocomum();

            CarregarListView();

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            acaocomum();
        }
        private void InicializarListView()
        {
            listView1.View = System.Windows.Forms.View.Details;
            listView1.LabelEdit = false;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("Cpf", 100, HorizontalAlignment.Right);
            listView1.Columns.Add("Nome", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("Data de Nascimento", 150, HorizontalAlignment.Right);
            listView1.Columns.Add("Email", 100, HorizontalAlignment.Right);
        }

        private void CarregarListView()
        {
            clientes = service.RecuperarpoTodos();

            listView1.Items.Clear();

            for (int i = 0; i < clientes.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(clientes[i].Cpf);
                lvi.SubItems.Add(clientes[i].Nome);
                lvi.SubItems.Add(clientes[i].Nome);
                lvi.SubItems.Add(clientes[i].DataNascimento.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(clientes[i].Email);

                listView1.Items.Add(lvi);
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView1.SelectedItems[0];

            cliente = clientes.Find(f => f.Id == Convert.ToInt32(item.SubItems[0]));

            txtNome.Text = cliente.Nome;
            txtCpf.Text = cliente.Cpf;
            txtNascimento.Text = cliente.DataNascimento.ToString("dd/WW/yyyy");
            txtEmail.Text = cliente.Email;

        }


    }



}
