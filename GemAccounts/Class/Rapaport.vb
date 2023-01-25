Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Text
Imports DevExpress.XtraEditors
Imports GemAccounts.GLOBAL_VARIABLES
Imports GemAccounts.MasterConnection
Imports Newtonsoft.Json.Linq
Imports GemAccounts.GENERAL_FUNCTIONS

Public Class Rapaport
    Dim dr As DataRow
    Public Sub deleterecords()
        Using cmd As New SqlCommand("DELETE FROM Dia_UpdateRapaport", VarCon.Connection)
            cmd.ExecuteNonQuery()
            VarCon.CloseConnection()
        End Using
    End Sub
    Public Function getrapaport() As Boolean
        getrapaport = False
        dr = GENERAL_FUNCTIONS.NFetchDataRow("select top(1)* from tbl_rapaport_credentials")
        If dr Is Nothing Then
            DevFunctions.ErrorMsg("Rapaport credentials are not set")
            Exit Function
        Else
            Dim Data As String = "{" & " ""request"": { " & " ""header"": { " & " ""username"": """ & dr("username").ToString() & """," & " ""password"": """ & dr("password").ToString() & """ " & "}," & " ""body"": {" & " ""shape"": ""round,pear"" " & "}" & "}" & "}"
            Dim URL As String = "https://technet.rapaport.com/HTTP/JSON/Prices/GetPriceSheet.aspx "
            Dim webRequest As WebRequest = WebRequest.Create(URL)
            webRequest.Method = "POST"
            webRequest.ContentType = "application/x-www-form-urlencoded"
            Dim reqStream As Stream = webRequest.GetRequestStream()
            Dim postData As String = Data
            Dim postArray As Byte() = Encoding.ASCII.GetBytes(postData)
            reqStream.Write(postArray, 0, postArray.Length)
            reqStream.Close()
            Dim sr As StreamReader = New StreamReader(webRequest.GetResponse().GetResponseStream())
            Dim Result As String = sr.ReadToEnd()
            Dim jObj As JObject = JObject.Parse(Result)

            Dim error_msg As String
            error_msg = jObj("response")("header")("error_message").ToString()
            If error_msg <> "" Then
                DevFunctions.ErrorMsg(error_msg)
                getrapaport = False
                Exit Function
            Else
                Dim dt As New DataTable
                dt = jObj("response")("body")("price").ToObject(Of DataTable)()
                deleterecords()
                VarCon.Connection()
                Using copy As New SqlBulkCopy(con)
                    copy.ColumnMappings.Add("shape", "shape")
                    copy.ColumnMappings.Add("low_size", "low_size")
                    copy.ColumnMappings.Add("high_size", "high_size")
                    copy.ColumnMappings.Add("color", "color")
                    copy.ColumnMappings.Add("clarity", "clarity")
                    copy.ColumnMappings.Add("caratprice", "caratprice")
                    copy.ColumnMappings.Add("date", "date")
                    copy.DestinationTableName = "Dia_UpdateRapaport"
                    copy.WriteToServer(dt)
                End Using
                VarCon.CloseConnection()
                getrapaport = True
            End If
        End If
        'Dim Data As String = "{" & " ""request"": { " & " ""header"": { " & " ""username"": ""xja89uni8la5nr7zfiqeiyzmd8svyu""," & " ""password"": ""ogBc4Ceh"" " & "}," & " ""body"": {" & " ""shape"": ""round,pear"" " & "}" & "}" & "}"
        Return getrapaport
    End Function

End Class
