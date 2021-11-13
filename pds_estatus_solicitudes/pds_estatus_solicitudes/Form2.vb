Public Class Form2
    Public bdlogin As New BD_PDS
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existe As String = "NO"
        Dim busca As String = "Select * from admin_pds Where usuario = '" & txtUsuario.Text & "' and contraseña = '" & txtContraseña.Text & "'"
        bdlogin.conectar()

        bdlogin.consultar(busca)
        Do While bdlogin.reader.Read
            existe = "SI"
        Loop
        bdlogin.cerrar()

        If existe = "SI" Then
            Me.Hide()
            frmExcepcion.Show()
        Else
            MsgBox("Credenciales Incorrectas")
        End If
    End Sub
End Class