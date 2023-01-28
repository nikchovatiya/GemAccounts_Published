Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports GemAccounts.GLOBAL_VARIABLES

Public Class MasterConnection
    Public Shared con As SqlConnection



    Public Function Connection() As SqlConnection
        Dim constring As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'Dim csb As System.Data.SqlClient.SqlConnectionStringBuilder = New System.Data.SqlClient.SqlConnectionStringBuilder(constring)

        'csb.Password = MasterConnection.Decrypt(csb.Password, "A@123")
        'constring = csb.ToString()
        con = New SqlConnection(constring)
        If con.State = 1 Then con.Close()
        con.Open()
        Return con
    End Function
    Public Function CloseConnection() As SqlConnection
        con.Close()
        Return con
    End Function

    Public Function NFetchDataset(ByVal Query As String) As DataSet
        Dim dtp As New DataSet
        ADP = New SqlDataAdapter(Query, con)
        ADP.Fill(dtp)
        NFetchDataset = dtp
    End Function

    Public Shared Function Encrypt(text As String, password As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Dim hash(31) As Byte
        Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password))
        Array.Copy(temp, 0, hash, 0, 16)
        Array.Copy(temp, 0, hash, 15, 16)
        AES.Key = hash
        AES.Mode = Security.Cryptography.CipherMode.ECB
        Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
        Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(text)
        encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
        Return encrypted
    End Function
    Public Shared Function Decrypt(text As String, password As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Dim hash(31) As Byte
        Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password))
        Array.Copy(temp, 0, hash, 0, 16)
        Array.Copy(temp, 0, hash, 15, 16)
        AES.Key = hash
        AES.Mode = Security.Cryptography.CipherMode.ECB
        Dim DESDecrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateDecryptor
        Dim Buffer As Byte() = Convert.FromBase64String(text)
        decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
        Return decrypted
    End Function
End Class
