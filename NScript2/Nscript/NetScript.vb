Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports System.Security
Namespace NScript
    <ComVisible(True)> <ComClass()> Public Class NetScript
        Private Shared IsInManMode As Boolean = False
        Private Shared IsNewed As Boolean = False
        Public Function CallFun(obj0 As Object, name As String, ParamArray args As Object()) As Object
            If obj0 Is Nothing Or name Is Nothing Then
                Return Nothing
            End If
            Dim tp As Type = CType(obj0, Object).GetType()
            Dim methods As MethodInfo() = tp.GetMethods()
            For Each mh In methods
                If name.ToLower() = mh.Name.ToLower() Then
                    Return mh.Invoke(obj0, args)
                End If
            Next
            Return Nothing
        End Function
        <SecurityCritical()> Public Sub Update()
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)
            Dim p As New System.Security.Permissions.PrincipalPermission("", "Administrators")
            p.Demand()
            NScript.Update.UpdateVer()
        End Sub
        Public Sub New()
            If IsNewed Then
                Environment.Exit(&H404)
            Else
                IsNewed = True
            End If
            If IsInManMode Then
            Else
                IsInManMode = True
                'inew()
            End If
        End Sub
        Private thd As Thread
        Public Sub KeepDebuggerSafe()
            If thd Is Nothing Then
                thd = New Thread(Sub()
                                     Dim CrashDG As [Delegate] = Sub()
                                                                     Try
                                                                         Try
                                                                             Throw New FileNotFoundException()
                                                                         Catch ex As FileNotFoundException When ex.FileName.EndsWith(".971", 2)
                                                                             MsgBox("???")
                                                                         Catch ex As FileNotFoundException
                                                                             MsgBox("???2")
                                                                         End Try
                                                                     Catch ex As NullReferenceException
                                                                         MsgBox("???3")
                                                                     End Try
                                                                     MsgBox("???4")
                                                                 End Sub
                                     Do
                                         If Debugger.IsAttached Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         Dim rt2 As Boolean = False
                                         If NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, rt2, IntPtr.Size, Nothing) <> 0 Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         If rt2 Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         Threading.Thread.Sleep(100)
                                     Loop
                                 End Sub)
                thd.Start()
                Dim CrashDG2 As [Delegate] = Sub()
                                                 Try
                                                     Try
                                                         Throw New FileNotFoundException()
                                                     Catch ex As FileNotFoundException When ex.FileName.EndsWith(".971", 1)
                                                         MsgBox("???")
                                                     Catch ex As FileNotFoundException
                                                         MsgBox("???2")
                                                     End Try
                                                 Catch ex As NullReferenceException
                                                     MsgBox("???3")
                                                 End Try
                                                 MsgBox("???4")
                                             End Sub
                If Debugger.IsAttached Then
                    CrashDG2.DynamicInvoke()
                End If
                Dim rt3 As Boolean = False
                If NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, rt3, IntPtr.Size, Nothing) <> 0 Then
                    CrashDG2.DynamicInvoke()
                End If
                If rt3 Then
                    CrashDG2.DynamicInvoke()
                End If
                Threading.Thread.Sleep(100)
            End If
        End Sub
        Public Function OnError(Ex) As Integer
            If TypeOf Ex IsNot Exception Then
                If TypeOf Ex Is IConvertible Then
                    Try
                        Ex = New Exception(DirectCast(Ex, IConvertible).ToString(Nothing))
                    Catch ex2 As Exception
                        Ex = ex2
                    End Try
                Else
                    Ex = New ArgumentException("ex不可转换", NameOf(Ex))
                End If
            End If
            Dim terr As New ThreadExceptionDialog(Ex)
            Dim ret = terr.ShowDialog()
            If ret = DialogResult.Abort Then
                Application.Exit()
            ElseIf ret = DialogResult.Retry Then
                Application.Restart()
                Application.Exit()
            ElseIf ret = DialogResult.Ignore Then
                Ex = Nothing
                terr.Dispose()
                Return 1
            End If
            terr.Dispose()
            Return 2
        End Function
        Public Sub RunFormProgram()
            Application.EnableVisualStyles()
            Try
                Application.Run()
            Catch ex As InvalidOperationException
                Throw
            Catch ex As Exception
                If OnError(ex) = 2 Then Throw
            Finally
                For Each frm As Form In Application.OpenForms
                    frm.Close()
                    frm.Dispose()
                Next
            End Try
        End Sub
        Public Sub TN()

        End Sub
        Friend Sub INew()
            Try
                InitManagedMode()
            Catch ex As COMException
                If ex.ErrorCode = &H80040154 Then
                    MsgBox("出现错误：托管脚本环境建立失败：（
(也许是64位com插件没有注册)
详细：" & ex.ToString(), MsgBoxStyle.MsgBoxHelp, "NScript")
                End If
                Dim tde As New ThreadExceptionDialog(ex)
                tde.ShowDialog()
                tde.Dispose()
            End Try
        End Sub
        Public Sub InitManagedMode()
            Debugger.Break()
            Dim fl As String
            For Each Str0 In Environment.GetCommandLineArgs()
                If Str0.EndsWith("script.exe", 1) Then
                Else
                    fl = Str0
                    Exit For
                End If
            Next
            Dim strb As New StringBuilder()
            Dim strreader As New StreamReader(New FileStream(fl, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), True)
            Do
                Dim line As String = strreader.ReadLine()
                If line.StartsWith("'OMM:", 1) And (line.ToLower() <> "'omm:") Then
                    strb.AppendLine(New String(line.Skip(5).ToArray()))
                ElseIf line.StartsWith("'SetManaged", 1) And (line.ToLower() <> "'setmanaged") Then
                    strb.AppendLine("NScript.SOV " & line.Skip(11).ToArray())
                Else
                    strb.AppendLine(line)
                End If
            Loop Until strreader.EndOfStream
            strreader.Dispose()
            Dim w As MSScriptControl.ScriptControl = CreateObject("MSScriptControl.ScriptControl")
            w.Language = "VBScript"
            w.AddObject("NScript", Me, True)
            w.AddCode(strb.ToString())
            Dim thd As New Threading.Thread(Sub()
                                                Try
                                                    w.Run("ManagedMain")
#Disable Warning
                                                Catch ex As Exception
#Enable Warning
                                                    Dim tde As New ThreadExceptionDialog(ex)
                                                    tde.ShowDialog()
                                                    tde.Dispose()
                                                    Environment.Exit(ex.HResult)
                                                End Try
                                            End Sub)
            thd.Start()
            ExitThread(0)
        End Sub
        Public Function NewThread(code As String, ParamArray args As Object()) As Object
            Dim w As MSScriptControl.ScriptControl = CreateObject("MSScriptControl.ScriptControl")
            w.Language = "VBScript"
            w.AddCode(code)
            Dim thd As New Threading.Thread(Sub()
                                                w.Run("ThdMain", args)
                                            End Sub)
            thd.Start()
            Return thd
        End Function
        ''' <summary>
        ''' 检察版本号
        ''' </summary>
        ''' <param name="targetver">目标版本</param>
        ''' <param name="Force">是否强制检查版本</param>
        ''' <param name="MaxVer">最大版本</param>
        ''' <returns>如果是True则在目标和最大之内，如果是False则相反></returns>
        Public Overloads Function CheckVer(TargetVer As Single, Optional Force As Boolean = True, Optional MaxVer As Single = 0)
            If TargetVer <= 0 Then
                Throw New ArgumentException(GetLocalString("Error0x2"))
            Else
                Return CheckVerFriend(TargetVer, Force, MaxVer)
            End If
        End Function


        Friend Function CheckVerFriend(targetver As Single, Force As Boolean, maxver As Single) As Boolean
            Dim res As Boolean
            Try
                Dim ver As String() = Split(NScriptModule.Ver, "|")
                Dim verv As Single = CSng(ver(0))
                If verv >= targetver Then
                    If verv < maxver And Not (maxver = 0) Then
                        res = True
                    Else
                        If Force Then
                            MsgBox(String.Format(Nothing, GetLocalString("CheckVer.TooHigh"), CStr(maxver)), 16 + MsgBoxStyle.SystemModal, "NScript")
                            Environment.Exit(4484)
                            res = False
                        Else
                            Select Case MsgBox(GetLocalString("CheckVer.NotForceTooHigh"), MsgBoxStyle.Exclamation + MsgBoxStyle.SystemModal + MsgBoxStyle.AbortRetryIgnore, "NScript")
                                Case MsgBoxResult.Abort
                                    Environment.Exit(4484)
                                Case MsgBoxResult.Retry
                                    MsgBox(String.Format(Nothing, GetLocalString("CheckVer.HighTips"), CStr(maxver)))
                            End Select
                            res = False
                        End If
                    End If
                Else
                    If Force Then
                        MsgBox(String.Format(Nothing, GetLocalString("CheckVer.Toolow"), CStr(targetver)), 16 + MsgBoxStyle.SystemModal, "NScript")
                        Environment.Exit(4484)
                        res = False
                    Else
                        Select Case MsgBox(GetLocalString("CheckVer.NotForceTooLow"), MsgBoxStyle.Exclamation + MsgBoxStyle.SystemModal + MsgBoxStyle.AbortRetryIgnore, "NScript")
                            Case MsgBoxResult.Abort
                                Environment.Exit(4484)
                            Case MsgBoxResult.Retry
                                MsgBox(String.Format(Nothing, GetLocalString("CheckVer.LowTips"), CStr(targetver)))
                        End Select
                        res = False
                    End If
                End If
            Finally
            End Try
            Return res
        End Function
        ''' <summary>
        ''' 获取版本代号（1.3更改）
        ''' 1.2是获取版本值：（
        ''' </summary>
        ''' <returns></returns>
        Public Function GetVer() As String
            Return Split(NScriptModule.Ver, "|")(1)
        End Function
        ''' <summary>
        ''' 获取版本（1.3加入）
        ''' </summary>
        ''' <returns></returns>
        Public Function GetVerNum() As Single
            Return CSng(Split(NScriptModule.Ver, "|")(0))
        End Function
        ''' <summary>
        ''' 获取对象的字符串表达式
        ''' </summary>
        ''' <param name="Obj0">你不可能有的对象</param>
        ''' <returns></returns>
        Public Function ToString(Obj0 As Object) As String
            If Obj0 Is Nothing Then
                Return ""
            End If
            Return Obj0.ToString()
        End Function
        ''' <summary>
        ''' 判断调试器是否附加，可用作防御调试器
        ''' </summary>
        ''' <returns></returns>
        Public Function HasDebugger() As Boolean
            Return Debugger.IsAttached
        End Function
        ''' <summary>
        ''' DLL函数动态调用（WinApi）
        ''' </summary>
        ''' <param name="DllName">DLL的名字</param>
        ''' <param name="DllMethodName">方法的名字</param>
        ''' <param name="HasReturn">是否有返回值</param>
        ''' <param name="Args">调用参数</param>
        ''' <returns></returns>
        <CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification:="<挂起>")>
        Public Function DllCall(DllName As String, DllMethodName As String, HasReturn As Boolean, ParamArray Args As Object()) As Object
            Dim HPtr As IntPtr = SafeNativeMethods.LoadLibraryW(DllName)
            If HPtr = -1 Then
                Throw New Win32Exception()
            ElseIf HPtr = 0 Then
                Throw New KeyNotFoundException()
            End If
            Dim MethodPtr As IntPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName)
            If MethodPtr = 0 Then
                MethodPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName & "A")
                If MethodPtr = 0 Then
                    MethodPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName & "W")
                    If MethodPtr = 0 Then
                        Throw New NotImplementedException(DllMethodName & "不存在。")
                    End If
                End If
            End If
            ' 设置编译控制参数 
            Dim cp As CompilerParameters = New CompilerParameters With {
            .GenerateExecutable = False, '生成DLL，如果是True则生成exe文件 
            .GenerateInMemory = True,
            .TreatWarningsAsErrors = False
        }
            Dim prn As New VBCodeProvider()
            Dim nstr As String = ""
            For i = 0 To Args.Length - 1
                nstr += "Arg" + CStr(i)
                If i = Args.Length - 1 Then
                Else
                    nstr += ","
                End If
            Next
            Dim unused = prn.CompileAssemblyFromSource(cp, String.Format("
Imports System
Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Namespace DllTemp
    Public Module {1}Temp
          <DllImport(""{3}"")> Public {0} {1}({2})

          End Function
    End Module
End Namespace", IIf(HasReturn, "Function", "Sub"), DllMethodName, nstr, DllName))
            If unused.Errors.Count = 0 Then
                Dim MethodTypes As Type() = unused.CompiledAssembly.GetTypes
                Dim MethodType As Type
                For Each wawa In MethodTypes
                    If wawa.Name = DllMethodName + "Temp" Then
                        MethodType = wawa
                    End If
                Next
                If MethodType Is Nothing Then
                    Throw New EntryPointNotFoundException()
                End If
                prn.Dispose()
                Return MethodType.GetMethod(DllMethodName).Invoke(Nothing, Args)
            Else
                Throw New ArgumentException(CType(unused.Errors.Item(0), CompilerError).ErrorText)
            End If
            prn.Dispose()
            Return Nothing
        End Function
        ''' <summary>
        ''' 获取Bytes所代表的字符串
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <returns></returns>
        Public Function GetStr(bytes As Byte()) As <MarshalAs(UnmanagedType.BStr)> String
            Return Encoding.Default.GetString(bytes)
        End Function
        ''' <summary>
        ''' 创造NET对象
        ''' </summary>
        ''' <param name="Name">对象名（不包括命名空间名）</param>
        ''' <param name="CreateArgs">创造参数（默认请留空）</param>
        ''' <returns></returns>
        Public Function CreateObject(Name As String, ParamArray CreateArgs As Object()) As Object
            Dim type As Type, aby2 As Assembly
            Dim aby As Assembly = Assembly.GetExecutingAssembly()
            For Each wa In aby.GetReferencedAssemblies()
                Dim aby1 As Assembly = Assembly.Load(wa)
                Dim wawas As Type() = aby1.GetTypes()
                For Each wawa In wawas
                    If wawa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = Name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        type = wawa
                        aby2 = aby1
                        Exit For
                    End If
                Next
            Next
            Dim wawass As Type() = aby.GetTypes()

            For Each wawa In wawass
                If wawa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = Name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                    type = wawa
                    aby2 = aby
                    Exit For
                End If
            Next
            If type Is Nothing Or aby2 Is Nothing Then
                Return Nothing
            Else
                Return aby2.CreateInstance(type.FullName, False, BindingFlags.Instance Or BindingFlags.[Public], Nothing, CreateArgs, Globalization.CultureInfo.CurrentCulture, Nothing)
            End If
        End Function
        Public Function GetHashCode(Obj0 As Object) As Int32
            If Obj0 Is Nothing Then
                Return 0
            End If
            Return Obj0.GetHashCode()
        End Function
        Public Sub SOV(Obj0 As Object, name As String, value As Object)
            If Obj0 IsNot Nothing Then
                Dim type As Type = Obj0.GetType()
                Dim shuxing As PropertyInfo() = type.GetProperties()
                For Each shuxings In shuxing
                    If shuxings.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        shuxings.SetValue(Obj0, value, Nothing)
                        Return
                    End If
                Next
            End If
        End Sub
        Public Function GetEumble(obj0 As Object) As IEnumerable
            Return TryCast(obj0, IEnumerable)
        End Function
        Public Function GOV(Obj0 As Object, name As String) As Object
            If Obj0 IsNot Nothing Then
                Dim type As Type = Obj0.GetType()
                Dim shuxing As PropertyInfo() = type.GetProperties()
                For Each shuxings In shuxing
                    If shuxings.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        Return shuxings.GetValue(Obj0, Nothing)
                    End If
                Next
            End If
            Return Nothing
        End Function
    End Class
End Namespace
