Public Class EventHandlerEdit
    Public Sub New(Frm1 As Form1, ctlid As String)
        Me.Frm1 = Frm1
        Me.ControlId = ctlid
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Label1.Text = String.Format(Label1.Text, ctlid)
    End Sub
    Private ControlId As String, Frm1 As Form1
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.Items.Contains(ComboBox1.Text) Then
            If Frm1 IsNot Nothing Then
                If Frm1.EventHandlers.ContainsKey(New Tuple(Of String, String)(ControlId, ComboBox1.Text)) Then
                    Frm1.EventHandlers.Remove(New Tuple(Of String, String)(ControlId, ComboBox1.Text))
                    Me.Hide()
                Else
                    MsgBox("该事件没有对应的绑定代码。")
                End If
            End If
        Else
                    MsgBox("事件类型不正确 :(")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.Items.Contains(ComboBox1.Text) Then
            Frm1.EventHandlers.Add(New Tuple(Of String, String)(ControlId, ComboBox1.Text), TextBox1.Text)
            Me.Hide()
        Else
            MsgBox("事件类型不正确 :(")
        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        If ComboBox1.Items.Contains(ComboBox1.Text) Then
            If Frm1 IsNot Nothing Then
                If Frm1.EventHandlers.ContainsKey(New Tuple(Of String, String)(ControlId, ComboBox1.Text)) Then
                    TextBox1.Text = Frm1.EventHandlers.Item(New Tuple(Of String, String)(ControlId, ComboBox1.Text))
                End If
            End If
        End If
    End Sub
End Class