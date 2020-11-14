using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CapaModelo;

namespace CapaVista
{
    public partial class frmProducto : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;
        private static frmProducto _Instancia;

        public static frmProducto GetInstance()
        {
            if (_Instancia == null)
                _Instancia = new frmProducto;
            return _Instancia;
        }

        public void SetCategoria(string IdCategoria, string nombre)
        {
            this.txtIdCategoria.Text = IdCategoria;
            this.txtCategoria.Text = nombre;
        }

        public frmProducto()
        {
            InitializeComponent();
            ttMensaje.SetToolTip(this.txtID, "Ingrese el ID del Producto");
            ttMensaje.SetToolTip(this.txtNombre, "Ingrese el Nombre");
            ttMensaje.SetToolTip(this.txtPrecio, "Ingrese el Precio");
            ttMensaje.SetToolTip(this.txtTipo, "Ingrese el Tipo");
            this.txtIdCategoria.ReadOnly = true;
            this.txtCategoria.ReadOnly = true;
            this.ComboPresentacion();
            this.Consultar();
        }

        public void MensajeConfirmacion(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Limpiar()
        {
            this.txtIdProducto.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.txtPrecio.Text = string.Empty;
            this.txtPrecio.Text = string.Empty;
        }

        public void Habilitar(bool valor)
        {
            this.txtIdProducto.ReadOnly = !valor;
            this.txtNombre.ReadOnly = !valor;
            this.txtPrecio.ReadOnly = !valor;
            this.txtTipo.ReadOnly = !valor;
            this.btnMostrar.Enabled = valor;
            this.btnLimpiar.Enabled = valor;
        }

        public void Buttons()
        {
            if (IsNuevo || IsEditar)
            {
                Habilitar(true);
                this.btnAgregar.Enabled = true;
                this.btnEditar.Enabled = true;
            }
            else
            {
                Habilitar(false);
                this.btnAgregar.Enabled = false;
                this.btnEditar.Enabled = false;
            }
        }

        public void HideColumns()
        {
            this.dataLista.Columns[0].Visible = false;
        }

        public void Consultar()
        {
            this.dataListado.DataSource = NProducto.Consultar();
            this.HideColumns();
            lblTotal.Text = "Total de Registros: " + Convert.ToString(this.dataLista.Rows.Count);
        }

        private void ComboPresentacion()
        {
            cbIdPresentacion.DataSource = NPresentacion.Consultar();
            cbIdPresentacion.ValueMember = "IdPresentacion";
            cbIdPresentacion.DisplayMember = "Nombre";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.Habilitar(false);
            this.Buttons();
            this.Consultar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.ConsultarNombre();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            this.IsNuevo = true;
            this.IsEditar = false;
            this.Buttons();
            this.Limpiar();
            this.Habilitar(true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string rpta = string.Empty;
                if (this.txtNombre.Text == string.Empty || this.txtIdProducto.Text == string.Empty || this.txtPrecio.Text == string.Empty || this.txtTipo.Text == string.Empty)
                {
                    MensajeError("Ingresar los datos faltantes");
                    this.ErrorIcono.SetError(txtIdProducto, "Ingresar datos");
                    this.ErrorIcono.SetError(txtNombre, "Ingresar datos");
                    this.ErrorIcono.SetError(txtPrecio, "Ingresar datos");
                    this.ErrorIcono.SetError(txtTipo, "Ingresar datos");
                }

                if (this.IsNuevo)
                {
                    rpta = NProducto.Insertar(
                        this.txtCodigoVenta.Text.Trin().ToUpper(),
                        this.txtNombre.Text.Trim().ToUpper(),
                        Convert.ToInt32(this.txtIdProducto.Text.Trim().ToUpper()),
                        Convert.ToInt32(this.cbIdPresentacion.SelectedValue));
                }
                else
                {
                    if (this.IsEditar)
                    {
                        rpta = NProducto.Modificar(Convert.ToInt32(this.txtIdProducto.Text.Trim()),
                            this.txtCodigoVenta.Text.Trim().ToUpper(),
                            this.txtNombre.Text.Trim().ToUpper(),
                            Convert.ToInt32(this.txtIdProducto.Text.Trim().ToUpper()),
                            Convert.ToInt32(this.cbIdPresentacion.SelectedValue));
                    }
                }
                if (rpta.Equals("OK"))
                {
                    if (this.IsNuevo)
                        this.MensajeConfirmacion("Registro Exitoso");
                    else if (this.IsEditar)
                        this.MensajeConfirmacion("Actualización Exitosa");
                }
                else
                    this.MensajeError(rpta);
                this.IsNuevo = false;
                this.IsEditar = false;
                this.Buttons();
                this.Limpiar();
                this.Consultar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnEditar_Click(Object sender, EventArgs e)
        {
            if(!txtIdProducto.Text.Equals(""))
            {
                this.IsEditar = true;
                this.Buttons();
                this.Habilitar(true);
            }
            else
            {
                this.MensajeError("Seleccione el registro a editar");
                this.ErrorIcono.SetError(this.txtIdCategoria, "Seleccione un registro");
            }
        }

        private void btnCancelar_Click(Object sender, EventArgs e)
        {
            this.IsNuevo = false;
            this.IsEditar = false;
            this.Buttons();
            this.Limpiar();
            this.Habilitar(false);
        }

        private void btnEliminar_Click(Object sender, EventArgs e)
        {
            try
            {
                DialogResult opt;
                opt = MessageBox.Show("¿Eliminar los Registros?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (opt == DialogResult.OK)
                {
                    string code;
                    string rpta = "";
                    foreach (DataGridViewRow row in dataLista.Rows)
                    {
                        if(Convert.ToBoolean(row.Cells[0].Value))
                        {
                            code = Convert.ToString(row.Cells[1].Value);
                            rpta = NProducto.Eliminar(Convert.ToInt32(code));
                            if (rpta.Equals("OK"))
                                this.MensajeConfirmacion("Eliminado");
                            else
                                this.MensajeError(rpta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            this.Consultar();
        }   
    }
}
