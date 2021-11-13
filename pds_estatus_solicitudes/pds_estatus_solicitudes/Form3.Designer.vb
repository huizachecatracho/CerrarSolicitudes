<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExcepcion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtListaSolicitudes = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rtbLista = New System.Windows.Forms.RichTextBox()
        Me.btnVerificar = New System.Windows.Forms.Button()
        Me.btnEjecutar = New System.Windows.Forms.Button()
        Me.btnMasDe48Horas = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtListaSolicitudes
        '
        Me.txtListaSolicitudes.Location = New System.Drawing.Point(20, 30)
        Me.txtListaSolicitudes.Name = "txtListaSolicitudes"
        Me.txtListaSolicitudes.Size = New System.Drawing.Size(488, 20)
        Me.txtListaSolicitudes.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(491, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Escriba las solicitudes que no se van a cerrar separadas por una coma cada una y " &
    "de click en verificar"
        '
        'rtbLista
        '
        Me.rtbLista.Location = New System.Drawing.Point(20, 91)
        Me.rtbLista.Name = "rtbLista"
        Me.rtbLista.Size = New System.Drawing.Size(488, 353)
        Me.rtbLista.TabIndex = 2
        Me.rtbLista.Text = ""
        '
        'btnVerificar
        '
        Me.btnVerificar.Location = New System.Drawing.Point(20, 56)
        Me.btnVerificar.Name = "btnVerificar"
        Me.btnVerificar.Size = New System.Drawing.Size(112, 26)
        Me.btnVerificar.TabIndex = 3
        Me.btnVerificar.Text = "Verificar exclusiones"
        Me.btnVerificar.UseVisualStyleBackColor = True
        '
        'btnEjecutar
        '
        Me.btnEjecutar.Location = New System.Drawing.Point(169, 450)
        Me.btnEjecutar.Name = "btnEjecutar"
        Me.btnEjecutar.Size = New System.Drawing.Size(187, 26)
        Me.btnEjecutar.TabIndex = 4
        Me.btnEjecutar.Text = "Aplicar"
        Me.btnEjecutar.UseVisualStyleBackColor = True
        '
        'btnMasDe48Horas
        '
        Me.btnMasDe48Horas.Location = New System.Drawing.Point(138, 56)
        Me.btnMasDe48Horas.Name = "btnMasDe48Horas"
        Me.btnMasDe48Horas.Size = New System.Drawing.Size(173, 26)
        Me.btnMasDe48Horas.TabIndex = 5
        Me.btnMasDe48Horas.Text = "Ver solicitudes de + de 48 horas"
        Me.btnMasDe48Horas.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(335, 59)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(173, 26)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Ver buques atracados"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmExcepcion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 488)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnMasDe48Horas)
        Me.Controls.Add(Me.btnEjecutar)
        Me.Controls.Add(Me.btnVerificar)
        Me.Controls.Add(Me.rtbLista)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtListaSolicitudes)
        Me.Name = "frmExcepcion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Excepcion de cierre de solicitudes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtListaSolicitudes As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents rtbLista As RichTextBox
    Friend WithEvents btnVerificar As Button
    Friend WithEvents btnEjecutar As Button
    Friend WithEvents btnMasDe48Horas As Button
    Friend WithEvents Button1 As Button
End Class
