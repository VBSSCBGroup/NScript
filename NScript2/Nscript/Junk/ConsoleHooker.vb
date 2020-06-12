Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Namespace NScript.Gameing
    <ComClass(), ComVisible(True)> Public Class ConsoleHooker
#Region "常量声明"
#Disable Warning
        Private Const WH_MOUSE_LL = 14
        Private Const WH_KEYBOARD_LL = 13
        Private Const WH_MOUSE = 7
        Private Const WH_KEYBOARD = 2
        Private Const WM_MOUSEMOVE = &H200
        Private Const WM_LBUTTONDOWN = &H201
        Private Const WM_RBUTTONDOWN = &H204
        Private Const WM_MBUTTONDOWN = &H207
        Private Const WM_LBUTTONUP = &H202
        Private Const WM_RBUTTONUP = &H205
        Private Const WM_MBUTTONUP = &H208
        Private Const WM_LBUTTONDBLCLK = &H203
        Private Const WM_RBUTTONDBLCLK = &H206
        Private Const WM_MBUTTONDBLCLK = &H209
        Private Const WM_MOUSEWHEEL = &H20A
        Private Const WM_KEYDOWN = &H100
        Private Const WM_KEYUP = &H101
        Private Const WM_SYSKEYDOWN = &H104
        Private Const WM_SYSKEYUP = &H105

        Private Const VK_SHIFT As Byte = &H10
        Private Const VK_CAPITAL As Byte = &H14
        Private Const VK_NUMLOCK As Byte = &H90
#Enable Warning
#End Region

        Protected Overrides Sub Finalize()
            Me.UnHook()
            Me.Finalize()
        End Sub
        Public Sub New()

        End Sub
        Private hMouseHook As Integer

        Private MouseHookProcedure As HookProc
        Public Sub UnHook(Optional ByVal UninstallMouseHook As Boolean = True)
            '卸载鼠标钩子
            If hMouseHook <> 0 AndAlso UninstallMouseHook Then
                Dim retMouse As Integer = UnhookWindowsHookEx(hMouseHook)
                hMouseHook = 0
                If retMouse = 0 Then
                    Throw New Win32Exception(Marshal.GetLastWin32Error)
                End If
            End If
        End Sub
        Public Sub StartHook(Optional ByVal InstallMouseHook As Boolean = True)
            '注册鼠标钩子
            If InstallMouseHook AndAlso hMouseHook = 0 Then
                MouseHookProcedure = New HookProc(AddressOf MouseHookProc)
                hMouseHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, LoadLibrary("nscript.dll"), 0)
                If hMouseHook = 0 Then
                    UnHook(False)
                    Throw New Win32Exception(Marshal.GetLastWin32Error)
                End If
            End If
        End Sub
        Public Event ConsoleClick(sender As Object, e As MouseEventArgs)
        Private Function MouseHookProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MouseLLHookStruct) As Integer
            Dim sentid As Long
            GetWindowThreadProcessId(GetConsoleWindow(), sentid)
            If sentid = 0 Or wParam <> WM_LBUTTONDOWN Then
                Return 0
            Else
                Dim rect As Rectangle = Nothing
                GetWindowRect(GetConsoleWindow(), rect)
                If rect.Contains(lParam.PT) Then
                    RaiseEvent ConsoleClick(Me, New MouseEventArgs(Nothing, MouseEventType.MousePress))
                    Return -1
                Else
                    Return 0
                End If
            End If
            Return 0
        End Function
    End Class
End Namespace
