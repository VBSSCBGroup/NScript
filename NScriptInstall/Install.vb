Public Class Install

    Private Sub MaskedTextBox1_TextChanged(sender As Object, e As EventArgs) Handles MaskedTextBox1.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dp As New Deployer()
        Hide()
        dp.ShowDialog()
        Close()
    End Sub
End Class