Imports System.Data.SqlClient
Imports DevExpress.XtraSplashScreen

Public Class FrmUpdatePrices
    Dim DT As New DataTable
    Dim OLD_RAP As Double
    Dim NEW_RAP As Double
    Dim UPDATE_QUERY As String
    Dim ASKING_BACK As Double
    Dim ASKING_PPC As Double
    Dim ASKING_TOTAL As Double
    Dim WEIGHT As Double
    Dim NEW_ASKING_BACK As Double
    Dim NEW_ASKING_PPC As Double
    Dim NEW_ASKING_TOTAL As Double

    Dim SQL As String = "select M.TransactionID AS TRANSCATIONID,
 M.Barcode AS BARCODE,
M.StockID AS STOCKID,
M.Carats AS WGT,
S.Shape_Name AS SHAPE,
C.color_name AS COL,
CL.clarity_name AS CLA,
M.RAP AS RAP,
S.Shape_ID as SHAPEID,
C.color_id AS COLORID,
CL.clarity_id AS CLARITYID,
M.AskingPercent AS AskingPercent,
M.AskingPPC AS AskingPPC,
M.AskingTotal AS AskingTotal
FROM (SELECT *, ROW_NUMBER() OVER(PARTITION BY Barcode ORDER BY TransactionID DESC) RowNo
FROM dbo.Dia_MainStock) M
LEFT JOIN Dia_Shape_Master S ON M.ShapeID=S.Shape_ID
LEFT JOIN Dia_Color_Master C ON C.color_id=M.ColorID
LEFT JOIN Dia_Clarity_Master CL ON M.ClarityID=CL.clarity_id

WHERE M.StatusID=(SELECT lot_status_id FROM Dia_Lot_Status_Master WHERE lot_status_name='AVAILABLE')
AND M.RowNo=1 ORDER BY M.TransactionID"
    Private Sub FrmUpdatePrices_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DT = GENERAL_FUNCTIONS.NFetchDatatable(SQL)
        dg.DataSource = DT
    End Sub

    Private Sub btnUpdatePrices_Click(sender As Object, e As EventArgs) Handles btnUpdatePrices.Click
        SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
        Try

            For i = 0 To DT.Rows.Count - 1
                WEIGHT = DT.Rows(i).Item("WGT")
                OLD_RAP = DT.Rows(i).Item("RAP")
                NEW_RAP = DevFunctions.GetRAP(DT.Rows(i).Item("SHAPEID"), DT.Rows(i).Item("COLORID"), DT.Rows(i).Item("CLARITYID"), DT.Rows(i).Item("WGT"))

                If IsDBNull(DT.Rows(i).Item("AskingPPC")) = True Then
                    NEW_ASKING_BACK = 0
                    NEW_ASKING_PPC = NEW_RAP
                    NEW_ASKING_TOTAL = NEW_RAP * WEIGHT
                Else
                    ASKING_PPC = DT.Rows(i).Item("AskingPPC")
                    ASKING_BACK = DT.Rows(i).Item("AskingPercent")
                    If NEW_RAP = 0 Then
                        NEW_ASKING_BACK = 0
                        NEW_ASKING_PPC = ASKING_PPC
                        NEW_ASKING_TOTAL = WEIGHT * ASKING_PPC
                    Else
                        NEW_ASKING_BACK = ASKING_BACK
                        NEW_ASKING_PPC = (NEW_RAP + (NEW_RAP * NEW_ASKING_BACK / 100))
                        NEW_ASKING_TOTAL = WEIGHT * NEW_ASKING_PPC
                    End If
                End If

                UPDATE_QUERY = UPDATE_QUERY & vbCrLf & "UPDATE Dia_MainStock SET RAP=" & NEW_RAP & ",AskingPercent=" & NEW_ASKING_BACK & ",AskingPPC=" & NEW_ASKING_PPC & ",AskingTotal=" & NEW_ASKING_TOTAL & " WHERE TRANSACTIONID=" & DT.Rows(i).Item("TRANSCATIONID")
            Next

            GENERAL_FUNCTIONS.NExcuteQuery(UPDATE_QUERY)
        Catch ex As Exception
            DevFunctions.ErrorMsg(ex.Message)
        Finally
            SplashScreenManager.CloseForm(False)
            DevFunctions.InfoMsg("Prices has been updated.")

        End Try
    End Sub

End Class