Public Class Form1
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        RichTextBox1.Rtf = "{\rtf1\ansi\deff0\nouicompat{\fonttbl{\f0\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
{\*\generator Riched20 10.0.18362}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\f0\fs22\lang2052 NScript\strike\'b0\'d4\'cd\'f5ELUA\strike0\par
\'c7\'eb\'d7\'d0\'cf\'b8\'d4\'c4\'b6\'c1\'d2\'d4\'cf\'c2\'cc\'f5\'bf\'ee\'a3\'ba\par
\'b1\'be\'c8\'ed\'bc\'fe\'ca\'c7\'bf\'aa\'d4\'b4\'b5\'c4\'d2\'bb\'b8\'f6\'c8\'ed\'bc\'fe\'a3\'ac\b\'b5\'ab\'ca\'c7\'d5\'e2\'b2\'a2\'b2\'bb\'d2\'e2\'ce\'b6\'d7\'c5\'c4\'e3\'bf\'c9\'d2\'d4\'bd\'abNScript\'b5\'c4\'b4\'fa\'c2\'eb\'d3\'c3\'d4\'da\'c6\'e4\'cb\'fb\'b5\'d8\'b7\'bd\b0\'a1\'a3\par
1.\'b1\'be\'c8\'ed\'bc\'fe\'ca\'f4\'d3\'da\'d4\'cb\'d0\'d0\'bf\'e2\'a3\'ac\'d0\'b6\'d4\'d8\'bf\'c9\'c4\'dc\'b2\'bb\'cd\'ea\'c8\'ab\'a1\'a3\par
\'a3\'a8\'b4\'fd\'b2\'b9\'b3\'e4...\'a3\'a9\par
\'d7\'a2\'d2\'e2\'ca\'c2\'cf\'ee\'a3\'ba\par
\'b1\'be\'c8\'ed\'bc\'fe\'ca\'c7\'b2\'bb\'d0\'e8\'d2\'aa\b\'bc\'a4\'bb\'ee\b0\'a3\'ac\'d2\'b2\'b2\'bb\'d3\'c3\b\'d7\'a2\'b2\'e1\b0\'b5\'c4\'a1\'a3\par
}
" & Chr(0)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Environment.Exit(0)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim i As New Install
        Hide()
        i.ShowDialog()
        i.Dispose()
        Close()
    End Sub
End Class
