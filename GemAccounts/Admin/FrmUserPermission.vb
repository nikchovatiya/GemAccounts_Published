Public Class FrmUserPermission
    Dim dr As DataRow
    Dim dt As New DataTable
    Dim UserID As Integer
    Dim sql As String

    Private Sub cmbuser_EditValueChanged(sender As Object, e As EventArgs) Handles cmbuser.EditValueChanged
        If cmbuser.EditValue = "" Or cmbuser.EditValue = Nothing Then
            Return
        Else
            UserID = GENERAL_FUNCTIONS.NExcuteScalerInt("Select user_id from Dia_User_Master where user_name='" & cmbuser.EditValue & "'")
            If UserID = 0 Then
                Return
            Else
                dt = GENERAL_FUNCTIONS.NFetchDatatable("select top(1)* from tbl_user_permission where UserID=" & UserID)
                dg.DataSource = dt
            End If
        End If
    End Sub

    Private Sub FrmUserPermission_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DevFunctions.NFillComboDev("Select user_name from Dia_User_Master", "user_name", cmbuser)
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If UserID = 0 Then
            DevFunctions.ErrorMsg("User not selected.")
            Exit Sub
        End If
        sql = "delete from tbl_user_permission where UserID=" & UserID
        GENERAL_FUNCTIONS.NExcuteQuery(sql)
        GENERAL_FUNCTIONS.DataTable_to_SqlTable_Copy(dt, "tbl_user_permission")
        DevFunctions.InfoMsg("User Permission Updated.")
        cmbuser.SelectedIndex = 0
    End Sub
End Class