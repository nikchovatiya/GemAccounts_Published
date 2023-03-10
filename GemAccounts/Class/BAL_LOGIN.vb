Imports System.Data
Imports System.Data.SqlClient
Imports GemAccounts.GENERAL_FUNCTIONS
Imports GemAccounts.GLOBAL_VARIABLES

Public Class BAL_LOGIN
    Public Structure RoleData
        Public Login As Boolean
        Public Urole As String
        Public Username As String
        Public BranchID As Integer
        Public UserID As Integer
        Public CompanyID As Integer
        Public StockLayoutID As Integer
        Public ShowAdminTab As Boolean
        Public EditInvoice As Boolean
        Public DeleteInvoice As Boolean
        Public ViewInvoiceList As Boolean
    End Structure
    Public Function CheckLogin(ByVal user As TextBox, ByVal password As TextBox, ByVal br_id As Integer, ByVal cmp_id As Integer) As RoleData
        Dim returnStructure As New RoleData
        returnStructure.Login = False
        returnStructure.BranchID = Nothing
        returnStructure.UserID = Nothing
        returnStructure.CompanyID = Nothing
        returnStructure.Username = ""
        returnStructure.Urole = ""
        returnStructure.StockLayoutID = Nothing
        CMD = New SqlCommand("CHEKLOGIN", VarCon.Connection())
        CMD.CommandType = CommandType.StoredProcedure
        CMD.Parameters.AddWithValue("@user_name", user.Text)
        CMD.Parameters.AddWithValue("@password", password.Text)
        ' CMD.Parameters.AddWithValue("@UserStatus", "True")
        CMD.Parameters.AddWithValue("@br_id", br_id)
        CMD.Parameters.AddWithValue("@cmp_id", cmp_id)
        RDR = CMD.ExecuteReader
        If RDR.HasRows Then

            While RDR.Read
                If UCase(RDR.Item("user_name").ToString) = UCase(user.Text) And RDR.Item("password").ToString = password.Text Then
                    returnStructure.Login = True
                    Dim dr1 As DataRow = GENERAL_FUNCTIONS.NFetchDataRow("select top(1)* from tbl_user_permission where UserID=" & RDR.Item("user_id").ToString)
                    If dr1 Is Nothing Then

                    Else
                        returnStructure.ShowAdminTab = dr1("show_admin_menu")
                        returnStructure.EditInvoice = dr1("edit_invoice")
                        returnStructure.DeleteInvoice = dr1("delete_invoice")
                        returnStructure.ViewInvoiceList = dr1("view_invoice_list")
                    End If
                    returnStructure.CompanyID = RDR.Item("cmp_id").ToString
                    returnStructure.BranchID = RDR.Item("br_id").ToString
                    returnStructure.Username = RDR.Item("user_name").ToString
                    returnStructure.Urole = "" 'RDR.Item("UTYPE").ToString
                    returnStructure.UserID = RDR.Item("user_id").ToString
                    returnStructure.StockLayoutID = RDR.Item("StockLayoutID").ToString

                    Exit While
                End If
            End While

        Else
            returnStructure.Login = False
            returnStructure.CompanyID = Nothing
            returnStructure.BranchID = Nothing
            returnStructure.UserID = Nothing
            returnStructure.Username = ""
            returnStructure.Urole = ""
            returnStructure.StockLayoutID = Nothing
        End If
        VarCon.CloseConnection()
        Return returnStructure
    End Function

End Class
