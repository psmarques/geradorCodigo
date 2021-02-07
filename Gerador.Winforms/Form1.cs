using Gerador.Core;
using Gerador.Winforms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerador.Winforms
{
    public partial class Form1 : Form
    {
        private GeradorHelper gerador;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gerador = new GeradorHelper();
            try
            {
                gerador.ConnectionString = txtConnString.Text;
                gerador.GetSchema();
                PopulateTables(gerador.GetTableNames());
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(gerador.ErrorMessage))
                    MessageBox.Show(this, gerador.ErrorMessage, "Error");
            }
        }

        private void PopulateTables(List<string> tables)
        {
            cmbTable.Items.Clear();

            foreach (var item in tables)
            {
                cmbTable.Items.Add(item);
            }
        }

        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtClassName.Text = cmbTable.Text.Replace("_", string.Empty);
            PopulateColumns(cmbTable.Text);
        }

        private void PopulateColumns(string table)
        {
            var r = gerador.GetColumns(table);
            dgColumns.DataSource = r;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lst = new List<TableField>();

            foreach (DataGridViewRow item in dgColumns.Rows)
            {
                lst.Add(new TableField(item.Cells[1].Value.ToString(),
                                       item.Cells[2].Value.ToString(),
                                       bool.Parse(item.Cells[4].Value.ToString()),
                                       int.Parse(item.Cells[5].Value.ToString())
                                       )
                    );
            }

            gerador.Namepsace = txtNamespace.Text;
            gerador.ClassName = txtClassName.Text;
            gerador.Tablefields = lst;

            //Generate DTO
            rtxtDto.Text = gerador.GerarDTO();
            rtxtController.Text = gerador.GerarWebApiController();
            rtxtFrontModel.Text = gerador.GerarModelTS();
            rtxtFrontService.Text = gerador.GerarFrontModelService();
            rtxtFrontHtml.Text = gerador.GerarFrontHtml();
            rtxtFrontComponent.Text = gerador.GerarFrontComponent();

            rtxtFrontGenericResult.Text = System.IO.File.ReadAllText("templates\\TemplateFrontGenericResult.txt");
            rtxtFrontLoader.Text = System.IO.File.ReadAllText("templates\\TemplateFrontLoaderComponent.ts.txt");
            rtxtFrontHttpinterceptor.Text = System.IO.File.ReadAllText("templates\\TemplateFrontHttpInterceptor.ts.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var i = new IniHelper();
            string connstr = i.Read(txtConnString.Name);
            string clsname = i.Read(txtClassName.Name);
            string nmspace = i.Read(txtNamespace.Name);

            txtConnString.Text = string.IsNullOrEmpty(connstr) ? txtConnString.Text : connstr;
            txtClassName.Text = string.IsNullOrEmpty(clsname) ? txtClassName.Text : clsname;
            txtNamespace.Text = string.IsNullOrEmpty(nmspace) ? txtNamespace.Text : nmspace;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var i = new IniHelper();

            i.Write(txtConnString.Name, txtConnString.Text);
            i.Write(txtClassName.Name, txtClassName.Text);
            i.Write(txtNamespace.Name, txtNamespace.Text);
        }
    }
}
