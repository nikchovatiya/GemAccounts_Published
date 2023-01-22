Public Class FrmSale
    Dim dt As New DataTable
    Dim dr As DataRow
    Private Sub FrmSale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Clear()
    End Sub
    Private Sub Clear()
        dtp.EditValue = Now()
        CmbCustomer.EditValue = Nothing
        cmbBroker.EditValue = Nothing
        CmbTerms.EditValue = Nothing
        TxtBarcode.EditValue = Nothing
        TxtStockID.EditValue = Nothing
        TxtCarats.EditValue = 0
        TxtShape.EditValue = Nothing
        TxtColor.EditValue = Nothing
        TxtClarity.EditValue = Nothing
        TxtFlo.EditValue = Nothing
        TxtCut.EditValue = Nothing
        TxtPolish.EditValue = Nothing
        TxtSym.EditValue = Nothing
        TxtLab.EditValue = Nothing
        TxtCertificateNo.EditValue = Nothing
        TxtRAP.EditValue = 0
        TxtDiscount.EditValue = 0
        TxtUSDperCarat.EditValue = 0
        TxtTotalUSD.EditValue = 0
        TxtLessPercent.EditValue = 0
        TxtAfterLessUSD.EditValue = 0
        TxtRate.EditValue = DevFunctions.GetLiveRate()
        TxtTotalINR.EditValue = 0
        TxtBrokeragePercent.EditValue = 0
        TxtBrokerageINR.EditValue = 0
        TxtAfterBrokerageINR.EditValue = 0
        TxtIGIExp.EditValue = 0
        TxtGIAExp.EditValue = 0
        TxtHRDExp.EditValue = 0
        TxtSuratExp.EditValue = 0
        TxtDelhiExp.EditValue = 0
        TxtExportExp.EditValue = 0
        TxtOtherExp.EditValue = 0
        TxtNetSaleINR.EditValue = 0
        TxtComment.EditValue = Nothing
        ' dt.Rows.Clear()
        dtp.Focus()
        DevFunctions.NFillComboDev("select Terms_Name from Dia_Terms_Master Order By Terms_Name", "Terms_Name", CmbTerms)
        DevFunctions.NFillComboDev("SELECT LedgerName FROM Dia_Ledger_Master WHERE LUnderGroup=(Select group_id FROM Dia_Group_Master WHERE group_name='Broker') order by LedgerName", "LedgerName", cmbBroker)
        DevFunctions.NFillComboDev("SELECT LedgerName FROM Dia_Ledger_Master WHERE LUnderGroup=(Select group_id FROM Dia_Group_Master WHERE group_name='Customers') order by LedgerName", "LedgerName", CmbCustomer)

    End Sub

    Private Sub dtp_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp.KeyDown, CmbCustomer.KeyDown, cmbBroker.KeyDown, CmbTerms.KeyDown, TxtDiscount.KeyDown, TxtUSDperCarat.KeyDown, TxtTotalUSD.KeyDown, TxtLessPercent.KeyDown, TxtAfterLessUSD.KeyDown, TxtRate.KeyDown, TxtTotalINR.KeyDown, TxtBrokeragePercent.KeyDown, TxtBrokerageINR.KeyDown, TxtAfterBrokerageINR.KeyDown, TxtIGIExp.KeyDown, TxtGIAExp.KeyDown, TxtHRDExp.KeyDown, TxtSuratExp.KeyDown, TxtDelhiExp.KeyDown, TxtExportExp.KeyDown, TxtOtherExp.KeyDown, TxtComment.KeyDown
        GENERAL_FUNCTIONS.FocusNext(sender, e)
    End Sub

    Private Sub TxtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtBarcode.KeyDown
        If TxtBarcode.EditValue = Nothing Then
            TxtStockID.Focus()
        Else
            dr = GENERAL_FUNCTIONS.NFetchDataRow("select top(1)* from Dia_MainStock where Barcode='" & TxtBarcode.EditValue & "' order by TransactionID desc")
            If dr Is Nothing Then
                DevFunctions.ErrorMsg("Wrong Barcode..")
                TxtBarcode.EditValue = Nothing
                TxtBarcode.Focus()
            Else

            End If
        End If
    End Sub
End Class