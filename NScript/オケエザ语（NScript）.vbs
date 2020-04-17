set wa=wscript.createobject("NScript.NSTForm","CAO_")
set NS=wscript.createobject("NScript.NetScript")
set NT=wscript.createobject("NScript.NTranslator")
NT.SetSPCLang"オ","ケ","エ","ザ"
wa.bulid "<FormData text=#My App# x=#0# y=#0# id=#FormAutoGen7055.475# px=#400# py=#400#><Label text=#中文：# x=#16# y=#25# id=#Label0# sizex=#0# sizey=#0#/><Label text=#伪日语：# x=#5# y=#56# id=#Label1# sizex=#0# sizey=#0#/><TextBox text=#哇# x=#65# y=#23# id=#NaiveLang# sizex=#200# sizey=#0#/><TextBox text=## x=#65# y=#53# id=#GuLang# sizex=#200# sizey=#0#/><Button text=#转伪日语# x=#34# y=#121# id=#ToGu# sizex=#0# sizey=#0#/><Button text=#转中文# x=#136# y=#121# id=#ToChi# sizex=#0# sizey=#0#/></FormData>"
NS.SOV wa.control("Test"),"Visible",false
wa.showdialog()
sub CAO_Click(sender,e)
if e.SenderId = "ToGu" then
NS.SOV wa.control("GuLang"),"Text",NT.ChineseToSPC(NS.GOV(wa.control("NaiveLang"),"Text"))
elseif e.SenderId = "ToChi" then
NS.SOV wa.control("NaiveLang"),"Text",NT.SPCToChinese(NS.GOV(wa.control("GuLang"),"Text"))
elseif e.SenderId = "Test" then
for each eee in wa.controls
NS.SOV eee,"Top",cint(rnd()*300)
next
end if
end sub