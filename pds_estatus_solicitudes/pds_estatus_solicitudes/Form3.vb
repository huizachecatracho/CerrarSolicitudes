

Public Class frmExcepcion
    Public vnx_pds As BD_PDS = New BD_PDS
    Public listado As String = ""
    Public aviso As String = ""
    Public mensaje As String = ""
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub btnVerificar_Click(sender As Object, e As EventArgs) Handles btnVerificar.Click
        Dim tamaxo As Integer = 0
        If txtListaSolicitudes.Text = "" Then
            listado = "Se cerraran las solicitudes con mas de 48 horas de acuerdo a los criterios especificados" & vbCrLf
            listado = listado & "si usted tiene algunas excepciones  escribalas en el cuadro de excepciones separadas por una coma "
            listado = listado & "y de click en el boton verificar. " & vbCrLf & "estas solicitudes no se cerraran aun cuando tengan mas de 48 horas sin tener actividad" & vbCrLf
            listado = listado & vbCrLf & "el boton de aplicar cerrara las solicitudes para cualquiera de las dos formas con o sin excepciones "
            rtbLista.Text = listado
        Else
            txtListaSolicitudes.Text = Replace(txtListaSolicitudes.Text, ",,", ",")
            txtListaSolicitudes.Text = Replace(txtListaSolicitudes.Text, ",,,", ",")
            txtListaSolicitudes.Text = Replace(txtListaSolicitudes.Text, ",,,,", ",")
            tamaxo = Len(txtListaSolicitudes.Text)
            If Mid(txtListaSolicitudes.Text, tamaxo, 1) = "," Then
                txtListaSolicitudes.Text = Mid(txtListaSolicitudes.Text, 1, tamaxo - 1)
            End If

            rtbLista.Text = ""


            Dim Busca = "Select [id_solicitud] ,[fecha] ,[siglas] ,[nombre_buque_terminal] ,[nombre_prestador] ,[nombre_servicio], [estatus] , [badge_id],[forzar_cierre], BA.nombre As nombre_buque, DateAdd(hh, -4, getdate()) As fecha4H "
            Busca = Busca & " From [pds_productivo].[dbo].[cnc_vista_solicitudes] S "
            Busca = Busca & "      Left Join [10.34.68.39].[SP3LAC].[dbo].[cnc_buques_atracados] BA ON BA.nombre COLLATE Modern_Spanish_CI_AS = S.nombre_buque_terminal COLLATE Modern_Spanish_CI_AS "
            Busca = Busca & "  Where fecha < DateAdd(hh, -4, getDate()) And barco_terminal = 1 And forzar_cierre Is null And estatus = 1 "
            Busca = Busca & "  and id_solicitud in (" & txtListaSolicitudes.Text & ")"


            vnx_pds.conectar()

            vnx_pds.consultar(Busca)

            listado = "Estas solicitudes no se cerraran " & vbCrLf & vbCrLf

            listado = listado & "Lista de solicitudes Cuando el Buque ya zarpo" & vbCrLf & vbCrLf

            Dim buques_zarpados As String = "NO"

            Do While vnx_pds.reader.Read
                If (vnx_pds.reader("nombre_buque") Is Nothing) Then
                    buques_zarpados = "SI"
                    listado = listado & "Solicitud : " & vnx_pds.reader("id_solicitud") & "  Fecha : " & vnx_pds.reader("fecha") & " Estatus : Abierta" & vbCrLf
                    listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                    listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                    listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
                End If
            Loop
            vnx_pds.cerrar()
            If buques_zarpados = "SI" Then
                listado = listado & vbCrLf
            Else
                listado = listado & " No se encontraron solicitudes para buquez Zarpados" & vbCrLf & vbCrLf
            End If
            '  rtbLista.Text = listado

            Dim terminal_ingreso As String = "NO"

            'Solicitudes de terminal que no entraron en dos dias 
            Dim BuscaTerrminales As String = "select * from cnc_vista_solicitudes where barco_terminal = 2 and badge_id = 0 and  fecha < getDate()- 2 and forzar_cierre is null and estatus = 1 "
            BuscaTerrminales = BuscaTerrminales & "  and id_solicitud in (" & txtListaSolicitudes.Text & ")"

            vnx_pds.conectar()

            vnx_pds.consultar(BuscaTerrminales)

            listado = listado & "Solictudes para terminales que no han ingresado en dos dias " & vbCrLf & vbCrLf

            Do While vnx_pds.reader.Read
                terminal_ingreso = "SI"
                listado = listado & "Solicitud : " & vnx_pds.reader("id_solicitud") & "  Fecha : " & vnx_pds.reader("fecha") & " Estatus : Abierta" & vbCrLf
                listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
            Loop

            If terminal_ingreso = "SI" Then
                listado = listado & vbCrLf
            Else
                listado = listado & " No se encontraron solicitudes para terminales que no entraron en dos dias " & vbCrLf & vbCrLf
            End If
            ' rtbLista.Text = listado

            vnx_pds.cerrar()


            'Solicitudes con ingreso y no regresaron en 48 horas a la terminal

            Dim busca48h As String = "Select dbo.prestadores.nombre as nombre_prestador, dbo.prestadores.siglas,dbo.servicios.nombre as nombre_servicio,  dbo.historialaccesosservicio.idsolicitud, dbo.solicitudes.nombre_buque_terminal, dbo.solicitudes.id_buque_terminal, "
            busca48h = busca48h & " max(dbo.historialaccesosservicio.fecha) As fechahistorial, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, getdate() -2 As hoy, "
            busca48h = busca48h & " dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus "
            busca48h = busca48h & " From dbo.historialaccesosservicio  "
            busca48h = busca48h & "  INNER Join dbo.solicitudes ON dbo.historialaccesosservicio.idsolicitud = dbo.solicitudes.id_solicitud   "
            busca48h = busca48h & " INNER JOIN dbo.prestadores on dbo.solicitudes.id_prestador = dbo.prestadores.id_prestador "
            busca48h = busca48h & "  INNER JOIN dbo.servicios on dbo.servicios.id_servicio = dbo.solicitudes.id_servicio "
            busca48h = busca48h & " WHERE(dbo.solicitudes.forzar_cierre Is NULL) And (dbo.solicitudes.barco_terminal = 2)     And dbo.solicitudes.estatus = 1 And  dbo.solicitudes.badge_id <> 0 "
            busca48h = busca48h & "  and idsolicitud in (" & txtListaSolicitudes.Text & ")"
            busca48h = busca48h & " GROUP BY dbo.historialaccesosservicio.idsolicitud, dbo.prestadores.nombre, dbo.solicitudes.nombre_buque_terminal, "
            busca48h = busca48h & " dbo.solicitudes.id_buque_terminal, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus, "
            busca48h = busca48h & " dbo.prestadores.siglas, dbo.servicios.nombre "



            vnx_pds.conectar()

            vnx_pds.consultar(busca48h)

            Dim terminal_ingreso48h As String = "NO"

            listado = listado & "Ingresaron a la terminal pero no regresaron en 48 horas" & vbCrLf & vbCrLf

            Do While vnx_pds.reader.Read
                If vnx_pds.reader("fechahistorial") < vnx_pds.reader("hoy") Then
                    terminal_ingreso48h = "SI"
                    listado = listado & "Solicitud : " & vnx_pds.reader("idsolicitud") & "  Fecha : " & vnx_pds.reader("fechahistorial") & " Estatus : Abierta" & vbCrLf
                    listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                    listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                    listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
                End If
            Loop
            vnx_pds.cerrar()

            If terminal_ingreso48h = "SI" Then
                listado = listado & vbCrLf
            Else
                listado = listado & " No se encontraron solicitudes para terminales que ingresaron y no regresaron en 48 horas " & vbCrLf & vbCrLf
            End If

            rtbLista.Text = listado


            vnx_pds.cerrar()
        End If
    End Sub

    Private Sub frmExcepcion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listado = "Se cerraran las solicitudes con mas de 48 horas de acuerdo a los criterios especificados" & vbCrLf
        listado = listado & "si usted tiene algunas excepciones  escribalas en el cuadro de excepciones separadas por una coma "
        listado = listado & "y de click en el boton verificar. " & vbCrLf & "estas solicitudes no se cerraran aun cuando tengan mas de 48 horas sin tener actividad" & vbCrLf
        listado = listado & vbCrLf & "el boton de aplicar cerrara las solicitudes para cualquiera de las dos formas con o sin excepciones "

        rtbLista.Text = listado
    End Sub

    Private Sub btnEjecutar_Click(sender As Object, e As EventArgs) Handles btnEjecutar.Click
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnMasDe48Horas.Click

        Dim msgBuquesZarpados As String = ""

        Dim Busca As String = " Select [id_solicitud] ,[fecha] ,  tipo_cierre = CASE "
        Busca = Busca & " WHEN S.badge_id = 0 THEN '1' "
        Busca = Busca & " Else '4' End, "
        Busca = Busca & "   [siglas] , [nombre_buque_terminal], [nombre_prestador], [nombre_servicio], [estatus], [badge_id], [forzar_cierre], isnull( BA.nombre, 0)  As nombre_buque, DateAdd(hh, -4, getdate()) As fecha4H "
        Busca = Busca & " From [pds_productivo].[dbo].[cnc_vista_solicitudes] S "
        Busca = Busca & "    Left Join [10.34.68.39].[SP3LAC].[dbo].[cnc_buques_atracados] BA ON BA.nombre COLLATE Modern_Spanish_CI_AS = S.nombre_buque_terminal COLLATE Modern_Spanish_CI_AS "
        Busca = Busca & "   Where Fecha < DateAdd(hh, -4, getDate()) And barco_terminal = 1 And forzar_cierre Is null And estatus = 1 and BA.nombre is null "

        If txtListaSolicitudes.Text <> "" Then
            Busca = Busca & "  And id_solicitud not in (" & RTrim(txtListaSolicitudes.Text) & ")"
        End If


        vnx_pds.conectar()

        vnx_pds.consultar(Busca)
        listado = "Lista de solicitudes Cuando el Buque ya zarpo" & vbCrLf & vbCrLf

        Dim buques_zarpados As String = "NO"

        Do While vnx_pds.reader.Read
            If vnx_pds.reader("nombre_buque") = "0" Then
                buques_zarpados = "SI"
                listado = listado & "Solicitud : " & vnx_pds.reader("id_solicitud") & "  Fecha : " & vnx_pds.reader("fecha") & " Estatus : Abierta" & vbCrLf
                listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
            End If
        Loop
        vnx_pds.cerrar()
        If buques_zarpados = "SI" Then
            listado = listado & vbCrLf
        Else
            listado = listado & " No se encontraron buquez Zarpados" & vbCrLf & vbCrLf
        End If
        '  rtbLista.Text = listado

        Dim terminal_ingreso As String = "NO"

        'Solicitudes de terminal que no entraron en dos dias 
        Dim BuscaTerrminales As String = "select * from cnc_vista_solicitudes where barco_terminal = 2 and badge_id = 0 and  fecha < getDate()- 2 and forzar_cierre is null and estatus = 1"

        vnx_pds.conectar()

        vnx_pds.consultar(BuscaTerrminales)

        listado = listado & "Solictudes para terminales que no han ingresado en dos dias " & vbCrLf & vbCrLf

        Do While vnx_pds.reader.Read
            terminal_ingreso = "SI"
            listado = listado & "Solicitud : " & vnx_pds.reader("id_solicitud") & "  Fecha : " & vnx_pds.reader("fecha") & " Estatus : Abierta" & vbCrLf
            listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
            listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
            listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
        Loop

        If terminal_ingreso = "SI" Then
            listado = listado & vbCrLf
        Else
            listado = listado & " No se encontraron solicitudes para terminales que no entraron en dos dias " & vbCrLf & vbCrLf
        End If
        ' rtbLista.Text = listado

        vnx_pds.cerrar()


        'Solicitudes con ingreso y no regresaron en 48 horas a la terminal

        Dim busca48h As String = "Select dbo.prestadores.nombre as nombre_prestador, dbo.prestadores.siglas,dbo.servicios.nombre as nombre_servicio,  dbo.historialaccesosservicio.idsolicitud, dbo.solicitudes.nombre_buque_terminal, dbo.solicitudes.id_buque_terminal, "
        busca48h = busca48h & " max(dbo.historialaccesosservicio.fecha) As fechahistorial, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, getdate() -2 As hoy, "
        busca48h = busca48h & " dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus "
        busca48h = busca48h & " From dbo.historialaccesosservicio  "
        busca48h = busca48h & "  INNER Join dbo.solicitudes ON dbo.historialaccesosservicio.idsolicitud = dbo.solicitudes.id_solicitud   "
        busca48h = busca48h & " INNER JOIN dbo.prestadores on dbo.solicitudes.id_prestador = dbo.prestadores.id_prestador "
        busca48h = busca48h & "  INNER JOIN dbo.servicios on dbo.servicios.id_servicio = dbo.solicitudes.id_servicio "
        busca48h = busca48h & " WHERE(dbo.solicitudes.forzar_cierre Is NULL) And (dbo.solicitudes.barco_terminal = 2)     And dbo.solicitudes.estatus = 1 And  dbo.solicitudes.badge_id <> 0 "
        busca48h = busca48h & " GROUP BY dbo.historialaccesosservicio.idsolicitud, dbo.prestadores.nombre, dbo.solicitudes.nombre_buque_terminal, "
        busca48h = busca48h & " dbo.solicitudes.id_buque_terminal, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus, "
        busca48h = busca48h & " dbo.prestadores.siglas, dbo.servicios.nombre "

        vnx_pds.conectar()

        vnx_pds.consultar(busca48h)

        Dim terminal_ingreso48h As String = "NO"

        listado = listado & "Ingresaron a la terminal pero no regresaron en 48 horas" & vbCrLf & vbCrLf

        Do While vnx_pds.reader.Read
            If vnx_pds.reader("fechahistorial") < vnx_pds.reader("hoy") Then
                terminal_ingreso48h = "SI"
                listado = listado & "Solicitud : " & vnx_pds.reader("idsolicitud") & "  Fecha : " & vnx_pds.reader("fechahistorial") & " Estatus : Abierta" & vbCrLf
                listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
            End If
        Loop
        vnx_pds.cerrar()

        If terminal_ingreso48h = "SI" Then
            listado = listado & vbCrLf
        Else
            listado = listado & " No se encontraron solicitudes para terminales que ingresaron y no regresaron en 48 horas " & vbCrLf & vbCrLf
        End If




        'Solicitudes con ingreso y no regresaron en 48 horas al Buque

        Dim busca48hBuque As String = "Select dbo.prestadores.nombre as nombre_prestador, dbo.prestadores.siglas,dbo.servicios.nombre as nombre_servicio,  dbo.historialaccesosservicio.idsolicitud, dbo.solicitudes.nombre_buque_terminal, dbo.solicitudes.id_buque_terminal, "
        busca48hBuque = busca48hBuque & " max(dbo.historialaccesosservicio.fecha) As fechahistorial, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, getdate() -2 As hoy, "
        busca48hBuque = busca48hBuque & " dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus "
        busca48hBuque = busca48hBuque & " From dbo.historialaccesosservicio  "
        busca48hBuque = busca48hBuque & "  INNER Join dbo.solicitudes ON dbo.historialaccesosservicio.idsolicitud = dbo.solicitudes.id_solicitud   "
        busca48hBuque = busca48hBuque & " INNER JOIN dbo.prestadores on dbo.solicitudes.id_prestador = dbo.prestadores.id_prestador "
        busca48hBuque = busca48hBuque & " INNER JOIN dbo.servicios on dbo.servicios.id_servicio = dbo.solicitudes.id_servicio "
        busca48hBuque = busca48hBuque & " WHERE(dbo.solicitudes.forzar_cierre Is NULL) And (dbo.solicitudes.barco_terminal = 1)     And dbo.solicitudes.estatus = 1 And  dbo.solicitudes.badge_id <> 0 "
        busca48hBuque = busca48hBuque & " GROUP BY dbo.historialaccesosservicio.idsolicitud, dbo.prestadores.nombre, dbo.solicitudes.nombre_buque_terminal, "
        busca48hBuque = busca48hBuque & " dbo.solicitudes.id_buque_terminal, dbo.solicitudes.forzar_cierre, dbo.solicitudes.nombre, dbo.solicitudes.barco_terminal, dbo.solicitudes.estatus, "
        busca48hBuque = busca48hBuque & " dbo.prestadores.siglas, dbo.servicios.nombre "

        vnx_pds.conectar()

        vnx_pds.consultar(busca48hBuque)

        Dim Buque_ingreso48h As String = "NO"

        listado = listado & "Ingresaron Buque pero no regresaron en 48 horas" & vbCrLf & vbCrLf

        Do While vnx_pds.reader.Read
            If vnx_pds.reader("fechahistorial") < vnx_pds.reader("hoy") Then
                terminal_ingreso48h = "SI"
                listado = listado & "Solicitud : " & vnx_pds.reader("idsolicitud") & "  Fecha : " & vnx_pds.reader("fechahistorial") & " Estatus : Abierta" & vbCrLf
                listado = listado & "Siglas :" & vnx_pds.reader("siglas") & " Buque / Terminal : " & vnx_pds.reader("nombre_buque_terminal") & vbCrLf
                listado = listado & "Nombre  :" & vnx_pds.reader("nombre_prestador") & " Servicio : " & vnx_pds.reader("nombre_servicio") & vbCrLf
                listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
            End If
        Loop
        vnx_pds.cerrar()

        If terminal_ingreso48h = "SI" Then
            listado = listado & vbCrLf
        Else
            listado = listado & " No se encontraron solicitudes para Buques que ingresaron y no regresaron en 48 horas " & vbCrLf & vbCrLf
        End If


        rtbLista.Text = listado

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim bdBuquesAtracados As New BD_SOP

        bdBuquesAtracados.conectar()

        Dim busca = "select * from cnc_buques_atracados"

        bdBuquesAtracados.consultar(busca)
        listado = "Lista de buques atracados " & vbCrLf & vbCrLf
        Do While bdBuquesAtracados.reader.Read
            listado = listado & bdBuquesAtracados.reader("BuqueID") & " | " & bdBuquesAtracados.reader("Nombre") & vbCrLf
            listado = listado & "------------------------------------------------------------------------------------------------------------------------------------------" & vbCrLf
        Loop
        bdBuquesAtracados.cerrar()
        rtbLista.Text = listado
    End Sub

    Private Sub rtbLista_TextChanged(sender As Object, e As EventArgs) Handles rtbLista.TextChanged

    End Sub

    Private Sub txtListaSolicitudes_TextChanged(sender As Object, e As EventArgs) Handles txtListaSolicitudes.TextChanged

    End Sub
End Class