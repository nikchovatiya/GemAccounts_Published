Public Class FrmRapaportCredentials
    Dim username As String
    Dim password As String
    Dim dr As DataRow
    Private Sub FrmRapaportCredentials_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_data()
    End Sub
    Private Sub load_data()
        dr = GENERAL_FUNCTIONS.NFetchDataRow("select top(1)* from tbl_rapaport_credentials")
        If dr Is Nothing Then
            Return
        Else
            txtUserName.EditValue = dr("username").ToString()
            txtPassword.EditValue = dr("password").ToString()
        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim sql As String
        dr = GENERAL_FUNCTIONS.NFetchDataRow("select top(1)* from tbl_rapaport_credentials")
        If dr Is Nothing Then
            sql = "INSERT INTO tbl_rapaport_credentials(username,password) VALUES('" & txtUserName.EditValue & "','" & txtPassword.EditValue & "')"
        Else
            sql = "UPDATE tbl_rapaport_credentials SET username='" & txtUserName.EditValue & "',password= '" & txtPassword.EditValue & "'"
        End If
        GENERAL_FUNCTIONS.NExcuteQuery(sql)
        DevFunctions.InfoMsg("Credentials updated.")
        load_data()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
End Class