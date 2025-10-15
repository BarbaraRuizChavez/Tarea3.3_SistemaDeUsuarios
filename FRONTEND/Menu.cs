using SistemaDeUsuarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SistemaDeUsuarios
{
    public partial class Menu : Form
    {
        private readonly UsuarioRepository usuarioRepo;

        public Menu()
        {
            InitializeComponent();
            usuarioRepo = new UsuarioRepository();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                dgvUsuarios.Rows.Clear();
                List<Usuario> usuarios = usuarioRepo.ObtenerUsuarios();

                foreach (var u in usuarios)
                {
                    dgvUsuarios.Rows.Add(u.Nombre, u.Apellidos, u.NombreUsuario, u.Correo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            Registro reg = new Registro();
            reg.ShowDialog();
        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewAExcel(dgvUsuarios);
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
                                    cells[i] = row.Cells[i].Value?.ToString().Replace(",", ";") ?? "";
                                }
                                sb.AppendLine(string.Join(",", cells));
                            }
                        }

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
    }
}


