 NScript 一款真正人性化的vbs库（串味了
 <br/>
 # NScript 窗体
 少说b话，直接上代码
```VBS
Set FormAutoGen705547 = Wscript.CreateObject("NScript.NSTForm","FormAutoGen705547_")
Sub FormAutoGen705547_Click(sender,e)
  Select Case e.SenderId
  Case "Label4"
    msgbox "echo offffff"
  Case "Label5"
    msgbox "see noe"
  Case "Label6"
    msgbox 552
  End Select
End Sub

FormAutoGen705547.bulid "<FormData text=#My App# x=#0# y=#0# id=#FormAutoGen705547# px=#400# py=#400#><Label text=#Label# x=#58# y=#125# id=#Label0# sizex=#0# sizey=#0#/><Label text=#Label# x=#120# y=#27# id=#Label1# sizex=#0# sizey=#0#/><Label text=#Label# x=#164# y=#96# id=#Label2# sizex=#0# sizey=#0#/><Label text=#Label# x=#63# y=#104# id=#Label3# sizex=#0# sizey=#0#/><Label text=#Label# x=#104# y=#75# id=#Label4# sizex=#0# sizey=#0#/><Label text=#Label# x=#39# y=#41# id=#Label5# sizex=#0# sizey=#0#/><Label text=#Label# x=#30# y=#136# id=#Label6# sizex=#0# sizey=#0#/></FormData>"
FormAutoGen705547.ShowDialog()
```
![](https://github.com/lx1587496147/Images/raw/master/%E6%8D%95%E8%8E%B7.PNG)

# NScript 绘画
```VBS
'注：功能更新ing
Set FormAutoGen705547 = Wscript.CreateObject("NScript.NSTForm","FormAutoGen705547_")
Sub FormAutoGen705547_Click(sender,e)
Select Case e.SenderId
Case "Label0"
msgbox "***"
End Select
End Sub

FormAutoGen705547.bulid "<FormData text=#Test 1# x=#0# y=#0# id=#FormAutoGen705547# px=#400# py=#400#><Label text=#点我# x=#105# y=#116# id=#Label0# sizex=#0# sizey=#0#/></FormData>"
FormAutoGen705547.Show()
Set Timer1 = Wscript.CreateObject("NScript.Timer","Timer_")
Sub Timer_Tick()
FormAutoGen705547.Graphics.DrawLine &HFF000000 + rnd()*&HFF+rnd()*&HFF00+rnd()*&HFF0000,rnd()*5,rnd()*500,rnd()*500,rnd()*500,rnd()*500
End Sub
Timer1.Start(1)
CreateObject("NScript.Netscript").RunFormProgram()

```
