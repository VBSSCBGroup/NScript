Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Module Module1

    Sub Main(args As String())
        Dim FileName As String
        Console.WriteLine("NScript 编译器 By Laoba
正在读取编译配置...")
        Dim eum As IEnumerable(Of String) = args
        Dim eut As IEnumerator(Of String) = eum.GetEnumerator()
        Do While True
            Dim t As String = eut.Current
            If Not eut.MoveNext() Then
                Exit Do
            End If
            Dim filereg As New Regex("(/|-|\\)file", RegexOptions.IgnoreCase)
            If t Is Nothing Then
                Continue Do
            End If
            If filereg.IsMatch(t) Then
                If Not eut.MoveNext() Then
                    Exit Do
                End If
                FileName = eut.Current
                Exit Do
            End If
        Loop
        Console.WriteLine("执行编译中....")
        Dim stmreader As New StreamReader(FileName, Encoding.ASCII, False)
        Dim strbulid As New StringBuilder(5000)
        Dim checkreg As New Regex("Set NScript=", 1)
        Dim reg1 As New Regex("Get\(", RegexOptions.IgnoreCase)
        strbulid.Append("'Generate By NScriptCompiler 1.0
Function Import(filePath)
Set stm = CreateObject(""Adodb.Stream"")
stm.Type = 2
stm.mode = 3
stm.charset = ""ASCII""
stm.Open
stm.LoadFromFile filePath
filestr = stm.readtext
stm.close
ExecuteGlobal filestr
End Function")
        strbulid.Append("Set NScript=CreateObject(""NScript.NetScript"")")
        Dim ls As Long = 0
        Dim importover As Boolean = False
        Dim iptemp As Boolean = True
        Do
            ls += 1
            Dim line As String = stmreader.ReadLine()
            line = reg1.Replace(line, "NScript.GOV(")
            If line.StartsWith("Import") Then
                iptemp = True
            End If
            If iptemp Then

            End If
            If checkreg.IsMatch(line) Then
                Console.WriteLine("编译错误在行" & ls & "字符" & checkreg.Match(line).Index & "
原因：不建议设置NScript对象.")
                Exit Sub
            End If
            strbulid.AppendLine()
        Loop
    End Sub

End Module
