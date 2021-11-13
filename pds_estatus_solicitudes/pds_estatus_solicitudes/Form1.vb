Public Class Form1
    Public cnx_buques As BD_SOP = New BD_SOP
    Public cnx_pds As BD_PDS = New BD_PDS
    Public cnx_pds2 As BD_PDS = New BD_PDS
    Public cnx_pds_actualiza As BD_PDS = New BD_PDS
    Public cnx_pds_inserta As BD_PDS = New BD_PDS
    Public cnx_pds3 As BD_PDS = New BD_PDS
    Public cnx_pds4 As BD_PDS = New BD_PDS
    Public Fecha As Date
    Public hoy As Date
    Public contador As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        PictureBox1.Image = Image.FromFile("C:\img\Archivo.gif")
        contador = contador + 1
    End Sub

    Sub ProgBuques()
        Dim Cierra_Buques_zarpados As String = ""
        'Buques Zarpados si no hay ingreso el estatus = 4 de lo cpontrario = 2 
        Dim Busca As String = " Select [id_solicitud] ,[fecha] ,  tipo_cierre = CASE "
        Busca = Busca & " WHEN S.badge_id = 0 THEN '1' "
        Busca = Busca & " Else '4' End, "
        Busca = Busca & "   [siglas] , [nombre_buque_terminal], [nombre_prestador], [nombre_servicio], [estatus], [badge_id], [forzar_cierre], BA.nombre As nombre_buque, DateAdd(hh, -4, getdate()) As fecha4H "
        Busca = Busca & " From [pds_productivo].[dbo].[cnc_vista_solicitudes] S "
        Busca = Busca & "    Left Join [10.34.68.39].[SP3LAC].[dbo].[cnc_buques_atracados] BA ON BA.nombre COLLATE Modern_Spanish_CI_AS = S.nombre_buque_terminal COLLATE Modern_Spanish_CI_AS "
        Busca = Busca & "   Where Fecha < DateAdd(hh, -4, getDate()) And barco_terminal = 1 And forzar_cierre Is null And estatus = 1 and BA.nombre is null "

        If frmExcepcion.txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "  And id_solicitud not in (" & RTrim(frmExcepcion.txtListaSolicitudes.Text) & ")"
        End If

        cnx_pds.conectar()
        cnx_pds.consultar(Busca)
        Do While cnx_pds.reader.Read
            cnx_pds_actualiza.conectar()
            Dim estatus As Integer = 0
            If cnx_pds.reader("tipo_cierre") = 1 Then
                estatus = 4
            Else
                estatus = 2
            End If
            Cierra_Buques_zarpados = "Update Solicitudes Set Estatus = " & estatus & ", Fecha_Termino = getdate(), forzar_cierre = " & cnx_pds.reader("tipo_cierre") & " Where Id_Solicitud = " & cnx_pds.reader("id_solicitud")
            cnx_pds_actualiza.ejecuta_sentencia(Cierra_Buques_zarpados)
            cnx_pds_actualiza.cerrar()
        Loop
        cnx_pds.cerrar()
        contador = contador + 1
        ProgBuques2()
    End Sub
    Sub ProgBuques2()
        'Solicitudes de terminal que no entraron en dos dias 
        Dim Busca As String = "select * from solicitudes where barco_terminal = 2 and badge_id = 0 and  fecha < getDate()- 2 and forzar_cierre is null and estatus = 1"

        If frmExcepcion.txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "  And id_solicitud not in (" & RTrim(frmExcepcion.txtListaSolicitudes.Text) & ")"
        End If

        cnx_pds.conectar()
        cnx_pds.consultar(Busca)
        Do While cnx_pds.reader.Read
            cnx_pds_actualiza.conectar()
            Dim Actualiza As String = "Update Solicitudes Set Estatus = 4 " & ", Fecha_Termino = getdate(), forzar_cierre = 1" & " Where Id_Solicitud = " & cnx_pds.reader("id_solicitud")
            cnx_pds_actualiza.ejecuta_sentencia(Actualiza)
            cnx_pds_actualiza.cerrar()
            'Busca_Buques(cnx_pds.reader("id_solicitud"), cnx_pds.reader("nombre_buque_terminal"), cnx_pds.reader("tipo_cierre"))
        Loop
        cnx_pds.cerrar()
        contador = contador + 1
        ProgBuques3()

    End Sub
    Sub ProgBuques3()
        'solicitudes que ingresaron a la terminal y no han regresado en dos dias 
        Dim Busca As String = "Select dbo.prestadores.nombre As nombre_prestador, dbo.prestadores.siglas,dbo.servicios.nombre As nombre_servicio,  dbo.historialaccesosservicio.idsolicitud, dbo.solicitudes.nombre_buque_terminal, dbo.solicitudes.id_buque_terminal, "
        Busca = Busca & "  max(dbo.historialaccesosservicio.fecha) As fechahistorial, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, DATEADD(HH, -48, GETDATE()) As hoy, "
        Busca = Busca & " dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus "
        Busca = Busca & " From dbo.historialaccesosservicio  "
        Busca = Busca & " INNER Join dbo.solicitudes ON dbo.historialaccesosservicio.idsolicitud = dbo.solicitudes.id_solicitud  "
        Busca = Busca & "  INNER Join dbo.prestadores on dbo.solicitudes.id_prestador = dbo.prestadores.id_prestador "
        Busca = Busca & " INNER Join dbo.servicios on dbo.servicios.id_servicio = dbo.solicitudes.id_servicio "
        Busca = Busca & " WHERE(dbo.solicitudes.forzar_cierre Is NULL) And (dbo.solicitudes.barco_terminal = 2)     And dbo.solicitudes.estatus = 1 And  dbo.solicitudes.badge_id <> 0 "
        Busca = Busca & "  GROUP BY dbo.historialaccesosservicio.idsolicitud, dbo.prestadores.nombre, dbo.solicitudes.nombre_buque_terminal, "
        Busca = Busca & " dbo.solicitudes.id_buque_terminal, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus, "
        Busca = Busca & "  dbo.prestadores.siglas, dbo.servicios.nombre "
        If frmExcepcion.txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "  And id_solicitud not in (" & RTrim(frmExcepcion.txtListaSolicitudes.Text) & ")"
        End If
        cnx_pds3.conectar()
        cnx_pds3.consultar(Busca)
        Do While cnx_pds3.reader.Read
            ' Fecha = Format(cnx_pds3.reader("fecha"), "yyyy MMM dd hh:mm:ss")
            ' hoy = Format(cnx_pds3.reader("hoy"), "yyyy MMM dd hh:mm:ss")
            If cnx_pds3.reader("fechahistorial") < cnx_pds3.reader("hoy") Then
                cnx_pds_actualiza.conectar()
                Dim Actualiza As String = "Update Solicitudes Set Estatus = 2 " & ", Fecha_Termino = getdate(), forzar_cierre = 1" & " Where Id_Solicitud = " & cnx_pds3.reader("idsolicitud")
                Dim Mensaje As String = "Ingresaron a la Terminal y no regresaron en 2 dias " & cnx_pds3.reader("idsolicitud") & " fecha=" & cnx_pds3.reader("fechahistorial") & " Hoy = " & cnx_pds3.reader("hoy")
                Debug.Print(Mensaje)
                cnx_pds_actualiza.ejecuta_sentencia(Actualiza)
                cnx_pds_actualiza.cerrar()
            End If
        Loop
        cnx_pds3.cerrar()

        contador = contador + 1
        ProgBuques4()
        ' MsgBox("fin")
    End Sub
    Sub ProgBuques4()
        'solicitudes que ingresaron al buque y no han regresado en dos dias 
        Dim Busca As String = "Select dbo.prestadores.nombre As nombre_prestador, dbo.prestadores.siglas,dbo.servicios.nombre As nombre_servicio,  dbo.historialaccesosservicio.idsolicitud, dbo.solicitudes.nombre_buque_terminal, dbo.solicitudes.id_buque_terminal, "
        Busca = Busca & "  max(dbo.historialaccesosservicio.fecha) As fechahistorial, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, DATEADD(HH, -48, GETDATE()) As hoy, "
        Busca = Busca & " dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus "
        Busca = Busca & " From dbo.historialaccesosservicio  "
        Busca = Busca & " INNER Join dbo.solicitudes ON dbo.historialaccesosservicio.idsolicitud = dbo.solicitudes.id_solicitud  "
        Busca = Busca & "  INNER Join dbo.prestadores on dbo.solicitudes.id_prestador = dbo.prestadores.id_prestador "
        Busca = Busca & " INNER Join dbo.servicios on dbo.servicios.id_servicio = dbo.solicitudes.id_servicio "
        Busca = Busca & " WHERE(dbo.solicitudes.forzar_cierre Is NULL) And (dbo.solicitudes.barco_terminal = 1)     And dbo.solicitudes.estatus = 1 And  dbo.solicitudes.badge_id <> 0 "
        Busca = Busca & "  GROUP BY dbo.historialaccesosservicio.idsolicitud, dbo.prestadores.nombre, dbo.solicitudes.nombre_buque_terminal, "
        Busca = Busca & " dbo.solicitudes.id_buque_terminal, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus, "
        Busca = Busca & "  dbo.prestadores.siglas, dbo.servicios.nombre "
        If frmExcepcion.txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "  And id_solicitud not in (" & RTrim(frmExcepcion.txtListaSolicitudes.Text) & ")"
        End If
        cnx_pds4.conectar()
        cnx_pds4.consultar(Busca)
        Do While cnx_pds4.reader.Read
            ' Fecha = Format(cnx_pds4.reader("fecha"), "yyyy MMM dd hh:mm:ss")
            ' hoy = Format(cnx_pds4.reader("hoy"), "yyyy MMM dd hh:mm:ss")
            If cnx_pds4.reader("Fechahistorial") < cnx_pds4.reader("hoy") Then
                cnx_pds_actualiza.conectar()
                Dim Actualiza As String = "Update Solicitudes Set Estatus = 2 " & ", Fecha_Termino = getdate(), forzar_cierre = 1" & " Where Id_Solicitud = " & cnx_pds4.reader("idsolicitud")
                Dim Mensaje As String = "Ingresaron a la Terminal y no regresaron en 2 dias " & cnx_pds4.reader("idsolicitud") & " fecha=" & cnx_pds4.reader("fechahistorial") & " Hoy = " & cnx_pds4.reader("hoy")
                Debug.Print(Mensaje)
                cnx_pds_actualiza.ejecuta_sentencia(Actualiza)
                cnx_pds_actualiza.cerrar()
            End If
        Loop
        cnx_pds4.cerrar()

        contador = contador + 1

        'Solicitudes de barco que no entraron en dos dias 
        Busca = "select * from solicitudes where barco_terminal = 1 and badge_id = 0 and estatus = 1 and  fecha < getDate()- 2 and forzar_cierre is null "

        If frmExcepcion.txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "   And id_solicitud not in (" & RTrim(frmExcepcion.txtListaSolicitudes.Text) & ")"
        End If
        cnx_pds.conectar()
        cnx_pds.consultar(Busca)
        Do While cnx_pds.reader.Read
            cnx_pds_actualiza.conectar()
            Dim Actualiza As String = "Update Solicitudes Set Estatus = 4 " & ", Fecha_Termino = getdate(), forzar_cierre = 1" & " Where Id_Solicitud = " & cnx_pds.reader("id_solicitud")
            cnx_pds_actualiza.ejecuta_sentencia(Actualiza)
            cnx_pds_actualiza.cerrar()
            'Busca_Buques(cnx_pds.reader("id_solicitud"), cnx_pds.reader("nombre_buque_terminal"), cnx_pds.reader("tipo_cierre"))
        Loop
        cnx_pds.cerrar()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgBuques()

        If contador >= 5 Or contador <= 6 Then
            MsgBox("FIN")
            Form1.ActiveForm.Close()
            Form2 = Nothing
            frmExcepcion = Nothing
            Me.Finalize()
            Me.Close()
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class
