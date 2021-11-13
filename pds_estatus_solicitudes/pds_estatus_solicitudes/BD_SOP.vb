Imports Microsoft.VisualBasic
Imports System.Data 'Espacio de Nombres para los datos
Imports System.Data.Sql 'Espacio de Nombres para usar SQl Server
Imports System.Data.SqlClient ' Espacio de Nombres para usar un cliente de conexion

Public Class BD_SOP
    '******** Propiedades de la clase ********************
    Public constr As String = ""  'Cadena para la conexion
    Public Conn As IDbConnection = New SqlConnection
    Public Cmd As IDbCommand
    Public Consulta As String
    Public reader As IDataReader
    Public mensaje_error As String = ""

    '******* Metodo para conectar la base de datos de LENEl ********************
    '        Este metodo realiza la conexion a la base de datos
    '******* Metodo para conectar la base de datos de LENEl ********************
    Public Sub conectar()
        Conn.ConnectionString = "Data Source=10.34.68.39;Initial Catalog=SP3LAC;User ID=pvmuser;PASSWORD=tgradar2011;timeout=600000;"
        constr = "Data Source=10.34.68.39;Initial Catalog=SP3LAC;User ID=pvmuser;PASSWORD=tgradar2011"
        Try
            Conn.Open()
        Catch ex As Exception
            mensaje_error = "No se encuentra la Base de Datos"
        End Try
    End Sub
    '******* Metodo para realizar consultas a la base de datos de LENEl *************************************
    '        Este metodo realiza una consulta y recibe como parametro la consulta que se va a realizar
    '******* Metodo para realizar consultas a la base de datos de LENEl *************************************
    Public Sub consultar(ByVal consulta As String)
        Cmd = Nothing
        Cmd = Conn.CreateCommand
        Cmd.CommandText = consulta
        Cmd.CommandTimeout = 600000
        reader = Cmd.ExecuteReader()
        Console.Write(reader)
    End Sub
    '******* Metodo para cerrar la conexiob de la base de datos  *************************************
    '        Este metodo cierra la conexion de la base de datos 
    '******* Metodo para cerrar la conexiob de la base de datos  *************************************
    Public Sub cerrar()
        Conn.Close()
    End Sub
    '******* Metodo para ejecutar sentencias en la base de datos  *************************************
    '        Este metodo ejecuta una sentencia en la base de datos y recibe el parametro la senetencia a ejecutar
    '       la cual puede ser UPDATE, DELETE, INSERT
    '******* Metodo para ejecutar sentencias en la base de datos  *************************************
    Public Sub ejecuta_sentencia(ByVal sentencia As String)
        Cmd = Nothing
        Cmd = Conn.CreateCommand
        Cmd.CommandTimeout = 600000
        Cmd.CommandText = sentencia

        Cmd.ExecuteNonQuery()
    End Sub

End Class
