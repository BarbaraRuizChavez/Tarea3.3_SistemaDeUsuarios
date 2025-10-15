using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SistemaDeUsuarios
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            string conexion = "server=localhost;port=3306;database=sistemaUsuarios;user=root;password=b2r4c6h8;SslMode=Required;";


            using (MySqlConnection cn = new MySqlConnection(conexion))
            {
                try
                {
                    cn.Open();
                    string consulta = "SELECT nombre, apellidos, usuario, correo FROM usuarios";
                    MySqlCommand cmd = new MySqlCommand(consulta, cn);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    dgvUsuarios.Rows.Clear();
                    while (dr.Read())
                    {
                        dgvUsuarios.Rows.Add(dr["nombre"], dr["apellidos"], dr["usuario"], dr["correo"]);
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar: " + ex.Message);
                }
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            Registro reg = new Registro();
            reg.ShowDialog();
        }

        private void ExportarDataGridViewAExcel(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Archivo CSV (*.csv)|*.csv|Archivo Excel (*.xls)|*.xls|Archivo Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "DatosExportados";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        string[] columnNames = new string[dgv.Columns.Count];
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            columnNames[i] = dgv.Columns[i].HeaderText;
                        }
                        sb.AppendLine(string.Join(",", columnNames));

                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string[] cells = new string[dgv.Columns.Count];
                                for (int i = 0; i < dgv.Columns.Count; i++)
                                {
                                    cells[i] = row.Cells[i].Value?.ToString().Replace(",", ";") ?? ""; // Evita conflictos con comas
                                }
                                sb.AppendLine(string.Join(",", cells));
                            }
                        }

                        // Guardar archivo
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewAExcel(dgvUsuarios);
        }
    }
}

