
cd /d %~dp0
set WEBDIR=..\Ampl.Web.Mvc.EditorTemplates.Web

for %%i in (Classes.css Fonts.css Margins.css Paddings.css BootstrapFixes.css) do copy /y %WEBDIR%\Content\%%i .\Content\
copy /y %WEBDIR%\Content\Site.css .\editor.templates.installation.instructions\Content\
copy /y %WEBDIR%\Scripts\ampl*    .\Scripts\

if not exist .\Scripts\cldr\nul              md .\Scripts\cldr
if not exist .\Scripts\cldr\main\nul         md .\Scripts\cldr\main
if not exist .\Scripts\cldr\supplemental\nul md .\Scripts\cldr\supplemental

xcopy /e /y %WEBDIR%\Scripts\cldr\main\*         .\Scripts\cldr\main\
copy  /y    %WEBDIR%\Scripts\cldr\supplemental\* .\Scripts\cldr\supplemental\
pause

del /q .\Views\Shared\DisplayTemplates\*
copy /y %WEBDIR%\Views\Shared\DisplayTemplates\* .\Views\Shared\DisplayTemplates\

del /q .\Views\Shared\EditorTemplates\*
copy /y %WEBDIR%\Views\Shared\EditorTemplates\* .\Views\Shared\EditorTemplates\

copy /y %WEBDIR%\Views\Shared\_AlertViewPartial.cshtml .\Views\Shared\

for %%i in (_HeadingViewPartial.cshtml _Layout.cshtml) do copy /y %WEBDIR%\Views\Shared\%%i .\editor.templates.installation.instructions\Views\Shared\

pause
